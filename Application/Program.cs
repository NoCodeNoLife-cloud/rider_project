using Common.Configuration;
using Common.Exception;
using Common.FileSystem.Serializer;
using Common.Log.Serilog;
using Common.Trace;
using Serilog.Events;

namespace Application;

public static class Program
{
    private const string SerilogSettingPath = "Common/Log/Serilog/Serilog-settings.yaml";

    [ConfigService<LogConfiguration, YamlFileSerializer>(SerilogSettingPath, LogEventLevel.Debug)]
    [RecordMethodTime]
    [RecordException(true)]
    public static void Main()
    {
    }
}