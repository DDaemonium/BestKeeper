namespace Server.Service.Dishes
{
    using Domain.Dto.Request.Dish;
    using Domain.Dto.Response.Dish;
    using Microsoft.EntityFrameworkCore;
    using Server.DataAccess;
    using Server.DataAccess.Models;

    public class CategoryService
    {
        private readonly DatabaseContext _databaseContext;

        public CategoryService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<CategoryInfo>> GetAll() => await _databaseContext.DishCategories.AsNoTracking()
            .Select(c => new CategoryInfo { Id = c.Id, Name = c.Name, Description = c.Description }).ToListAsync();

        public async Task<CategoryInfo> UpdateCategoryAsync(UpdateCategory updateCategory)
        {
            var category = await _databaseContext.DishCategories
                .FirstOrDefaultAsync(dc => dc.Id == updateCategory.Id);

            if(category is null)
            {
                return null;
            }

            category.Name = updateCategory.Name;
            category.Description = updateCategory.Description;
            _databaseContext.DishCategories.Update(category);
            await _databaseContext.SaveChangesAsync();

            return new CategoryInfo
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _databaseContext.DishCategories.Include(dc => dc.Dishes)
                .FirstOrDefaultAsync(dc => dc.Id == id);

            if(category is null)
            {
                return;
            }

            _databaseContext.Dishes.RemoveRange(category.Dishes);
            _databaseContext.DishCategories.Remove(category);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<CategoryInfo> AddCategoryAsync(NewCategory newCategory)
        {
            var dishCategory = new DishCategory
            {
                Id = Guid.NewGuid(),
                Name = newCategory.Name,
                Description = newCategory.Description,
            };

            _databaseContext.DishCategories.Add(dishCategory);
            await _databaseContext.SaveChangesAsync();

            return new CategoryInfo
            {
                Id = dishCategory.Id,
                Name = dishCategory.Name,
                Description = dishCategory.Description,
            };
        }
    }
}
