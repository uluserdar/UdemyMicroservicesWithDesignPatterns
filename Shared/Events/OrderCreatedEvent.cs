using Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace Shared.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public OrderCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; }
    }
}
