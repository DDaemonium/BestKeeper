namespace WebApplication.Data.Identity
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;

    public class Identity
    {
        public Identity(JwtSecurityToken jwtSecurityToken)
        {
            Id = Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value);
            Role = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            FullName = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        }

        public Guid Id { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
