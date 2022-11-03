namespace API_Dapper.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public string Customer { get; set; }
        public List<OrderDetail> Details { get; set; }
    }
}
