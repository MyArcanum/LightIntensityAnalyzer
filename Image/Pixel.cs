namespace LightIntensityAnalyzer.Imaging
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
                    _Intensity = (byte)(0.299 * R + 0.587 * G + 0.114 * B);
                return (byte)_Intensity;
            }
        }

        public Pixel ToGray()
        {
            var shade = Intensity;
            var gray = new Pixel();
            gray.R = gray.G = gray.B = shade;
            return gray;
        }

        private byte? _Intensity;
    }
}
