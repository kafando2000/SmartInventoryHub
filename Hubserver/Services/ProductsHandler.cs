
using System.Text.Json;
using SharedModels.Models;
using HubServer;
using Microsoft.EntityFrameworkCore;
namespace HubServer.Services
{
    public interface IProductsHandler
    {   
        public Task<List<ProductResponseDto>> GetProducts();
        public Task<ProductResponseDto?> GetProductById(int id);
        public  Task<bool> addProduct(ProductRequestDto newProduct);
        public  Task<bool> RemoveProduct(int id);
        public  Task<bool> UpdateProduct(int Id, ProductRequestDto updatedProduct);
        
    }
    public class ProductsHandler : IProductsHandler
    {   
        public ProductsHandler(InventoryHubDbContext dbContext)
        {
            _ServiceId = new Random().Next(201, 2000);
            _DbContext = dbContext;
        }
        private readonly int _ServiceId;
        private InventoryHubDbContext _DbContext;
        public async Task<ProductResponseDto?> GetProductById(int id)
        {
            var Product = await _DbContext.Products.Include(e => e.Category).FirstOrDefaultAsync(e => e.ProductID == id);
            if( Product == null)
            {
                return null;
            }
          
            return   Product.ToResponse();
        }
        public async Task<bool> addProduct(ProductRequestDto newProduct)
        {   bool isAdded = false;
            if(newProduct == null)
            {
               return isAdded;
            }
            var category = await _DbContext.Categories.FindAsync(newProduct.CategoryID);
            if(category == null)
            {
               return isAdded;
            }
            var dbProduct =  newProduct.ToProduct();
           _DbContext.Add(dbProduct);
           await _DbContext.SaveChangesAsync();
           isAdded = true;
           return isAdded;
        }

        public async Task<bool> RemoveProduct(int id)
        {  
            bool isRemoved = false;
            var Product = await _DbContext.FindAsync<Product>(id);
            if (Product == null)
            {
               return isRemoved;
            }
            _DbContext.Remove(Product);
            await _DbContext.SaveChangesAsync();
            isRemoved = true;
            return isRemoved;
        }
        public async Task<bool> UpdateProduct(int id, ProductRequestDto updatedProduct)
        {   
            bool isUpdated = false;
            var trackedProduct = await _DbContext.Products.Include(e => e.Category)
                                                            .FirstOrDefaultAsync( e => e.ProductID == id);
            if (trackedProduct == null)
            {
               return isUpdated;
            }
            var category = await _DbContext.Categories.FindAsync(updatedProduct.CategoryID);
            if(category == null)
            {
               return isUpdated;
            }
            trackedProduct.Name = updatedProduct.Name;
            trackedProduct.Description = updatedProduct.Description;
            trackedProduct.Name = updatedProduct.Name;
            trackedProduct.Price = updatedProduct.Price;
            trackedProduct.Quantity = updatedProduct.Quantity;
            await _DbContext.SaveChangesAsync();
            isUpdated = true;
            return isUpdated;
        }
        public async Task<List<ProductResponseDto>> GetProducts()
        {
            return await _DbContext.Products.Include(e => e.Category).Select(e => e.ToResponse()).ToListAsync();
                
        }
      
    }
}