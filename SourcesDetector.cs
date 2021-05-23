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

        public static BackSourcesReport AnalyzeBackground(Image binary, IEnumerable<Pixel> frontPlane)
        {
            //var frNum = frontPlane.Count();
            //var frDN = frontPlane.Distinct().Count();

            var frontBin = new List<Pixel>();
            //foreach(var fp in frontPlane)
            //{
            //    var front = binary.FirstOrDefault(p => p.X == fp.X
            //                                        && p.Y == fp.Y);
            //    if (front != null)
            //        frontBin.Add(front);
            //}
            //var backPx = binary.Except(frontBin);
            //var overThreshold = backPx.Where(bb => bb.Intensity > 0);

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
