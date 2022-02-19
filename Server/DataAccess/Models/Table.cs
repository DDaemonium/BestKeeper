namespace Server.DataAccess.Models
{
    public class Table
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsReserved { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
