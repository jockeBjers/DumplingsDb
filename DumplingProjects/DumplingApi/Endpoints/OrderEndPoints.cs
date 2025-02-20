using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Endpoints;

public class OrderEndPoints
{

    public static void Map(WebApplication app)
    {

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
                        i.MenuItem,
                        i.Quantity
                    })
                })
                .ToListAsync();
        });






    }
}
