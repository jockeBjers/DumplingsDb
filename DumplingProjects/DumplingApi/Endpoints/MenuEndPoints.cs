using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Endpoints
{
    public static class MenuEndPoints
    {

        public static void Map(WebApplication app)
        {
            // GET all MenuItems
            app.MapGet("/api/menuitems", async (PubContext dbContext) =>
            {
                return await dbContext.MenuItems.ToListAsync();
            })
            .WithName("GetMenuItems")
            .WithOpenApi();

            // Get menu items by category
            app.MapGet("/api/menuitems/category/{category}", async (string category, PubContext dbContext) =>
            {
                var menuItems = await dbContext.MenuItems
                    .Where(x => x.Category.ToLower() == category.ToLower())
                    .ToListAsync();
                return Results.Ok(menuItems);
            })
            .WithName("GetMenuItemsByCategory")
            .WithOpenApi();

            // GET single MenuItem by ID
            app.MapGet("/api/menuitems/{id}", async (int id, PubContext dbContext) =>
            {
                var menuItem = await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
                return menuItem is not null ? Results.Ok(menuItem) : Results.NotFound();
            })
            .WithName("GetMenuItem")
            .WithOpenApi();

            // POST new MenuItem
            app.MapPost("/api/menuitems", async (MenuItem menuItem, PubContext dbContext) =>
            {
                dbContext.MenuItems.Add(menuItem);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/menuitems/{menuItem.Id}", menuItem);
            });

            // Update MenuItem
            app.MapPut("/api/menuitems/update/{id}", async (int id, MenuItem menuItem, PubContext dbContext) =>
            {
                var existingMenuItem = await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
                if (existingMenuItem is null) return Results.NotFound();

                existingMenuItem.Name = menuItem.Name;
                existingMenuItem.Description = menuItem.Description;
                existingMenuItem.Price = menuItem.Price;
                existingMenuItem.Category = menuItem.Category;

                await dbContext.SaveChangesAsync();
                return Results.Ok(existingMenuItem);
            });

            // DELETE MenuItem
            app.MapDelete("/api/menuitems/delete/{id}", async (int id, PubContext dbContext) =>
            {
                var menuItem = await dbContext.MenuItems.FirstOrDefaultAsync(x => x.Id == id);
                if (menuItem is null) return Results.NotFound();

                dbContext.MenuItems.Remove(menuItem);
                await dbContext.SaveChangesAsync();
                return Results.Ok();
            });

        }
    }
}
