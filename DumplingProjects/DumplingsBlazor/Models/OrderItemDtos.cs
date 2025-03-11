    public class OrderItemDto
    {
        public required string MenuItemName { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderCustomerDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Telephone { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsCompleted { get; set; }
        public decimal TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemDto>? Items { get; set; }
        public OrderCustomerDto? Customer { get; set; }
    }