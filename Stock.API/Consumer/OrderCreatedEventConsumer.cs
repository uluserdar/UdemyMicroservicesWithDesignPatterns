using MassTransit;
using Microsoft.Extensions.Logging;
using Shared;
using Stock.API.Models;
using System.Threading.Tasks;

namespace Stock.API.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly AppDbContext _context;
        private ILogger<OrderCreatedEventConsumer> _logger;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            
        }
    }
}
