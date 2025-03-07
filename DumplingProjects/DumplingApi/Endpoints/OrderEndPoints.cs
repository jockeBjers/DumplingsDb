using DumplingApi.Services;
using DumplingApi.Validators;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Endpoints;

public class OrderEndPoints
{
    public static void Map(WebApplication app)
    {
        // Get all orders
        app.MapGet("/api/orders", async ([FromServices] IOrderService orderService) =>
        {
            var orders = await orderService.GetAllOrdersAsync();
            return Results.Ok(orders);
        });

        // Get order by ID
        app.MapGet("/api/orders/{id}", async (int id, [FromServices] IOrderService orderService) =>
        {
            var order = await orderService.GetOrderByIdAsync(id);
            return order is not null ? Results.Ok(order) : Results.NotFound();
        });

        // Create new order
        app.MapPost("/api/orders", async ([FromBody] OrderDto newOrder, [FromServices] IOrderService orderService) =>
        {
            var validator = new OrderDtoValidator();
            var result = await validator.ValidateAsync(newOrder);

            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors.Select(x => new
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage
                }));
            }


            var createdOrder = await orderService.CreateOrderAsync(newOrder);
            return Results.Created($"/api/orders/{createdOrder.Id}", createdOrder);
        });

        // Update order
        app.MapPut("/api/orders/{id}", async (int id, [FromBody] OrderDto updatedOrder, [FromServices] IOrderService orderService) =>
        {
            var validator = new OrderDtoValidator();
            var result = await validator.ValidateAsync(updatedOrder);

            if (!result.IsValid)
            {
                return Results.BadRequest(result.Errors.Select(x => new
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage
                }));
            }

            var updated = await orderService.UpdateOrderAsync(id, updatedOrder);
            return updated is not null ? Results.Ok(updated) : Results.NotFound();
        });

        // Delete order
        app.MapDelete("/api/orders/{id}", async (int id, [FromServices] IOrderService orderService) =>
        {
            var deleted = await orderService.DeleteOrderAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }

}
