using System.Threading.Tasks;
using Checkout.PaymentGateway.Manager.Clients.Requests;
using Checkout.PaymentGateway.Manager.Clients.Responses;

namespace Checkout.PaymentGateway.Manager.Clients
{
    public interface IBankClient
    {
        Task<BankTransactionResponse> MakeTransaction(BankTransactionRequest request);
    }
}
