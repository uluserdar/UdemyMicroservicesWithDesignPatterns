using MassTransit;
using Microsoft.Extensions.Logging;
using Order.API.Models;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.API.Consumer
{
    public class OrderRequestComplatedEventConsumer : IConsumer<OrderRequestComplatedEvent>
    {
        private readonly AppDbContext _context;

        private readonly ILogger<OrderRequestComplatedEventConsumer> _logger;

        public OrderRequestComplatedEventConsumer(AppDbContext context, ILogger<OrderRequestComplatedEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderRequestComplatedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = OrderStatus.Complate;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
