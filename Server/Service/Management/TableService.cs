namespace Server.Service.Management
{
    using Domain.Dto.Request.Management;
    using Domain.Dto.Response.Management;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess;
    using Server.DataAccess.Models;

    public class TableService
    {
        private readonly DatabaseContext _databaseContext;

        public TableService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<TableInfo> GetTableAsync(Guid tableId)
        => await _databaseContext.Tables.AsNoTracking()
            .Where(t => t.Id == tableId)
            .Select(t => new TableInfo
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsReserved = t.IsReserved,
            }).FirstOrDefaultAsync();

        public async Task DeleteTableAsync(Guid id)
        {
            var table = await _databaseContext.Tables.Include(t => t.Orders).ThenInclude(o => o.DishOrders)
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsReserved);
            if (table is null)
            {
                return;
            }

            _databaseContext.DishOrders.RemoveRange(table.Orders.SelectMany(o => o.DishOrders));
            _databaseContext.Orders.RemoveRange(table.Orders);
            _databaseContext.Tables.Remove(table);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<TableInfo>> GetTablesAsync()
        => await _databaseContext.Tables.AsNoTracking()
            .Select(t => new TableInfo
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                IsReserved = t.IsReserved,
            }).ToListAsync();

        public async Task<TableInfo> UpdateTableAsync(UpdateTable updateTable)
        {
            var table = await _databaseContext.Tables.FirstOrDefaultAsync(t => t.Id == updateTable.Id);
            if (table is null)
            {
                return null;
            }

            table.Name = updateTable.Name;
            table.Description = updateTable.Description;
            table.IsReserved = updateTable.IsReserved;

            // close related order
            if(!updateTable.IsReserved)
            {
                var openedOrder = await _databaseContext.Orders.FirstOrDefaultAsync(o => o.TableId == updateTable.Id && o.Closed != null);
                if (openedOrder != null)
                {
                    openedOrder.Closed = DateTime.Now;
                    openedOrder.CloseDescription = "Closed by manager.";
                    _databaseContext.Orders.Update(openedOrder);
                }
            }

            _databaseContext.Tables.Update(table);
            await _databaseContext.SaveChangesAsync();

            return new TableInfo
            {
                Id = table.Id,
                Name = table.Name,
                Description = table.Description,
                IsReserved = table.IsReserved,
            };
        }

        public async Task<bool> ChangeTableReservation(Guid tableId, bool isReserved)
        {
            var table = await _databaseContext.Tables.FirstOrDefaultAsync(t => t.Id == tableId);
            if (table is null)
            {
                throw new ArgumentException("Ivalid table id.");
            }

            if(table.IsReserved == isReserved)
            {
                return table.IsReserved;
            }

            table.IsReserved = isReserved;

            // close related order
            if (!table.IsReserved)
            {
                var openedOrder = await _databaseContext.Orders.FirstOrDefaultAsync(o => o.TableId == table.Id && o.Closed != null);
                if (openedOrder != null)
                {
                    openedOrder.Closed = DateTime.Now;
                    openedOrder.CloseDescription = "Closed by manager.";
                    _databaseContext.Orders.Update(openedOrder);
                }
            }
            
            _databaseContext.Tables.Update(table);
            await _databaseContext.SaveChangesAsync();

            return table.IsReserved;
        }

        public async Task<TableInfo> AddTableAsync(NewTable newTable)
        {
            var table = new Table
            {
                Id = Guid.NewGuid(),
                Name = newTable.Name,
                Description = newTable.Description,
                IsReserved = true,
            };

            _databaseContext.Tables.Add(table);
            await _databaseContext.SaveChangesAsync();

            return new TableInfo
            {
                Id = table.Id,
                Name = table.Name,
                Description = table.Description,
                IsReserved = table.IsReserved,
            };
        }
    }
}
