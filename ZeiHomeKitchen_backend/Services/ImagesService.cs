using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ZeiHomeKitchen_backend.Services
{
    public class ImagesService : IImagesService
    {
        public byte[] OptimizeImage(byte[] imageBytes, int width = 500, int height = 500, int quality = 75)
        {
            //Je charge mon image
            using (var image = Image.Load(imageBytes))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    //Ne déforme pas l'image mais ajuste aux dimension max
                    Mode = ResizeMode.Max
                }));

                // Ajouter un effet d’arrondi aux coins avec ImageSharp.Drawing
                // Rayon de 20px
                ApplyRoundedCorners(image, 20);

          

                using (var outputStream = new MemoryStream())
                {
                    // Compression JPEG (75% de qualité par défaut)
                    var encoder = new JpegEncoder { Quality = quality };

                    // Sauvegarde l'image optimisée
                    image.Save(outputStream, encoder);

                    // Retourner l'image optimisée en byte[]
                    return outputStream.ToArray();

                }
            }
        }

        private void ApplyRoundedCorners(Image image, float cornerRadius)
        {
            var cornerShape = new RectangularPolygon(0,0, image.Width, image.Height)
                .Clip(new EllipsePolygon(cornerRadius, cornerRadius, image.Width - cornerRadius, image.Height - cornerRadius));

            image.Mutate(x => x.Fill(Color.White, cornerShape));
        }
    }
}
