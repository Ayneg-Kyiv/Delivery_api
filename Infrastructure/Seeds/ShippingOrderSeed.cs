//using Domain.Models.Orders;
//using Infrastructure.Contexts;
//using Microsoft.EntityFrameworkCore;

//namespace Infrastructure.Seeds
//{
//    public static class ShippingOrderSeed
//    {
//        public static async Task SeedAsync(ShippingDbContext context)
//        {
//            // Перевіряємо, чи вже є дані
//            if (await context.ShippingOrders.AnyAsync())
//            {
//                return; // База вже містить дані
//            }

//            var shippingOrders = new List<ShippingOrder>
//            {
//                new ShippingOrder
//                {
//                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
//                    CustomerId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
//                    EstimatedCost = 150.50m,
//                    EstimatedDistance = 25.5f,
//                    EstimatedShippingDate = new DateOnly(2025, 8, 1),
//                    EstimatedShippingTime = new TimeOnly(14, 30),
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdatedAt = DateTime.UtcNow
//                },
//                new ShippingOrder
//                {
//                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
//                    CustomerId = Guid.Parse("987fcdeb-51a2-43d1-9c4e-123456789abc"),
//                    EstimatedCost = 275.00m,
//                    EstimatedDistance = 45.2f,
//                    EstimatedShippingDate = new DateOnly(2025, 8, 2),
//                    EstimatedShippingTime = new TimeOnly(9, 15),
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdatedAt = DateTime.UtcNow
//                },
//                new ShippingOrder
//                {
//                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
//                    CustomerId = Guid.Parse("456def78-9abc-12de-f345-678901234567"),
//                    EstimatedCost = 89.99m,
//                    EstimatedDistance = 12.7f,
//                    EstimatedShippingDate = new DateOnly(2025, 7, 31),
//                    EstimatedShippingTime = new TimeOnly(16, 45),
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdatedAt = DateTime.UtcNow
//                },
//                new ShippingOrder
//                {
//                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
//                    CustomerId = Guid.Parse("789abc12-3def-4567-8901-234567890def"),
//                    EstimatedCost = 320.75m,
//                    EstimatedDistance = 67.3f,
//                    EstimatedShippingDate = new DateOnly(2025, 8, 3),
//                    EstimatedShippingTime = new TimeOnly(11, 0),
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdatedAt = DateTime.UtcNow
//                },
//                new ShippingOrder
//                {
//                    Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
//                    CustomerId = Guid.Parse("abc12345-6789-0def-abc1-23456789abcd"),
//                    EstimatedCost = 45.25m,
//                    EstimatedDistance = 8.1f,
//                    EstimatedShippingDate = new DateOnly(2025, 8, 4),
//                    EstimatedShippingTime = new TimeOnly(13, 20),
//                    CreatedAt = DateTime.UtcNow,
//                    LastUpdatedAt = DateTime.UtcNow
//                }
//            };

//            await context.ShippingOrders.AddRangeAsync(shippingOrders);
//            await context.SaveChangesAsync();
//        }
//    }
//}
