namespace Domain.Dto.Request.Dish
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateCategory
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
