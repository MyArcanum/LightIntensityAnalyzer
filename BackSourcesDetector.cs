using LightIntensityAnalyzer.Imaging;
using System.Collections.Generic;
using System.Linq;

namespace LightIntensityAnalyzer
{
    public class BackSourcesDetector
    {
        public bool Detect(IEnumerable<Pixel> backPixels)
        {
            return backPixels.Any(pixel => pixel.IsAboveThreshold(IntensityThreshold));
        }

        private byte IntensityThreshold = 250;
    }
}
