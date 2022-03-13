namespace Domain.Dto.Response.Order
{
    using System;

    public class OrderInfo
    {
        public Guid Id { get; set; }
        public Guid TableId { get; set; }
        public string TableName { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerFullName { get; set; }
        public DateTime Opened { get; set; }
        public DateTime? Closed { get; set; }
        public List<OrderDishInfo> Dishes { get; set; }
    }
}
