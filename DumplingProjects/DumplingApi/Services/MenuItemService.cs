using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Services;
public interface IMenuItemService
{
    Task<List<MenuItem>> GetAllMenuItemsAsync();
    Task<MenuItem?> GetMenuItemByIdAsync(int id);
    Task<List<MenuItem>> GetMenuItemsByCategoryAsync(string category);
    Task<MenuItem> CreateMenuItemAsync(MenuItem newMenuItem);
    Task<MenuItem?> UpdateMenuItemAsync(int id, MenuItem updatedMenuItem);
    Task<bool> DeleteMenuItemAsync(int id);
}

public class MenuItemService : IMenuItemService
{
    private readonly PubContext dbContext;
    public MenuItemService(PubContext dbContext)
    {
        this.dbContext = dbContext;
    }

    //get all items
    public async Task<List<MenuItem>> GetAllMenuItemsAsync()
    {
        return await dbContext.MenuItems.ToListAsync();
    }

    //get item by id
    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        return await dbContext.MenuItems.FindAsync(id);
    }

    //get item by category
    public async Task<List<MenuItem>> GetMenuItemsByCategoryAsync(string category)
    {
        return await dbContext.MenuItems
            .Where(x => x.Category.ToLower() == category.ToLower())
            .ToListAsync();
    }

    //create new item
    public async Task<MenuItem> CreateMenuItemAsync(MenuItem newMenuItem)
    {
        dbContext.MenuItems.Add(newMenuItem);
        await dbContext.SaveChangesAsync();
        return newMenuItem;
    }

    //update item
    public async Task<MenuItem?> UpdateMenuItemAsync(int id, MenuItem updatedMenuItem)
    {
        var menuItem = await dbContext.MenuItems.FindAsync(id);
        if (menuItem == null) return null;
        menuItem.Name = updatedMenuItem.Name;
        menuItem.Description = updatedMenuItem.Description;
        menuItem.Price = updatedMenuItem.Price;
        menuItem.Category = updatedMenuItem.Category;
        await dbContext.SaveChangesAsync();
        return menuItem;
    }

    //delete item
    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        var menuItem = await dbContext.MenuItems.FindAsync(id);
        if (menuItem == null) return false;
        dbContext.MenuItems.Remove(menuItem);
        await dbContext.SaveChangesAsync();
        return true;
    }



}
