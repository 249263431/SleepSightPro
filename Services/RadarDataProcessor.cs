using SleepSightPro.Models;

namespace SleepSightPro.Services;

/// <summary>
/// 雷达数据处理器 - 将原始帧数据解析为有意义的业务数据
/// </summary>
public class RadarDataProcessor
{
    // 实时数据
    public bool IsPresence { get; private set; }
    public byte MotionState { get; private set; }
    public byte BodyMoveParam { get; private set; }
    public ushort Distance { get; private set; }
    public short PosX { get; private set; }
    public short PosY { get; private set; }
    public short PosZ { get; private set; }

    // 呼吸数据
    public byte BreathValue { get; private set; }
    public byte BreathState { get; private set; }
    public byte[] BreathWave { get; private set; } = new byte[5];
    public byte TimeDomainBreath { get; private set; }
    /// <summary>是否已收到过呼吸波形数据（波形开关开启后才为 true）</summary>
    public bool HasBreathWave { get; private set; }

    // 心率数据
    public byte HeartValue { get; private set; }
    public byte[] HeartWave { get; private set; } = new byte[5];
    /// <summary>是否已收到过心率波形数据（波形开关开启后才为 true）</summary>
    public bool HasHeartWave { get; private set; }

    // 睡眠数据
    public bool IsInBed { get; private set; }
    public byte SleepState { get; private set; }
    public ushort AwakeDuration { get; private set; }
    public ushort LightSleepDuration { get; private set; }
    public ushort DeepSleepDuration { get; private set; }
    public byte SleepScore { get; private set; }

    // 睡眠综合状态
    public bool CompPresence { get; private set; }
    public byte CompSleepState { get; private set; }
    public byte CompAvgBreath { get; private set; }
    public byte CompAvgHeart { get; private set; }
    public byte CompTurnOver { get; private set; }
    public byte CompLargeMoveRatio { get; private set; }
    public byte CompSmallMoveRatio { get; private set; }
    public byte CompApneaCount { get; private set; }

    // 睡眠质量分析
    public byte AnalysisScore { get; private set; }
    public ushort AnalysisTotalDuration { get; private set; }
    public byte AnalysisAwakeRatio { get; private set; }
    public byte AnalysisLightSleepRatio { get; private set; }
    public byte AnalysisDeepSleepRatio { get; private set; }
    public byte AnalysisOutBedDuration { get; private set; }
    public byte AnalysisOutBedCount { get; private set; }
    public byte AnalysisTurnOverCount { get; private set; }
    public byte AnalysisAvgBreath { get; private set; }
    public byte AnalysisAvgHeart { get; private set; }
    public byte AnalysisApneaCount { get; private set; }

    // 异常状态
    public byte SleepAbnormalState { get; private set; }
    public byte StruggleState { get; private set; }
    public byte NoPersonTimeState { get; private set; }
    public byte SleepRating { get; private set; }

    // 产品信息
    public string ProductModel { get; private set; } = "";
    public string ProductId { get; private set; } = "";
    public string HardwareVersion { get; private set; } = "";
    public string FirmwareVersion { get; private set; } = "";

    // 状态
    public bool IsInitComplete { get; private set; }
    public byte BoundaryState { get; private set; }

    // 事件
    public event EventHandler<string>? DataUpdated;

    public void ProcessFrame(RadarFrame frame)
    {
        if (!frame.IsValid) return;

        try
        {
            switch (frame.ControlWord)
            {
                case RadarProtocol.ControlWord.HUMAN_PRESENCE:
                    ProcessHumanPresence(frame);
                    break;
                case RadarProtocol.ControlWord.BREATH_DETECT:
                    ProcessBreathDetect(frame);
                    break;
                case RadarProtocol.ControlWord.HEART_RATE:
                    ProcessHeartRate(frame);
                    break;
                case RadarProtocol.ControlWord.SLEEP_MONITOR:
                    ProcessSleepMonitor(frame);
                    break;
                case RadarProtocol.ControlWord.HEARTBEAT:
                    ProcessHeartbeat(frame);
                    break;
                case RadarProtocol.ControlWord.PRODUCT_INFO:
                    ProcessProductInfo(frame);
                    break;
                case RadarProtocol.ControlWord.WORK_STATUS:
                    ProcessWorkStatus(frame);
                    break;
                case RadarProtocol.ControlWord.RADAR_RANGE:
                    ProcessRadarRange(frame);
                    break;
            }
        }
        catch (Exception ex)
        {
            AppLogService.Instance.Error(ex, "[DataProcessor] ProcessFrame error");
        }
    }

