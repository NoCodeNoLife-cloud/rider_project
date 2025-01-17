using System.Text;
using Rougamo;
using Rougamo.Context;

namespace Common.Banner;

public class ShowBanner : MoAttribute
{
	private const string RootPath = "../../../../";

	public override void OnEntry(MethodContext context)
	{
		var content = File.ReadAllText(RootPath + "Common/Banner/Banner.txt", Encoding.UTF8);
		Console.OutputEncoding = Encoding.UTF8;
		Console.WriteLine(content, Console.ForegroundColor = ConsoleColor.Green);
		Console.ResetColor();
		base.OnEntry(context);
	}
}