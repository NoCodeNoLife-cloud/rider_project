namespace Common.Trace;

[Flags]
public enum TracedItemEnum
{
	OnEntry,
	OnExit,
	OnException,
	OnSuccess
}