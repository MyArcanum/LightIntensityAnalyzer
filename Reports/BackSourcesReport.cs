using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
