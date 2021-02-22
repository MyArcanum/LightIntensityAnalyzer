using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace LightIntensityAnalyzer.Imaging
{
    public static class ToUsableBitmapConverter
    {
        public static async Task<Image> Convert(SoftwareBitmap softwareBitmap)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.BmpEncoderId, stream);
                encoder.SetSoftwareBitmap(softwareBitmap);
                await encoder.FlushAsync();
                return await SelectPixels(softwareBitmap, stream.AsStream());
            }
        }

        public static async Task<Image> SelectPixels(SoftwareBitmap image, Stream bufferStream)
        {
            var n = image.PixelHeight * image.PixelWidth;
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                await bufferStream.CopyToAsync(ms);
                buffer = ms.ToArray();
            }
            buffer = buffer.Skip(54).ToArray();
            var pixels = new List<Pixel>();
            var bytesPerPixel = buffer.Length / n;

            for(var i = 0; i < image.PixelHeight; i++)
            {
                for(var j = 0; j < image.PixelWidth; j++)
                {
                    var pixelPosition = i * image.PixelWidth * bytesPerPixel + j * bytesPerPixel;
                    pixels.Add(new Pixel
                    {
                        B = buffer[pixelPosition],
                        G = buffer[pixelPosition + 1],
                        R = buffer[pixelPosition + 2],
                        X = j,
                        Y = i
                    });
                }
            }
            return new Image(image.PixelHeight, pixels);
        }

    }
}
