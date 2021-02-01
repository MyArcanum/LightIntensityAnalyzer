using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightIntensityAnalyzer
{
    public class Brightness
    {
        public Brightness(double intensity)
        {
            Intensity = intensity;
        }

        public static Brightness operator -(Brightness first, Brightness second)
        {
            return new Brightness(first.Intensity - second.Intensity);
        }

        public static bool operator >(Brightness first, Brightness second) => first.Intensity > second.Intensity;

        public static bool operator <(Brightness first, Brightness second) => first.Intensity < second.Intensity;

        public readonly double Intensity;
    }
}
