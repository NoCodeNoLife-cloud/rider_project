namespace Common.Trace;

[Flags]
public enum TraceCalledItem
{
	OnEntry,
	OnExit,
	OnException,
	OnSuccess
}