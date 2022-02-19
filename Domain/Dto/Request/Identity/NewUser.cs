namespace Domain.Dto.Request.Identity
{
    using System;

    public class NewUser
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
    }
}
