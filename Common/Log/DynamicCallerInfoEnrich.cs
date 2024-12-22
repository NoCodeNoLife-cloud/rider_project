using Serilog.Core;
using Serilog.Events;

namespace Common.Log;

public class DynamicCallerInfoEnrich(string callerThreadName, string callerMemberName, string callerFileUrl, int callerLineNumber) : ILogEventEnricher
{
	public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
	{
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("ThreadName", callerThreadName));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("MemberName", callerMemberName));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("FileUrl", callerFileUrl));
		logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("LineNumber", callerLineNumber));
	}
}