namespace ZeiHomeKitchen_backend.Services
{
    public interface IImagesService
    {
        byte[] OptimizeImage(byte[] imageBytes, int width = 500, int height = 500, int quality = 75);
    }
}
