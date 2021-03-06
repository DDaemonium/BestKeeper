namespace Server.DataAccess.Models
{
    public class Dish
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public bool IsAvailabel { get; set; }
        public decimal Cost { get; set; }
        public int GramWeight { get; set; }
        public Guid DishCategoryId { get; set; }
        public DishCategory DishCategory { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
