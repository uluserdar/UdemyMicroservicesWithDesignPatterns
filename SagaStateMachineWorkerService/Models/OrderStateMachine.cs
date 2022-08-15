using MassTransit;
using Shared;
using Shared.Events;
using Shared.Interfaces;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }

        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public State StockReserved { get; private set; }

        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }
        public State StockNotReserved { get; private set; }

        public Event<IPaymentComplatedEvent> PaymentComplatedEvent { get; set; }
        public State PaymentComplated { get; private set; }

        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }
        public State PaymentFailed { get; private set; }

        [Obsolete]
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => PaymentComplatedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Instance.BuyerId = context.Data.BuyerId;
                context.Instance.OrderId = context.Data.OrderId;
                context.Instance.CreatedDate = DateTime.Now;
                context.Instance.CardName = context.Data.Payment.CardName;
                context.Instance.CardNumber = context.Data.Payment.CardNumber;
                context.Instance.Expiration = context.Data.Payment.Expiration;
                context.Instance.Cvv = context.Data.Payment.Cvv;
                context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
            }).Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent before : {context.Instance}");
            })
            .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId)
            {
                OrderItems = context.Data.OrderItems
            })
            .TransitionTo(OrderCreated).Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent after : {context.Instance}");
            }));

            During(OrderCreated,
                When(StockReservedEvent).TransitionTo(StockReserved)
                .Send(new Uri($"queue:{RabbitMqSettingsConst.PaymentStockReservedRequestEventQueueName}"), context => new StockReservedRequestPaymentEvent(context.Instance.CorrelationId)
                {
                    OrderItems = context.Data.OrderItems,
                    Payment = new PaymentMessage
                    {
                        CardName = context.Instance.CardName,
                        CardNumber = context.Instance.CardNumber,
                        Cvv = context.Instance.Cvv,
                        Expiration = context.Instance.Expiration,
                        TotalPrice = context.Instance.TotalPrice
                    },
                    BuyerId = context.Instance.BuyerId
                }).Then(context =>
                {
                    Console.WriteLine($"StockReservedEvent after : {context.Instance}");
                }),
                When(StockNotReservedEvent).TransitionTo(StockNotReserved)
                .Publish(context => new OrderRequestFailedEvent()
                {
                    OrderId = context.Instance.OrderId,
                    Reason = context.Data.Reason
                })
                .Then(context =>
                {
                    Console.WriteLine($"StockNotReservedEvent after : {context.Instance}");
                })
                );

            During(StockReserved, When(PaymentComplatedEvent).TransitionTo(PaymentComplated)
                .Publish(context => new OrderRequestComplatedEvent()
                {
                    OrderId = context.Instance.OrderId
                })
                .Then(context =>
                {
                    Console.WriteLine($"PaymentComplatedEvent after : {context.Instance}");
                }).Finalize(),
                When(PaymentFailedEvent)
                .Publish(context => new OrderRequestFailedEvent()
                {
                    OrderId = context.Instance.OrderId,
                    Reason = context.Data.Reason
                })
                .Send(new Uri($"queue:{RabbitMqSettingsConst.StockRollbackMessageQueueName}"), context => new StockRollbackMessage()
                {
                    OrderItems = context.Data.OrderItems
                })
                .TransitionTo(PaymentFailed)
                .Then(context =>
                {
                    Console.WriteLine($"PaymentFailedEvent after : {context.Instance}");
                }));

            SetCompletedWhenFinalized();

        }
    }
}
