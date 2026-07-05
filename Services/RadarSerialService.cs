using System.IO.Ports;
using SleepSightPro.Models;

namespace SleepSightPro.Services;

/// <summary>
/// 串口通信服务 - 管理雷达模组的串口通信
/// </summary>
public class RadarSerialService : IDisposable
{
    private SerialPort? _serialPort;
    private readonly FrameParser _parser = new();
    private CancellationTokenSource? _readCts;

    public event EventHandler<RadarFrame>? FrameReceived;
    public event EventHandler<byte[]>? RawDataReceived;
    public event EventHandler<string>? StatusChanged;
    public event EventHandler<Exception>? ErrorOccurred;

    public bool IsConnected => _serialPort?.IsOpen ?? false;
    public string PortName => _serialPort?.PortName ?? "";
    public int BaudRate => _serialPort?.BaudRate ?? 0;

    public string[] GetAvailablePorts() => SerialPort.GetPortNames();

    public void Connect(string portName, int baudRate = 115200)
    {
        if (_serialPort?.IsOpen == true)
            Disconnect();

        _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
        {
            ReadTimeout = 500,
            WriteTimeout = 500,
            ReadBufferSize = 4096
        };

        try
        {
            _serialPort.Open();
            _readCts = new CancellationTokenSource();
            _ = ReadLoopAsync(_readCts.Token);
            var loc = LocalizationService.Instance;
            var msg = $"{loc.Get("StatusConnected")}: {portName} @ {baudRate}bps";
            AppLogService.Instance.Info($"[Serial] {msg}");
            StatusChanged?.Invoke(this, msg);
        }
        catch (Exception ex)
        {
            AppLogService.Instance.Error(ex, "[Serial] Connect failed");
            ErrorOccurred?.Invoke(this, ex);
            throw;
        }
    }

    public void Disconnect()
    {
        AppLogService.Instance.Info($"[Serial] Disconnecting from {_serialPort?.PortName ?? "N/A"}");
        _readCts?.Cancel();
        _serialPort?.Close();
        _serialPort?.Dispose();
        _serialPort = null;
        _parser.Reset();
        StatusChanged?.Invoke(this, LocalizationService.Instance.Get("StatusDisconnected"));
    }

    public void SendCommand(byte[] frame)
    {
        if (_serialPort?.IsOpen != true)
            throw new InvalidOperationException(LocalizationService.Instance.Get("StatusDisconnected"));

        _serialPort.Write(frame, 0, frame.Length);
    }

    /// <summary>
    /// 发送查询指令
    /// </summary>
    public void SendQuery(byte controlWord, byte commandWord)
    {
        var frame = FrameBuilder.BuildQuery(controlWord, commandWord);
        SendCommand(frame);
    }

    /// <summary>
    /// 发送开关设置指令
    /// </summary>
    public void SendSwitchCommand(byte controlWord, byte commandWord, bool enable)
    {
        var frame = FrameBuilder.BuildSwitchCommand(controlWord, commandWord, enable);
        SendCommand(frame);
    }

    /// <summary>
    /// 发送自定义数据指令
    /// </summary>
    public void SendDataCommand(byte controlWord, byte commandWord, byte[] data)
    {
        var frame = FrameBuilder.BuildCommand(controlWord, commandWord, data);
        SendCommand(frame);
    }

    private async Task ReadLoopAsync(CancellationToken ct)
    {
        var buffer = new byte[1024];
        while (!ct.IsCancellationRequested && _serialPort?.IsOpen == true)
        {
            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    int count = _serialPort.Read(buffer, 0, Math.Min(buffer.Length, _serialPort.BytesToRead));
                    if (count > 0)
                    {
                        var data = new byte[count];
                        Array.Copy(buffer, data, count);

                        // 触发原始数据事件（无论是否能解析出帧，都通知 UI）
                        RawDataReceived?.Invoke(this, data);

                        var frames = _parser.Feed(data);
                        foreach (var frame in frames)
                        {
                            FrameReceived?.Invoke(this, frame);
                        }
                    }
                }
                await Task.Delay(10, ct);
            }
            catch (OperationCanceledException) { break; }
            catch (TimeoutException) { /* ignore */ }
            catch (Exception ex)
            {
                AppLogService.Instance.Error(ex, "[Serial] ReadLoop error");
                ErrorOccurred?.Invoke(this, ex);
            }
        }
    }

    public void Dispose()
    {
        Disconnect();
        _readCts?.Dispose();
    }
}
