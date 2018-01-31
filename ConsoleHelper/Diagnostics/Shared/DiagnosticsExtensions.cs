using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleHelper.Diagnostics.Entities;
using ConsoleHelper.Diagnostics.Interface;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using SimpleInjector;

namespace ConsoleHelper.Diagnostics.Shared
{
    public static class DiagnosticsExtensions
    {
        private static bool _isTelemetryRegistered = false;

        /// <summary>
        /// Adding AppInsights Logger without IApplicationBuilder middleware
        /// </summary>
        /// <param name="container"></param>
        public static Container AddApplicationInsightsUsingSimpleInjector(this Container container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            container.RegisterSingleton(typeof(ILogger<>), typeof(ApplicationInsightsLogger<>));
            return container;
        }

        /// <summary>
        /// Add options like instrumentation key and log levels 
        /// </summary>
        /// <param name="container"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static Container AddAppInsightsLoggingOptions(this Container container, AppInsightsOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (!_isTelemetryRegistered)
                container.RegisterSingleton(new TelemetryClient(new TelemetryConfiguration(options.InstrumentationKey)));
            container.RegisterSingleton(typeof(LogOptions), options);
            return container;
        }
    }
}
