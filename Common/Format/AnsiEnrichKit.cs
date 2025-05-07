namespace Common.Format;

[Flags]
public enum AnsiStyleEnum
{
    Bold,
    Underline,
    Inverse,
    Strikethrough,
    Bright,
    Invisible,
    Black,
    Red,
    Green,
    Yellow,
    Blue,
    Magenta,
    Cyan,
    White,
    BgBlack,
    BgRed,
    BgGreen,
    BgYellow,
    BgBlue,
    BgMagenta,
    BgCyan,
    BgWhite
}

public abstract class AnsiEnrichKit
{
    private static readonly Dictionary<AnsiStyleEnum, string[]> AnsiStyles = new()
    {
        { AnsiStyleEnum.Bold, ["\e[1m", "\e[0m"] },
        { AnsiStyleEnum.Underline, ["\e[4m", "\e[0m"] },
        { AnsiStyleEnum.Inverse, ["\e[7m"] },
        { AnsiStyleEnum.Strikethrough, ["\e[9m", "\e[0m"] },
        { AnsiStyleEnum.Bright, ["\e[1m", "\e[22m"] },
        { AnsiStyleEnum.Invisible, ["\e[8m", "\e[0m"] },
        { AnsiStyleEnum.Black, ["\e[30m", "\e[39m"] },
        { AnsiStyleEnum.Red, ["\e[31m", "\e[39m"] },
        { AnsiStyleEnum.Green, ["\e[32m", "\e[39m"] },
        { AnsiStyleEnum.Yellow, ["\e[33m", "\e[39m"] },
        { AnsiStyleEnum.Blue, ["\e[34m", "\e[39m"] },
        { AnsiStyleEnum.Magenta, ["\e[35m", "\e[39m"] },
        { AnsiStyleEnum.Cyan, ["\e[36m", "\e[39m"] },
        { AnsiStyleEnum.White, ["\e[37m", "\e[39m"] },
        { AnsiStyleEnum.BgBlack, ["\e[40m", "\e[49m"] },
        { AnsiStyleEnum.BgRed, ["\e[41m", "\e[49m"] },
        { AnsiStyleEnum.BgGreen, ["\e[42m", "\e[49m"] },
        { AnsiStyleEnum.BgYellow, ["\e[43m", "\e[49m"] },
        { AnsiStyleEnum.BgBlue, ["\e[44m", "\e[49m"] },
        { AnsiStyleEnum.BgMagenta, ["\e[45m", "\e[49m"] },
        { AnsiStyleEnum.BgCyan, ["\e[46m", "\e[49m"] },
        { AnsiStyleEnum.BgWhite, ["\e[47m", "\e[49m"] }
    };

    public static string RenderString(string target, AnsiStyleEnum[] styles)
    {
        var res = target;
        foreach (var style in styles) res = AnsiStyles[style][0] + res + AnsiStyles[style][1];

        return res;
    }
}