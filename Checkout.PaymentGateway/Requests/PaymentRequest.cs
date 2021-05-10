using Checkout.PaymentGateway.Data.Enum;

namespace Checkout.PaymentGateway.Requests
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Cvv { get; set; }
    }
}
