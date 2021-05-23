using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightIntensityAnalyzer.Reports
{
    public sealed class FaceSourcesReport : Report
    {
        public FaceSourcesReport(bool isBad)
        {
            if (!isBad)
                return;

            ReportString = "You are blinded! Switch off the lamp in front of you!";
        }
    }
}
