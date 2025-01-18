using Microsoft.EntityFrameworkCore;
using publisherData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumplingsEFCore
{
    public class CustomerManager
    {
        private readonly PubContext context;

        public CustomerManager(PubContext _context)
        {
            context = _context;
        }



        public void StartMenu()
        {
            bool exit = false;
            while (!exit)
            {

                string choice = InputHelper.GetUserInput<string>(
                    "Välkommen, här kan du hantera Kunder\n" +
                    "1: Printa ut Kunder\n" +
                    "2: Lägg till kund\n" +
                    "3: Uppdatera Kund\n" +
                    "4: Ta bort Kund\n" +
                    "5: Gå tillbaka till menyn\n" +
                    "6: Avsluta program"
                    );

                switch (choice)
                {
                    case "1":
                        PrintCustomer();
                        break;
                    case "2":
                        AddCustomer();
                        break;
                    case "3":
                        UpdateCustomer();
                        break;
                    case "4":
                        RemoveCustomer();
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

        public void PrintCustomer()
        {
            /* sorts and prints out the customers and their orders */
            var customers = context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.Items)
                .OrderByDescending(c => c.Orders.Max(o => o.OrderDate))
                .ToList();

            Console.WriteLine("Kunder");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Kund: {customer.Name}, antal ordrar: {customer.Orders.Count}");
                foreach (var order in customer.Orders.OrderBy(o => o.OrderDate))
                {
                    Console.WriteLine($"- Order ID: {order.Id}, Datum: {order.OrderDate}");
                }
            }
        }

        public void AddCustomer()
        {
            // Adds a new customer. related orders can be created in the OrderManager 
            string name = InputHelper.GetUserInput<string>("Skriv i namn:");
            string telephone = InputHelper.GetUserInput<string>("Skriv i telefonnummer: ");

            var newCustomer = new Customer
            {
                Name = name,
                Telephone = telephone
            };
            context.Customers.Add(newCustomer);
            context.SaveChanges();
            Console.WriteLine("Ny kund tillagd!");

        }

        public Customer SearchCustomer()
        {
            string updateCustomer = InputHelper.GetUserInput<string>("Ange namn på den kund du vill hitta: ");
            var customer = context.Customers.FirstOrDefault(p => p.Name.ToLower().Equals(updateCustomer.ToLower()));

            if (customer == null)
            {
                Console.WriteLine("Person hittades inte");
            }
            return customer!;
        }

        public void UpdateCustomer()
        {
            var customer = SearchCustomer();
            Console.WriteLine($"Uppdatera {customer.Name}: ");
            customer.Name = InputHelper.GetUserInput<string>("Namn:");
            customer.Telephone = InputHelper.GetUserInput<string>("Telefon:");
            context.SaveChanges();

            Console.WriteLine($"Uppdatering sparad: {customer.Name} (ID: {customer.Id})");
        }

        public void RemoveCustomer()
        {
            var customer = SearchCustomer();

            context.Customers.Remove(customer);
            context.SaveChanges();
            Console.WriteLine($"{customer.Name} (ID: {customer.Id}) har tagits bort.");
        }
    }
}
