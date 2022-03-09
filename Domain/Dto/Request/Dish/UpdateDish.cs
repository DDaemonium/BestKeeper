namespace Domain.Dto.Request.Dish
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateDish
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        [Required]
        [MaxLength(512)]
        public string Description { get; set; }

        [Required]
        [MaxLength(512)]
        public string Ingredients { get; set; }

        public bool IsAvailabel { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public int GramWeight { get; set; }

        [Required]
        public Guid DishCategoryId { get; set; }
    }
}
