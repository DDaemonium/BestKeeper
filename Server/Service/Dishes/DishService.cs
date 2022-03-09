namespace Server.Service.Dishes
{
    using Domain.Dto.Request.Dish;
    using Domain.Dto.Response.Dish;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess;
    using Server.DataAccess.Models;

    public class DishService
    {
        private readonly DatabaseContext _databaseContext;

        public DishService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<DishInfo> GetDishAsync(Guid dishId)
        => await _databaseContext.Dishes.AsNoTracking()
            .Where(d => d.Id == dishId)
            .Select(d => new DishInfo 
            { 
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Ingredients = d.Ingredients,
                IsAvailabel = d.IsAvailabel,
                Cost = d.Cost,
                GramWeight = d.GramWeight,
                DishCategoryId = d.DishCategoryId,
                DishCategoryName = d.DishCategory.Name,
            }).FirstOrDefaultAsync();

        public async Task DeleteDishAsync(Guid id)
        {
            var dish = await _databaseContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);
            if (dish is null)
            {
                return;
            }

            _databaseContext.Dishes.Remove(dish);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<DishInfo>> GetDishesAsync()
        => await _databaseContext.Dishes.AsNoTracking()
            .Select(d => new DishInfo 
            { 
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Ingredients = d.Ingredients,
                IsAvailabel = d.IsAvailabel,
                Cost = d.Cost,
                GramWeight = d.GramWeight,
                DishCategoryId = d.DishCategoryId,
                DishCategoryName = d.DishCategory.Name,
            }).ToListAsync();

        public async Task<DishInfo> UpdateDishAsync(UpdateDish updateDish)
        {
            var dish = await _databaseContext.Dishes.FirstOrDefaultAsync(d => d.Id == updateDish.Id);
            if (dish is null)
            {
                return null;
            }

            var category = await _databaseContext.DishCategories.FirstOrDefaultAsync(dc => dc.Id == updateDish.DishCategoryId);
            if (category is null)
            {
                return null;
            }

            dish.Name = updateDish.Name;
            dish.Description = updateDish.Description;
            dish.Cost = updateDish.Cost;
            dish.GramWeight = updateDish.GramWeight;
            dish.Ingredients = updateDish.Ingredients;
            dish.DishCategoryId = updateDish.DishCategoryId;
            dish.IsAvailabel = updateDish.IsAvailabel;

            _databaseContext.Dishes.Update(dish);
            await _databaseContext.SaveChangesAsync();

            return new DishInfo
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Ingredients = dish.Ingredients,
                IsAvailabel = dish.IsAvailabel,
                Cost = dish.Cost,
                GramWeight = dish.GramWeight,
                DishCategoryId = dish.DishCategoryId,
                DishCategoryName = dish.DishCategory.Name,
            };
        }

        public async Task<DishInfo> AddDishAsync(NewDish newDIsh)
        {
            var dish = new Dish
            {
                Id = Guid.NewGuid(),
                Name = newDIsh.Name,
                Description = newDIsh.Description,
                Cost = newDIsh.Cost,
                IsAvailabel = newDIsh.IsAvailabel,
                DishCategoryId = newDIsh.DishCategoryId,
                GramWeight = newDIsh.GramWeight,
                Ingredients = newDIsh.Ingredients,
            };

            _databaseContext.Dishes.Add(dish);
            await _databaseContext.SaveChangesAsync();

            var categoryName = _databaseContext.DishCategories.First(dc => dc.Id == dish.DishCategoryId).Name;

            return new DishInfo
            {
                Id = dish.Id,
                Name = dish.Name,
                Description = dish.Description,
                Ingredients = dish.Ingredients,
                IsAvailabel = dish.IsAvailabel,
                Cost = dish.Cost,
                GramWeight = dish.GramWeight,
                DishCategoryId = dish.DishCategoryId,
                DishCategoryName = categoryName,
            };
        }
    }
}
