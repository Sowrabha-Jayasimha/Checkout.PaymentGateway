using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Data.Enum;
using Checkout.PaymentGateway.Manager.Responses;

namespace Checkout.PaymentGateway.Manager
{
    public interface IPaymentManager
    {
        Task<ProcessPaymentResponse> ProcessPayment(string cardholderName, string cardNumber, int expiryMonth, int expiryYear, int cvv,
            decimal amount, Currency currency);

        Task<GetTransactionReponse> GetPaymentDetails(Guid transactionId);
    }
}
