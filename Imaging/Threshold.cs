using System;
using System.Collections.Generic;
using System.Linq;

namespace LightIntensityAnalyzer.Imaging
{
    public static class Threshold
    {
        public static Image Apply(Image grayImage)
        {
            var threshold = GetOtsuThreshold(grayImage);
            var binaryImg = ApplyThreshold(grayImage, threshold);
            return binaryImg;
        }

        private static Image ApplyThreshold(Image grayImg, int thresholdValue)
        {
            var maxVal = 768;
            if (thresholdValue < 0) throw new Exception($"Thresholding is kinda broken. Threshold value is < 0. Actual value is @{thresholdValue}");
            if (thresholdValue > maxVal) throw new Exception($"Thresholding is kinda broken. Threshold value is > 768 (max).Actual value is @{thresholdValue}");

            var binaryPixels = new List<Pixel>();

            foreach(var grayPixel in grayImg)
            {
                var binary = new Pixel();
                binary.X = grayPixel.X;
                binary.Y = grayPixel.Y;
                if(grayPixel.Intensity * 3 > thresholdValue)
                    binary.R = binary.G = binary.B = 255;
                else
                    binary.R = binary.G = binary.B = 0;
                binaryPixels.Add(binary);
            }
            return new Image(grayImg.H, binaryPixels);
        }

        public static Image Grayscale(Image img)
        {
            var greys = new List<Pixel>();

            foreach (var p in img)
                greys.Add(p.ToGray());

            return new Image(img.H, greys);
        }

        private static float Px(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;

            for (i = init; i <= end; i++)
                sum += hist[i];

            return (float)sum;
        }

        private static float Mx(int init, int end, int[] hist)
        {
            int sum = 0;
            int i;

            for (i = init; i <= end; i++)
                sum += i * hist[i];

            return (float)sum;
        }

        private static int[] GetHistogram(Image grayImg)
        {
            var hist = new int[256];
            hist.Initialize();

            foreach (var pixel in grayImg)
                hist[pixel.Intensity]++;

            return hist;
        }

        private static int FindIndexOfMax(IEnumerable<float> vector)
        {
            var indexOfMax = 0;
            var max = vector.First();
            for (var i = 0; i < vector.Count(); i++)
            {
                if (vector.ElementAt(i) > max)
                {
                    max = vector.ElementAt(i);
                    indexOfMax = i;
                }
            }
            return indexOfMax;
        }

        private static int GetOtsuThreshold(Image img)
        {
            var vet = new float[256];
            vet.Initialize();

            float p1, p2, p12;
            int k;

            var histogram = GetHistogram(img);

            for (k = 1; k != 255; k++)
            {
                p1 = Px(0, k, histogram);
                p2 = Px(k + 1, 255, histogram);
                p12 = p1 * p2;
                if (p12 == 0)
                    p12 = 1;
                float diff = (Mx(0, k, histogram) * p2) - (Mx(k + 1, 255, histogram) * p1);
                vet[k] = (float)diff * diff / p12;
            }

            return FindIndexOfMax(vet);
        }
    }
}