    private void ProcessHumanPresence(RadarFrame frame)
    {
        switch (frame.CommandWord)
        {
            case RadarProtocol.HumanPresenceCmd.PRESENCE_REPORT:
                if (frame.DataLength >= 1) IsPresence = frame.Data[0] == 0x01;
                break;
            case RadarProtocol.HumanPresenceCmd.MOTION_REPORT:
                if (frame.DataLength >= 1) MotionState = frame.Data[0];
                break;
            case RadarProtocol.HumanPresenceCmd.BODY_MOVE_REPORT:
                if (frame.DataLength >= 1) BodyMoveParam = frame.Data[0];
                break;
            case RadarProtocol.HumanPresenceCmd.DISTANCE_REPORT:
                if (frame.DataLength >= 2) Distance = (ushort)((frame.Data[0] << 8) | frame.Data[1]);
                break;
            case RadarProtocol.HumanPresenceCmd.POSITION_REPORT:
                if (frame.DataLength >= 6)
                {
                    PosX = ParseSignedShort(frame.Data, 0);
                    PosY = ParseSignedShort(frame.Data, 2);
                    PosZ = ParseSignedShort(frame.Data, 4);
                }
                break;
        }
        DataUpdated?.Invoke(this, "HumanPresence");
    }

    private void ProcessBreathDetect(RadarFrame frame)
    {
        switch (frame.CommandWord)
        {
            case RadarProtocol.BreathDetectCmd.BREATH_INFO_REPORT:
                if (frame.DataLength >= 1) BreathState = frame.Data[0];
                break;
            case RadarProtocol.BreathDetectCmd.BREATH_VALUE_REPORT:
                if (frame.DataLength >= 1) BreathValue = frame.Data[0];
                break;
            case RadarProtocol.BreathDetectCmd.BREATH_WAVE_REPORT:
                if (frame.DataLength >= 5) { Array.Copy(frame.Data, BreathWave, 5); HasBreathWave = true; }
                break;
            case RadarProtocol.BreathDetectCmd.TIME_DOMAIN_REPORT:
                if (frame.DataLength >= 1) TimeDomainBreath = frame.Data[0];
                break;
        }
        // 收到有效呼吸数据 → 说明有人
        if (BreathValue > 0 && BreathState != RadarProtocol.BreathState.NONE)
            IsPresence = true;
        DataUpdated?.Invoke(this, "BreathDetect");
    }

    private void ProcessHeartRate(RadarFrame frame)
    {
        switch (frame.CommandWord)
        {
            case RadarProtocol.HeartRateCmd.HEART_VALUE_REPORT:
                if (frame.DataLength >= 1) HeartValue = frame.Data[0];
                break;
            case RadarProtocol.HeartRateCmd.HEART_WAVE_REPORT:
                if (frame.DataLength >= 5) { Array.Copy(frame.Data, HeartWave, 5); HasHeartWave = true; }
                break;
        }
        // 收到有效心率数据 → 说明有人
        if (HeartValue > 0)
            IsPresence = true;
        DataUpdated?.Invoke(this, "HeartRate");
    }

