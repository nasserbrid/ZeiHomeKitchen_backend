namespace ZeiHomeKitchen_backend.Application.Ports
{
    public interface IImagesService
    {
        byte[] OptimizeImage(byte[] imageBytes, int width = 500, int height = 500, int quality = 90);
    }
}
