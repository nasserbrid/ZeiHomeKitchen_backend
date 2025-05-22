using Stripe;
using ZeiHomeKitchen_backend.Application.Ports;

namespace ZeiHomeKitchen_backend.Domain.Services
{
    public class StripeService : IStripeService
    {
        private readonly PaymentIntentService _paymentIntentService;
        private readonly ILogger<StripeService> _logger;

        public StripeService(ILogger<StripeService> logger)
        {
            _logger = logger;
            _paymentIntentService = new PaymentIntentService();
        }

        public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency = "eur", string description = null)
        {
            try
            {
                var amountInCents = (long)(amount * 100);

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amountInCents,
                    Currency = currency,
                    Description = description,
                    //Méthodes de paiements autorisées
                    PaymentMethodTypes = new List<string> { "card" },
                    //Config pour le confirmation automatique
                    ConfirmationMethod = "automatic",
                };

                var paymentIntent = await _paymentIntentService.CreateAsync(options);

                _logger.LogInformation($"Payment créé avec succès : {paymentIntent.Id}");

                return paymentIntent;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, $"Erreur Stripe lors de la création du PaymentIntent: {ex.Message}");
                throw new Exception($"Erreur lors de la création du paiement: {ex.Message}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur générale lors de la création du PaymentIntent: {ex.Message}");
                throw;
            }
        }
        public async Task<PaymentIntent> ConfirmPaymentIntent(string paymentIntentId)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.ConfirmAsync(paymentIntentId);

                _logger.LogInformation($"Payment confirmé avec succès: {paymentIntentId}");

                return paymentIntent;
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex,$"Erreur Stripe lors de la confirmation du PaymentIntent : {ex.Message}");
                throw new Exception($"Erreur lors de la confirmation du paiement:  {ex.Message}");
                
            }
        }

       

        public async Task<PaymentIntent> GetPaymentIntent(string paymentIntentId)
        {
            try
            {
                var paymentIntent = await _paymentIntentService.GetAsync(paymentIntentId);
                return paymentIntent;

            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, $"Erreur Stripe lors de la récupération du PaymentIntent: {ex.Message}");
                throw new Exception($"Erreur lors de la récupération du paiement: {ex.Message}");
            }
        }
    }
}
