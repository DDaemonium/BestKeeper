namespace Domain.Dto.Request.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class LoginInfo
    {
        [Required]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }
    }
}
