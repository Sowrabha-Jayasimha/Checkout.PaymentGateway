using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.Data.Models
{
    public class CardDetails
    {
        [Key]
        public Guid CardId { get; set; }
        [MaxLength(16)]
        public string CardNumber { get; set; }
        [MaxLength(100)]
        public string CardHolderName { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int Cvv { get; set; }
    }
}
