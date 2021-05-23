using LightIntensityAnalyzer.Imaging;
using LightIntensityAnalyzer.Reports;
using System.Collections.Generic;
using System.Linq;
using Windows.Media.FaceAnalysis;

namespace LightIntensityAnalyzer
{
    public class SourcesDetector
    {
        public static Image GetBinary(Image img, int thresholdOverride)
        {
            var binaryImage = Threshold.Apply(img, thresholdOverride);

            return binaryImage;
        }

        public static Report AnalyzeBackground(Image binary, IEnumerable<Pixel> frontPlane)
        {
            var frontBin = new List<Pixel>();

            var intense = binary.Where(b => b.Intensity > 0);
            foreach(var i in intense)
            {
                var front = frontPlane.FirstOrDefault(p => p.X == i.X
                                                        && p.Y == i.Y);
                if (front != null)
                    frontBin.Add(front);
            }
            var backPx = intense.Except(frontBin);
            var rate = (double)backPx.Count() / binary.Count();
            return new BackSourcesReport(rate > 0.01);
        }

        public static Report AnalyzeFace(Image binary, IEnumerable<Pixel> frontPlane)
        {
            var frontBin = new List<Pixel>();

            var intense = binary.Where(b => b.Intensity > 0);
            foreach (var i in intense)
            {
                var front = frontPlane.FirstOrDefault(p => p.X == i.X
                                                        && p.Y == i.Y);
                if (front != null)
                    frontBin.Add(front);
            }
            return new FaceSourcesReport(frontBin.Any());
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
