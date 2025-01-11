using Microsoft.EntityFrameworkCore;
using publisherData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MenuItem = publisherData.MenuItem;

namespace DumplingWPF
{


    public class FoodItemsViewModel
    {
        private readonly PubContext context;

        public ObservableCollection<MenuItem> FoodItems { get; set; }

        public FoodItemsViewModel(PubContext _context)
        {
            context = _context;
            FoodItems = new ObservableCollection<MenuItem>();
            GetFoodItems();
        }

        void GetFoodItems()
        {
            var foodItems = context.MenuItems.Where(d => d.Category == "Food").ToList();
            FoodItems.Clear();
            foreach (var dish in foodItems)
            {
                FoodItems.Add(dish);
            }
        }

        public void AddFoodItem(string name, string description, decimal price)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) || price <= 0)
            {
                throw new ArgumentException("Name cannot be empty, and price must be greater than zero.");
            }

            // Create a new food item
            var newFoodItem = new MenuItem
            {
                Name = name,
                Description = description,
                Price = price,
                Category = "Food"
            };

            context.MenuItems.Add(newFoodItem);
            context.SaveChanges();

            
            FoodItems.Add(newFoodItem);
        }




    }
}
