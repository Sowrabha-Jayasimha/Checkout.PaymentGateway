using System;

namespace Checkout.Bank.Mock
{
    public class ProcessPaymentResponse
    {
        public Guid TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionMessage { get; set; }
    }
}