    private void ProcessSleepMonitor(RadarFrame frame)
    {
        switch (frame.CommandWord)
        {
            // 入床/离床状态
            case RadarProtocol.SleepMonitorCmd.SLEEP_STATE_REPORT:
                if (frame.DataLength >= 1) IsInBed = frame.Data[0] == 0x01;
                break;
            // 睡眠状态（深睡/浅睡/清醒）
            case RadarProtocol.SleepMonitorCmd.SLEEP_DURATION_REPORT:
                if (frame.DataLength >= 1) SleepState = frame.Data[0];
                break;
            // 清醒时长
            case RadarProtocol.SleepMonitorCmd.AWAKE_DURATION_REPORT:
                if (frame.DataLength >= 2)
                    AwakeDuration = (ushort)((frame.Data[0] << 8) | frame.Data[1]);
                break;
            // 浅睡时长
            case RadarProtocol.SleepMonitorCmd.LIGHT_DURATION_REPORT:
                if (frame.DataLength >= 2)
                    LightSleepDuration = (ushort)((frame.Data[0] << 8) | frame.Data[1]);
                break;
            // 深睡时长
            case RadarProtocol.SleepMonitorCmd.DEEP_DURATION_REPORT:
                if (frame.DataLength >= 2)
                    DeepSleepDuration = (ushort)((frame.Data[0] << 8) | frame.Data[1]);
                break;
            // 睡眠质量评分
            case RadarProtocol.SleepMonitorCmd.SLEEP_SCORE_REPORT:
                if (frame.DataLength >= 1) SleepScore = frame.Data[0];
                break;
            // 睡眠综合状态
            case RadarProtocol.SleepMonitorCmd.SLEEP_COMP_REPORT:
                if (frame.DataLength >= 8)
                {
                    CompPresence = frame.Data[0] == 0x01;
                    CompSleepState = frame.Data[1];
                    CompAvgBreath = frame.Data[2];
                    CompAvgHeart = frame.Data[3];
                    CompTurnOver = frame.Data[4];
                    CompLargeMoveRatio = frame.Data[5];
                    CompSmallMoveRatio = frame.Data[6];
                    CompApneaCount = frame.Data[7];
                }
                break;
            // 睡眠质量分析
            case RadarProtocol.SleepMonitorCmd.SLEEP_ANALYSIS_REPORT:
                if (frame.DataLength >= 12)
                {
                    int idx = 0;
                    AnalysisScore = frame.Data[idx++];
                    AnalysisTotalDuration = (ushort)((frame.Data[idx++] << 8) | frame.Data[idx++]);
                    AnalysisAwakeRatio = frame.Data[idx++];
                    AnalysisLightSleepRatio = frame.Data[idx++];
                    AnalysisDeepSleepRatio = frame.Data[idx++];
                    AnalysisOutBedDuration = frame.Data[idx++];
                    AnalysisOutBedCount = frame.Data[idx++];
                    AnalysisTurnOverCount = frame.Data[idx++];
                    AnalysisAvgBreath = frame.Data[idx++];
                    AnalysisAvgHeart = frame.Data[idx++];
                    AnalysisApneaCount = frame.Data[idx++];
                }
                break;
            // 睡眠异常
            case RadarProtocol.SleepMonitorCmd.SLEEP_ABNORMAL_REPORT:
                if (frame.DataLength >= 1) SleepAbnormalState = frame.Data[0];
                break;
            // 异常挣扎
            case RadarProtocol.SleepMonitorCmd.STRUGGLE_REPORT:
                if (frame.DataLength >= 1) StruggleState = frame.Data[0];
                break;
            // 无人计时
            case RadarProtocol.SleepMonitorCmd.NO_PERSON_TIME_REPORT:
                if (frame.DataLength >= 1) NoPersonTimeState = frame.Data[0];
                break;
            // 睡眠质量评级
            case RadarProtocol.SleepMonitorCmd.SLEEP_RATING_REPORT:
                if (frame.DataLength >= 1) SleepRating = frame.Data[0];
                break;
        }
        DataUpdated?.Invoke(this, "SleepMonitor");
    }

    private void ProcessHeartbeat(RadarFrame frame)
    {
        // 心跳包响应处理
    }

    private void ProcessProductInfo(RadarFrame frame)
    {
        var len = frame.DataLength;
        switch (frame.CommandWord)
        {
            case RadarProtocol.ProductInfoCmd.MODEL_REPORT:
                ProductModel = System.Text.Encoding.ASCII.GetString(frame.Data, 0, len);
                break;
            case RadarProtocol.ProductInfoCmd.ID_REPORT:
                ProductId = System.Text.Encoding.ASCII.GetString(frame.Data, 0, len);
                break;
            case RadarProtocol.ProductInfoCmd.HARDWARE_REPORT:
                HardwareVersion = System.Text.Encoding.ASCII.GetString(frame.Data, 0, len);
                break;
            case RadarProtocol.ProductInfoCmd.FIRMWARE_REPORT:
                FirmwareVersion = System.Text.Encoding.ASCII.GetString(frame.Data, 0, len);
                break;
        }
        DataUpdated?.Invoke(this, "ProductInfo");
    }

    private void ProcessWorkStatus(RadarFrame frame)
    {
        if (frame.CommandWord == RadarProtocol.WorkStatusCmd.INIT_COMPLETE_REPORT ||
            frame.CommandWord == RadarProtocol.WorkStatusCmd.INIT_QUERY)
        {
            if (frame.DataLength >= 1)
                IsInitComplete = frame.Data[0] == 0x01;
        }
        DataUpdated?.Invoke(this, "WorkStatus");
    }

    private void ProcessRadarRange(RadarFrame frame)
    {
        if (frame.CommandWord == RadarProtocol.RadarRangeCmd.BOUNDARY_REPORT)
        {
            if (frame.DataLength >= 1)
                BoundaryState = frame.Data[0];
        }
        DataUpdated?.Invoke(this, "RadarRange");
    }

    /// <summary>
    /// 解析有符号的16位整数（首位为符号位，剩余15位为值）
    /// </summary>
    private static short ParseSignedShort(byte[] data, int offset)
    {
        ushort raw = (ushort)((data[offset] << 8) | data[offset + 1]);
        bool isNegative = (raw & 0x8000) != 0;
        short value = (short)(raw & 0x7FFF);
        return isNegative ? (short)-value : value;
    }
}
