using System.ComponentModel.DataAnnotations;

namespace ZeiHomeKitchen_backend.Domain.Dtos
{
    /// <summary>
    /// DTO pour la création d'un PaymentIntent Stripe
    /// </summary>
    public class CreatePaymentIntentRequestDto
    {
        [Required]
        public int ReservationId { get; set; }

        public string? Description { get; set; }

        public string Currency { get; set; } = "eur";
    }
}
