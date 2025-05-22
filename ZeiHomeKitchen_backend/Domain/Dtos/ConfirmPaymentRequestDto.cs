using System.ComponentModel.DataAnnotations;

namespace ZeiHomeKitchen_backend.Domain.Dtos
{
    /// <summary>
    ///DTO pour la confirmation d'un paiement Stripe 
    /// </summary>
    public class ConfirmPaymentRequestDto
    {
        [Required]
        public string PaymentIntentId { get; set; } = string.Empty;

        [Required]
        public int ReservationId { get; set; }
    }
}
