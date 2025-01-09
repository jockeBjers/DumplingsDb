
using Microsoft.EntityFrameworkCore;

namespace publisherData
{
    public class PubContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Configures the database connection, bra för att lära sigg men ska inte hardkodas i vanliga projekt, använd config fil
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=DESKTOP-V7OGRKD;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }


    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Book> Books
        { get; set; } = new List<Book>();
    }

    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public DateOnly PublishDate { get; set; }
        public decimal BasePrice { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }


}
