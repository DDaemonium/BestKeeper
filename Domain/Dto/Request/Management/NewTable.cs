namespace Domain.Dto.Request.Management
{
    using System.ComponentModel.DataAnnotations;

    public class NewTable
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
