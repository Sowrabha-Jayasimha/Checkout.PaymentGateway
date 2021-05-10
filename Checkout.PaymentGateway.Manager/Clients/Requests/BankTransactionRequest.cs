using Checkout.PaymentGateway.Data.Enum;

namespace Checkout.PaymentGateway.Manager.Clients.Requests
{
    public class BankTransactionRequest
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Cvv { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
