using System;

namespace Checkout.PaymentGateway.Manager.Responses
{
    public class ProcessPaymentResponse : BaseResponse
    {
        public Guid TransactionId { get; set; }
        public string TransactionCode { get; set; }
        public string TransactionNote { get; set; }
    }
}
