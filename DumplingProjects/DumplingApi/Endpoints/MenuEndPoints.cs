using DumplingApi.Services;
using DumplingApi.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Endpoints
{
    public static class MenuEndPoints
    {

        public static void Map(WebApplication app)
        {
            // get all menu items
            app.MapGet("/api/menuitems", async (IMenuItemService service) =>
            {
                return await service.GetAllMenuItemsAsync();
            });

            // Get menu items by category
            app.MapGet("/api/menuitems/category/{category}", async (string category, IMenuItemService service) =>
            {
                return await service.GetMenuItemsByCategoryAsync(category);
            });

            // GET single MenuItem by ID
            app.MapGet("/api/menuitems/{id}", async (int id, IMenuItemService service) =>
            {
                var menuItem = await service.GetMenuItemByIdAsync(id);
                if (menuItem is null) return Results.NotFound();
                return Results.Ok(menuItem);
            });

            // POST new MenuItem
            app.MapPost("/api/menuitems", async (MenuItem menuItem, IMenuItemService service) =>
            {
                var validator = new MenuItemValidator();
                var result = validator.Validate(menuItem);

                if (!result.IsValid)
                {
                    return Results.BadRequest(result.Errors.Select(x => new
                    {
                        Field = x.PropertyName,
                        Message = x.ErrorMessage
                    }));
                }

                var newMenuItem = await service.CreateMenuItemAsync(menuItem);
                return Results.Created($"/api/menuitems/{newMenuItem.Id}", newMenuItem);
            });

            // Update MenuItem
            app.MapPut("/api/menuitems/update/{id}", async (int id, MenuItem menuItem, IMenuItemService service) =>
            {
                var validator = new MenuItemValidator();
                var result = validator.Validate(menuItem);

                if (!result.IsValid)
                {
                    return Results.BadRequest(result.Errors.Select(x => new
                    {
                        Field = x.PropertyName,
                        Message = x.ErrorMessage
                    }));
                }

                var existingMenuItem = await service.UpdateMenuItemAsync(id, menuItem);
                if (existingMenuItem is null) return Results.NotFound();
                return Results.Ok(existingMenuItem);
            });

            // DELETE MenuItem
            app.MapDelete("/api/menuitems/delete/{id}", async (int id, IMenuItemService service) =>
            {
                var menuItem = await service.DeleteMenuItemAsync(id);
                if (!menuItem) return Results.NotFound();
                return Results.Ok();
            });

        }
    }
}
