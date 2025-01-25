namespace Common.Trace;

[Flags]
public enum TracedItem
{
	OnEntry,
	OnExit,
	OnException,
	OnSuccess
}