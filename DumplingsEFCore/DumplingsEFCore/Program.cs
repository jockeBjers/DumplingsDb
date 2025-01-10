using publisherData;
namespace DumplingsEFCore
{
    internal class Program
    {

        static void Main(string[] args)
        {
            using PubContext context = new PubContext();

            context.Database.EnsureCreated(); // makes sure the db is created.

           /* AddDrinks();
            AddFood();
            AddStaff();*/
            GetDrinks();



            void AddDrinks()
            {
                var newDrinks = new List<MenuItem> {
                new MenuItem{ Name = "Coca Cola", Description = "Soda", Category = "Drink", Price = 4 },
                new MenuItem{ Name = "Fanta", Description = "Soda", Category = "Drink", Price = 4 },
                new MenuItem{ Name = "Coca Cola Zero", Description = "Soda", Category = "Drink", Price = 4 },
                new MenuItem{ Name = "Loka naturell", Description = "Soda", Category = "Drink", Price = 4 },
                new MenuItem{ Name = "Loka Citron", Description = "Soda", Category = "Drink", Price = 4 },
                new MenuItem{ Name = "Trocadero", Description = "Soda", Category = "Drink", Price = 4 }
                };
                context.MenuItems.AddRange(newDrinks);
                context.SaveChanges();
            }
            void AddFood()
            {
                var newFoodItems = new List<MenuItem> {
                new MenuItem{ Name = "Pork dumplings", Description = "Klassisk dumpling med fläsk", Category = "Food", Price = 4 },
                new MenuItem{ Name = "Chicken dumplings", Description = "Kyckling och thaibasilika", Category = "Food", Price = 4 },
                new MenuItem{ Name = "Shrimp dumplings", Description = "Dumplings med räka", Category = "Food", Price = 4 },
                new MenuItem{ Name = "mushroom dumplings", Description = "Vegetarianskt", Category = "Food", Price = 4 },
                new MenuItem{ Name = "spicy pork dumplings", Description = "För er som tycker om lite mer hetta", Category = "Food", Price = 4 },
                new MenuItem{ Name = "Trocadero", Description = "Soda", Category = "Food", Price = 4 }
                };
                context.MenuItems.AddRange(newFoodItems);
                context.SaveChanges();
            }

            void AddStaff()
            {
                var newStaff = new List<Staff>
                {
                    new Staff{Name = "Viktor Thörn", Telephone= "0701101010", Role ="Chef"},
                    new Staff{Name = "Jesper Wallentin", Telephone= "0701104455", Role ="Chef"},
                    new Staff{Name = "Joakim Bjerselius", Telephone= "0701104198", Role ="Manager"}
                };
            }

            void GetDrinks()
            {
                var drinks = context.MenuItems.Where(d => d.Category == "Drink");
                foreach (var drink in drinks)
                {
                    Console.WriteLine($"{drink.Name}, {drink.Description}, {drink.Category}, {drink.Price} kr");
                }
            }
        }
    }
}
