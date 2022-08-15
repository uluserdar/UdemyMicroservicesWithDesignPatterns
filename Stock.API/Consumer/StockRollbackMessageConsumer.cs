using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Messages;
using Stock.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Consumer
{
    public class StockRollbackMessageConsumer : IConsumer<StockRollbackMessage>
    {

        private readonly AppDbContext _context;
        private ILogger<StockRollbackMessageConsumer> _logger;

        public StockRollbackMessageConsumer(AppDbContext context, ILogger<StockRollbackMessageConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockRollbackMessage> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Stock was released");
        }
    }
}
