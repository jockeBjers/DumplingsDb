using Microsoft.EntityFrameworkCore;
using publisherData;
using System.Threading.Tasks;

namespace DumplingApi.Endpoints;

public class CustomerEndPoints
{
    public static void Map(WebApplication app)
    {

        // Get all customers
        app.MapGet("/api/customers", async (PubContext dbContext) =>
        {
            var customers = await dbContext.Customers.Select(c => new
            {
                c.Id,
                c.Name,
                c.Telephone,
            }).ToListAsync();
            return Results.Ok(customers);
        });

        // Get single customer by ID
        app.MapGet("/api/customers/{id}/orders", async (int id, PubContext dbContext) =>
        {
            var customer = await dbContext.Customers
             .Where(c => c.Id == id)
             .Include(c => c.Orders)
             .ThenInclude(o => o.Items)
             .ThenInclude(i => i.MenuItem)
             .FirstOrDefaultAsync();

            if (customer == null) return Results.NotFound();

            return Results.Ok(new
            {
                customer.Id,
                customer.Name,
                customer.Telephone,
                Orders = customer.Orders.Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.IsCompleted,
                    o.TotalPrice,
                    Items = o.Items.Select(i => new
                    {
                        MenuItemName = i.MenuItem.Name,
                        i.Quantity
                    })
                })
            });
        });

        // Post new Customer
        app.MapPost("/api/customers", async (Customer newCustomer, PubContext dbContext) =>
        {
            dbContext.Customers.Add(newCustomer);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/customers/{newCustomer.Id}", newCustomer);

        });

        //update customer
        app.MapPut("/api/customers/{id}", async (int id, Customer customer, PubContext dbContext) =>
        {
            var existingCustomer = await dbContext.Customers.FindAsync(id);
            if (existingCustomer == null) return Results.NotFound();

            existingCustomer.Name = customer.Name;
            existingCustomer.Telephone = customer.Telephone;

            await dbContext.SaveChangesAsync();
            return Results.Ok(existingCustomer);

        });

        // Delete customer
        app.MapDelete("/api/customer/{id}", async (int id, PubContext dbContext) =>
        {
            var existingCustomer = await dbContext.Customers.FindAsync(id);
            if (existingCustomer == null) return Results.NotFound();

            dbContext.Customers.Remove(existingCustomer);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        });
    }
}