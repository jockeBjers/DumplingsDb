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

        }
    }
}
