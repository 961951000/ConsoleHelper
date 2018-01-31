using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper.Diagnostics.Entities
{
    /// <summary>
    /// AppInsightsOptions class 
    /// </summary>
    public class AppInsightsOptions : LogOptions
    {
        /// <summary>
        /// Instrumentation Key of the App Insights Repo
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}
