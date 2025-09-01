using property.Domain.Interfaces.Handlers;
using SixLabors.ImageSharp.Formats;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;

namespace property.Application.Handlers
{
    public class ImageHandler : IImageHandler
    {
        public bool ValidateImage(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
                return false;

            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64Image);

                using var memory = new MemoryStream(imageBytes);
                using var image = System.Drawing.Image.FromStream(memory);

                return image.RawFormat.Equals(ImageFormat.Jpeg)
                    || image.RawFormat.Equals(ImageFormat.Png);
            }
            catch
            {
                return false;
            }
        }
    }
}
