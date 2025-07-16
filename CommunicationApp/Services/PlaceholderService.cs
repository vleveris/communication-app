using System.Collections.Generic;

namespace CommunicationApp.Services
{
    public class PlaceholderService : IPlaceholderService
    {
        public Dictionary<string, string> GetAvailablePlaceholders()
        {
            return new Dictionary<string, string>
            {
                { "name", "The customer's full name." },
                { "email", "The customer's email address." },
                { "city", "The customer's city." },
                { "postalcode", "The customer's postal code." }
            };
        }
    }
}
