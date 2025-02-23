using publisherData;
using Microsoft.EntityFrameworkCore;

namespace DumplingApi.Services;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerWithOrdersDto?> GetCustomerByIdAsync(int id);
    Task<Customer> CreateCustomerAsync(Customer newCustomer);
    Task<Customer?> UpdateCustomerAsync(int id, Customer updatedCustomer);
    Task<bool> DeleteCustomerAsync(int id);
}

// DTO's to handle circular references
public class CustomerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Telephone { get; set; }
}
public class OrderItemDto
{
    public string? MenuItemName { get; set; }
    public int Quantity { get; set; }
}
public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsCompleted { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItemDto>? Items { get; set; }
}
public class CustomerWithOrdersDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Telephone { get; set; }
    public List<OrderDto>? Orders { get; set; }
}

public class CustomerService : ICustomerService
{
    private readonly PubContext dbContext;
    public CustomerService(PubContext dbContext)
    {
        this.dbContext = dbContext;
    }

    //get all customers
    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        return await dbContext.Customers
        .Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Telephone = c.Telephone
        })
        .ToListAsync();
    }

    //get customer by id
    public async Task<CustomerWithOrdersDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await dbContext.Customers
            .Where(c => c.Id == customerId)
            .Include(c => c.Orders)
                .ThenInclude(o => o.Items)
                    .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync();

        if (customer == null) return null;

        return new CustomerWithOrdersDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Telephone = customer.Telephone,
            Orders = customer.Orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                IsCompleted = o.IsCompleted,
                TotalPrice = o.TotalPrice,
                Items = o.Items.Select(i => new OrderItemDto
                {
                    MenuItemName = i.MenuItem.Name,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList()
        };
    }

    //create new customer
    public async Task<Customer> CreateCustomerAsync(Customer newCustomer)
    {
        dbContext.Customers.Add(newCustomer);
        await dbContext.SaveChangesAsync();
        return newCustomer;
    }

    //update customer
    public async Task<Customer?> UpdateCustomerAsync(int id, Customer updatedCustomer)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null) return null;
        customer.Name = updatedCustomer.Name;
        customer.Telephone = updatedCustomer.Telephone;
        await dbContext.SaveChangesAsync();
        return customer;
    }

    //delete customer
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null) return false;
        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
