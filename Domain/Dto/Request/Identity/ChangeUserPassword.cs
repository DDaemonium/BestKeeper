namespace Domain.Dto.Request.Identity
{
    using System.ComponentModel.DataAnnotations;

    public class ChangeUserPassword
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(256)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(256)]
        public string NewPassword { get; set; }
    }
}
