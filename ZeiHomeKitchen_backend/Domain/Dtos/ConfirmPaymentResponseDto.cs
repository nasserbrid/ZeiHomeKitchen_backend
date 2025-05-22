namespace ZeiHomeKitchen_backend.Domain.Dtos
{
    /// <summary>
    /// DTO de réponse pour la confirmation de paiement
    /// </summary>
    public class ConfirmPaymentResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PaiementDto? Paiement { get; set; }
    }
}
