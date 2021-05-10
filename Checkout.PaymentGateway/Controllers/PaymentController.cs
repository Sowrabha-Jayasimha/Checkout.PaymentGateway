using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentGateway.Manager;
using Checkout.PaymentGateway.Requests;
using Checkout.PaymentGateway.Responses;
using Microsoft.Extensions.Logging;
using System.Net;
using Checkout.PaymentGateway.Manager.Responses;

namespace Checkout.PaymentGateway.Controllers
{
    [ApiController]
    [Route("api/v1/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentManager _paymentManager;
        private readonly IMapper _mapper;

        public PaymentController(ILogger<PaymentController> logger, IPaymentManager paymentManager, IMapper mapper)
        {
            _logger = logger;
            _paymentManager = paymentManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets payment details for given transaction id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Transaction details</returns>
        /// <response code="200">When successful.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetTransactionReponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<GetTransactionReponse>> GetPayment(Guid id)
        {
            _logger.LogInformation($"Executing GetPayment for Id={id}");

            var response = await _paymentManager.GetPaymentDetails(id);

            if (response == null)
            {
                _logger.LogInformation($"Could not get payment details for Id={id}");
                return NotFound();
            }

            _logger.LogInformation($"Returning payment details for Id={id}");
            return Ok(response);
        }

        /// <summary>
        /// Processes payment for given card and amount
        /// </summary>
        /// <param name="request"></param>
        /// <returns>TransactionId</returns>
        /// <respose code="200">When payment is successful.</respose>
        /// <response code="404">When payment is not successful.</response>
        [HttpPost("make-payment")]
        [ProducesResponseType(typeof(PaymentResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<PaymentResponse>> MakePayment(PaymentRequest request)
        {
            #region Validation

            if (request == null)
            {
                return BadRequest("Request cannot be null.");
            }

            if (request.CardNumber == null)
            {
                return BadRequest("Invalid card number.");
            }

            if (request.CardHolderName == null)
            {
                return BadRequest("Card holder cannot be null.");
            }

            if (request.ExpiryMonth <= 0 || request.ExpiryMonth > 12)
            {
                return BadRequest("Invalid card expiry month");
            }

            if (request.ExpiryYear <= 0 || request.ExpiryYear < DateTime.Today.Year)
            {
                return BadRequest("Invalid card expiry year");
            }

            if (request.Cvv <= 0)
            {
                return BadRequest("Invalid CVV");
            }

            #endregion

            var cardLastFourDigits = request.CardNumber.Substring(12, 4);
            _logger.LogInformation($"Executing MakePayment for card with LastFourDigits={cardLastFourDigits}");

            var processPaymentResponse = await _paymentManager.ProcessPayment(request.CardHolderName, request.CardNumber,
                request.ExpiryMonth, request.ExpiryYear, request.Cvv, request.Amount, request.Currency);

            if (!processPaymentResponse.IsSuccess)
            {
                _logger.LogInformation($"MakePayment was not successful for card with LastFourDigits={cardLastFourDigits}");
                return BadRequest(processPaymentResponse.Error);
            }

            var response = _mapper.Map<PaymentResponse>(processPaymentResponse);

            _logger.LogInformation($"Finished execution of MakePayment for card with LastFourDigits={cardLastFourDigits}");
            return Ok(response);
        }
    }
}
