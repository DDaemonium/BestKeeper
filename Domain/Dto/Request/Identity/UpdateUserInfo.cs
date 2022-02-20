namespace Domain.Dto.Request.Identity
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdateUserInfo
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(256)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(256)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
