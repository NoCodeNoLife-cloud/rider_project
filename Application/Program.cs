using Common.Banner;
using Common.Cache;
using Common.Configuration;
using Common.Exception;
using Common.FileSystem.Serializer;
using Common.Log.Serilog;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
	private const string SerilogSettingPath = "Application/Configuration/Serilog-settings.xml";

	[ShowBanner]
	[ConfigService<LogConfiguration, XmlFileSerializer>(SerilogSettingPath, LogEventLevel.Information)]
	[RecordMethodTime(LogEventLevel.Information)]
	[RecordException(true)]
	public static void Main() { }
}