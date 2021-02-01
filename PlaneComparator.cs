using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;

namespace LightIntensityAnalyzer
{
    public class PlaneComparator
    {
        public async Task<BrightnessReport> CompareBackAndFrontAsync(SoftwareBitmap image, IEnumerable<DetectedFace> faces)
        {
            var (front, back) = await GetAvgBrightness(image, faces);
            if (front is null || back is null)
                return null;
            return Compare(front, back);
        }

        private BrightnessReport Compare(Brightness front, Brightness back)
        {
            var difference = front - back;
            var text = new StringBuilder();
            if (difference > MaxBrightnessDifference)
                text.Append("Bad");
            else text.Append("Good");
            return new BrightnessReport(difference, text.ToString());
        }

        public static async Task<(Brightness, Brightness)> GetAvgBrightness(SoftwareBitmap image, IEnumerable<DetectedFace> faces)
        {
            var backPixels = new List<Pixel>();
            var frontPixels = new List<Pixel>();

            var pixels = await ToUsableBitmapConverter.Convert(image);
            foreach (var pixel in pixels)
            {
                bool isPixelForegound = false;
                foreach (var face in faces)
                {
                    isPixelForegound = pixel.Y > face.FaceBox.Y && pixel.Y < face.FaceBox.Y + face.FaceBox.Height
                                     && pixel.X > face.FaceBox.X && pixel.X < face.FaceBox.X + face.FaceBox.Width;
                }
                if (isPixelForegound)
                    frontPixels.Add(pixel);
                else backPixels.Add(pixel);
            }
            var avgFront = GetAvg(frontPixels);
            if (avgFront is null)
                return (null, null);
            var avgBack = GetAvg(backPixels);
            return (avgFront, avgBack);
        }

        private static Brightness GetAvg(IEnumerable<Pixel> pixels)
        {
            if (!pixels.Any())
                return null;
            var avgBrightness = pixels.Select(pixel => (int)pixel.Intensity).Average();
            return new Brightness(avgBrightness);
        }

        // declares horisontal dinstance between piels to analyze
        private static readonly int PixelRate = 10;

        private readonly Brightness MaxBrightnessDifference = new Brightness(100f);
    }
}
