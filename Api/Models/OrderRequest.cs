namespace Api.Models
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public string Productname { get; set; }
        public int Quantity { get; set; }

        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        InProgress,
        Completed,
        Rejected
    }
}