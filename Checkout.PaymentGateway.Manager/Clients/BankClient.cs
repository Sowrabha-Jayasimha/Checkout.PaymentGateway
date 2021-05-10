using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Manager.Clients.Requests;
using Checkout.PaymentGateway.Manager.Clients.Responses;
using Newtonsoft.Json;

namespace Checkout.PaymentGateway.Manager.Clients
{
    public class BankClient: IBankClient
    {
        private readonly HttpClient _client;

        public BankClient(HttpClient client)
        {
            if (client?.BaseAddress == null)
            {
                throw new ArgumentNullException("[httpClient].[BaseAddress] is missing");
            }

            _client = client;
        }

        public async Task<BankTransactionResponse> MakeTransaction(BankTransactionRequest request)
        {
            var requestObject = JsonConvert.SerializeObject(request);

            var response =
                await _client.PostAsync("bank/process-payment",
                    new StringContent(requestObject, Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<BankTransactionResponse>(await response.Content.ReadAsStringAsync());
        }
    }
}
