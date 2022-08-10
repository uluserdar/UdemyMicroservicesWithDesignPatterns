using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IOrderCreatedRequestEvent
    {
        int OrderId { get; set; }
        string BuyerId { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
        PaymentMessage Payment { get; set; }
    }
}
