namespace ZeiHomeKitchen_backend.Dtos
{
    public record PlatDto(
        int IdPlat,
        string? Nom,
        string? Description,
        byte[] Image,
        decimal? Prix
        )
    {

        public string ImageBase64 => Convert.ToBase64String(Image);
    }

}
