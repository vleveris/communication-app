using CommunicationApp.Models;
using CommunicationApp.Repositories;
using System.Text.RegularExpressions;

namespace CommunicationApp.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ITemplateRepository _templateRepository;
        private readonly IPlaceholderService _placeholderService;

        public CommunicationService(ICustomerRepository customerRepository, ITemplateRepository templateRepository, IPlaceholderService placeholderService)
        {
            _customerRepository = customerRepository;
            _templateRepository = templateRepository;
            _placeholderService = placeholderService;
        }

        public async Task<string> GenerateMessageAsync(int customerId, int templateId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            var template = await _templateRepository.GetByIdAsync(templateId);

            if (customer == null || template == null)
            {
                throw new KeyNotFoundException("Customer or template not found.");
            }

            var placeholders = _placeholderService.GetAvailablePlaceholders();
            var message = template.Body;

            foreach (var placeholder in placeholders)
            {
                string placeholderKey = "{{" + placeholder.Key + "}}";
                var replacementValue = string.Empty;

                switch (placeholder.Key)
                {
                    case "name":
                        replacementValue = customer.Name;
                        break;
                    case "email":
                        replacementValue = customer.Email;
                        break;
                    case "city":
                        replacementValue = customer.City;
                        break;
                    case "postalcode":
                        replacementValue = customer.PostalCode;
                        break;

                    default:
                        throw new NotSupportedException($"Placeholder key {placeholder.Key} not supported.");
                }

                message = message.Replace(placeholderKey, replacementValue);
            }

            return message;
        }

        public async Task SendMessageAsync(int customerId, int templateId)
        {
            var message = await GenerateMessageAsync(customerId, templateId);

            var template = await _templateRepository.GetByIdAsync(templateId);

            if (template == null)
            {
                throw new KeyNotFoundException("Template not found.");
            }

            Console.WriteLine("Sending message to customer:");
            Console.WriteLine($"Subject: {template.Subject}");
            Console.WriteLine($"Body: {message}");
        }
    }
}
