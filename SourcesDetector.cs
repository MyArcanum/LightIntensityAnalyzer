using LightIntensityAnalyzer.Imaging;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.FaceAnalysis;

namespace LightIntensityAnalyzer
{
    public class SourcesDetector
    {
        public (Image gray, Image bin) Detect(Image img)
        {
            var grayImage = Threshold.Grayscale(img);
            var binaryImage = Threshold.Apply(img);

            return (grayImage, binaryImage);
        }

        public (double x, double y) FindCenterFront(Image binaryImage, IEnumerable<DetectedFace> faces) 
        {
            var frontPixels = faces.SelectMany(face => binaryImage.Where(pixel => pixel.X > face.FaceBox.X
                                                                               && pixel.X < face.FaceBox.X + face.FaceBox.Width
                                                                               && pixel.Y > face.FaceBox.Y
                                                                               && pixel.Y < face.FaceBox.Y + face.FaceBox.Height));
            var avgX = frontPixels.Average(pixel => pixel.X);
            var avgY = frontPixels.Average(pixel => pixel.Y);

            return (avgX, avgY);
        }

        public (double x, double y) FindCenterBack(Image binaryImage, IEnumerable<DetectedFace> faces)
        {
            var frontPixels = faces.SelectMany(face => binaryImage.Where(pixel =>!(pixel.X > face.FaceBox.X
                                                                               && pixel.X < face.FaceBox.X + face.FaceBox.Width
                                                                               && pixel.Y > face.FaceBox.Y
                                                                               && pixel.Y < face.FaceBox.Y + face.FaceBox.Height)));
            var avgX = frontPixels.Average(pixel => pixel.X);
            var avgY = frontPixels.Average(pixel => pixel.Y);

            return (avgX, avgY);
        }
    }
}
