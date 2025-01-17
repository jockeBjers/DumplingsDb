using Microsoft.EntityFrameworkCore;
using publisherData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumplingsEFCore
{
    public class OrderManager
    {

        private readonly PubContext context;

        public OrderManager(PubContext _context)
        {

            context = _context;

        }

        public void StartMenu()
        {
            bool exit = false;
            while (!exit)
            {

                string choice = InputHelper.GetUserInput<string>(
                    "Välkommen, här kan du hantera dina ordrar\n" +
                    "1: Printa ut alla ordrar\n" +
                    "2: Lägg till order\n" +
                    "3: Uppdatera uppdatera order\n" +
                    "4: Ta bort order\n" +
                    "5: Gå tillbaka till menyn\n" +
                    "6: Avsluta program"
                    );

                switch (choice)
                {
                    case "1":
                        PrintAllOrders();
                        break;
                    case "2":
                        AddOrder();
                        break;
                    case "3":
                        UpdateOrder();
                        break;
                    case "4":
                        DeleteOrder();
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

        public void PrintAllOrders()
        {
            var orders = context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
                .OrderBy(o => o.Id)
                .ToList();

            foreach (var order in orders)
            {

                decimal calculatedTotalPrice = order.Items.Sum(item => item.MenuItem.Price * item.Quantity);


                if (order.TotalPrice != calculatedTotalPrice)
                {
                    order.TotalPrice = calculatedTotalPrice;
                }

                Console.WriteLine($"Order ID: {order.Id}, Datum: {order.OrderDate}, Totalpris: {order.TotalPrice}");
                Console.WriteLine($"Kund: {order.Customer?.Name ?? "Okänd kund"}");

                foreach (var item in order.Items)
                {
                    Console.WriteLine($"  - {item.MenuItem.Name} x{item.Quantity} ({item.MenuItem.Price} styck)");
                }

                Console.WriteLine();
            }

            context.SaveChanges();
        }



        public void AddOrder()
        {
            Console.Clear();
            Console.WriteLine("Lägg till en ny order:");

            // Search for existing customer
            string customerName = InputHelper.GetUserInput<string>("Ange kundens namn: ");
            string customerPhone = InputHelper.GetUserInput<string>("Ange kundens telefonnummer: ");

            var existingCustomer = context.Customers.FirstOrDefault(c =>
                c.Name.ToLower().Equals(customerName.ToLower()) &&
                c.Telephone == customerPhone);

            Customer customer;
            if (existingCustomer != null)
            {
                customer = existingCustomer;
                Console.WriteLine($"Existerande kund hittad: {customer.Name} (ID: {customer.Id})");
            }
            else
            {
                customer = new Customer
                {
                    Name = customerName,
                    Telephone = customerPhone
                };
                context.Customers.Add(customer);
                context.SaveChanges();
                Console.WriteLine($"Ny kund {customer.Name} (ID: {customer.Id}) har lagts till");
            }

            Console.WriteLine("Här är menyn");
            var menu = new MenuManager(context);
            menu.PrintMenuItems();

            var newOrder = new Order
            {

                CustomerId = customer.Id,
                Customer = customer,
                OrderDate = DateTime.Now,

            };

            // add items to the order

            while (true)
            {

                var menuItemName = InputHelper.GetUserInput<string>("\nSkriv in namnet på varan att lägga till (eller 'sluta' för att avsluta): ");
                if (menuItemName.ToLower() == "sluta")
                {
                    break;
                }

                var menuItem = context.MenuItems.FirstOrDefault(m => m.Name.Equals(menuItemName.ToLower()));
                if (menuItem == null)
                {
                    Console.WriteLine("Maträtten/drycken hittades inte. Försök igen.");
                    continue;
                }
                var quantity = InputHelper.GetUserInput<int>("Hur många? ");
                if (quantity <= 0)
                {
                    Console.WriteLine("Ogiltig mängd. Försök igen.");
                    continue;
                }
                var newOrderItem = new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    MenuItem = menuItem,
                    Quantity = quantity,
                };

                newOrder.Items.Add(newOrderItem);
            }
            newOrder.TotalPrice = newOrder.Items.Sum(item => item.MenuItem.Price * item.Quantity);

            context.Orders.Add(newOrder);
            context.SaveChanges();

            Console.WriteLine($"\nNy order har lagts till för: {customer.Name}, totalt pris: {newOrder.TotalPrice}");
        }

        /* UPDATE */

        public void UpdateOrder()
        {

            string customerName = InputHelper.GetUserInput<string>("Ange kundens namn: ");
            var customer = context.Customers
                .Include(c => c.Orders)
                .ThenInclude(o => o.Items)
                .ThenInclude(oi => oi.MenuItem)
                .FirstOrDefault(c => c.Name.Equals(customerName.ToLower()));
            if (customer == null)
            {
                Console.WriteLine("Kunden hittades inte.");
                return;
            }
            var latestOrder = customer.Orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();
            if (latestOrder == null)
            {
                Console.WriteLine("Inga orders hittades.");
                return;
            }
            Console.WriteLine($"Uppdaterar order {latestOrder.Id} från {latestOrder.OrderDate} av {customer.Name}");

            DisplayOrderItems(latestOrder);

            UpdateSwitch(latestOrder);

            latestOrder.TotalPrice = latestOrder.Items.Sum(i => i.MenuItem.Price * i.Quantity);
            context.SaveChanges();
            Console.WriteLine($"\nOrder {latestOrder.Id} uppdaterades. Ny totalpris: {latestOrder.TotalPrice:C}");
        }

        private void DisplayOrderItems(Order order)
        {
            Console.WriteLine("\nNuvarande orderdetaljer:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"- {item.MenuItem.Name} x{item.Quantity} ({item.MenuItem.Price:C} styck)");
            }
        }

        private void UpdateSwitch(Order latestOrder)
        {
            bool updating = true;
            while (updating)
            {

                Console.WriteLine("\nVad vill du göra?");
                Console.WriteLine("1: Lägg till ny vara");
                Console.WriteLine("2: Uppdatera mängd för en befintlig vara");
                Console.WriteLine("3: Ta bort en vara");
                Console.WriteLine("4: Avsluta uppdatering");
                var choice = InputHelper.GetUserInput<int>("Ange ditt val: ");

                switch (choice)
                {

                    case 1: //add new item to order
                        AddNewItem(latestOrder);
                        DisplayOrderItems(latestOrder);
                        break;

                    case 2: // update order item
                        UpdateItemQuantity(latestOrder);
                        DisplayOrderItems(latestOrder);
                        break;
                    case 3: // remove item from order
                        RemoveItem(latestOrder);
                        DisplayOrderItems(latestOrder);
                        break;
                    case 4:
                        updating = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }

        private void AddNewItem(Order order)
        {
            var menuItemName = InputHelper.GetUserInput<string>("Skriv in namnet på varan att lägga till: ");
            var menuItem = context.MenuItems.FirstOrDefault(m => m.Name.Equals(menuItemName.ToLower()));

            if (menuItem == null)
            {
                Console.WriteLine("Maträtten/drycken hittades inte.");
                return;
            }

            var quantity = InputHelper.GetUserInput<int>("Skriv in mängd: ");
            if (quantity <= 0)
            {
                Console.WriteLine("Ogiltig mängd.");
                return;
            }

            order.Items.Add(new OrderItem
            {
                MenuItemId = menuItem.Id,
                MenuItem = menuItem,
                Quantity = quantity
            });

            Console.WriteLine($"{menuItemName} lades till i ordern.");
        }

        private void UpdateItemQuantity(Order order)
        {
            var itemToUpdate = InputHelper.GetUserInput<string>("Skriv in namnet på varan att uppdatera: ");
            var orderItem = order.Items.FirstOrDefault(i => i.MenuItem.Name.ToLower().Equals(itemToUpdate.ToLower()));

            if (orderItem == null)
            {
                Console.WriteLine("Varan hittades inte i ordern.");
                return;
            }

            var newQuantity = InputHelper.GetUserInput<int>("Skriv in ny mängd: ");
            if (newQuantity <= 0)
            {
                Console.WriteLine("Ogiltig mängd.");
                return;
            }

            orderItem.Quantity = newQuantity;
            Console.WriteLine($"Mängden för {orderItem.MenuItem.Name} uppdaterades till {newQuantity}.");
        }

        private void RemoveItem(Order order)
        {
            var itemToRemove = InputHelper.GetUserInput<string>("Skriv in namnet på varan att ta bort: ");
            var orderItemToRemove = order.Items.FirstOrDefault(i => i.MenuItem.Name.ToLower().Equals(itemToRemove.ToLower()));

            if (orderItemToRemove == null)
            {
                Console.WriteLine("Varan hittades inte i ordern.");
                return;
            }

            order.Items.Remove(orderItemToRemove);
            Console.WriteLine($"{orderItemToRemove.MenuItem.Name} togs bort från ordern.");
        }

        /* DELETE ORDER*/

        public void DeleteOrder()
        {
            string customerName = InputHelper.GetUserInput<string>("Skriv in kundens namn:");

            var customer = context.Customers
                .Include(c => c.Orders)
                .FirstOrDefault(c => c.Name.ToLower().Equals(customerName.ToLower()));
            if (customer == null)
            {
                Console.WriteLine("Kunden hittades inte.");
                return;
            }
            var latestOrder = customer.Orders.OrderByDescending(o => o.OrderDate).FirstOrDefault();
            if (latestOrder == null)
            {
                Console.WriteLine("Ingen order hittades för denna kund.");
                return;
            }

            var confirmation = InputHelper.GetUserInput<string>("Är du säker på att du vill ta bort ordern? ja/nej");
            if (confirmation.Equals("ja"))
            {
                context.Orders.Remove(latestOrder);
                context.SaveChanges();
                Console.WriteLine($"Ordern {latestOrder.Id} av kund {customer.Name} har tagits bort!");
            }
            else
            {
                Console.WriteLine("Avbruten handling.");
            }
        }
    }
}
