using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper.Diagnostics.Entities
{
    public abstract class LogOptions
    {
        /// <summary>
        /// Sets a category of logs 
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// The minimum or higher log level for logging to occur
        /// </summary>
        public LogLevel LogLevel { get; set; }
    }
}
