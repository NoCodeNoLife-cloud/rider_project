using System.Diagnostics;

namespace common.Application;

public class ProgramTimer : IDisposable
{
	private readonly Stopwatch _stopwatch;
	private bool _disposed;

	public ProgramTimer()
	{
		_stopwatch = new Stopwatch();
		_stopwatch.Start();
		_disposed = false;
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (_disposed) return;
		if (disposing)
		{
			_stopwatch.Stop();
			Log.LoggerExtensions.LogWithCallerInfo(Serilog.Log.Logger, $"Program finished in {_stopwatch.ElapsedMilliseconds} ms.");
		}

		_disposed = true;
	}

	~ProgramTimer()
	{
		Dispose(false);
	}
}