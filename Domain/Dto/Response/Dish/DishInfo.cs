namespace Domain.Dto.Response.Dish
{
    public class DishInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public bool IsAvailabel { get; set; }
        public decimal Cost { get; set; }
        public int GramWeight { get; set; }
        public Guid DishCategoryId { get; set; }
        public string DishCategoryName { get; set; }
    }
}
