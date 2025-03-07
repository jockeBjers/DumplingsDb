using Microsoft.EntityFrameworkCore;
using publisherData;

namespace DumplingApi.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<OrderDto> CreateOrderAsync(OrderDto newOrder);
    Task<OrderDto?> UpdateOrderAsync(int id, OrderDto updatedOrder);
    Task<bool> DeleteOrderAsync(int id);
}
public class OrderItemDto
{
    public required string MenuItemName { get; set; }
    public int Quantity { get; set; }
}

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsCompleted { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal CustomerId { get; set; }
    public List<OrderItemDto>? Items { get; set; }
    public OrderCustomerDto? Customer { get; set; }
}

public class OrderCustomerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Telephone { get; set; }
}

public class OrderService : IOrderService
{
    private readonly PubContext dbContext;
    public OrderService(PubContext dbContext)
    {
        this.dbContext = dbContext;
    }



    // Get all orders
    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                IsCompleted = o.IsCompleted,
                TotalPrice = o.TotalPrice,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    MenuItemName = i.MenuItem.Name,
                    Quantity = i.Quantity
                }).ToList(),
                CustomerId = o.CustomerId,
                Customer = o.Customer == null ? null : new OrderCustomerDto
                {
                    Id = o.Customer.Id,
                    Name = o.Customer.Name,
                    Telephone = o.Customer.Telephone
                }
            }).ToListAsync();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await dbContext.Orders
            .Include(o => o.Customer)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            IsCompleted = order.IsCompleted,
            TotalPrice = order.TotalPrice,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemName = i.MenuItem.Name,
                Quantity = i.Quantity
            }).ToList(),
            Customer = order.Customer == null ? null : new OrderCustomerDto
            {
                Id = order.Customer.Id,
                Name = order.Customer.Name,
                Telephone = order.Customer.Telephone
            }
        };
    }


    // Create a new order
    public async Task<OrderDto> CreateOrderAsync(OrderDto newOrderDto)
    {
        var customer = await dbContext.Customers.FindAsync(newOrderDto.CustomerId);
        if (customer == null)
        {
            throw new InvalidOperationException("Customer with the provided ID does not exist.");
        }

        var order = new Order
        {
            OrderDate = newOrderDto.OrderDate,
            IsCompleted = newOrderDto.IsCompleted,
            TotalPrice = newOrderDto.TotalPrice,
            CustomerId = customer.Id, 
            Items = newOrderDto.Items!.Select(i => new OrderItem
            {
                MenuItemId = dbContext.MenuItems.FirstOrDefault(m => m.Name == i.MenuItemName)!.Id,
                Quantity = i.Quantity
            }).ToList()
        };

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            IsCompleted = order.IsCompleted,
            TotalPrice = order.TotalPrice,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemName = i.MenuItem.Name,
                Quantity = i.Quantity
            }).ToList(),
            CustomerId = order.CustomerId 
        };
    }

    // Update existing order
    public async Task<OrderDto?> UpdateOrderAsync(int id, OrderDto updatedOrderDto)
    {
        var order = await dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return null;

        order.OrderDate = updatedOrderDto.OrderDate;
        order.IsCompleted = updatedOrderDto.IsCompleted;
        order.TotalPrice = updatedOrderDto.TotalPrice;
        order.Items = updatedOrderDto.Items.Select(i => new OrderItem
        {
            MenuItemId = dbContext.MenuItems.FirstOrDefault(m => m.Name == i.MenuItemName)!.Id,
            Quantity = i.Quantity
        }).ToList();

        await dbContext.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            IsCompleted = order.IsCompleted,
            TotalPrice = order.TotalPrice,
            Items = order.Items.Select(i => new OrderItemDto
            {
                MenuItemName = i.MenuItem.Name,
                Quantity = i.Quantity
            }).ToList()
        };
    }

    // Delete an order
    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await dbContext.Orders.FindAsync(id);
        if (order == null) return false;

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
