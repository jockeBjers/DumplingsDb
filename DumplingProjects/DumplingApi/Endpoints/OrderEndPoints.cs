using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Endpoints;

public class OrderEndPoints
{

    public static void Map(WebApplication app)
    {
        // Get orders
        app.MapGet("/api/orders", async (PubContext dbContext) =>
        {
            return await dbContext.Orders
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.TotalPrice,
                    o.IsCompleted,
                    Customer = new { o.CustomerId, o.Customer!.Name },
                    Items = o.Items.Select(i => new
                    {
                        i.MenuItem.Name,
                        i.MenuItem.Price,
                        i.Quantity
                    })
                })
                .ToListAsync();
        });

        // get orders by id
        app.MapGet("/app/orders/{id}", async (int id, PubContext dbContext) =>
        {

            var order = await dbContext.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
                .FirstOrDefaultAsync();

            if (order == null) return Results.NotFound();

            return Results.Ok(new
            {
                order.Id,
                order.OrderDate,
                order.TotalPrice,
                order.IsCompleted,
                Customer = new
                {
                    order.Customer!.Id,
                    order.Customer.Name,
                    order.Customer.Telephone,
                },
                Items = order.Items.Select(i => new
                {
                    i.MenuItem,
                    i.Quantity,
                })

            });
        });




    }
}
