using common.Configuration;

namespace common.Application;

public static class Program
{
	private static void Configure()
	{
		List<IConfigurable> configurableList =
		[
			new LogConfiguration(),
		];
		ConfigurationManager.Configure(configurableList);
	}

	public static void Main()
	{
		using var programTimer = new ProgramTimer();
		Configure();
		MainTask();
	}

	private static void MainTask() { }
}