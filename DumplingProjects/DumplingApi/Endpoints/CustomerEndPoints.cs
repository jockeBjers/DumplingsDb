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
            return await dbContext.Customers.ToListAsync();

        });

        // Get single customer by ID
        app.MapGet("/api/customers/{id}", async (int id, PubContext dbContext) =>
        {
            var customer = await dbContext.Customers.FindAsync(id);
            if (customer == null) return Results.NotFound();

            return Results.Ok(customer);
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