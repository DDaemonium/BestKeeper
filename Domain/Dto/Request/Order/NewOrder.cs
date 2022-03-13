namespace Domain.Dto.Request.Order
{
    using System;
    using System.Collections.Generic;

    public class NewOrder
    {
        public Guid TableId { get; set; }
        public List<NewDishOrder> DishOrders { get; set; } = new();
    }
}
