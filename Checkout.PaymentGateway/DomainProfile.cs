using AutoMapper;
using Checkout.PaymentGateway.Manager.Responses;
using Checkout.PaymentGateway.Responses;

namespace Checkout.PaymentGateway
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<ProcessPaymentResponse, PaymentResponse>();
        }
    }
}
