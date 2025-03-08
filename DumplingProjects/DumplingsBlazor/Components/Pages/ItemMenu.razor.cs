namespace DumplingsBlazor.Components.Pages
{
    public partial class ItemMenu
    {
        private List<MenuItem> FoodItems = new()
        {
            new MenuItem { Name = "Pork Dumplings",Description="classic dumplings", Price = 45, Category = "Food" },
            new MenuItem { Name = "Chicken Dumplings",Description="Chicken and thai basil dumplings", Price = 45, Category = "Food" },
            new MenuItem { Name = "shrimp Dumplings",Description="shrimp dumplings", Price = 55, Category = "Food" },
            new MenuItem { Name = "mushroom Dumplings",Description="vegan alternative", Price = 45, Category = "Food" },
            new MenuItem { Name = "spicy pork Dumplings",Description="a spicier alternative", Price = 45, Category = "Food" },
            new MenuItem { Name = "Karaage",Description="Crispy fried chicken", Price = 55, Category = "Food" },
            new MenuItem { Name = "Spring Rolls", Price = 45, Category = "Food" }
        };

        private List<MenuItem> DrinkItems = new()
        {
            new MenuItem { Name = "Coca Cola", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Coca Cola Zero", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Sprite", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Zingo", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Ice Tea", Price = 28, Category = "Drink" }
        };

        private List<MenuItem> cart = new()
        {
            new MenuItem { Name = "mushroom Dumplings",Description="vegan alternative", Price = 99, Category = "Food" },
            new MenuItem { Name = "spicy pork dumplings",Description="a spicier alternative", Price = 99, Category = "Food" },
            new MenuItem { Name = "Ice Tea", Price = 25, Category = "Drink" },
            new MenuItem { Name = "Zingo", Price = 45, Category = "Drink" }
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
