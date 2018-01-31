using ConsoleHelper.Diagnostics.Entities;
using ConsoleHelper.Diagnostics.Shared;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNet.SignalR;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHelper.App_Start
{
    public static class CompositionRoot
    {
        public static void Initialize()
        {
            InitializeContainer();
        }

        private static void InitializeContainer()
        {
            var container = new Container();
            RegisterServices(container);

            container.Verify();

            var resolver = new SimpleInjectorResolver(container);
            GlobalHost.DependencyResolver = resolver;
        }

        private static void RegisterServices(Container container)
        {
            var appInsightsOptions = new AppInsightsOptions { LogLevel = (SeverityLevel)Enum.Parse(typeof(SeverityLevel), ConfigurationManager.AppSettings["LogLevel"]), InstrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"] };
            container.AddApplicationInsightsUsingSimpleInjector().AddAppInsightsLoggingOptions(appInsightsOptions);
        }
    }
}
