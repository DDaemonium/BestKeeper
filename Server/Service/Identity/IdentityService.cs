namespace Server.Service.Identity
{
    using Domain.Dto.Request.Identity;
    using Domain.Dto.Response.Identity;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess;
    using Server.DataAccess.Models;
    using System.Security.Cryptography;
    using System.Text;

    public class IdentityService
    {
        private readonly DatabaseContext _databaseContext;

        public IdentityService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<IdentityUser>> GetAllUsersAsync()
        => await _databaseContext.ApplicationUsers.AsNoTracking()
            .Select(u => new IdentityUser
            {
                Email = u.Email,
                Id = u.Id,
                Name = u.Name,
                Surname = u.Surname,
                PhoneNumber = u.PhoneNumber,
                Role = u.ApplicationUserRole.Name,
                IsActive = u.IsActive,
            }).ToListAsync();

        public async Task<IdentityUser> GetUserByIdAsync(Guid id)
        => await _databaseContext.ApplicationUsers.Where(u => u.Id == id).AsNoTracking()
                .Select(u => new IdentityUser {
                    Email = u.Email,
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.ApplicationUserRole.Name,
                    IsActive = u.IsActive,
                }).FirstOrDefaultAsync();

        public async Task<IdentityUser> LoginAsync(LoginInfo loginInfo)
        {
            var passwordHash = this.EncryptPassword(loginInfo.Password);
            return await _databaseContext.ApplicationUsers.Where(
                u => u.Email.Equals(loginInfo.Email) && u.Password.Equals(passwordHash) && u.IsActive)
                .Select(u => new IdentityUser
                {
                    Email = u.Email,
                    Id = u.Id,
                    Name = u.Name,
                    Surname = u.Surname,
                    PhoneNumber = u.PhoneNumber,
                    Role = u.ApplicationUserRole.Name,
                    IsActive = u.IsActive,
                }).FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveAsync(Guid userId)
        {
            var user = await _databaseContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new Exception("Invalid user Id.");
            }

            _databaseContext.ApplicationUsers.Remove(user);

            return await _databaseContext.SaveChangesAsync() > 0;
        }

        public async Task<ResetPasswrdMessage> ResetPasswordAsync(ChangeUserPassword changeUserPassword)
        {
            var user = await _databaseContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == changeUserPassword.UserId && u.Password.Equals(EncryptPassword(changeUserPassword.OldPassword)));

            if (user is null)
            {
                return new ResetPasswrdMessage { IsSuccess = false, Message = "Incorrect password." };
            }

            user.Password = EncryptPassword(changeUserPassword.NewPassword);
            _databaseContext.ApplicationUsers.Update(user);
            await _databaseContext.SaveChangesAsync();

            return new ResetPasswrdMessage { IsSuccess = true };
        }

        public async Task<bool> ChangeUserActivityAsync(Guid id, bool isActive)
        {
            var user = await _databaseContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new Exception("Invalid user Id.");
            }

            if(user.IsActive == isActive)
            {
                return user.IsActive;
            }
            else
            {
                user.IsActive = isActive;
                _databaseContext.ApplicationUsers.Update(user);
                await _databaseContext.SaveChangesAsync();
            }
            
            return user.IsActive;
        }

        public async Task<IdentityUser> UpdateUserInfoAsync(UpdateUserInfo updateUserInfo)
        {
            var user = await _databaseContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == updateUserInfo.Id);

            if (user is null)
            {
                throw new Exception("Invalid user Id.");
            }

            var role = await _databaseContext.ApplicationUserRoles.FirstOrDefaultAsync(r => r.Id == updateUserInfo.RoleId);

            if (role is null)
            {
                throw new Exception("Invalid role Id.");
            }

            user.Name = updateUserInfo.Name;
            user.Surname = updateUserInfo.Surname;
            user.PhoneNumber = updateUserInfo.PhoneNumber;
            user.IsActive = updateUserInfo.IsActive;
            user.Email = updateUserInfo.Email;
            user.ApplicationUserRoleId = role.Id;
            _databaseContext.ApplicationUsers.Update(user);

            if (await _databaseContext.SaveChangesAsync() <= 0)
            {
                return null;
            }

            return new IdentityUser
            {
                Email = user.Email,
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Role = role.Name,
                IsActive = user.IsActive,
            };
        }

        public async Task<IdentityUser> RegisterAsync(NewUser newUser)
        {
            var existedUser = await _databaseContext.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Email.Equals(newUser.Email));
            
            if(existedUser is not null)
            {
                throw new Exception("User with same email already exists.");
            }

            var role = await _databaseContext.ApplicationUserRoles.FirstOrDefaultAsync(r => r.Id == newUser.RoleId);

            if(role is null)
            {
                throw new Exception("Invalid role Id.");
            }

            var userShouldBeCreated = new ApplicationUser
            { 
                Id = Guid.NewGuid(),
                Name = newUser.Name,
                Surname = newUser.Surname,
                Password = this.EncryptPassword(newUser.Password),
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                ApplicationUserRoleId = role.Id,
                IsActive = true,
            };

            _databaseContext.ApplicationUsers.Add(userShouldBeCreated);

            if(await _databaseContext.SaveChangesAsync() <= 0)
            {
                return null;
            }

            return new IdentityUser
            {
                Email = userShouldBeCreated.Email,
                Id = userShouldBeCreated.Id,
                Name = userShouldBeCreated.Name,
                Surname = userShouldBeCreated.Surname,
                PhoneNumber = userShouldBeCreated.PhoneNumber,
                Role = role.Name,
                IsActive = userShouldBeCreated.IsActive,
            };
        }

        private string EncryptPassword(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            return GetHash(sha256Hash, password);
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
