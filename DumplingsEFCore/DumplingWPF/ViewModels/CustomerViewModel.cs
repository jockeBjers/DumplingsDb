using Microsoft.EntityFrameworkCore;
using publisherData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DumplingWPF.ViewModels
{
    class CustomerViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }
        public readonly PubContext context;
        public CustomerViewModel ViewModel;

        public CustomerViewModel()
        {
            context = new PubContext();
            Customers = new ObservableCollection<Customer>();
            LoadCustomers();
        }


        /* Prints out customers */
        public void LoadCustomers()
        {
            var customers = context.Customers
          .Include(c => c.Orders)
          .Where(c => c.Orders.Any(o => o.IsCompleted)) // Customers with active orders
          .ToList();

            foreach (var customer in customers)
            {
                // Filter out completed orders
                customer.Orders = customer.Orders.Where(o => o.IsCompleted).ToList();

                // making sure the customer isnt already in the list.
                if (customer.Orders.Any() && !Customers.Any(c => c.Id == customer.Id))
                {
                    Customers.Add(customer);
                }
            }
        }
    }
}
