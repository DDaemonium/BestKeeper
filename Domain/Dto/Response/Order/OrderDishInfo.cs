namespace Domain.Dto.Response.Order
{
    using System;

    public class OrderDishInfo
    {
        public Guid DishId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Cost { get; set; }
    }
}
