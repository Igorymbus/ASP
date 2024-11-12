namespace Kovukov.Models
{
    public class Order_Items
    {
        public int ID { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
        public decimal price { get;set;}

    }
}
