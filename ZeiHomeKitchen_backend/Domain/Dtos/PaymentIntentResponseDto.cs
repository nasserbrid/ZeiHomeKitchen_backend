namespace ZeiHomeKitchen_backend.Domain.Dtos
{
    /// <summary>
    /// DTO de réponse pour la création d'un PaymentIntent
    /// </summary>
    public class PaymentIntentResponseDto
    {
        public string ClientSecret { get; set; } = string.Empty;
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "eur";
        public int ReservationId { get; set; }
    }
}
