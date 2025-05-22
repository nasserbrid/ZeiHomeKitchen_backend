using Stripe;

namespace ZeiHomeKitchen_backend.Application.Ports
{
    public interface IStripeService
    {
        Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency = "eur", string description = null);
        Task<PaymentIntent> ConfirmPaymentIntent(string paymentIntentId);
        Task<PaymentIntent> GetPaymentIntent(string paymentIntentId);
    }
}
