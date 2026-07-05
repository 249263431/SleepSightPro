using System.Globalization;
using System.Resources;
using System.Reflection;

namespace SleepSightPro.Services;

/// <summary>
/// 多语言本地化服务
/// </summary>
public class LocalizationService
{
    private static LocalizationService? _instance;
    public static LocalizationService Instance => _instance ??= new LocalizationService();

    private readonly Dictionary<string, Dictionary<string, string>> _translations = new();
    private string _currentLang = "zh-CN";

    public string CurrentLanguage
    {
        get => _currentLang;
        set
        {
            if (_currentLang != value)
            {
                _currentLang = value;
                LanguageChanged?.Invoke(this, value);
            }
        }
    }

    public event EventHandler<string>? LanguageChanged;

    private LocalizationService()
    {
        InitTranslations();
    }

    public string Get(string key)
    {
        if (_translations.TryGetValue(key, out var dict))
        {
            string langKey = _currentLang == "zh-CN" ? "zh" : "en";
            if (dict.TryGetValue(langKey, out var value))
                return value;
        }
        return key;
    }

    public void SetLanguage(string lang)
    {
        CurrentLanguage = lang;
    }

    public void ToggleLanguage()
    {
        CurrentLanguage = _currentLang == "zh-CN" ? "en-US" : "zh-CN";
    }

