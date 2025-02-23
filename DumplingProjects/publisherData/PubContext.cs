using Microsoft.EntityFrameworkCore;

namespace publisherData
{
    public class PubContext : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public PubContext(DbContextOptions<PubContext> options) : base(options) { }

        /// <summary>
        /// Configures the database connection, bra för att lära sigg men ska inte hardkodas i vanliga projekt, använd config fil
        /// </summary>
        /// <param name="optionsBuilder"></param>
       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=DESKTOP-V7OGRKD;Database=DumplingDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }*/
    }

    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; }
        public string Category { get; set; } 
    }

    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }  

        public Customer? Customer { get; set; }
        public bool IsCompleted { get; set; } = false;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }

        public MenuItem MenuItem { get; set; }
        public Order? Order { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public class Staff
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Role { get; set; }
    }
}
