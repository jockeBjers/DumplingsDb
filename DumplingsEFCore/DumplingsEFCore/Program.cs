using publisherData;
namespace DumplingsEFCore
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using (PubContext context = new PubContext())
            {
                context.Database.EnsureCreated();
            }

        }
    }
}
