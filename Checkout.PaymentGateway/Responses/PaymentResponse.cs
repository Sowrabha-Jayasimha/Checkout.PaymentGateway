using System;
using System.Security.AccessControl;

namespace Checkout.PaymentGateway.Responses
{
    public class PaymentResponse
    {
        public Guid TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionNote { get; set; }
    }
}
