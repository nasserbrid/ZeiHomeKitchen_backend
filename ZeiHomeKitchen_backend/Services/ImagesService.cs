using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZeiHomeKitchen_backend.Dtos;

namespace ZeiHomeKitchen_backend.Services
{
    public class ImagesService : IImagesService
    {
        public byte[] OptimizeImage(byte[] imageBytes, int width = 500, int height = 500, int quality = 90)
        {
            //Je charge mon image
            using (var image = Image.Load(imageBytes))
            {
                //image.Mutate(x => x.Resize(new ResizeOptions
                //{
                //    Size = new Size(width, height),
                //    //Ne déforme pas l'image mais ajuste aux dimension max
                //    Mode = ResizeMode.Max

                //}));

                // Calculer le ratio de recadrage pour obtenir un carré
                float sourceRatio = (float)image.Width / image.Height;

                // Définir les dimensions de recadrage
                int cropWidth, cropHeight;
                if (sourceRatio > 1)
                {
                    // Image plus large que haute
                    cropHeight = image.Height;
                    cropWidth = (int)(cropHeight * 1); // Carré 1:1
                }
                else
                {
                    // Image plus haute que large
                    cropWidth = image.Width;
                    cropHeight = (int)(cropWidth * 1); // Carré 1:1
                }

                // Recadrer au centre
                var cropRectangle = new Rectangle(
                    (image.Width - cropWidth) / 2,
                    (image.Height - cropHeight) / 2,
                    cropWidth,
                    cropHeight
                );

                image.Mutate(x => x
                    .Crop(cropRectangle) // Recadrer au centre
                    .Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    })
                );


                // Ajouter un effet d’arrondi aux coins avec ImageSharp.Drawing
                // Rayon de 20px
                ApplyRoundedCorners(image, 20);


                // Log des nouvelles dimensions
                Console.WriteLine($"Image redimensionnée : {image.Width}x{image.Height}");

                using (var outputStream = new MemoryStream())
                {
                    // Compression JPEG (75% de qualité par défaut)
                    var encoder = new JpegEncoder { Quality = quality };

                    // Sauvegarde l'image optimisée
                    image.Save(outputStream, encoder);

                    // Retourner l'image optimisée en byte[]
                    byte[] finalBytes = outputStream.ToArray();
                    Console.WriteLine($"Image finale : {image.Width}x{image.Height}, Taille : {finalBytes.Length} octets");

                    // Convertir l'image en Base64 pour l'envoi
                    string base64Image = Convert.ToBase64String(finalBytes);
                    Console.WriteLine($"Base64 envoyé (longueur) : {base64Image.Length}"); // Log de la longueur de la chaîne Base64

                    
                    return finalBytes;

                }
            }
        }

        //private void ApplyRoundedCorners(Image image, float cornerRadius)
        //{
        //    var cornerShape = new RectangularPolygon(0, 0, image.Width, image.Height)
        //        .Clip(new EllipsePolygon(cornerRadius, cornerRadius, image.Width - cornerRadius, image.Height - cornerRadius));

        //    image.Mutate(x => x.Fill(Color.White, cornerShape));




        //}

        //private void ApplyRoundedCorners(Image image, float cornerRadius)
        //{
        //    // Créer un masque blanc avec des coins arrondis
        //    var brush = Brushes.Solid(Color.White);

        //    // Dessiner un rectangle avec des coins arrondis 
        //    var options = new DrawingOptions
        //    {
        //        GraphicsOptions = new GraphicsOptions
        //        {
        //            AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
        //        }
        //    };

        //    // Utiliser RectangularPolygon avec un rayon de coin
        //    var roundedRect = new RectangularPolygon(0, 0, image.Width, image.Height)
        //        .Clip(new EllipsePolygon(cornerRadius, cornerRadius,
        //              image.Width - 2 * cornerRadius, image.Height - 2 * cornerRadius));

        //    image.Mutate(x => x.Fill(options, brush, roundedRect));
        //}

        private void ApplyRoundedCorners(Image image, float cornerRadius)
        {
            var brush = Brushes.Solid(Color.White);

            var options = new DrawingOptions
            {
                GraphicsOptions = new GraphicsOptions
                {
                    AlphaCompositionMode = PixelAlphaCompositionMode.DestOut
                }
            };

            var roundedRect = new RectangularPolygon(0, 0, image.Width, image.Height)
                .Clip(new EllipsePolygon(cornerRadius, cornerRadius,
                      image.Width - 2 * cornerRadius, image.Height - 2 * cornerRadius));

            image.Mutate(x => x.Fill(options, brush, roundedRect));
        }


    }
}
