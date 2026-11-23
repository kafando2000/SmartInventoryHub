using Microsoft.AspNetCore.SignalR;
using SharedModels.Models;
using HubServer;
using Microsoft.EntityFrameworkCore;
namespace HubServer.Hubs{
public class CategoryHub : Hub
{       
        private InventoryHubDbContext _DbContext;
        public CategoryHub( InventoryHubDbContext categoryHandler)
        {
            _DbContext = categoryHandler;
        }
        public async Task PopulateCategories(List<CategoryResponseDto> categoriesResponse)
        {
            var CategoriesTasks = await _DbContext.Categories.ToListAsync();
            var categories = await Task.WhenAll(CategoriesTasks.Select(e => e.ToResponseAsync()));
            categoriesResponse = categories.ToList();
            await Clients.All.SendAsync("ReceiveCategories", categoriesResponse);
        }
    }
}