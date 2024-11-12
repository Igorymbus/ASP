namespace Kovukov.Models
{
    public record class Customers
    {
        public int ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string addresss { get; set; }
        public string passwords { get; set; }

        public string role_name { get; set; }

    }
}
