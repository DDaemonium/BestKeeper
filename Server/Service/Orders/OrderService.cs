namespace Server.Service.Management
{
    using Domain.Dto.Request.Order;
    using Domain.Dto.Response.Order;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess;
    using Server.DataAccess.Models;

    public class OrderService
    {
        private readonly DatabaseContext _databaseContext;

        public OrderService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<OrderInfo> GetOrderByIdAsync(Guid orderId)
        => await _databaseContext.Orders.AsNoTracking()
            .Where(o => o.Id == orderId)
            .Select(o => new OrderInfo
            {
                Id = o.Id,
                OwnerId = o.ApplicationUserId,
                OwnerFullName = $"{o.ApplicationUser.Name} {o.ApplicationUser.Surname}",
                Closed = o.Closed,
                Opened = o.Opened,
                TableId = o.TableId,
                TableName = o.Table.Name,
                Dishes = o.DishOrders
                .Select(d => new OrderDishInfo { Cost = d.Dish.Cost, Count = d.Count, Name = d.Dish.Name, DishId = d.DishId }).ToList(),
            }).FirstOrDefaultAsync();

        public async Task CloseOrderAsync(Guid orderId, string reason)
        {
            var order = await _databaseContext.Orders.Include(o => o.Table)
                .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order is null)
            {
                return;
            }

            order.Table.IsReserved = false;
            order.Closed = DateTime.Now;
            order.CloseDescription = reason;
            _databaseContext.Tables.Update(order.Table);
            _databaseContext.Orders.Update(order);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<OrderInfo>> GetOrdersAsync()
        => await _databaseContext.Orders.AsNoTracking()
            .Select(o => new OrderInfo
            {
                Id = o.Id,
                OwnerId = o.ApplicationUserId,
                OwnerFullName = $"{o.ApplicationUser.Name} {o.ApplicationUser.Surname}",
                Closed = o.Closed,
                Opened = o.Opened,
                TableId = o.TableId,
                TableName = o.Table.Name,
                Dishes = o.DishOrders
                .Select(d => new OrderDishInfo { Cost = d.Dish.Cost, Count = d.Count, Name = d.Dish.Name, DishId = d.DishId }).ToList(),
            }).ToListAsync();

        public async Task<List<OrderInfo>> GetOrdersOfCurrentUserAsync(Guid userId)
        => await _databaseContext.Orders.AsNoTracking()
            .Where(o => o.ApplicationUserId == userId)
            .Select(o => new OrderInfo
            {
                Id = o.Id,
                OwnerId = o.ApplicationUserId,
                OwnerFullName = $"{o.ApplicationUser.Name} {o.ApplicationUser.Surname}",
                Closed = o.Closed,
                Opened = o.Opened,
                TableId = o.TableId,
                TableName = o.Table.Name,
                Dishes = o.DishOrders
                .Select(d => new OrderDishInfo { Cost = d.Dish.Cost, Count = d.Count, Name = d.Dish.Name, DishId = d.DishId }).ToList(),
            }).ToListAsync();

        public async Task<List<OrderInfo>> GetOpenedOrdersOfCurrentUserAsync(Guid userId)
        => await _databaseContext.Orders.AsNoTracking()
            .Where(o => o.ApplicationUserId == userId && o.Closed == null)
            .Select(o => new OrderInfo
            {
                Id = o.Id,
                OwnerId = o.ApplicationUserId,
                OwnerFullName = $"{o.ApplicationUser.Name} {o.ApplicationUser.Surname}",
                Closed = o.Closed,
                Opened = o.Opened,
                TableId = o.TableId,
                TableName = o.Table.Name,
                Dishes = o.DishOrders
                .Select(d => new OrderDishInfo { Cost = d.Dish.Cost, Count = d.Count, Name = d.Dish.Name, DishId = d.DishId }).ToList(),
            }).ToListAsync();

        public async Task<OrderInfo> CreateOrderAsync(NewOrder newOrder, Guid applicationUserId)
        {
            var table = await _databaseContext.Tables
                .FirstOrDefaultAsync(t => t.Id == newOrder.TableId);
            if (table is null)
            {
                return null;
            }

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ApplicationUserId = applicationUserId,
                TableId = newOrder.TableId,
                Opened = DateTime.Now,
            };

            _databaseContext.Orders.Add(order);
            _databaseContext.DishOrders.AddRange(newOrder.DishOrders.Select(o => new DishOrder
            {
                OrderId = order.Id,
                DishOrderStatus = DishOrderStatus.Added,
                Count = o.Count,
                DishId = o.DishId,
            }));

            table.IsReserved = true;
            await _databaseContext.SaveChangesAsync();

            return await GetOrderByIdAsync(order.Id);
        }
    }
}
