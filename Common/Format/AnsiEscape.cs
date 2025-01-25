namespace Common.Format;

public abstract class AnsiEscape
{
	public static string Bold(string text)
	{
		return $"\e[1m{text}\e[0m";
	}
}