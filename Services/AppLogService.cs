using System.Collections.Concurrent;

namespace SleepSightPro.Services;

/// <summary>
/// 应用级运行日志服务：文件持久化 + 自动轮转 + 异步写入
/// 用法: AppLogService.Instance.Info("message");
/// </summary>
public sealed class AppLogService : IDisposable
{
    public static AppLogService Instance { get; } = new();

    private readonly string _logDir;
    private readonly string _logFile;
    private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
    private readonly int _maxBackupCount = 3;

    private readonly BlockingCollection<LogEntry> _queue = new(new ConcurrentQueue<LogEntry>());
    private readonly CancellationTokenSource _cts = new();
    private StreamWriter? _writer;
    private long _currentSize;

    private AppLogService()
    {
        _logDir = Path.Combine(AppContext.BaseDirectory, "logs");
        _logFile = Path.Combine(_logDir, "app.log");
        Directory.CreateDirectory(_logDir);
        OpenWriter();
        Task.Run(() => WriteLoop(_cts.Token));
    }

    public void Info(string message) => Enqueue(LogLevel.Info, message);
    public void Warning(string message) => Enqueue(LogLevel.Warning, message);
    public void Error(string message) => Enqueue(LogLevel.Error, message);
    public void Error(Exception ex, string? context = null)
    {
        var msg = context != null ? $"{context}: {ex}" : ex.ToString();
        Enqueue(LogLevel.Error, msg);
    }

    private void Enqueue(LogLevel level, string message)
    {
        try { _queue.Add(new LogEntry(level, message)); }
        catch (InvalidOperationException) { /* queue closed */ }
    }

    private void WriteLoop(CancellationToken ct)
    {
        try
        {
            foreach (var entry in _queue.GetConsumingEnumerable(ct))
            {
                RotateIfNeeded();
                var line = $"{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{entry.Level}] {entry.Message}";
                _writer!.WriteLine(line);
                _writer.Flush();
                _currentSize += System.Text.Encoding.UTF8.GetByteCount(line) + Environment.NewLine.Length;
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"[AppLog] WriteLoop failed: {ex}"); }
    }

    private void OpenWriter()
    {
        _writer?.Dispose();
        _writer = new StreamWriter(_logFile, append: true, encoding: System.Text.Encoding.UTF8)
        { AutoFlush = false };
        _currentSize = File.Exists(_logFile) ? new FileInfo(_logFile).Length : 0;
    }

    private void RotateIfNeeded()
    {
        if (_currentSize < _maxFileSize) return;

        _writer?.Flush();
        _writer?.Dispose();
        _writer = null;

        // 轮转: app.log → app.1.log → app.2.log → app.3.log
        var oldest = Path.Combine(_logDir, $"app.{_maxBackupCount}.log");
        if (File.Exists(oldest)) File.Delete(oldest);

        for (int i = _maxBackupCount - 1; i >= 1; i--)
        {
            var src = Path.Combine(_logDir, $"app.{i}.log");
            var dst = Path.Combine(_logDir, $"app.{i + 1}.log");
            if (File.Exists(src)) File.Move(src, dst);
        }

        var firstBackup = Path.Combine(_logDir, "app.1.log");
        if (File.Exists(_logFile)) File.Move(_logFile, firstBackup);

        _writer = new StreamWriter(_logFile, append: false, encoding: System.Text.Encoding.UTF8) { AutoFlush = false };
        _currentSize = 0;
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _cts.Cancel();
        try { _cts.Dispose(); } catch { }
        _writer?.Flush();
        _writer?.Dispose();
    }

    private enum LogLevel { Info, Warning, Error }

    private readonly record struct LogEntry(LogLevel Level, string Message)
    {
        public DateTime Timestamp { get; } = DateTime.Now;
    }
}
