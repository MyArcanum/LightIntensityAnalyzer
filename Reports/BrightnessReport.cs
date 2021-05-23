namespace LightIntensityAnalyzer.Reports
{
    public sealed class BrightnessReport : Report
    {
        public BrightnessReport(Brightness brightness, bool isBad)
        {
            if (!isBad)
                return;

            if (brightness > 0)
                ReportString = "The room is too dark! Switch on the illumination!";
            else
                ReportString = "The room is too bright! Switch off the illumination!";
        }
    }
}
