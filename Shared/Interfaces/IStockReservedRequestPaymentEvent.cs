using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IStockReservedRequestPaymentEvent: CorrelatedBy<Guid>
    {
        PaymentMessage Payment { get; set; }
         List<OrderItemMessage> OrderItems { get; set; }
         string BuyerId { get; set; }
    }
}
