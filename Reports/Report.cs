namespace LightIntensityAnalyzer.Reports
{
    public abstract class Report
    {
        public override string ToString()
        {
            return ReportString;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(ReportString);
        }

        protected string ReportString = string.Empty;
    }
}
