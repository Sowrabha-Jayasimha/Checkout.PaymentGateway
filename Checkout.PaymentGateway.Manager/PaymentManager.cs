using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Data;
using Checkout.PaymentGateway.Data.Enum;
using Checkout.PaymentGateway.Data.Models;
using Checkout.PaymentGateway.Manager.Clients;
using Checkout.PaymentGateway.Manager.Clients.Requests;
using Checkout.PaymentGateway.Manager.Clients.Responses;
using Checkout.PaymentGateway.Manager.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Manager
{
    public class PaymentManager : IPaymentManager
    {
        private readonly ILogger<PaymentManager> _logger;
        private readonly IBankClient _bankClient;
        private readonly IAppDbContext _dbContext;

        public PaymentManager(ILogger<PaymentManager> logger, IBankClient bankClient, IAppDbContext dbContext)
        {
            _logger = logger;
            _bankClient = bankClient;
            _dbContext = dbContext;
        }

        public async Task<GetTransactionReponse> GetPaymentDetails(Guid transactionId)
        {
            _logger.LogInformation($"Executing GetPaymentDetails for TransactionId={transactionId}");

            var transaction = await _dbContext.Transactions.Include(x => x.Cards)
                .FirstOrDefaultAsync(x => x.TransactionId == transactionId);

            if (transaction == null)
            {
                _logger.LogInformation($"Could not find transaction with TransactionId={transactionId}");
                return null;
            }

            return new GetTransactionReponse
            {
                CardNumber = $"************{transaction.Cards?.CardNumber?.Substring(12, 4)}",
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionCode = transaction.TransactionCode,
                TransactionNote = transaction.TransactionNote
            };
        }

        public async Task<ProcessPaymentResponse> ProcessPayment(string cardholderName, string cardNumber, int expiryMonth, int expiryYear, int cvv, decimal amount, Currency currency)
        {
            var cardLastFourDigits = cardNumber.Substring(12, 4);
            _logger.LogInformation($"Executing ProcessPayment for card with LastFourDigits={cardLastFourDigits}");

            var response = new ProcessPaymentResponse();
            var bankTransactionResponse = new BankTransactionResponse();

            var expiryDate = new DateTime(expiryYear, expiryMonth, DateTime.DaysInMonth(expiryYear, expiryMonth)).Date;
                
            if (expiryDate < DateTime.Today)
            {
                _logger.LogInformation($"Card with LastFourDigits={cardLastFourDigits} is expired");
                response.Error = new ErrorResponse("Card is expired.");
                return response;
            }

            var bankTransactionRequest = new BankTransactionRequest
            {
                CardNumber = cardNumber,
                ExpiryYear = expiryYear,
                ExpiryMonth = expiryMonth,
                Cvv = cvv,
                Amount = amount,
                Currency = Enum.GetName(typeof(Currency), (int)currency)
            };

            try
            {
                bankTransactionResponse = await _bankClient.MakeTransaction(bankTransactionRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error while performing bank transaction for card with LastFourDigits={cardLastFourDigits}. Error={ex.Message}");
                response.IsSuccess = false;
                response.Error.ErrorMessage = ex.Message;
            }

            if (bankTransactionResponse.TransactionId == Guid.Empty)
            {
                _logger.LogError($"Transaction was not successful for card with LastFourDigits={cardLastFourDigits}");
                response.Error = new ErrorResponse(bankTransactionResponse.TransactionMessage);
                return response;
            }

            response.TransactionCode = bankTransactionResponse.TransactionCode;
            response.TransactionNote = bankTransactionResponse.TransactionMessage;

            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = amount,
                Currency = currency,
                TransactionCode = response.TransactionCode,
                TransactionNote = response.TransactionNote,
                BankTransactionId = bankTransactionResponse.TransactionId,
                Cards = new CardDetails
                {
                    CardId = Guid.NewGuid(),
                    CardHolderName = cardholderName,
                    CardNumber = cardNumber,
                    ExpiryMonth = expiryMonth,
                    ExpiryYear = expiryYear,
                    Cvv = cvv
                },
                CreatedDateTime = DateTime.Now
            };

            _dbContext.Transactions.Add(transaction);

            await _dbContext.SaveChangesAsync();

            response.IsSuccess = true;
            response.TransactionId = transaction.TransactionId;

            return response;
        }
    }
}
