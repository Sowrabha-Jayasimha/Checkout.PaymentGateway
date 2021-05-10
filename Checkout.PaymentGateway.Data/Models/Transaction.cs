using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Checkout.PaymentGateway.Data.Enum;

namespace Checkout.PaymentGateway.Data.Models
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        [ForeignKey("CardId")]
        public CardDetails Cards { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        [MaxLength(10)]
        public string TransactionCode { get; set; }
        [MaxLength(50)]
        public string TransactionNote { get; set; }
        public Guid BankTransactionId { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
