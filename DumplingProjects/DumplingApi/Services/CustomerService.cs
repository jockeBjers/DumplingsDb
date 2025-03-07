using publisherData;
using Microsoft.EntityFrameworkCore;

namespace DumplingApi.Services;

public interface ICustomerService
{
    Task<List<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerWithOrdersDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CustomerDto newCustomer);
    Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto updatedCustomer);
    Task<bool> DeleteCustomerAsync(int id);
}

// DTO's to handle circular references
public class CustomerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Telephone { get; set; }
}
public class CustomerOrderItemDto
{
    public string? MenuItemName { get; set; }
    public int Quantity { get; set; }
}
public class CustomerOrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsCompleted { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CustomerOrderItemDto>? Items { get; set; }
}

public class CustomerWithOrdersDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Telephone { get; set; }
    public List<CustomerOrderDto>? Orders { get; set; }
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
            Orders = customer.Orders.Select(o => new CustomerOrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                IsCompleted = o.IsCompleted,
                TotalPrice = o.TotalPrice,
                Items = o.Items.Select(i => new CustomerOrderItemDto
                {
                    MenuItemName = i.MenuItem.Name,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList()
        };
    }

    //create new customer
    public async Task<CustomerDto> CreateCustomerAsync(CustomerDto newCustomerDto)
    {
        var customer = new Customer
        {
            Name = newCustomerDto.Name,
            Telephone = newCustomerDto.Telephone
        };

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Telephone = customer.Telephone
        };
    }

    // Update existing customer
    public async Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto updatedCustomerDto)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null) return null;

        customer.Name = updatedCustomerDto.Name;
        customer.Telephone = updatedCustomerDto.Telephone;

        await dbContext.SaveChangesAsync();

        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Telephone = customer.Telephone
        };
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
