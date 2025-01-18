using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using publisherData;
namespace DumplingsEFCore
{
    public class MenuManager
    {

        private readonly PubContext context;

        public MenuManager(PubContext _context)
        {
            context = _context;
        }

        public void StartMenu()
        {
            bool exit = false;
            while (!exit)
            {

                string choice = InputHelper.GetUserInput<string>(
                    "Välkommen, här kan du hantera din meny\n" +
                    "1: Printa ut alla maträtter/dryck\n" +
                    "2: Lägg till maträtt/dryck\n" +
                    "3: Uppdatera maträtt/dryck\n" +
                    "4: Ta bort maträtt/dryck\n" +
                    "5: Gå tillbaka till menyn\n" +
                    "6: Avsluta program"
                    );

                switch (choice)
                {
                    case "1":
                        PrintMenuItems();
                        break;
                    case "2":
                        AddMenuItem();
                        break;
                    case "3":
                        UpdateMenuItem();
                        break;
                    case "4":
                        DeleteMenuItem();
                        break;
                    case "5":
                        exit = true;
                        break;
                    case "6":
                        Program.CloseProgram();
                        break;
                    default:
                        Console.WriteLine("Ogiltig input försök igen");
                        break;

                }
            }
        }

        public void PrintMenuItems()
        {
            /* Printing out menu items, filtering by category food/drinks */
            var foodItems = context.MenuItems
                .Where(d => d.Category == "Food")
                .OrderBy(d => d.Name)
                .ToList();
            Console.WriteLine("Maträtter:\n");
            foreach (var Item in foodItems)
            {
                Console.WriteLine($"Namn: {Item.Name}, Beskrivning: {Item.Description}, Kategori: {Item.Category}, Pris: {Item.Price}");
            }

            var drinkItems = context.MenuItems
                .Where(d => d.Category == "Drink")
                .OrderBy(d => d.Name)
                .ToList();
            Console.WriteLine("Drycker:\n");
            foreach (var Item in drinkItems)


            {
                Console.WriteLine($"Namn: {Item.Name}, Beskrivning: {Item.Description}, Kategori: {Item.Category}, Pris: {Item.Price}");
            }
        }

        public void AddMenuItem()
        {
            string name = InputHelper.GetUserInput<string>("Skriv in namnet på ny vara: ");
            string description = InputHelper.GetUserInput<string>("Beskrivning: ");
            string category;
            string choice = InputHelper.GetUserInput<string>("Välj kategori:\n1. Mat\n2. Dryck");

            if (choice == "1")
            {
                category = "Food";
            }
            else
            {
                category = "Drink";
            }

            decimal price = InputHelper.GetUserInput<decimal>("Skriv in priset: ");

            /* Add a new product */
            var newItem = new MenuItem
            {
                Name = name,
                Description = description,
                Category = category,
                Price = price
            };

            context.MenuItems.Add(newItem);
            context.SaveChanges();

            Console.WriteLine("Ny maträtt/dryck tillagd");
        }

        public void UpdateMenuItem()
        {
            var item = SearchMenuItem();

            if (item != null)
            {
                Console.WriteLine($"Uppdatera {item.Name}");
                item.Description = InputHelper.GetUserInput<string>("Beskrivning: ");
                item.Price = InputHelper.GetUserInput<decimal>("pris: ");
                context.SaveChanges();

            }
        }

        public void DeleteMenuItem()
        {
            var item = SearchMenuItem();

            if (item != null)
            {
                context.MenuItems.Remove(item);
                context.SaveChanges();
                Console.WriteLine($"Maträtten eller drycken '{item.Name}' har raderats.");
            }
        }

        public MenuItem SearchMenuItem()
        {
            string name = InputHelper.GetUserInput<string>("Skriv i namnet på vad du vill hitta");

            var item = context.MenuItems.FirstOrDefault(d => d.Name.ToLower().Equals(name.ToLower()));

            if (item == null)
            {
                Console.WriteLine("Maträtten eller drycken kunde inte hittas.");

            }
            return item;

        }
    }
}
