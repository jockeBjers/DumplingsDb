using DumplingApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using publisherData;
using System.Threading.Tasks;

namespace DumplingApi.Endpoints;

public class CustomerEndPoints
{
    public static void Map(WebApplication app)
    {

        // Get all customers
        app.MapGet("/api/customers", async (ICustomerService service) =>
        {
            var customers = await service.GetAllCustomersAsync();
            return Results.Ok(customers);
        });


        // Get single customer by ID
        app.MapGet("/api/customers/{id}/orders", async (int id, ICustomerService service) =>
        {
            var customerDto = await service.GetCustomerByIdAsync(id);
            if (customerDto == null) return Results.NotFound();
            return Results.Ok(customerDto);
        });

        // Post new Customer
        app.MapPost("/api/customers", async (Customer newCustomer, ICustomerService service) =>
        {
            var customer = await service.CreateCustomerAsync(newCustomer);
            return Results.Created($"/api/customers/{customer.Id}", customer);
        });

        //update customer
        app.MapPut("/api/customers/{id}", async (int id, Customer customer, ICustomerService service) =>
        {
            var existingCustomer = await service.UpdateCustomerAsync(id, customer);
            if (existingCustomer == null) return Results.NotFound();
            return Results.Ok(existingCustomer);
        });

        // Delete customer
        app.MapDelete("/api/customer/{id}", async (int id, ICustomerService service) =>
        {
            var existingCustomer = await service.DeleteCustomerAsync(id);
            if (!existingCustomer) return Results.NotFound();
            return Results.Ok();
        });
    }
}