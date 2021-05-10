using System;
using Checkout.PaymentGateway.Data.Enum;

namespace Checkout.PaymentGateway.Manager.Responses
{
    public class GetTransactionReponse
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionNote { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
