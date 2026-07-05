using SleepSightPro.Models;

namespace SleepSightPro.Services;

/// <summary>
/// 雷达数据帧 - 表示一个完整的协议帧
/// </summary>
public class RadarFrame
{
    public byte ControlWord { get; set; }
    public byte CommandWord { get; set; }
    public ushort DataLength { get; set; }
    public byte[] Data { get; set; } = Array.Empty<byte>();
    public byte Checksum { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public bool IsValid { get; set; }

    public override string ToString()
    {
        var hex = BitConverter.ToString(Data).Replace("-", " ");
        return $"Ctrl=0x{ControlWord:X2} Cmd=0x{CommandWord:X2} Len={DataLength} Data=[{hex}]";
    }
}

/// <summary>
/// 协议帧构建器 - 构建下发指令帧
/// </summary>
public static class FrameBuilder
{
    public static byte[] BuildCommand(byte controlWord, byte commandWord, byte[] data)
    {
        ushort dataLen = (ushort)(data?.Length ?? 0);
        int frameLen = 2 + 1 + 1 + 2 + dataLen + 1 + 2; // 帧头+控制字+命令字+长度+数据+校验+帧尾
        var frame = new byte[frameLen];
        int idx = 0;

        // 帧头
        frame[idx++] = RadarProtocol.FRAME_HEAD1;
        frame[idx++] = RadarProtocol.FRAME_HEAD2;
        // 控制字
        frame[idx++] = controlWord;
        // 命令字
        frame[idx++] = commandWord;
        // 长度标识 (大端)
        frame[idx++] = (byte)(dataLen >> 8);
        frame[idx++] = (byte)(dataLen & 0xFF);
        // 数据
        if (data != null && dataLen > 0)
        {
            Array.Copy(data, 0, frame, idx, dataLen);
            idx += dataLen;
        }
        // 校验码 (帧头+控制字+命令字+长度标识+数据 求和取低8位)
        byte sum = 0;
        for (int i = 0; i < idx; i++)
            sum += frame[i];
        frame[idx++] = sum;
        // 帧尾
        frame[idx++] = RadarProtocol.FRAME_TAIL1;
        frame[idx++] = RadarProtocol.FRAME_TAIL2;

        return frame;
    }

    /// <summary>
    /// 构建查询指令 (数据为0x0F)
    /// </summary>
    public static byte[] BuildQuery(byte controlWord, byte commandWord)
    {
        return BuildCommand(controlWord, commandWord, new byte[] { RadarProtocol.DEFAULT_QUERY_DATA });
    }

    /// <summary>
    /// 构建开关设置指令
    /// </summary>
    public static byte[] BuildSwitchCommand(byte controlWord, byte commandWord, bool enable)
    {
        return BuildCommand(controlWord, commandWord, new byte[] { (byte)(enable ? 0x01 : 0x00) });
    }
}

/// <summary>
/// 协议帧解析器 - 解析雷达上报的数据帧
/// </summary>
public class FrameParser
{
    private readonly List<byte> _buffer = new();
    private const int MaxBufferSize = 8192; // 防止数据损坏时内存无限增长

    /// <summary>
    /// 喂入原始字节数据，尝试解析出完整的帧
    /// </summary>
    public List<RadarFrame> Feed(byte[] data)
    {
        var frames = new List<RadarFrame>();
        _buffer.AddRange(data);

        // 防止数据损坏时缓冲区无限增长
        if (_buffer.Count > MaxBufferSize)
            _buffer.RemoveRange(0, _buffer.Count - MaxBufferSize / 2);

        while (_buffer.Count >= 9) // 最小帧长度: 2(头)+1+1+2+0+1+2(尾)=9
        {
            // 查找帧头
            int headIdx = -1;
            for (int i = 0; i <= _buffer.Count - 2; i++)
            {
                if (_buffer[i] == RadarProtocol.FRAME_HEAD1 && _buffer[i + 1] == RadarProtocol.FRAME_HEAD2)
                {
                    headIdx = i;
                    break;
                }
            }

            if (headIdx < 0)
            {
                _buffer.Clear();
                break;
            }

            // 移除帧头前的无效数据
            if (headIdx > 0)
                _buffer.RemoveRange(0, headIdx);

            // 检查是否有足够的数据解析完整帧
            if (_buffer.Count < 9) break;

            // 读取控制字和命令字
            byte ctrl = _buffer[2];
            byte cmd = _buffer[3];
            ushort dataLen = (ushort)((_buffer[4] << 8) | _buffer[5]);

            int totalLen = 9 + dataLen; // 2头+1+1+2+n+1+2尾
            if (_buffer.Count < totalLen) break;

            // 检查帧尾
            if (_buffer[totalLen - 2] != RadarProtocol.FRAME_TAIL1 ||
                _buffer[totalLen - 1] != RadarProtocol.FRAME_TAIL2)
            {
                _buffer.RemoveAt(0); // 帧尾不匹配，移除一个字节继续
                continue;
            }

            // 提取数据
            byte[] frameData = new byte[dataLen];
            if (dataLen > 0)
                _buffer.CopyTo(6, frameData, 0, dataLen);

            // 验证校验码
            byte expectedChecksum = _buffer[6 + dataLen];
            byte calculatedSum = 0;
            for (int i = 0; i < 6 + dataLen; i++)
                calculatedSum += _buffer[i];

            var frame = new RadarFrame
            {
                ControlWord = ctrl,
                CommandWord = cmd,
                DataLength = dataLen,
                Data = frameData,
                Checksum = expectedChecksum,
                IsValid = calculatedSum == expectedChecksum,
                Timestamp = DateTime.Now
            };

            frames.Add(frame);
            _buffer.RemoveRange(0, totalLen);
        }

        return frames;
    }

    public void Reset()
    {
        _buffer.Clear();
    }
}
