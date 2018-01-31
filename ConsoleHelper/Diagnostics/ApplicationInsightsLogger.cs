using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleHelper.Diagnostics.Entities;
using ConsoleHelper.Diagnostics.Interface;
using ConsoleHelper.Diagnostics.Shared;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace ConsoleHelper.Diagnostics
{
    public class ApplicationInsightsLogger<T> : ILogger<T>
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly LogOptions _options;

        public ApplicationInsightsLogger(TelemetryClient client, LogOptions options)
        {
            _telemetryClient = client ?? throw new ArgumentNullException(nameof(client));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void Log<TState>(SeverityLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;
            var logEntry = state as LogEntry;
            if (exception == null)
            {
                LogTrace(logLevel, logEntry);
            }
            else
            {
                LogException(logLevel, state, exception, formatter);
            }
        }

        public bool IsEnabled(SeverityLevel logLevel)
        {
            return logLevel >= _options.LogLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        #region Private Methods
        private void LogException<TState>(SeverityLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var exceptionTelemetry = new ExceptionTelemetry(exception)
            {
                Message = formatter(state, exception),
                SeverityLevel = logLevel
            };
            exceptionTelemetry.Context.Properties["Exception"] = exception.ToString();
            _telemetryClient.TrackException(exceptionTelemetry);
        }

        private void LogTrace(SeverityLevel logLevel, LogEntry logEntry)
        {
            if (logEntry == null)
            {
                _telemetryClient.TrackTrace(DefaultValues.StateNotSetToLogEntry, logLevel, PopulateLogAttributes());
            }
            else
            {
                _telemetryClient.TrackTrace(logEntry.Message, logLevel, PopulateLogAttributes(logEntry));
            }
        }

        private static Dictionary<string, string> PopulateLogAttributes(LogEntry logEntry = null)
        {
            var attributes = new Dictionary<string, string>
            {
                {DefaultValues.TypeNameKey , nameof(T) }
            };
            if (logEntry?.CustomLogAttributes != null)
            {
                attributes = logEntry.CustomLogAttributes as Dictionary<string, string>;
                attributes?.Add(DefaultValues.CorrelationId, logEntry.CorrelationId);
                attributes?.Add(DefaultValues.CallerMethodName, logEntry.CallerMethodName);
            }
            return attributes;
        }

        #endregion
    }
}