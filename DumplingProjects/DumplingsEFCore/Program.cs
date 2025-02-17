﻿using publisherData;
namespace DumplingsEFCore
{
    internal class Program
    {
        private static PubContext context = new PubContext();

        private static MenuManager menuManager;
        private static OrderManager orderManager;
        private static StaffManager staffManager;
        private static CustomerManager customerManager;

        static void Main(string[] args)
        {

            context.Database.EnsureCreated(); // ensure db is created
            orderManager = new OrderManager(context);
            menuManager = new MenuManager(context);
            staffManager = new StaffManager(context);
            customerManager = new CustomerManager(context);
            StartMenu();
        }
        public static void StartMenu()
        {
            bool exit = false;
            while (!exit)
            {

                string choice = InputHelper.GetUserInput<string>(
                    "Välkommen, här kan du hantera din databas\n" +
                    "1: Initialisera databasen\n" +
                    "2: Hantera Meny\n" +
                    "3: Hantera Ordrar\n" +
                    "4: Hantera personal\n" +
                    "5: Hantera kunder\n" +
                    "6: Avsluta program"
                    );

                switch (choice)
                {
                    case "1":
                        break;
                    case "2":
                        menuManager.StartMenu();
                        break;
                    case "3":
                        orderManager.StartMenu();
                        break;
                    case "4":
                        staffManager.StartMenu();
                        break;
                    case "5":
                        customerManager.StartMenu();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Ogiltig input försök igen");
                        break;

                }
            }
        }

        /* Some methods to till the database*/

        public static void AddDrinks()
        {
            var newDrinks = new List<MenuItem> {
                new MenuItem{ Name = "Coca Cola", Description = "Soda", Category = "Drink", Price = 19 },
                new MenuItem{ Name = "Fanta", Description = "Soda", Category = "Drink", Price = 19 },
                new MenuItem{ Name = "Coca Cola Zero", Description = "Soda", Category = "Drink", Price =19 },
                new MenuItem{ Name = "Loka naturell", Description = "Soda", Category = "Drink", Price = 19 },
                new MenuItem{ Name = "Loka Citron", Description = "Soda", Category = "Drink", Price = 19 },
                new MenuItem{ Name = "Trocadero", Description = "Soda", Category = "Drink", Price = 10 }
                };
            context.MenuItems.AddRange(newDrinks);
            context.SaveChanges();
        }
        void AddFood()
        {
            var newFoodItems = new List<MenuItem> {
                new MenuItem{ Name = "Pork dumplings", Description = "Klassisk dumpling med fläsk", Category = "Food", Price = 45 },
                new MenuItem{ Name = "Chicken dumplings", Description = "Kyckling och thaibasilika", Category = "Food", Price = 45 },
                new MenuItem{ Name = "Shrimp dumplings", Description = "Dumplings med räka", Category = "Food", Price = 55 },
                new MenuItem{ Name = "mushroom dumplings", Description = "Vegetarianskt", Category = "Food", Price = 45 },
                new MenuItem{ Name = "spicy pork dumplings", Description = "För er som tycker om lite mer hetta", Category = "Food", Price = 45 }
                };
            context.MenuItems.AddRange(newFoodItems);
            context.SaveChanges();
        }

        public static void AddStaff()
        {
            var newStaff = new List<Staff>
                {
                    new Staff{Name = "Viktor Thörn", Telephone= "0701101010", Role ="Chef"},
                    new Staff{Name = "Jesper Wallentin", Telephone= "0701104455", Role ="Chef"},
                    new Staff{Name = "Joakim Bjerselius", Telephone= "0701104198", Role ="Manager"}
                };
            context.Staff.AddRange(newStaff);
            context.SaveChanges();
        }

        public static void CloseProgram() // the program closes the environment.
        {
            Console.Clear();
            Console.WriteLine("Programmet kommer att avslutas!");
            Console.ReadLine();
            Environment.Exit(0);
        }

    }
}

