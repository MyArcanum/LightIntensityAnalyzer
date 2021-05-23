namespace LightIntensityAnalyzer.Reports
{
    public sealed class BackSourcesReport : Report
    {
        public BackSourcesReport(bool isBad)
        {
            if (!isBad)
                return;

            ReportString = "There might be reflections on screen! Turn off intense light behind!";
        }
    }
}
