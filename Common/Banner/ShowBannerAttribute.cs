using System.Text;
using Rougamo;
using Rougamo.Context;

namespace Common.Banner;

[AttributeUsage(AttributeTargets.Method)]
public class ShowBannerAttribute : MoAttribute
{
    private const string RootPath = "../../../../";

    public override void OnEntry(MethodContext context)
    {
        var content = File.ReadAllText(RootPath + "Common/Banner/Banner.txt", Encoding.UTF8);
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine(content, Console.ForegroundColor = ConsoleColor.Magenta);
        Console.ResetColor();
        base.OnEntry(context);
    }
}