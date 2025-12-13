using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DBUtils
{
    public enum LogLevel
    {
        Info,
        Warn,
        Error,
        Debug
    }

    public class SimpleLogger
    {
        private readonly string? _logFilePath;
        private readonly object _lock = new();

        public SimpleLogger(string? logFilePath = null)
        {
            _logFilePath = logFilePath;
            if (_logFilePath != null)
            {
                var dir = Path.GetDirectoryName(_logFilePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        public void Info(string message) => Write(LogLevel.Info, message);
        public void Warn(string message) => Write(LogLevel.Warn, message);
        public void Error(string message) => Write(LogLevel.Error, message);
        public void Debug(string message) => Write(LogLevel.Debug, message);

        public void Error(Exception ex, string context = "")
        {
            var msg = string.IsNullOrEmpty(context)
                ? $"{ex.Message}\n{ex.StackTrace}"
                : $"[{context}] {ex.Message}\n{ex.StackTrace}";
            Write(LogLevel.Error, msg);
        }

        private void Write(LogLevel level, string message)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var formatted = $"[{timestamp}] [{level}] {message}";

            lock (_lock)
            {
                SetConsoleColor(level);
                Console.WriteLine(formatted);
                Console.ResetColor();

                if (!string.IsNullOrEmpty(_logFilePath))
                    File.AppendAllText(_logFilePath, formatted + Environment.NewLine);
            }
        }

        private static void SetConsoleColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                default:
                    Console.ResetColor();
                    break;
            }
        }

        public async Task WriteAsync(LogLevel level, string message, CancellationToken token = default)
        {
            await Task.Run(() => Write(level, message), token);
        }
    }
}
