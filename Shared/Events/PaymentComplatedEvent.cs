using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class PaymentComplatedEvent : IPaymentComplatedEvent
    {
        public Guid CorrelationId { get; }

        public PaymentComplatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
