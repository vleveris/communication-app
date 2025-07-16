namespace CommunicationApp.Models
{
    public class Template
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}
