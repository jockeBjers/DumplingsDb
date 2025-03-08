namespace DumplingsBlazor.Components.Pages
{
    public partial class Home
    {
        private List<MenuItem> FoodItems = new()
        {
            new MenuItem { Name = "Pork Dumplings",Description="classic dumplings", Price = 99, Category = "Food" },
            new MenuItem { Name = "Chicken Dumplings",Description="Chicken and thai basil dumplings", Price = 99, Category = "Food" },
            new MenuItem { Name = "shrimp Dumplings",Description="shrimp dumplings", Price = 99, Category = "Food" },
            new MenuItem { Name = "mushroom Dumplings",Description="vegan alternative", Price = 99, Category = "Food" },
            new MenuItem { Name = "spicy pork Dumplings",Description="a spicier alternative", Price = 99, Category = "Food" },

            new MenuItem { Name = "Spring Rolls", Price = 79, Category = "Food" }
        };

        private List<MenuItem> DrinkItems = new()
        {
            new MenuItem { Name = "Green Tea", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Green Tea", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Green Tea", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Green Tea", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Bubble Tea", Price = 45, Category = "Drink" }
        };

      
        private class MenuItem
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public string Category { get; set; } = string.Empty;
        }
    }
}
