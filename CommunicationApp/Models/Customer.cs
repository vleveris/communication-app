namespace CommunicationApp.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
    }
}
