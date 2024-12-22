using Serilog;

namespace common.Configuration;

public class LogConfiguration : IConfigurable
{
	public void Configure()
	{
		Serilog.Log.Logger = new LoggerConfiguration()
			.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] ({FileUrl}:{LineNumber}) [{ThreadName}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
			.CreateLogger();
	}
}