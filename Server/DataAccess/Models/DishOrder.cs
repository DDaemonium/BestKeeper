namespace Server.DataAccess.Models
{
    public class DishOrder
    {
        public Guid DishId { get; set; }
        public Dish Dish { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }
        public DishOrderStatus DishOrderStatus { get; set; }
    }
}
