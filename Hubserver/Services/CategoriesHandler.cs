
using System.Text.Json;
using SharedModels.Models;
using HubServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
namespace HubServer.Services
{
    public interface ICategoriesHandler
    {   
        public Task<List<CategoryResponseDto>> GetCategories();
        public Task<CategoryResponseDto?> GetCategoryById(int id);
        public  Task<bool> addCategory(CategoryRequestDto newCategory);
        public  Task<bool> RemoveCategory(int id);
        public  Task<bool> UpdateCategory(int Id, CategoryRequestDto updatedCategory);
        
    }
    public class CategoriesHandler : ICategoriesHandler
    {   
        public CategoriesHandler(InventoryHubDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private InventoryHubDbContext _DbContext;
        public async Task<CategoryResponseDto?> GetCategoryById(int id)
        {
            var Category = await _DbContext.Categories.Include(e => e.Products)
            .FirstOrDefaultAsync(e => e.CategoryID == id);
            if( Category == null)
            {
                return null;
            }
          
            return await  Category.ToResponseAsync();
        }
        public async Task<bool> addCategory(CategoryRequestDto newCategory)
        {   
            var isAdded = false;
            if(newCategory == null)
            {
               return isAdded;
            }
            var dbCategory = await newCategory.ToCategoryAsync();
           _DbContext.Add(dbCategory);
           await _DbContext.SaveChangesAsync();
           isAdded = true;
           return isAdded;
        }

        public async Task<bool> RemoveCategory(int id)
        {  
            var isRemoved = false;
            var Category = await _DbContext.FindAsync<Category>(id);
            if (Category == null)
            {
               return isRemoved;
            }
            _DbContext.Remove(Category);
            await _DbContext.SaveChangesAsync();
            isRemoved = true;
            return isRemoved;
    
        }
        public async Task<bool> UpdateCategory(int id, CategoryRequestDto updatedCategory)
        {
            var isUpdated = false;
            var trackedCategory = await _DbContext.FindAsync<Category>(id);
            if (trackedCategory == null)
            {
               return isUpdated;
            }
            trackedCategory.Name = updatedCategory.Name;
            await _DbContext.SaveChangesAsync();
            isUpdated = true;
            return isUpdated;
        }
        public async Task<List<CategoryResponseDto>> GetCategories()
        {
            var categories = await _DbContext.Categories.Include( e => e.Products).ToListAsync();
            var categoriesResponse = await Task.WhenAll(categories.Select(async e => await e.ToResponseAsync()));
            return categoriesResponse.ToList();
                
        }
      
    }
}