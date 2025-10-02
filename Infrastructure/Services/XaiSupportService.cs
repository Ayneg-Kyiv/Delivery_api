using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Domain.Models.DTOs.Support;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public interface IXaiSupportService
    {
        Task<string> GetSupportReplyAsync(string userMessage, CancellationToken cancellationToken);
    }

    public class XaiSupportService : IXaiSupportService
    {
        private readonly HttpClient _httpClient;
        private readonly ShippingDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private string ApiKey; 
        private const string Endpoint = "https://api.x.ai/v1/chat/completions";

        public XaiSupportService(HttpClient httpClient, ShippingDbContext dbContext, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _configuration = configuration;
            ApiKey = _configuration["Xai:ApiKey"] ?? throw new ArgumentNullException("Missing Xai API Key");
        }

        public async Task<string> GetSupportReplyAsync(string userMessage, CancellationToken cancellationToken)
        {
            // Отримати дані з бази (наприклад, кількість замовлень та останнє замовлення)
            var ordersCount = await _dbContext.DeliveryOrders.CountAsync(cancellationToken);
            var lastOrder = await _dbContext.DeliveryOrders
                .Include(o => o.DeliverySlot)
                .Include(o => o.EndLocation)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            string dbInfo = $"Current orders count: {ordersCount}. ";
            if (lastOrder != null)
            {
                dbInfo += $"Last order: Estimated cost {lastOrder.DeliverySlot.ApproximatePrice}, Shipping date {lastOrder.EndLocation.DateTime}. ";
            }

            var payload = new
            {
                messages = new[]
                {
                    new { role = "system", content = $"Ти є оналйн консультантом на сайті. Сайт надає можливість відправляти посилки через водія якому по дорозі, або брати посилки як водій. Відповіді не повині бути великими. Ти можеш аналізувати і надавати дані про замовлення, поїзди з бази у разі запиту про таке: {dbInfo}" },
                    new { role = "user", content = userMessage }
                },
                model = "grok-4-latest",
                stream = false,
                temperature = 0
            };

            var request = new HttpRequestMessage(HttpMethod.Post, Endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(json);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return reply ?? "No reply from assistant.";
        }
    }
}
