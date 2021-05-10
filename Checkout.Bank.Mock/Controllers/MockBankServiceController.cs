using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Checkout.Bank.Mock.Controllers
{
    [ApiController]
    [Route("api/v1/bank")]
    public class MockBankServiceController : ControllerBase
    {
        [HttpPost("process-payment")]
        public async Task<ActionResult<ProcessPaymentResponse>> ProcessPayment(ProcessPaymentRequest request)
        {
            var response = new ProcessPaymentResponse();

            var declineReasons = new[] {"invalid card details", "insufficient balance", "retained card"};

            int num = Convert.ToInt32(request.CardNumber.Substring(15, 1));

            if (num == 0)
            {
                response.TransactionCode = "Failed";
                response.TransactionMessage = "Bank transaction failed";
                return BadRequest(response);
            }

            var mod = num % 2;
            switch (mod)
            {
                case 0:
                    response.TransactionId = Guid.NewGuid();
                    response.TransactionCode = "Success";
                    response.TransactionMessage = "Transaction successful";
                    break;
                case 1:
                    var random = new Random();
                    
                    response.TransactionCode = "Failed";
                    response.TransactionMessage = $"Transaction failed because of {declineReasons[random.Next(0, 3)]}";
                    break;
            }

            return Ok(response);
        }
    }
}
