using System;

namespace Checkout.PaymentGateway.Manager.Clients.Responses
{
    public class BankTransactionResponse
    {
        public Guid TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionMessage { get; set; }
    }
}
