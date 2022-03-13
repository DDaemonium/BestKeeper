namespace Domain.Dto.Request.Order
{
    using System;

    public class NewDishOrder
    {
        public Guid DishId { get; set; }
        public int Count { get; set; }
    }
}
