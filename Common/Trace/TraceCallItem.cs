namespace Common.Trace;

[Flags]
public enum TraceCallItem
{
	OnEntry,
	OnExit,
	OnException,
	OnSuccess
}