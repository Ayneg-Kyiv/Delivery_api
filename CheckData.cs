using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CheckShippingData
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Server=OBI\\SQLEXPRESS;Database=DeliveryDb;Trusted_Connection=True;TrustServerCertificate=True;";
            
            var options = new DbContextOptionsBuilder<ShippingDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            using var context = new ShippingDbContext(options);
            
            Console.WriteLine("=== ShippingOrders in database ===");
            var orders = await context.ShippingOrders.ToListAsync();
            Console.WriteLine($"Total count: {orders.Count}");
            
            foreach (var order in orders)
            {
                Console.WriteLine($"Id: {order.Id}, CustomerId: {order.CustomerId}, Cost: {order.EstimatedCost}");
            }
            
            if (orders.Count == 0)
            {
                Console.WriteLine("No ShippingOrders found in database!");
            }
        }
    }
}
