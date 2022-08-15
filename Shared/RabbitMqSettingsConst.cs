using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMqSettingsConst
    {
        public const string OrderSaga = "order-saga-queue";

        public const string StockRollbackMessageQueueName = "stock-rollback-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string OrderRequestComplatedEventQueueName = "order-request-complated-queue";
        public const string OrderRequestFailedEventQueueName = "order-request-failed-queue";
        public const string OrderStockNotReservedEventQueueName = "order-stock-not-reserved-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";
        public const string PaymentStockReservedRequestEventQueueName = "payment-stock-reserved-request-queue";

    }
}
