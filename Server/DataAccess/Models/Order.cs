namespace Server.DataAccess.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Opened { get; set; }
        public DateTime Closed { get; set; }
        public string CloseDescription { get; set; }
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid TableId { get; set; }
        public Table Table { get; set; }
        public ICollection<DishOrder> DishOrders { get; set; }
    }
}