    private void InitTranslations()
    {
        // ========== 主窗口 ==========
        Add("AppTitle", "SleepSightPro V1.0.0.0", "SleepSightPro V1.0.0.0");
        Add("StatusConnected", "已连接", "Connected");
        Add("StatusDisconnected", "未连接", "Disconnected");
        Add("StatusConnecting", "连接中...", "Connecting...");
        Add("StatusDblClickHint", "双击状态栏可截图到剪贴板", "Double-click status bar to screenshot");
        Add("StatusFW", "固件: ", "FW: ");

        // ========== 菜单栏 ==========
        Add("MenuFile", "文件", "File");
        Add("MenuSettings", "设置", "Settings");
        Add("MenuLanguage", "语言", "Language");
        Add("MenuAbout", "关于", "About");
        Add("MenuExit", "退出", "Exit");

        // ========== 串口设置 ==========
        Add("SerialSettings", "串口设置", "Serial Port");
        Add("PortName", "串口号", "Port");
        Add("BaudRate", "波特率", "Baud Rate");
        Add("BtnConnect", "连接", "Connect");
        Add("BtnDisconnect", "断开", "Disconnect");
        Add("BtnRefresh", "刷新", "Refresh");
        Add("SerialDataBit", "数据位", "Data Bits");
        Add("SerialStopBit", "停止位", "Stop Bits");
        Add("SerialParity", "校验位", "Parity");

        // ========== Tab页（三大类） ==========
        Add("TabDisplay", "显示", "Display");
        Add("TabSettings", "设置", "Settings");
        Add("TabProtocol", "通讯协议", "Protocol");
        Add("TabEquations", "雷达方程", "Equations");
        Add("TabLog", "日志", "Log");
        Add("TabDashboard", "仪表盘", "Dashboard");
        Add("TabPresence", "人体存在", "Presence");
        Add("TabBreath", "呼吸监测", "Breath");
        Add("TabHeartRate", "心率监测", "Heart Rate");
        Add("TabSleep", "睡眠监测", "Sleep");

        // ========== 仪表盘 ==========
        Add("DashboardTitle", "实时监测仪表盘", "Real-time Dashboard");
        Add("CardPresence", "人体存在", "Presence");
        Add("CardBreath", "呼吸频率", "Breath Rate");
        Add("CardHeartRate", "心率", "Heart Rate");
        Add("CardSleepState", "睡眠状态", "Sleep State");
        Add("CardBodyMove", "体动幅度", "Body Movement");
        Add("CardSleepScore", "睡眠评分", "Sleep Score");
        Add("CardDistance", "人体距离", "Distance");
        Add("UnitBpm", "次/分", "bpm");
        Add("UnitCm", "厘米", "cm");
        Add("UnitPercent", "%", "%");
        Add("UnitScore", "分", "pts");
        Add("UnitBpmShort", " bpm", " bpm");
        Add("UnitCmShort", " cm", " cm");
        Add("UnitPercentShort", " %", " %");

        // ========== 人体存在 ==========
        Add("PresenceTitle", "人体存在监测", "Human Presence Monitor");
        Add("PresenceState", "存在状态", "Presence State");
        Add("MotionState", "运动状态", "Motion State");
        Add("BodyMoveParam", "体动参数", "Body Movement");
        Add("DistanceValue", "距离", "Distance");
        Add("PositionX", "X坐标", "X Position");
        Add("PositionY", "Y坐标", "Y Position");
        Add("PositionZ", "Z坐标", "Z Position");
        Add("BtnSwitchPresence", "人体存在功能", "Presence Function");
        Add("BtnOn", "开启", "ON");
        Add("BtnOff", "关闭", "OFF");
        Add("StatePresent", "有人", "Present");
        Add("StateAbsent", "无人", "Absent");
        Add("StateStatic", "静止", "Static");
        Add("StateActive", "活跃", "Active");
        Add("StateNone", "无", "None");

        // ========== 呼吸监测 ==========
        Add("BreathTitle", "呼吸监测", "Breath Monitor");
        Add("BreathValue", "呼吸值", "Breath Value");
        Add("BreathState", "呼吸状态", "Breath State");
        Add("BreathWave", "呼吸波形", "Breath Waveform");
        Add("BtnSwitchBreath", "呼吸监测功能", "Breath Function");
        Add("BtnSwitchBreathWave", "呼吸波形上报", "Breath Waveform Report");
        Add("StateBreathNormal", "正常", "Normal");
        Add("StateBreathHigh", "过高", "High");
        Add("StateBreathLow", "过低", "Low");
        Add("StateBreathNone", "无", "None");

        // ========== 心率监测 ==========
        Add("HeartTitle", "心率监测", "Heart Rate Monitor");
        Add("HeartValue", "心率值", "Heart Rate");
        Add("HeartWave", "心率波形", "Heart Rate Waveform");
        Add("BtnSwitchHeart", "心率监测功能", "Heart Rate Function");
        Add("BtnSwitchHeartWave", "心率波形上报", "Heart Waveform Report");

        // ========== 睡眠监测 ==========
        Add("SleepTitle", "睡眠监测", "Sleep Monitor");
        Add("SleepState", "睡眠状态", "Sleep State");
        Add("SleepScore", "睡眠评分", "Sleep Score");
        Add("SleepDuration", "睡眠时长", "Sleep Duration");
        Add("AwakeDuration", "清醒时长", "Awake Time");
        Add("LightSleepDuration", "浅睡时长", "Light Sleep");
        Add("DeepSleepDuration", "深睡时长", "Deep Sleep");
        Add("SleepRating", "睡眠评级", "Sleep Rating");
        Add("SleepAbnormal", "睡眠异常", "Sleep Abnormal");
        Add("StruggleState", "异常挣扎", "Struggle");
        Add("TurnOverCount", "翻身次数", "Turn Over Count");
        Add("OutBedCount", "离床次数", "Out of Bed");
        Add("AvgBreath", "平均呼吸", "Avg Breath");
        Add("AvgHeart", "平均心率", "Avg Heart");
        Add("ApneaCount", "呼吸暂停", "Apnea");
        Add("BtnSwitchSleep", "睡眠监测功能", "Sleep Function");
        Add("StateDeepSleep", "深睡", "Deep Sleep");
        Add("StateLightSleep", "浅睡", "Light Sleep");
        Add("StateAwake", "清醒", "Awake");
        Add("StateOutOfBed", "离床", "Out of Bed");
        Add("RatingGood", "良好", "Good");
        Add("RatingAverage", "一般", "Average");
        Add("RatingPoor", "较差", "Poor");
        Add("StateSleepInsufficient", "睡眠不足", "Insufficient");
        Add("StateSleepExcessive", "睡眠过长", "Excessive");
        Add("StateAbnormalAbsent", "异常无人", "Abnormal Absent");
        Add("StateNormal", "正常", "Normal");
        Add("StateAbnormal", "异常", "Abnormal");

        // ========== 日志 ==========
        Add("LogTitle", "通信日志", "Communication Log");
        Add("LogPanelTitle", "通信日志", "Communication Log");
        Add("BtnClearLog", "清空", "Clear");
        Add("BtnExportLog", "导出", "Export");
        Add("LogTime", "时间", "Time");
        Add("LogDirection", "方向", "Dir");
        Add("LogControl", "控制字", "Ctrl");
        Add("LogCommand", "命令字", "Cmd");
        Add("LogLength", "长度", "Len");
        Add("LogData", "数据", "Data");
        Add("LogDirSend", "发送", "TX");
        Add("LogDirRecv", "接收", "RX");

        // ========== 产品信息 ==========
        Add("ProductInfo", "产品信息", "Product Info");
        Add("ProductModel", "产品型号", "Model");
        Add("ProductId", "产品ID", "Product ID");
        Add("HardwareVer", "硬件版本", "Hardware");
        Add("FirmwareVer", "固件版本", "Firmware");
        Add("InitStatus", "初始化状态", "Init Status");
        Add("InitComplete", "已完成", "Complete");
        Add("InitNotComplete", "未完成", "Not Complete");

        // ========== 设置 ==========
        Add("SettingsTitle", "设置", "Settings");
        Add("LanguageSetting", "语言设置", "Language");
        Add("LanguageZH", "中文", "中文");
        Add("LanguageEN", "EN", "EN");
        Add("ThemeSetting", "主题设置", "Theme");
        Add("ThemeLight", "浅色", "Light");
        Add("ThemeDark", "深色", "Dark");
        Add("BtnSaveSettings", "保存设置", "Save");

        // ========== 消息 ==========
        Add("MsgConnectSuccess", "串口连接成功", "Connected successfully");
        Add("MsgConnectFailed", "串口连接失败", "Connection failed");
        Add("MsgDisconnected", "串口已断开", "Disconnected");
        Add("MsgSelectPort", "请选择串口", "Please select a port");
        Add("MsgNoData", "暂无数据", "No data");
        Add("MsgError", "错误", "Error");
        Add("MsgWarning", "警告", "Warning");
        Add("MsgInfo", "信息", "Info");
        Add("MsgAbout", "SleepSightPro V1.0.0.0\n基于R60ABD1毫米波雷达的睡眠监测上位机软件\n联系邮箱: 249263431@qq.com", "SleepSightPro V1.0.0.0\nSleep monitoring software based on R60ABD1 mmWave radar\nContact: 249263431@qq.com");

        // ========== 通用按钮 ==========
        Add("BtnQuery", "查询", "Query");
        Add("BtnSet", "设置", "Set");
        Add("BtnReset", "重置", "Reset");
        Add("BtnClose", "关闭", "Close");
        Add("BtnOK", "确定", "OK");
        Add("BtnCancel", "取消", "Cancel");

        // ========== 原始数据日志面板 ==========
        Add("RawDataTitle", "原始数据流 (Raw Data Stream)", "Raw Data Stream");
        Add("RawPause", "暂停", "Pause");
        Add("RawResume", "继续", "Resume");
        Add("RawClear", "清空", "Clear");
        Add("LblSettingsResult", "返回结果", "Result");
        Add("SettingsRuleHint", "单击按钮后显示返回结果 | 约10秒无操作后自动恢复动态更新", "Click a button to view response | Auto-resumes live updates ~10s after last action");
        Add("BtnClearResult", "清空", "Clear");

        // ========== 原始 Hex Tab 按钮 ==========
        Add("BtnRawHexClear", "清空", "Clear");

        // ========== 原始 Hex Tab 状态标签 ==========
        Add("RawHexStatus", "已接收: {0} 字节", "Received: {0} bytes");
        Add("RawHexStatusLine", "已接收: {0} 字节 | 本次: {1} B", "Received: {0} bytes | This: {1} B");

        // ========== 诊断对话框 ==========
        Add("DiagDialogTitle", "诊断", "Diagnose");
        Add("DiagEmptyBuffer", "原始 Hex 缓冲为空，请先接收数据。", "Raw Hex buffer is empty. Please receive data first.");
        Add("DiagNoHexParsed", "无法从缓冲中解析到有效 Hex 字节。", "Unable to parse valid Hex bytes from buffer.");
        Add("DiagReportTitle", "Hex 诊断报告", "Hex Diagnostic Report");
        Add("DiagReportBanner", "══════════ 原始 Hex 诊断报告 ══════════", "══════════ Raw Hex Diagnostic Report ══════════");
        Add("DiagTotalBytes", "  总字节数:  {0}", "  Total bytes:  {0}");
        Add("DiagFrameHeadCount", "  帧头 53 59 出现次数:  {0}", "  Frame head 53 59 count:  {0}");
        Add("DiagFrameTailCount", "  帧尾 54 43 出现次数:  {0}", "  Frame tail 54 43 count:  {0}");
        Add("DiagBaudRate", "  当前波特率设置:  {0} bps", "  Current baud rate:  {0} bps");
        Add("DiagByteRate", "  估算字节速率:  {0:F0} B/s", "  Estimated byte rate:  {0:F0} B/s");
        Add("DiagTopFreqHeader", "  ── 最高频字节 (Top 10) ──", "  ── Most Frequent Bytes (Top 10) ──");
        Add("DiagFreqRow", "    0x{0:X2} ({0,3})  出现 {1} 次", "    0x{0:X2} ({0,3})  {1} times");
        Add("DiagInferenceHeader", "  ── 诊断推断 ──", "  ── Diagnostic Inference ──");
        Add("DiagFrameInterval", "  帧头间隔平均:  {0:F1} 字节", "  Avg frame interval:  {0:F1} bytes");
        Add("DiagInterval32", "  → 帧间隔 ~32 字节，疑似波特率不匹配 (应为 115200?)", "  → Frame interval ~32 bytes, possible baud rate mismatch (should be 115200?)");
        Add("DiagInterval64", "  → 帧间隔 ~64 字节，波特率可能正常", "  → Frame interval ~64 bytes, baud rate appears correct");
        Add("DiagIntervalAbnormal", "  → 帧间隔不标准，请检查是否有多余噪声字节混入", "  → Abnormal frame interval, check for noise/interference");
        Add("DiagNoHeadTitle", "  ✗ 未检测到帧头 (53 59)！可能原因:", "  ✗ Frame head (53 59) NOT detected! Possible causes:");
        Add("DiagNoHeadA", "    a) 波特率不匹配 → 尝试切换 115200/57600/9600", "    a) Baud rate mismatch → try 115200/57600/9600");
        Add("DiagNoHeadB", "    b) 数据位/停止位/校验位不匹配", "    b) Data/stop/parity bit mismatch");
        Add("DiagNoHeadC", "    c) 串口收到了噪声而非有效数据", "    c) Serial port received noise instead of valid data");
        Add("DiagNoHeadD", "    d) GND 未接好导致电平漂移", "    d) GND not properly connected, causing signal level drift");
        Add("DiagHeadNoTail", "  → 有帧头无帧尾，可能数据被截断或帧结构损坏", "  → Frame head found but no tail. Data may be truncated or corrupted");
        Add("DiagHeadTailMismatch", "  → 帧头帧尾数量不匹配，帧完整性可能有问题", "  → Head/tail count mismatch: frame integrity may be compromised");
        Add("DiagZeroBytes", "  → {0}/{1} 字节为 0x00，大量零值→可能电平异常", "  → {0}/{1} bytes are 0x00, excessive zeros → possible signal level issue");
        Add("DiagSampleHeader", "  ── 前 64 字节原始样本 ──", "  ── First 64 Bytes Sample ──");

        // ========== 导出 ==========
        Add("MsgLogExported", "日志导出成功", "Log exported successfully");

        // ========== 退出/复位确认 ==========
        Add("MsgExitConfirm", "确定要退出 SleepSightPro 吗？", "Are you sure you want to exit SleepSightPro?");
        Add("MsgExitTitle", "退出确认", "Exit Confirmation");
        Add("MsgResetConfirm", "确定要复位模组吗？", "Are you sure you want to reset the module?");

        // ========== 设备信息查询 ==========
        Add("BtnQueryDev", "查询设备信息", "Query Device Info");

        // ========== InfoRow 标签 (AddInfoRow 的 label 前缀) ==========
        Add("LblMotionState", "运动状态:", "Motion:");
        Add("LblBodyMove", "体动参数:", "Body Move:");
        Add("LblDistance", "距离:", "Distance:");
        Add("LblPosX", "X坐标:", "X:");
        Add("LblPosY", "Y坐标:", "Y:");
        Add("LblPosZ", "Z坐标:", "Z:");
        Add("LblBreathState", "呼吸状态:", "Breath State:");
        Add("LblTimeDomainBreath", "时域呼吸值:", "Time Domain:");
        Add("LblProductModel", "产品型号:", "Model:");
        Add("LblProductId", "产品ID:", "Product ID:");
        Add("LblHardwareVer", "硬件版本:", "Hardware:");
        Add("LblSleepScore", "睡眠评分:", "Score:");
        Add("LblSleepRating", "睡眠评级:", "Rating:");
        Add("LblAwakeTime", "清醒时长:", "Awake:");
        Add("LblLightSleepTime", "浅睡时长:", "Light:");
        Add("LblDeepSleepTime", "深睡时长:", "Deep:");
        Add("LblTurnOver", "翻身次数:", "Turn Over:");
        Add("LblOutBedCount", "离床次数:", "Out of Bed:");
        Add("LblAvgBreath", "平均呼吸:", "Avg Breath:");
        Add("LblAvgHeart", "平均心率:", "Avg Heart:");
        Add("LblApnea", "呼吸暂停:", "Apnea:");
        Add("LblStruggle", "异常挣扎:", "Struggle:");

        // ========== 单位后缀 ==========
        Add("UnitBreathRate", " 次/分", " bpm");
        Add("UnitMin", "分", " min");
        Add("UnitTimes", "次", "");
        Add("UnitTimesCN", "次", " times");

        // ========== 各页按钮文本 ==========
        Add("BtnPresenceOn", "开启人体存在", "Enable Presence");
        Add("BtnPresenceOff", "关闭人体存在", "Disable Presence");
        Add("BtnQueryPresence", "查询状态", "Query State");
        Add("BtnQueryMotion", "查询运动信息", "Query Motion");
        Add("BtnQueryDistance", "查询距离", "Query Distance");
        Add("BtnQueryPosition", "查询方位", "Query Position");

        Add("BtnBreathOn", "开启呼吸监测", "Enable Breath");
        Add("BtnBreathOff", "关闭呼吸监测", "Disable Breath");
        Add("BtnBreathWaveOn", "开启波形上报", "Enable Wave");
        Add("BtnBreathWaveOff", "关闭波形上报", "Disable Wave");
        Add("BtnQueryBreathValue", "查询呼吸值", "Query Value");
        Add("BtnQueryBreathState", "查询呼吸状态", "Query State");

        Add("BtnHeartOn", "开启心率监测", "Enable Heart Rate");
        Add("BtnHeartOff", "关闭心率监测", "Disable Heart Rate");
        Add("BtnHeartWaveOn", "开启波形上报", "Enable Wave");
        Add("BtnHeartWaveOff", "关闭波形上报", "Disable Wave");
        Add("BtnQueryHeartValue", "查询心率值", "Query Value");

        Add("BtnSleepOn", "开启睡眠监测", "Enable Sleep");
        Add("BtnSleepOff", "关闭睡眠监测", "Disable Sleep");
        Add("BtnQuerySleepComp", "查询综合状态", "Query Status");
        Add("BtnQuerySleepAnalysis", "查询睡眠分析", "Query Analysis");
        Add("BtnQuerySleepRating", "查询睡眠评级", "Query Rating");

        // ========== 日志列头 ==========
        Add("LogColTime", "时间", "Time");
        Add("LogColDir", "方向", "Dir");
        Add("LogColCtrl", "控制字", "Ctrl");
        Add("LogColCmd", "命令字", "Cmd");
        Add("LogColLen", "长度", "Len");
        Add("LogColData", "数据", "Data");

        // ========== 日志 Tab 页标题 ==========
        Add("LogTabParsed", "解析数据", "Parsed Data");
        Add("LogTabRawHex", "原始数据 (Hex)", "Raw Data (Hex)");
        Add("BtnDiagnose", "诊断", "Diagnose");

        // ========== 系统功能 ==========
        Add("BtnHeartbeatQuery", "心跳包查询", "Heartbeat Query");
        Add("BtnModuleReset", "模组复位", "Module Reset");
        Add("BtnInitQuery", "初始化查询", "Init Query");

        // ========== 呼吸监测扩展 ==========
        Add("BtnSetLowBreath", "设置低缓判读", "Set Low Breath");
        Add("BtnQueryLowBreath", "查询低缓判读", "Query Low Breath");
        Add("BtnQueryBreathWaveSwitch", "查询波形开关", "Query Wave Switch");
        Add("BtnQueryBreathSwitch", "查询呼吸开关", "Query Breath SW");
        Add("BtnBoundaryQuery", "查询越界状态", "Query Boundary");

        // ========== 心率监测扩展 ==========
        Add("BtnQueryHeartSwitch", "查询心率开关", "Query Switch");
        Add("BtnQueryHeartWave", "查询心率波形", "Query Wave");
        Add("BtnQueryHeartWaveSwitch", "查询波形开关", "Query Wave Switch");

        // ========== 睡眠监测扩展 - 设置 ==========
        Add("BtnStruggleSwitchOn", "开启挣扎检测", "Enable Struggle");
        Add("BtnStruggleSwitchOff", "关闭挣扎检测", "Disable Struggle");
        Add("BtnNoPersonSwitchOn", "开启无人计时", "Enable No-Person Timer");
        Add("BtnNoPersonSwitchOff", "关闭无人计时", "Disable No-Person Timer");
        Add("BtnSetNoPersonDuration", "设置无人时长", "Set No-Person Duration");
        Add("BtnSetSleepDeadline", "设置睡眠截止", "Set Sleep Deadline");
        Add("BtnSetStruggleSensitivity", "设置挣扎灵敏度", "Set Struggle Sensitivity");
        Add("BtnExtCtrlSwitchOn", "开启外部控制", "Enable Ext Ctrl");
        Add("BtnExtCtrlSwitchOff", "关闭外部控制", "Disable Ext Ctrl");
        Add("BtnSleepPeriodStart", "睡眠周期开始", "Sleep Period Start");
        Add("BtnSleepPeriodEnd", "睡眠周期结束", "Sleep Period End");

        // ========== 睡眠监测扩展 - 查询 ==========
        Add("BtnQueryBedState", "查询入离床", "Query Bed State");
        Add("BtnQuerySleepState", "查询睡眠状态", "Query Sleep State");
        Add("BtnQueryAwakeDur", "查询清醒时长", "Query Awake");
        Add("BtnQueryLightDur", "查询浅睡时长", "Query Light");
        Add("BtnQueryDeepDur", "查询深睡时长", "Query Deep");
        Add("BtnQuerySleepScore", "查询睡眠评分", "Query Score");
        Add("BtnQuerySleepAbnormal", "查询睡眠异常", "Query Abnormal");
        Add("BtnQuerySleepStats", "查询睡眠统计", "Query Stats");
        Add("BtnQueryStruggle", "查询挣扎状态", "Query Struggle");
        Add("BtnQueryNoPerson", "查询无人计时", "Query No-Person");
        Add("BtnQueryStruggleSwitch", "查询挣扎开关", "Query Struggle SW");
        Add("BtnQueryNoPersonSwitch", "查询无人开关", "Query No-Person SW");
        Add("BtnQueryNoPersonDur", "查询无人时长", "Query No-Person Dur");
        Add("BtnQuerySleepDeadline", "查询截止时间", "Query Deadline");
        Add("BtnQueryStruggleLevel", "查询挣扎灵敏度", "Query Sensitivity");
        Add("BtnQueryExtCtrlSwitch", "查询外部开关", "Query Ext SW");
        Add("BtnQuerySleepPeriod", "查询睡眠周期", "Query Period");

        // ========== 睡眠扩展标签 ==========
        Add("LblSleepTotalDuration", "睡眠总时长:", "Total Sleep:");
        Add("LblAwakeRatio", "清醒占比:", "Awake%:");
        Add("LblLightSleepRatio", "浅睡占比:", "Light%:");
        Add("LblDeepSleepRatio", "深睡占比:", "Deep%:");
        Add("LblOutBedDuration", "离床时长:", "Out Bed Dur:");
        Add("LblLargeMoveRatio", "大幅度体动:", "Large Move:");
        Add("LblSmallMoveRatio", "小幅度体动:", "Small Move:");
        Add("LblInBedState", "入离床状态:", "Bed State:");
        Add("LblBoundaryState", "越界状态:", "Boundary:");

        // ========== 系统信息 ==========
        Add("GrpSystemFunc", "系统功能", "System Functions");
        Add("GrpRadarRange", "雷达探测范围", "Radar Range");
        Add("GrpWorkStatus", "工作状态", "Work Status");
        Add("GrpSleepSettings", "睡眠参数设置", "Sleep Settings");
        Add("GrpSleepQuery", "睡眠参数查询", "Sleep Queries");

        // ========== 睡眠扩展状态 ==========
        Add("StateInBed", "入床", "In Bed");
        Add("StateBoundaryIn", "范围内", "Inside");
        Add("StateBoundaryOut", "范围外", "Outside");
        Add("StateInitComplete", "已完成", "Complete");
        Add("StateInitNotComplete", "未完成", "Not Complete");
        Add("StateSleepTooShort", "睡眠不足", "Too Short");
        Add("StateSleepTooLong", "睡眠过长", "Too Long");
        Add("StateLongNoPerson", "异常无人", "Long No Person");
        Add("StateStruggleNormal", "正常", "Normal");
        Add("StateStruggleAbnormal", "挣扎异常", "Struggle");
        Add("StateNoPersonNormal", "正常", "Normal");
        Add("StateNoPersonAbnormal", "异常", "Abnormal");
        Add("StateSensitivityLow", "低", "Low");
        Add("StateSensitivityMid", "中", "Medium");
        Add("StateSensitivityHigh", "高", "High");
        Add("StateExtCtrlOn", "外部控制:开", "Ext Ctrl: ON");
        Add("StateExtCtrlOff", "外部控制:关", "Ext Ctrl: OFF");
        Add("StatePeriodStart", "周期:开始", "Period: Start");
        Add("StatePeriodEnd", "周期:结束", "Period: End");
        Add("StatePeriodNone", "周期:无", "Period: None");

        // ========== 睡眠扩展输入对话框 ==========
        Add("MsgInputLowBreath", "请输入低缓呼吸判读值 (10~20):", "Enter low breath threshold (10~20):");
        Add("MsgInputNoPersonDuration", "请输入无人计时时长 (30~180分钟):", "Enter no-person duration (30~180 min):");
        Add("MsgInputSleepDeadline", "请输入睡眠截止时长 (5~120分钟):", "Enter sleep deadline (5~120 min):");
        Add("MsgInputStruggleSensitivity", "请输入灵敏度 (0=低, 1=中, 2=高):", "Enter sensitivity (0=Low, 1=Medium, 2=High):");

        // ========== 副标题 ==========
        Add("AppSubtitle", "R60ABD1 毫米波雷达睡眠监测系统", "R60ABD1 mmWave Radar Sleep Monitor");
        Add("LblPort", "串口:", "Port:");

        // ========== 设置页查询结果解析标签（ParseFrameData 使用） ==========
        Add("ParsedPresence", "存在", "Presence");
        Add("ParsedPresent", "有人", "Present");
        Add("ParsedAbsent", "无人", "Absent");
        Add("ParsedMotion", "运动", "Motion");
        Add("ParsedMotionNone", "无", "None");
        Add("ParsedMotionStatic", "静止", "Static");
        Add("ParsedMotionActive", "活跃", "Active");
        Add("ParsedDistance", "距离", "Distance");
        Add("ParsedPosition", "位置", "Position");
        Add("ParsedBodyMove", "体动", "BodyMove");
        Add("ParsedSwitch", "开关", "Switch");
        Add("ParsedOn", "开启", "ON");
        Add("ParsedOff", "关闭", "OFF");
        Add("ParsedBreathValue", "呼吸值", "Breath");
        Add("ParsedBreathState", "呼吸状态", "BreathState");
        Add("ParsedBreathSwitch", "呼吸开关", "Breath SW");
        Add("ParsedWaveSwitch", "波形开关", "Wave SW");
        Add("ParsedLowBreath", "低缓判读值", "LowBreath");
        Add("ParsedHeartRate", "心率", "HeartRate");
        Add("ParsedHeartSwitch", "心率开关", "Heart SW");
        Add("ParsedBedState", "入离床", "BedState");
        Add("ParsedOutBed", "离床", "OutBed");
        Add("ParsedInBed", "入床", "InBed");
        Add("ParsedRealTime", "实时模式", "RealTime");
        Add("ParsedSleepState", "睡眠状态", "SleepState");
        Add("ParsedAwakeDur", "清醒时长", "AwakeDur");
        Add("ParsedLightDur", "浅睡时长", "LightDur");
        Add("ParsedDeepDur", "深睡时长", "DeepDur");
        Add("ParsedSleepScore", "睡眠评分", "SleepScore");
        Add("ParsedRating", "评级", "Rating");
        Add("ParsedRatingGood", "良好", "Good");
        Add("ParsedRatingAvg", "一般", "Average");
        Add("ParsedRatingPoor", "较差", "Poor");
        Add("ParsedRatingNone", "无", "None");
        Add("ParsedAbnormal", "异常", "Abnormal");
        Add("ParsedAbnormalShort", "睡眠不足", "TooShort");
        Add("ParsedAbnormalLong", "睡眠过长", "TooLong");
        Add("ParsedAbnormalNoPerson", "异常无人", "NoPerson");
        Add("ParsedAbnormalNone", "无异常", "Normal");
        Add("ParsedComposite", "综合", "Composite");
        Add("ParsedAnalysis", "分析", "Analysis");
        Add("ParsedStruggle", "挣扎", "Struggle");
        Add("ParsedSleepSwitch", "睡眠开关", "Sleep SW");
        Add("ParsedModel", "型号", "Model");
        Add("ParsedId", "ID", "ID");
        Add("ParsedHardware", "硬件", "Hardware");
        Add("ParsedFirmware", "固件", "Firmware");
        Add("ParsedProductInfo", "产品信息", "ProductInfo");
        Add("ParsedInit", "初始化", "Init");
        Add("ParsedInitDone", "已完成", "Complete");
        Add("ParsedInitNotDone", "未完成", "NotDone");
        Add("ParsedBoundary", "越界", "Boundary");
        Add("ParsedInside", "范围内", "Inside");
        Add("ParsedOutside", "范围外", "Outside");
        Add("ParsedHeartbeat", "心跳响应", "Heartbeat");
    }

    private void Add(string key, string zh, string en)
    {
        _translations[key] = new Dictionary<string, string>
        {
            ["zh"] = zh,
            ["en"] = en
        };
    }
}
