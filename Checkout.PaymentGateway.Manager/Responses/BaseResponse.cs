using Checkout.PaymentGateway.Manager.Responses;

namespace Checkout.PaymentGateway.Manager.Responses
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
