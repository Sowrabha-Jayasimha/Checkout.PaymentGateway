namespace Checkout.Bank.Mock
{
    public class ProcessPaymentRequest
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
