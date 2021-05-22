namespace LightIntensityAnalyzer
{
    public class BrightnessReport
    {
        public BrightnessReport(Brightness brightness, bool isBad)
        {
            if (!isBad)
                return;

            if (brightness > 0)
                Report = "The room is too dark! Switch on the illumination!";
            else
                Report = "The room is too bright! Switch off the illumination!";
        }

        public override string ToString()
        {
            return Report;
        }

        private readonly string Report = string.Empty;
    }
}
