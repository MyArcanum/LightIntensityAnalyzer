namespace LightIntensityAnalyzer
{
    public class Pixel
    {
        /// <summary>
        /// Hirizontal pixel position
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Vertical pixel position
        /// </summary>
        public int Y { get; set; }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte Intensity
        {
            get
            {
                if (!_Intensity.HasValue)
                    _Intensity = (byte)(0.2126 * R + 0.7152 * G + 0.0722 * B);
                return (byte)_Intensity;
            }
        }

        public bool IsAboveThreshold(byte threshold) => Intensity > threshold;

        private byte? _Intensity;
    }
}
