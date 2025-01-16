using Microsoft.EntityFrameworkCore;
using publisherData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumplingWPF
{

    public class OrdersViewModel
    {
        public ObservableCollection<Order> IncomingOrders { get; set; }
        public ObservableCollection<Order> InProgressOrders { get; set; }

        private PubContext _dbContext;

        public OrdersViewModel()
        {
            _dbContext = new PubContext();
            IncomingOrders = new ObservableCollection<Order>();
            InProgressOrders = new ObservableCollection<Order>();
            LoadOrders();
        }


        /* Prints out orders */
        public void LoadOrders()
        {
            var orders = _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.MenuItem)
                .Where(o => !o.IsCompleted)
                .ToList();

            foreach (var order in orders)
            {
                if (!IncomingOrders.Any(o => o.Id == order.Id))
                {
                    // If the order ID doesn't exist, add it to the collection
                    IncomingOrders.Add(order);
                };
            }
        }

        /* Move from incoming to in pogress to the other */
        public void MoveToInProgress(Order order)
        {
            IncomingOrders.Remove(order);
            InProgressOrders.Add(order);
        }
        /* Removes from in progress list */
        public void MarkAsDone(Order order)
        {
            if (order == null) return;

            // Mark as completed
            order.IsCompleted = true;

            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
            InProgressOrders.Remove(order);
        }
    }
}
