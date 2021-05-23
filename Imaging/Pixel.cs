namespace LightIntensityAnalyzer.Imaging
{
    /// <summary>
    /// Representation of the dot of a picture
    /// </summary>
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

        /// <summary>
        /// Red channel 
        /// </summary>
        public byte R { get; set; }
        /// <summary>
        /// Green channel 
        /// </summary>
        public byte G { get; set; }
        /// <summary>
        /// Blue channel 
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// Intensity of pixel according to human color perception
        /// </summary>
        public byte Intensity
        {
            get
            {
                if (!_Intensity.HasValue)
                    _Intensity = (byte)(0.299 * R + 0.587 * G + 0.114 * B);
                return (byte)_Intensity;
            }
        }

        /// <summary>
        /// Converts colorful pixel to grayscale shade
        /// </summary>
        /// <returns></returns>
        public Pixel ToGray()
        {
            var gray = new Pixel();
            gray.X = X;
            gray.Y = Y;
            gray._Intensity = gray.R = gray.G = gray.B = Intensity;
            return gray;
        }

        private byte? _Intensity;
    }
}
