using Microsoft.VisualBasic;
using SleepSightPro.Services;
using SleepSightPro.Controls;
using SleepSightPro.Models;
using System.Windows.Forms.DataVisualization.Charting;

namespace SleepSightPro;

public partial class MainForm : Form
{
    private readonly RadarSerialService _serialService = new();
    private readonly RadarDataProcessor _dataProcessor = new();
    private readonly LocalizationService _loc = LocalizationService.Instance;

    // ===== 多语言注册 =====
    private readonly Dictionary<string, Label> _locLabels = new();
    private readonly Dictionary<string, GroupBox> _locGroups = new();
    private readonly Dictionary<string, Button> _locButtons = new();

    // ===== 国旗 =====
    private Image? _imgFlagCN;
    private Image? _imgFlagUS;
    private bool _hasFlags;

    // ===== _settingsResultBox 控制（简化版） =====
    // _resultPaused: true 时 RX 数据不写入 _settingsResultBox，由后台 Timer 设为 true/false
    // _resultResumeTimer: 两步 Timer（1s 窗口后暂停 → 10s 后恢复），访问需持有 _timerLock
    // _clearingResultBox: TextChanged 重入保护
    // 超限清空：TextChanged 事件检测 TextLength > 50000 时自动 Clear()
    private volatile bool _resultPaused;
    private bool _clearingResultBox;
    private bool _disposed;
    private System.Threading.Timer? _resultResumeTimer;
    private readonly object _timerLock = new();

    // ===== 波形图表环形缓冲区 =====
    // 15 秒窗口 × 5 采样点/秒 = 75 个点，实际存 80 防止边界问题
    private const int WaveBufferSize = 80;
    private readonly Queue<int> _breathWaveBuffer = new(WaveBufferSize);
    private readonly Queue<int> _heartWaveBuffer = new(WaveBufferSize);
    private bool _chartsInitialized;

    // ===== 数据接收标记（用于区分"未收到数据"与"值为0"） =====
    private bool _hasBreathData;
    private bool _hasHeartData;
    private bool _hasDistanceData;
    private bool _hasSleepData;

    // 固件版本自动查询
    private bool _fwQueried;

    // ===== 原始 Hex 数据 =====
    private long _rawByteCount;
    private const int MaxRawHexChars = 200000; // 约 200KB 文本，防止内存溢出

    public MainForm()
    {
        LoadFlagResources();
        InitializeComponent();
        InitStaticContent();
        InitEvents();
        RefreshPorts();
        ApplyLocalization();
    }

    private void LoadFlagResources()
    {
        try
        {
            var baseDir = AppContext.BaseDirectory;
            _imgFlagCN = Image.FromFile(Path.Combine(baseDir, "Resources", "flag_cn.png"));
            _imgFlagUS = Image.FromFile(Path.Combine(baseDir, "Resources", "flag_us.png"));
            _hasFlags = true;
        }
        catch { _hasFlags = false; }
    }

    // ========================================================================
    //  初始化设计器无法处理的动态内容
    // ========================================================================
    private void InitStaticContent()
    {
        // 语言按钮国旗图片
        if (_hasFlags)
        {
            _btnLang.Image = _imgFlagCN;
            _btnLang.Text = _loc.Get("LanguageZH");
        }
        else
        {
            _btnLang.Text = $"{_loc.Get("LanguageZH")} | {_loc.Get("LanguageEN")}";
        }

        // 设置顶部右侧工具栏动态排列
        ArrangeRightToolbar();

        // Tab 自绘模式（必须在运行时设置，设计器无法渲染 OwnerDraw 控件）
        _mainTab.DrawMode = TabDrawMode.OwnerDrawFixed;

        // 初始化仪表盘卡片标题和数值
        InitDashboardCards();

        // 初始化波形图表
        InitWaveCharts();

        // 设置页 GroupBox 本地化注册
        InitSettingsPageLocGroups();

        // 初始化通讯协议参考 Tab
        InitProtocolTab();

        // 初始化雷达方程 Tab
        InitEquationsTab();

        // 多语言注册
        RegisterLocStrings();
    }

    // ========================================================================
    //  顶部工具栏排列（从右向左 - FlowLayoutPanel 自动排列）
    // ========================================================================
    private void ArrangeRightToolbar()
    {
        // FlowLayoutPanel 的 FlowDirection.RightToLeft 会自动从右向左排列控件
        // 只需要调整面板在顶部栏中的水平位置
        _rightToolbar.Left = _topPanel.ClientSize.Width - _rightToolbar.PreferredSize.Width - 12;
    }

    // ========================================================================
    //  仪表盘卡片初始化
    // ========================================================================
    private void InitDashboardCards()
    {
        _cardPresence.Title = _loc.Get("CardPresence");
        _cardPresence.Value = _loc.Get("StateAbsent");
        _cardPresence.AccentColor = Color.FromArgb(52, 152, 219);

        _cardBreath.Title = _loc.Get("CardBreath");
        _cardBreath.Value = "--" + _loc.Get("UnitBreathRate");
        _cardBreath.AccentColor = Color.FromArgb(46, 204, 113);

        _cardHeart.Title = _loc.Get("CardHeartRate");
        _cardHeart.Value = "--" + _loc.Get("UnitBpmShort");
        _cardHeart.AccentColor = Color.FromArgb(231, 76, 60);

        _cardSleepState.Title = _loc.Get("CardSleepState");
        _cardSleepState.Value = _loc.Get("StateOutOfBed");
        _cardSleepState.AccentColor = Color.FromArgb(155, 89, 182);

        _cardBodyMove.Title = _loc.Get("CardBodyMove");
        _cardBodyMove.Value = "0" + _loc.Get("UnitPercentShort");
        _cardBodyMove.AccentColor = Color.FromArgb(243, 156, 18);

        _cardSleepScore.Title = _loc.Get("CardSleepScore");
        _cardSleepScore.Value = "-- " + _loc.Get("UnitScore");
        _cardSleepScore.AccentColor = Color.FromArgb(26, 188, 156);

        _cardDistance.Title = _loc.Get("CardDistance");
        _cardDistance.Value = "0" + _loc.Get("UnitCmShort");
        _cardDistance.AccentColor = Color.FromArgb(52, 73, 94);

        _cardApnea.Title = _loc.Get("ApneaCount");
        _cardApnea.Value = "0";
        _cardApnea.AccentColor = Color.FromArgb(192, 57, 43);

        _cardTurnOver.Title = _loc.Get("TurnOverCount");
        _cardTurnOver.Value = "0";
        _cardTurnOver.AccentColor = Color.FromArgb(41, 128, 185);
    }

    // ========================================================================
    //  设置页 GroupBox 本地化注册（按钮事件已迁移至 Designer.cs 严格设计器模式）
    // ========================================================================
    private void InitSettingsPageLocGroups()
    {
        _locGroups["GrpPresenceCtrl"] = _grpPresenceCtrl;
        _locGroups["GrpBreathCtrl"] = _grpBreathCtrl;
        _locGroups["GrpHeartCtrl"] = _grpHeartCtrl;
        _locGroups["GrpSleepCtrl"] = _grpSleepCtrl;
        _locGroups["GrpSleepSettings"] = _grpSleepSettings;
        _locGroups["GrpSleepQuery"] = _grpSleepQuery;
        _locGroups["GrpSystemFunc"] = _grpSystemFunc;
    }

    // ========================================================================
    //  通讯协议参考 Tab —— 基于 R60ABD1 用户手册 V3.6（双语）
    // ========================================================================

    /// <summary>协议命令条目（双语：方向CN/EN, 备注CN/EN）</summary>
    private record ProtoCmd(string DirCN, string DirEN, string Cmd, string Len, string DataCN, string DataEN, string NoteCN, string NoteEN);

    private void InitProtocolTab() => RefreshProtocolTab();

    private void RefreshProtocolTab()
    {
        if (_protocolTextBox == null) return;

        bool isEN = _loc.CurrentLanguage != "zh-CN";
        var sb = new System.Text.StringBuilder();

        // ── 头部 ──
        WriteBox(sb, "═══════════════════════════════════════════════════", '═', false);
        sb.AppendLine(isEN
            ? "  R60ABD1 Breathing Sleep Radar · Protocol Reference"
            : "  R60ABD1 呼吸睡眠雷达 · 通讯协议参考");
        sb.AppendLine(isEN
            ? "  Based on User Manual V3.6 | Yunfan Ruida Technology (Shenzhen) Co., Ltd."
            : "  基于用户手册 V3.6 | 云帆瑞达科技（深圳）有限公司");
        WriteBox(sb, "═══════════════════════════════════════════════════", '═', false);
        sb.AppendLine();

        // ── 一、串口配置 ──
        WriteSection(sb, isEN ? "1. Serial Port Configuration" : "一、串口配置", false);
        WriteSectionBody(sb, new[] {
            isEN ? ("Interface Level", "TTL") : ("接口电平", "TTL"),
            isEN ? ("Baud Rate", "115200 bps") : ("波特率", "115200 bps"),
            isEN ? ("Stop Bits", "1") : ("停止位", "1"),
            isEN ? ("Data Bits", "8") : ("数据位", "8"),
            isEN ? ("Parity", "None") : ("奇偶校验", "无"),
        }, "│");

        // ── 二、帧结构定义 ──
        WriteSection(sb, isEN ? "2. Frame Structure" : "二、帧结构定义", false);
        sb.AppendLine(@"│                                                                 │");
        sb.AppendLine(isEN
            ? @"│  Header     Ctrl     Cmd       Length          Data     Checksum  Tail│"
            : @"│  帧头      控制字   命令字   长度标识       数据     校验码  帧尾│");
        sb.AppendLine(@"│  53 59     Ctrl     Cmd      LenH LenL      n Byte    Sum   54 43│");
        sb.AppendLine(@"│  2 Byte    1 Byte   1 Byte   2 Byte         n Byte    1 Byte 2 Byte│");
        sb.AppendLine(@"│                                                                 │");
        if (isEN)
        {
            AppendBoxLine(sb, "Header      : Fixed 0x53 0x59");
            AppendBoxLine(sb, "Ctrl        : Function category (see §3)");
            AppendBoxLine(sb, "Cmd         : Specific command ID");
            AppendBoxLine(sb, "Len         : 2 bytes, actual data length (big-endian)");
            AppendBoxLine(sb, "Data        : n bytes, per command definition");
            AppendBoxLine(sb, "Checksum    : 1 byte = sum(Header+Ctrl+Cmd+Len+Data) & 0xFF");
            AppendBoxLine(sb, "Tail        : Fixed 0x54 0x43");
        }
        else
        {
            AppendBoxLine(sb, "帧头    : 固定为 0x53 0x59");
            AppendBoxLine(sb, "控制字  : 功能类别标识 (见第三节)");
            AppendBoxLine(sb, "命令字  : 具体命令标识");
            AppendBoxLine(sb, "长度标识: 2 字节，等于数据域的实际字节数 (大端)");
            AppendBoxLine(sb, "数据    : n 字节，按具体命令定义");
            AppendBoxLine(sb, "校验码  : 1 字节 = (帧头 + 控制字 + 命令字 + 长度 + 数据) 求和后取低 8 位");
            AppendBoxLine(sb, "帧尾    : 固定为 0x54 0x43");
        }
        WriteBoxFooter(sb, '─', false);

        // ── 三、控制字定义 ──
        WriteSection(sb, isEN ? "3. Control Word (Ctrl) Definitions" : "三、控制字 (Ctrl) 定义", false);
        var ctrlDefs = new (string Hex, string CN, string EN)[] {
            ("0x01", "心跳包标识", "Heartbeat"),
            ("0x02", "产品信息", "Product Info"),
            ("0x03", "OTA 升级", "OTA Update"),
            ("0x05", "工作状态", "Work Status"),
            ("0x07", "雷达探测范围信息", "Radar Range Info"),
            ("0x80", "人体存在", "Human Presence"),
            ("0x81", "呼吸检测", "Breath Detection"),
            ("0x84", "睡眠监测", "Sleep Monitoring"),
            ("0x85", "心率监测", "Heart Rate Monitoring"),
        };
        foreach (var (h, cn, en) in ctrlDefs)
            AppendBoxLine(sb, $"{h} ─ {(isEN ? en : cn)}");
        WriteBoxFooter(sb, '─', false);

        // ── 四、功能点总览 ──
        WriteSection(sb, isEN ? "4. Data Point (DP) Overview" : "四、功能点 (DP) 总览", false);
        var dpDefs = new (string DP, string NameCN, string NameEN, string DetailCN, string DetailEN)[] {
            ("DP1",  "有人/无人",         "Occupied/Vacant",        "状态变化上报   有人→0.5s，无人→40s",       "State change: Occ→0.5s, Vac→40s"),
            ("DP2",  "有人静止/活跃",     "Stationary/Active",      "状态变化上报   切换 0.5s 内",              "State change: switch within 0.5s"),
            ("DP3",  "人体距离",          "Body Distance",          "2s 上报一次    单位 cm，范围 0~65535",     "Report every 2s, cm, range 0-65535"),
            ("DP4",  "体动幅度参数",      "Motion Amplitude",       "1s 上报一次    范围 0~100",                "Report every 1s, range 0-100"),
            ("DP5",  "人体方位",          "Body Position",          "2s 上报一次    6B(x/y/z),cm,有正负",      "Report every 2s, 6B(x/y/z),cm,signed"),
            ("DP6",  "心跳数值",          "Heart Rate Value",       "3s 上报一次    范围 60~120 bpm",           "Report every 3s, range 60-120 bpm"),
            ("DP7",  "心率波形",          "Heart Rate Waveform",    "1s 上报一次    5B,+128偏移,0~255",        "Report every 1s, 5B,+128 offset,0-255"),
            ("DP8",  "呼吸数值",          "Breath Rate Value",      "3s 上报一次    范围 0~35 次/min",          "Report every 3s, range 0-35 bpm"),
            ("DP9",  "呼吸信息",          "Breath Info",            "状态变化上报   正常/过高/过低/无",         "State change: Normal/High/Low/None"),
            ("DP10", "呼吸波形",          "Breath Waveform",        "1s 上报一次    5B,+128偏移,0~255",        "Report every 1s, 5B,+128 offset,0-255"),
            ("DP11", "入床/离床",         "In/Out of Bed",          "状态变化上报   离→入即报/入→离~30s",      "State change: Out→In immediate / In→Out ~30s"),
            ("DP12", "睡眠状态",          "Sleep State",            "10min 上报一次 深睡/浅睡/清醒",            "Every 10min: Deep/Light/Awake"),
            ("DP13", "清醒/浅睡/深睡时长","Awake/Light/Deep Duration","随 DP12 上报   单位 min, 0~65535",       "With DP12, min, 0-65535"),
            ("DP14", "睡眠质量评分",      "Sleep Quality Score",    "睡眠结束时上报  0~100 分",                 "At sleep end, 0-100 points"),
            ("DP15", "睡眠综合状态",      "Sleep Composite Status", "10min 上报一次 存在/状态/心率/翻身等",     "Every 10min: Presence/State/HR/TurnOver/etc."),
            ("DP16", "睡眠质量分析",      "Sleep Quality Analysis", "睡眠结束时上报  含 12 项统计数据",          "At sleep end, 12 statistical items"),
            ("DP17", "睡眠异常",          "Sleep Abnormality",      "状态变化上报   不足/过长/异常无人",         "State change: TooShort/TooLong/AbnormalVacant"),
            ("DP18", "异常挣扎",          "Abnormal Struggle",      "状态变化上报   正常/异常/无",              "State change: Normal/Abnormal/None"),
            ("DP19", "无人计时",          "Vacancy Timer",          "状态变化上报   正常/异常/无",              "State change: Normal/Abnormal/None"),
            ("DP20", "睡眠质量评级",      "Sleep Quality Rating",   "状态变化上报   较差/一般/良好",            "State change: Poor/Fair/Good"),
        };
        foreach (var (dp, cnName, enName, cnDetail, enDetail) in dpDefs)
        {
            AppendBoxLine(sb, $"{dp}  {(isEN ? enName : cnName),-18} {(isEN ? enDetail : cnDetail)}");
        }
        WriteBoxFooter(sb, '─', false);

        // ── 第五至十三节：命令表 ──
        AppendProtoSection(sb, isEN,
            isEN ? "5. System Functions (Ctrl=0x01)" : "五、系统功能 (Ctrl=0x01)",
            new ProtoCmd[] {
                new("上报", "Upload", "0x01", "0001", "0F", "0F", "心跳包主动上报", "Heartbeat auto-report"),
                new("下发/回复", "Send/Reply", "0x80", "0001", "0F", "0F", "心跳包查询", "Heartbeat query"),
                new("下发", "Send", "0x02", "0001", "0F", "0F", "模组复位 → 上报 0F", "Module reset → reports 0F"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "6. Product Info (Ctrl=0x02)" : "六、产品信息 (Ctrl=0x02)",
            new ProtoCmd[] {
                new("上报", "Upload", "0x01", "var", "产品型号 (lenB 字符串)", "Product model (lenB string)", "主动上报", "Auto-report"),
                new("上报", "Upload", "0x02", "var", "产品 ID (lenB 字符串)", "Product ID (lenB string)", "主动上报", "Auto-report"),
                new("上报", "Upload", "0x03", "var", "硬件型号 (lenB 字符串)", "Hardware model (lenB string)", "主动上报", "Auto-report"),
                new("上报", "Upload", "0x04", "var", "固件版本 (lenB 字符串)", "Firmware version (lenB string)", "主动上报", "Auto-report"),
                new("下发", "Send", "0xA1", "0001", "0F → 回复产品型号", "0F → replies model", "信息查询入口", "Info query entry"),
                new("下发", "Send", "0xA2", "0001", "0F → 回复产品 ID", "0F → replies product ID", "", ""),
                new("下发", "Send", "0xA3", "0001", "0F → 回复硬件型号", "0F → replies hardware model", "", ""),
                new("下发", "Send", "0xA4", "0001", "0F → 回复固件版本", "0F → replies firmware version", "", ""),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "7. Work Status (Ctrl=0x05)" : "七、工作状态 (Ctrl=0x05)",
            new ProtoCmd[] {
                new("上报", "Upload", "0x01", "0001", "0F", "0F", "初始化完成信息", "Init complete info"),
                new("下发", "Send", "0x81", "0001", "0F → 回复: 01=已完成 00=未完成", "0F → reply: 01=Done 00=NotDone", "初始化是否完成查询", "Init complete query"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "8. Radar Range (Ctrl=0x07)" : "八、雷达探测范围 (Ctrl=0x07)",
            new ProtoCmd[] {
                new("上报", "Upload", "0x07", "0001", "00=范围外 01=范围内", "00=OutOfRange 01=InRange", "位置越界状态上报(变化时)", "Position boundary report (on change)"),
                new("下发", "Send", "0x87", "0001", "0F → 回复: 00/01", "0F → reply: 00/01", "位置越界状态查询", "Position boundary query"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "9. Human Presence (Ctrl=0x80)" : "九、人体存在 (Ctrl=0x80)",
            new ProtoCmd[] {
                new("下发", "Send", "0x00", "0001", "01=开 00=关", "01=On 00=Off", "开关人体存在功能", "Toggle presence function"),
                new("上报", "Upload", "0x01", "0001", "00=无人 01=有人", "00=Vacant 01=Occupied", "存在信息主动上报(变化时)", "Presence report (on change)"),
                new("上报", "Upload", "0x02", "0001", "00=无 01=静止 02=活跃", "00=None 01=Stationary 02=Active", "运动信息主动上报(变化时)", "Motion report (on change)"),
                new("上报", "Upload", "0x03", "0001", "1B 体动参数(0~100)", "1B motion param(0-100)", "体动参数主动上报(1s一次)", "Motion param report (every 1s)"),
                new("上报", "Upload", "0x04", "0002", "2B 人体距离(0~65535 cm)", "2B distance(0-65535 cm)", "人体距离主动上报(2s一次)", "Distance report (every 2s)"),
                new("上报", "Upload", "0x05", "0006", "6B: x(2B) y(2B) z(2B) cm", "6B: x(2B) y(2B) z(2B) cm", "人体方位主动上报(2s一次)", "Position report (every 2s)"),
                new("下发", "Send", "0x80", "0001", "0F → 回复: 01=开 00=关", "0F → reply: 01=On 00=Off", "查询人体存在开关", "Query presence switch"),
                new("下发", "Send", "0x81", "0001", "0F → 回复: 00/01", "0F → reply: 00/01", "存在信息查询", "Query presence"),
                new("下发", "Send", "0x82", "0001", "0F → 回复: 00/01/02", "0F → reply: 00/01/02", "运动信息查询", "Query motion"),
                new("下发", "Send", "0x83", "0001", "0F → 回复: 1B 体动参数", "0F → reply: 1B motion param", "体动参数查询", "Query motion param"),
                new("下发", "Send", "0x84", "0001", "0F → 回复: 2B 距离", "0F → reply: 2B distance", "人体距离查询", "Query distance"),
                new("下发", "Send", "0x85", "0001", "0F → 回复: 6B 方位", "0F → reply: 6B position", "人体方位查询", "Query position"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "10. Heart Rate Monitoring (Ctrl=0x85)" : "十、心率监测 (Ctrl=0x85)",
            new ProtoCmd[] {
                new("下发", "Send", "0x00", "0001", "01=开 00=关", "01=On 00=Off", "开关心率监测功能", "Toggle HR monitoring"),
                new("上报", "Upload", "0x02", "0001", "1B 心率数值(60~120)", "1B HR value(60-120)", "心率数值主动上报(3s一次)", "HR value report (every 3s)"),
                new("上报", "Upload", "0x05", "0005", "5B 心率波形(0~255)", "5B HR waveform(0-255)", "心率波形上报(1s一次,5采样点)", "HR waveform (every 1s, 5 samples)"),
                new("下发", "Send", "0x0A", "0001", "00=关 01=开", "00=Off 01=On", "心率波形上报开关设置(默认关闭)", "HR waveform report switch (default off)"),
                new("下发", "Send", "0x80", "0001", "0F → 回复: 01=开 00=关", "0F → reply: 01=On 00=Off", "查询心率监测开关", "Query HR monitoring switch"),
                new("下发", "Send", "0x82", "0001", "0F → 回复: 1B 心率数值", "0F → reply: 1B HR value", "心率数值查询", "Query HR value"),
                new("下发", "Send", "0x85", "0001", "0F → 回复: 5B 心率波形", "0F → reply: 5B HR waveform", "心率波形查询", "Query HR waveform"),
                new("下发", "Send", "0x8A", "0001", "0F → 回复: 00=关 01=开", "0F → reply: 00=Off 01=On", "心率波形上报开关查询", "Query HR waveform switch"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "11. Breath Detection (Ctrl=0x81)" : "十一、呼吸检测 (Ctrl=0x81)",
            new ProtoCmd[] {
                new("下发", "Send", "0x00", "0001", "01=开 00=关", "01=On 00=Off", "开关呼吸监测功能", "Toggle breath monitoring"),
                new("上报", "Upload", "0x01", "0001", "01=正常 02=过高 03=过低 04=无", "01=Normal 02=High 03=Low 04=None", "呼吸信息上报(变化时)", "Breath info report (on change)"),
                new("上报", "Upload", "0x02", "0001", "1B 呼吸数值(0~35)", "1B breath value(0-35)", "呼吸数值主动上报(3s一次)", "Breath value report (every 3s)"),
                new("上报", "Upload", "0x05", "0005", "5B 呼吸波形(0~255)", "5B breath waveform(0-255)", "呼吸波形上报(1s一次,5采样点)", "Breath waveform (every 1s, 5 samples)"),
                new("下发", "Send", "0x0B", "0001", "1B 阈值(10~20,默认10)", "1B threshold(10-20, default 10)", "低缓呼吸判读设置", "Set bradypnea threshold"),
                new("下发", "Send", "0x0C", "0001", "00=关 01=开", "00=Off 01=On", "呼吸波形上报开关设置(默认关闭)", "Breath waveform report switch (default off)"),
                new("上报", "Upload", "0x0D", "0001", "1B 数值(0~35)", "1B value(0-35)", "时域呼吸值(内测,1s一次)", "Time-domain breath value (beta, every 1s)"),
                new("下发", "Send", "0x80", "0001", "0F → 回复: 01=开 00=关", "0F → reply: 01=On 00=Off", "查询呼吸监测开关", "Query breath monitoring switch"),
                new("下发", "Send", "0x81", "0001", "0F → 回复: 01/02/03/04", "0F → reply: 01/02/03/04", "呼吸信息查询", "Query breath info"),
                new("下发", "Send", "0x82", "0001", "0F → 回复: 1B 呼吸数值", "0F → reply: 1B breath value", "呼吸数值查询", "Query breath value"),
                new("下发", "Send", "0x85", "0001", "0F → 回复: 5B 呼吸波形", "0F → reply: 5B breath waveform", "呼吸波形查询", "Query breath waveform"),
                new("下发", "Send", "0x8B", "0001", "0F → 回复: 1B 阈值", "0F → reply: 1B threshold", "低缓呼吸判读查询", "Query bradypnea threshold"),
                new("下发", "Send", "0x8C", "0001", "0F → 回复: 00=关 01=开", "0F → reply: 00=Off 01=On", "呼吸波形上报开关查询", "Query breath waveform switch"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "12. Sleep Monitoring (Ctrl=0x84)" : "十二、睡眠监测 (Ctrl=0x84)",
            new ProtoCmd[] {
                new("下发", "Send", "0x00", "0001", "01=开 00=关", "01=On 00=Off", "开关睡眠监测功能", "Toggle sleep monitoring"),
                new("上报", "Upload", "0x01", "0001", "00=离床 01=入床 02=无(实时)", "00=Out 01=In 02=None(realtime)", "入床/离床状态上报(变化时)", "Bed state report (on change)"),
                new("上报", "Upload", "0x02", "0001", "00=深睡 01=浅睡 02=清醒 03=无", "00=Deep 01=Light 02=Awake 03=None", "睡眠状态上报(10min一次)", "Sleep state report (every 10min)"),
                new("上报", "Upload", "0x03", "0002", "2B 清醒时长(min,0~65535)", "2B awake duration(min,0-65535)", "清醒时长上报(随DP12)", "Awake duration (with DP12)"),
                new("上报", "Upload", "0x04", "0002", "2B 浅睡时长(min,0~65535)", "2B light sleep duration(min,0-65535)", "浅睡时长上报(随DP12)", "Light sleep duration (with DP12)"),
                new("上报", "Upload", "0x05", "0002", "2B 深睡时长(min,0~65535)", "2B deep sleep duration(min,0-65535)", "深睡时长上报(随DP12)", "Deep sleep duration (with DP12)"),
                new("上报", "Upload", "0x06", "0001", "1B 睡眠评分(0~100)", "1B sleep score(0-100)", "睡眠质量评分(睡眠结束时)", "Sleep score (at sleep end)"),
                new("上报", "Upload", "0x0C", "0008", "8B 综合状态(存在/状态/呼吸/心率/翻身/大动/小动/呼吸暂停)", "8B composite(Presence/State/Breath/HR/Turn/Large/Small/Apnea)", "睡眠综合状态上报(10min)", "Sleep composite status (every 10min)"),
                new("上报", "Upload", "0x0D", "000C", "12B 睡眠分析(评分/总时/各占比/离床/翻身/呼吸/心率/暂停)", "12B analysis(Score/Total/Ratios/OutOfBed/Turn/Breath/HR/Apnea)", "睡眠质量分析上报(睡眠结束时)", "Sleep analysis (at sleep end)"),
                new("上报", "Upload", "0x0E", "0001", "00=不足4h 01>12h 02=异常无人 03=无", "00=<4h 01=>12h 02=AbnormalVacant 03=None", "睡眠异常上报(变化时)", "Sleep abnormality (on change)"),
                new("上报", "Upload", "0x10", "0001", "00=无 01=良好 02=一般 03=较差", "00=None 01=Good 02=Fair 03=Poor", "睡眠质量评级(结束时)", "Sleep quality rating (at end)"),
                new("上报", "Upload", "0x11", "0001", "00=无 01=正常 02=异常挣扎", "00=None 01=Normal 02=AbnormalStruggle", "异常挣扎状态上报(变化时)", "Abnormal struggle report (on change)"),
                new("上报", "Upload", "0x12", "0001", "00=无 01=正常 02=异常", "00=None 01=Normal 02=Abnormal", "无人计时状态上报(变化时)", "Vacancy timer report (on change)"),
                new("下发", "Send", "0x13", "0001", "00=关 01=开", "00=Off 01=On", "异常挣扎状态开关设置", "Set abnormal struggle switch"),
                new("下发", "Send", "0x14", "0001", "00=关 01=开", "00=Off 01=On", "无人计时功能开关设置", "Set vacancy timer switch"),
                new("下发", "Send", "0x15", "0001", "1B 时长(30~180 min,步长10)", "1B duration(30-180 min, step 10)", "无人计时时长设置", "Set vacancy timer duration"),
                new("下发", "Send", "0x16", "0001", "1B 时长(5~120 min)", "1B duration(5-120 min)", "睡眠截止时长设置", "Set sleep deadline duration"),
                new("下发", "Send", "0x1A", "0001", "00=低 01=中(默认) 02=高", "00=Low 01=Mid(default) 02=High", "挣扎状态判读设置(需配合固件)", "Set struggle detection level (firmware dependent)"),
                new("下发", "Send", "0x1C", "0001", "00=关 01=开, 默认打开", "00=Off 01=On, default On", "睡眠周期外部控制开关", "Sleep cycle external control switch"),
                new("下发", "Send", "0x1D", "0001", "00=无 01=开始 02=结束", "00=None 01=Start 02=End", "睡眠周期开始/截止设置", "Sleep cycle start/end"),
                new("下发", "Send", "0x80", "0001", "0F → 回复: 01=开 00=关", "0F → reply: 01=On 00=Off", "查询睡眠监测开关", "Query sleep monitoring switch"),
                new("下发", "Send", "0x81", "0001", "0F → 回复: 00=离床 01=入床", "0F → reply: 00=Out 01=In", "入床/离床状态查询", "Query bed state"),
                new("下发", "Send", "0x82", "0001", "0F → 回复: 00/01/02/03", "0F → reply: 00/01/02/03", "睡眠状态查询", "Query sleep state"),
                new("下发", "Send", "0x83", "0001", "0F → 回复: 2B 清醒时长", "0F → reply: 2B awake duration", "清醒时长查询", "Query awake duration"),
                new("下发", "Send", "0x84", "0001", "0F → 回复: 2B 浅睡时长", "0F → reply: 2B light duration", "浅睡时长查询", "Query light sleep duration"),
                new("下发", "Send", "0x85", "0001", "0F → 回复: 2B 深睡时长", "0F → reply: 2B deep duration", "深睡时长查询", "Query deep sleep duration"),
                new("下发", "Send", "0x86", "0001", "0F → 回复: 1B 睡眠评分", "0F → reply: 1B sleep score", "睡眠质量评分查询", "Query sleep score"),
                new("下发", "Send", "0x8D", "0001", "0F → 回复: 8B 综合状态", "0F → reply: 8B composite status", "睡眠综合状态查询", "Query composite status"),
                new("下发", "Send", "0x8E", "0001", "0F → 回复: 00/01/02/03", "0F → reply: 00/01/02/03", "睡眠异常查询", "Query sleep abnormality"),
                new("下发", "Send", "0x8F", "0001", "0F → 回复: 12B 睡眠分析", "0F → reply: 12B sleep analysis", "睡眠统计查询", "Query sleep statistics"),
                new("下发", "Send", "0x90", "0001", "0F → 回复: 00/01/02/03", "0F → reply: 00/01/02/03", "睡眠质量评级查询", "Query sleep rating"),
                new("下发", "Send", "0x91", "0001", "0F → 回复: 00/01/02", "0F → reply: 00/01/02", "异常挣扎状态查询", "Query abnormal struggle"),
                new("下发", "Send", "0x92", "0001", "0F → 回复: 00/01/02", "0F → reply: 00/01/02", "无人计时状态查询", "Query vacancy timer"),
                new("下发", "Send", "0x93", "0001", "0F → 回复: 00=关 01=开", "0F → reply: 00=Off 01=On", "异常挣扎开关查询", "Query struggle switch"),
                new("下发", "Send", "0x94", "0001", "0F → 回复: 00=关 01=开", "0F → reply: 00=Off 01=On", "无人计时开关查询", "Query vacancy timer switch"),
                new("下发", "Send", "0x95", "0001", "0F → 回复: 1B 时长", "0F → reply: 1B duration", "无人计时时长查询", "Query vacancy timer duration"),
                new("下发", "Send", "0x96", "0001", "0F → 回复: 1B 时长", "0F → reply: 1B duration", "睡眠截止时间查询", "Query sleep deadline"),
                new("下发", "Send", "0x9A", "0001", "0F → 回复: 00/01/02", "0F → reply: 00/01/02", "挣扎状态判读查询", "Query struggle detection level"),
                new("下发", "Send", "0x9C", "0001", "0F → 回复: 00=关 01=开", "0F → reply: 00=Off 01=On", "睡眠周期外部控制开关查询", "Query sleep cycle external control"),
                new("下发", "Send", "0x9D", "0001", "0F → 回复: 00/01/02", "0F → reply: 00/01/02", "睡眠周期开始截止查询", "Query sleep cycle start/end"),
            });

        AppendProtoSection(sb, isEN,
            isEN ? "13. OTA Update (Ctrl=0x03)" : "十三、OTA 升级 (Ctrl=0x03)",
            new ProtoCmd[] {
                new("下发", "Send", "0x01", "0004", "4B 固件包大小(大端)", "4B firmware size (big-endian)", "开始OTA升级", "Start OTA"),
                new("回复", "Reply", "0x01", "0004", "4B 每帧传输升级包大小", "4B packet size per frame", "上位机按此大小分包", "Host splits by this size"),
                new("下发", "Send", "0x02", "4+len", "4B 包偏移地址 + lenB 数据包", "4B offset + lenB data packet", "升级包传输", "Upgrade packet transfer"),
                new("回复", "Reply", "0x02", "0001", "01=接收成功 02=接收失败", "01=RxSuccess 02=RxFail", "", ""),
                new("下发", "Send", "0x03", "0001", "01=发送完成 02=未完成", "01=Done 02=NotDone", "结束OTA升级", "End OTA"),
                new("回复", "Reply", "0x03", "0001", "01", "01", "", ""),
            });

        // ── 十四、指令生成示例 ──
        sb.AppendLine();
        WriteSection(sb, isEN ? "14. Command Examples" : "十四、指令生成示例", false);
        if (isEN)
        {
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  Example: Query Presence                                        │");
            sb.AppendLine(@"│  Header: 53 59 | Ctrl: 80 | Cmd: 81 | Len: 00 01 | Data: 0F   │");
            sb.AppendLine(@"│  Sum = (53+59+80+81+00+01+0F) = 01BD → low byte = BD          │");
            sb.AppendLine(@"│  Full command: 53 59 80 81 00 01 0F BD 54 43                   │");
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  Example: Enable Heart Rate Monitoring                          │");
            sb.AppendLine(@"│  Ctrl=85, Cmd=00, Data=01                                      │");
            sb.AppendLine(@"│  Sum = (53+59+85+00+00+01+01) = 0133 → BD                     │");
            sb.AppendLine(@"│  Full command: 53 59 85 00 00 01 01 33 54 43                    │");
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  Notes:                                                         │");
            sb.AppendLine(@"│  · Data=0F means 'Query' (fixed value, no actual meaning)       │");
            sb.AppendLine(@"│  · Data=actual value means 'Set'                                │");
            sb.AppendLine(@"│  · Reply frame Data = actual query result                       │");
            sb.AppendLine(@"│  · Waveform frame Data=5B, each byte = real+128, midline=128    │");
            sb.AppendLine(@"│  · Position: 16-bit, bit0=0 positive, bit0=1 negative, 15b=distance│");
        }
        else
        {
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  例：查询存在信息                                               │");
            sb.AppendLine(@"│  帧头: 53 59 | Ctrl: 80 | Cmd: 81 | Len: 00 01 | Data: 0F      │");
            sb.AppendLine(@"│  Sum = (53+59+80+81+00+01+0F) = 01BD → 取低字节 = BD          │");
            sb.AppendLine(@"│  完整指令: 53 59 80 81 00 01 0F BD 54 43                       │");
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  例：开启心率监测                                               │");
            sb.AppendLine(@"│  Ctrl=85, Cmd=00, Data=01                                      │");
            sb.AppendLine(@"│  Sum = (53+59+85+00+00+01+01) = 0133 → BD                     │");
            sb.AppendLine(@"│  完整指令: 53 59 85 00 00 01 01 33 54 43                        │");
            sb.AppendLine(@"│                                                                 │");
            sb.AppendLine(@"│  说明：                                                         │");
            sb.AppendLine(@"│  · 下发命令的数据为 0F 时表示「查询」（固定值，无实际意义）     │");
            sb.AppendLine(@"│  · 下发命令的数据为实际值时表示「设置」                         │");
            sb.AppendLine(@"│  · 回复帧的 Data 为实际查询结果                                 │");
            sb.AppendLine(@"│  · 波形帧 Data=5B，每个字节为 real+128，中轴线=128              │");
            sb.AppendLine(@"│  · 方位坐标：16位bit0=0为正,bit0=1为负,剩余15位=距离值         │");
        }
        WriteBoxFooter(sb, '─', false);

        _protocolTextBox.Text = sb.ToString();
    }

    // ── 协议 Tab 辅助方法 ──

    private void AppendProtoSection(System.Text.StringBuilder sb, bool isEN, string title, ProtoCmd[] cmds)
    {
        WriteSection(sb, title, false);
        string dirHdr = isEN ? "Dir" : "方向";
        string cmdHdr = isEN ? "Cmd" : "命令字";
        string lenHdr = isEN ? "Len" : "长度";
        string dataHdr = isEN ? "Data / Note" : "数据 / 备注";
        sb.AppendLine($"│ {dirHdr,-4} │ {cmdHdr,-6} │ {lenHdr,-6} │ {dataHdr,-38} │");
        sb.AppendLine(@"├──────┼────────┼────────┼────────────────────────────────────────┤");
        foreach (var c in cmds)
        {
            string dir = isEN ? c.DirEN : c.DirCN;
            string data = isEN ? c.DataEN : c.DataCN;
            string note = isEN ? c.NoteEN : c.NoteCN;
            string line = $"│ {dir,-4} │ {c.Cmd,-6} │ {c.Len,-6} │ {data}";
            if (!string.IsNullOrEmpty(note))
                line += $"    // {note}";
            int pad = Math.Max(1, 66 - GetDisplayWidth(line));
            sb.AppendLine(line + new string(' ', pad) + "│");
        }
        sb.AppendLine(@"└──────┴────────┴────────┴────────────────────────────────────────┘");
        sb.AppendLine();
    }

    private static void WriteSection(System.Text.StringBuilder sb, string title, bool wrap = true)
    {
        if (wrap) sb.AppendLine();
        sb.AppendLine(@"┌─────────────────────────────────────────────────────────────────┐");
        sb.AppendLine($"│ {title}{new string(' ', Math.Max(0, 63 - title.Length))}│");
        sb.AppendLine(@"├─────────────────────────────────────────────────────────────────┤");
    }

    private static void WriteSectionBody(System.Text.StringBuilder sb, (string Label, string Value)[] rows, string prefix)
    {
        foreach (var (label, value) in rows)
            AppendBoxLine(sb, $"{label,-10}: {value}");
    }

    private static void WriteBoxFooter(System.Text.StringBuilder sb, char fill, bool wrap = true)
    {
        sb.AppendLine($"└{new string(fill, 65)}┘");
        if (wrap) sb.AppendLine();
    }

    private static void WriteBox(System.Text.StringBuilder sb, string content, char fill, bool wrap = true)
    {
        sb.AppendLine(content);
    }

    private static void AppendBoxLine(System.Text.StringBuilder sb, string content)
    {
        int pad = Math.Max(1, 66 - GetDisplayWidth(content));
        sb.AppendLine($"│  {content}{new string(' ', pad)}│");
    }

    /// <summary>粗略计算中文字符显示宽度（中文≈2，ASCII≈1）</summary>
    private static int GetDisplayWidth(string s)
    {
        int w = 0;
        foreach (char c in s)
            w += c > 127 ? 2 : 1;
        return w;
    }

    // ========================================================================
    //  60GHz 毫米波雷达核心方程 Tab（GDI+ 绘制 - 垂直单列卡片布局）
    // ========================================================================

    private void InitEquationsTab()
    {
        // Enable double-buffering to eliminate flicker during scroll redraws
        typeof(Panel).InvokeMember("DoubleBuffered",
            System.Reflection.BindingFlags.SetProperty |
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.NonPublic,
            null, _equationsPanel, new object[] { true });

        UpdateEquationsScrollSize();
        _equationsPanel!.Resize += (s, e) => UpdateEquationsScrollSize();
        _equationsPanel.MouseDoubleClick += OnEquationsPanelDoubleClick;
    }

    private void UpdateEquationsScrollSize()
    {
        if (_equationsPanel == null) return;
        // Use Width (not ClientRectangle.Width) to avoid oscillation when
        // vertical scrollbar appears/disappears cycling the Resize event.
        int refW = _equationsPanel.Width;
        int totalH = CalculateEquationsHeight(refW);
        _equationsPanel.AutoScrollMinSize = new Size(refW, totalH);
    }

    private int CalculateEquationsHeight(int panelW)
    {
        const int pad = 28;
        int cardW = Math.Min(panelW - pad * 2, 960);
        int cardX = (panelW - cardW) / 2;
        int y = pad; // banner top

        y += 78;  // banner
        y += 24;  // gap

        // 5 cards + gaps
        for (int i = 0; i < 5; i++)
        {
            y += GetCardHeight(i, cardW);
            y += (i < 4) ? 20 : 0; // gap between cards (except last)
        }

        y += 24; // gap before summary
        y += 290; // summary
        y += pad; // bottom padding

        return y;
    }

    private static int GetCardHeight(int idx, int cardW)
    {
        // Card heights tuned for content
        int[] heights = { 280, 270, 260, 240, 260 };
        // Scale slightly for narrow panels
        float scale = cardW < 600 ? 1.1f : 1.0f;
        return (int)(heights[idx] * scale);
    }

    private void RefreshEquationsTab()
    {
        if (_equationsPanel == null) return;
        UpdateEquationsScrollSize();
        _equationsPanel.Invalidate();
    }

    // ──────────────────────────────────────────────
    //  双击卡片标题 → 截图到剪贴板
    // ──────────────────────────────────────────────

    private void OnEquationsPanelDoubleClick(object? sender, MouseEventArgs e)
    {
        if (_equationsPanel == null) return;

        // 将鼠标坐标从屏幕坐标映射到虚拟绘制坐标（考虑滚动偏移）
        Point scroll = _equationsPanel.AutoScrollPosition;
        int virtX = e.X - scroll.X;
        int virtY = e.Y - scroll.Y;

        int panelW = _equationsPanel.Width;
        const int pad = 28;
        int cardW = Math.Min(panelW - pad * 2, 960);
        int cardX = (panelW - cardW) / 2;

        // 遍历 5 张卡片，检查是否点击在标题区域（卡片顶部 headerH = 48px）
        int y = pad + 78; // banner offset
        for (int i = 0; i < 5; i++)
        {
            if (i > 0) y += 4;
            int cardH = GetCardHeight(i, cardW);
            var cardRect = new Rectangle(cardX, y, cardW, cardH);
            int headerH = 48;
            var titleRect = new Rectangle(cardX, y, cardW, headerH);

            if (titleRect.Contains(virtX, virtY))
            {
                CaptureEquationsScreenshot();
                return;
            }

            // advance y to next card
            if (i < 4)
                y = cardRect.Bottom + 2 + 16 + 6; // arrow gap
            else
                y = cardRect.Bottom;
        }
    }

    private void CaptureEquationsScreenshot()
    {
        try
        {
            const int renderW = 960;
            bool isEN = _loc.CurrentLanguage != "zh-CN";
            int totalH = CalculateEquationsHeight(renderW);

            using var bmp = new Bitmap(renderW, totalH);
            bmp.SetResolution(96, 96);

            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            DrawEquationsContent(g, renderW, isEN);

            Clipboard.SetImage(bmp);

            string msg = isEN
                ? "Equations screenshot copied to clipboard  (" + renderW + "×" + totalH + ")"
                : "方程卡片截图已复制到剪贴板  (" + renderW + "×" + totalH + ")";
            _statusLabel.Text = msg;
        }
        catch (Exception ex)
        {
            _statusLabel.Text = _loc.CurrentLanguage != "zh-CN"
                ? "Failed to capture screenshot: " + ex.Message
                : "截图失败：" + ex.Message;
        }
    }

    private void _equationsPanel_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        // CRITICAL: translate by scroll offset so content draws at correct
        // virtual coordinates when the panel is scrolled.
        Point scroll = _equationsPanel!.AutoScrollPosition;
        g.TranslateTransform(scroll.X, scroll.Y);
        DrawEquationsContent(g, _equationsPanel!.Width, _loc.CurrentLanguage != "zh-CN");
    }

    /// <summary>绘制全部方程卡片内容（供 Paint 和截图复用）。</summary>
    private void DrawEquationsContent(Graphics g, int panelW, bool isEN)
    {

        const int pad = 28;
        int cardW = Math.Min(panelW - pad * 2, 960);
        int cardX = (panelW - cardW) / 2;

        // ── 字体 ──
        using var fontBanner = new Font("Microsoft YaHei", 18F, FontStyle.Bold);
        using var fontCardTitle = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        using var fontEq = new Font("Consolas", 14F, FontStyle.Bold);
        using var fontParam = new Font("Microsoft YaHei", 9F);
        using var fontParamTitle = new Font("Microsoft YaHei", 9F, FontStyle.Bold);
        using var fontInsight = new Font("Microsoft YaHei", 9.5F);
        using var fontNum = new Font("Consolas", 24F, FontStyle.Bold);
        using var fontSummaryTitle = new Font("Microsoft YaHei", 15F, FontStyle.Bold);
        using var fontSummaryBody = new Font("Microsoft YaHei", 10F);
        using var fontSummaryArrow = new Font("Segoe UI Symbol", 12F, FontStyle.Bold);
        using var fontFooter = new Font("Microsoft YaHei", 8.5F);

        // ── 背景 ──
        Color bg = Color.FromArgb(18, 20, 26);
        g.Clear(bg);

        // ── 颜色 ──
        Color[] accents = {
            Color.FromArgb(68, 138, 255),
            Color.FromArgb(255, 82, 82),
            Color.FromArgb(0, 230, 118),
            Color.FromArgb(255, 200, 30),
            Color.FromArgb(200, 80, 255),
        };

        string[][] titles = {
            new[] { "麦克斯韦方程组 → 电磁波",     "Maxwell Equations → EM Wave" },
            new[] { "雷达距离方程",               "Radar Range Equation" },
            new[] { "FMCW 差频 · 测距核心",       "FMCW Beat Frequency" },
            new[] { "距离分辨率",                 "Range Resolution" },
            new[] { "相位差测速 · 二维 FFT",      "Velocity via Phase · 2D-FFT" },
        };

        // Each card: (big equation lines, parameter lines, insight line)
        var eqTexts = new[] {
            // ① Maxwell
            new[] {
                new[] { "∇×E = -∂B/∂t    ∇×B = μ₀J + μ₀ε₀∂E/∂t", "⇒ ∇²E = (1/c²)·∂²E/∂t²", "λ = c/f  ≈  5 mm  @ 60 GHz" },
                new[] { "∇×E = -∂B/∂t    ∇×B = μ₀J + μ₀ε₀∂E/∂t", "⇒ ∇²E = (1/c²)·∂²E/∂t²", "λ = c/f  ≈  5 mm  @ 60 GHz" },
            },
            // ② Range equation
            new[] {
                new[] { "       Pₜ · Gₜ · Gᵣ · λ² · σ", "Pᵣ  =  ────────────────────", "            (4π)³ · R⁴", "Pᵣ ∝ 1/R⁴" },
                new[] { "       Pₜ · Gₜ · Gᵣ · λ² · σ", "Pᵣ  =  ────────────────────", "            (4π)³ · R⁴", "Pᵣ ∝ 1/R⁴" },
            },
            // ③ FMCW
            new[] {
                new[] { "         2B", "f_IF  =  ──── · R", "        c · T_c" },
                new[] { "         2B", "f_IF  =  ──── · R", "        c · T_c" },
            },
            // ④ Resolution
            new[] {
                new[] { "         c", "ΔR  =  ────", "         2B", "B = 4 GHz  →  ΔR ≈ 3.75 cm" },
                new[] { "         c", "ΔR  =  ────", "         2B", "B = 4 GHz  →  ΔR ≈ 3.75 cm" },
            },
            // ⑤ Velocity
            new[] {
                new[] { "        λ · Δφ", "v  =  ─────────", "        4π · T_c" },
                new[] { "        λ · Δφ", "v  =  ─────────", "        4π · T_c" },
            },
        };

        string[][][] paramTexts = {
            new[] {
                new[] { "E: 电场  |  B: 磁场  |  c: 光速 (3×10⁸ m/s)", "μ₀ε₀: 真空介电/磁导率  |  60GHz → 毫米波" },
                new[] { "E: Electric field    B: Magnetic field    c: Speed of light", "μ₀ε₀: Vacuum permittivity/permeability  →  mmWave" },
            },
            new[] {
                new[] { "Pᵣ: 接收功率  |  Pₜ: 发射功率  |  Gₜ, Gᵣ: 天线增益", "σ: 目标 RCS  |  λ: 波长 (5 mm)  |  R: 目标距离" },
                new[] { "Pᵣ: Rx power  |  Pₜ: Tx power  |  Gₜ,Gᵣ: Antenna gain", "σ: Target RCS  |  λ: Wavelength (5 mm)  |  R: Range" },
            },
            new[] {
                new[] { "f_IF: 差频  |  B: Chirp 扫频带宽 (如 4 GHz)  |  T_c: Chirp 周期", "c: 光速  |  R: 目标距离  |  f_IF ∝ R (线性关系)" },
                new[] { "f_IF: Beat frequency  |  B: Chirp bandwidth  |  T_c: Chirp duration", "c: Speed of light  |  R: Target range  |  f_IF ∝ R" },
            },
            new[] {
                new[] { "ΔR: 最小可分辨距离差  |  B: Chirp 扫频带宽", "带宽越宽 → 分辨率越高  |  B = 4 GHz  →  ΔR ≈ 3.75 cm" },
                new[] { "ΔR: Min resolvable range  |  B: Chirp bandwidth", "Larger B → finer resolution  |  B = 4 GHz  →  ΔR ≈ 3.75 cm" },
            },
            new[] {
                new[] { "v: 径向速度  |  λ: 波长  |  Δφ: Chirp 间相位差  |  T_c: Chirp 间隔", "60GHz 下 λ = 5mm → 相位对亚毫米位移极度敏感" },
                new[] { "v: Radial velocity  |  λ: Wavelength  |  Δφ: Phase diff  |  T_c: Interval", "At 60GHz, λ = 5mm → phase ultra-sensitive to sub-mm motion" },
            },
        };

        string[][] insightTexts = {
            new[] {
                "麦克斯韦方程组统一了电场与磁场，预言电磁波的存在。60GHz 毫米波即 λ≈5mm 的电磁波，是一切雷达物理的根基。",
                "Maxwell's equations unified electric & magnetic fields, predicted EM waves. 60GHz mmWave (λ≈5mm) is the physical foundation of all radar.",
            },
            new[] {
                "接收功率 ∝ 1/R⁴：距离翻倍，信号衰减至 1/16。此方程决定了雷达的最大有效探测距离。",
                "Rx power ∝ 1/R⁴. Double the range → signal drops to 1/16×. Determines the maximum detection range.",
            },
            new[] {
                "FFT 将差频直接映射为距离轴。这是 FMCW 雷达的核心 —— 使亚毫米级胸壁位移检测 (呼吸/心率) 成为可能。",
                "FFT maps beat frequency directly to range. Core of FMCW radar — enables sub-mm chest wall displacement detection.",
            },
            new[] {
                "ΔR 决定了雷达系统的测距精度极限。3.75 cm 的分辨率足以识别呼吸引起的胸壁微动变化。",
                "ΔR sets the range accuracy limit. 3.75 cm resolution is sufficient to detect chest wall motion from breathing.",
            },
            new[] {
                "通过多 Chirp 间的相位差计算速度，2D-FFT 得到距离-速度图。胸壁微多普勒 → 呼吸频率 & 心率提取。",
                "Velocity from inter-chirp phase difference; 2D-FFT → Range-Velocity map. Chest wall micro-Doppler → breath & heart rate.",
            },
        };

        // ── Banner ──
        int y = pad;
        var bannerRect = new Rectangle(cardX, y, cardW, 66);
        using (var bannerBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
            bannerRect, Color.FromArgb(32, 42, 62), Color.FromArgb(20, 28, 40), 90f))
        using (var bannerPath = RoundedRect(bannerRect, 8))
        using (var bannerBorder = new Pen(Color.FromArgb(60, 75, 105), 1f))
        {
            g.FillPath(bannerBrush, bannerPath);
            g.DrawPath(bannerBorder, bannerPath);
        }

        string bannerText = isEN
            ? "60GHz mmWave Radar · Five Core Equations"
            : "60GHz 毫米波雷达 · 五大核心方程";
        var bannerSz = g.MeasureString(bannerText, fontBanner);
        using (var bannerTextBrush = new SolidBrush(Color.FromArgb(210, 220, 245)))
            g.DrawString(bannerText, fontBanner, bannerTextBrush,
                bannerRect.X + (bannerRect.Width - bannerSz.Width) / 2,
                bannerRect.Y + (bannerRect.Height - bannerSz.Height) / 2);

        y += 78;

        // ── 5 张卡片 ──
        for (int i = 0; i < 5; i++)
        {
            if (i > 0) y += 4; // small extra gap

            int cardH = GetCardHeight(i, cardW);
            var cardRect = new Rectangle(cardX, y, cardW, cardH);

            DrawVerticalCard(g, cardRect,
                $"{i + 1}", titles[i][isEN ? 1 : 0],
                eqTexts[i][isEN ? 1 : 0],
                paramTexts[i][isEN ? 1 : 0],
                insightTexts[i][isEN ? 1 : 0],
                accents[i],
                fontCardTitle, fontEq, fontParam, fontParamTitle, fontInsight, fontNum);

            // Connector arrow between cards
            if (i < 4)
            {
                y = cardRect.Bottom + 2;
                float midX = cardRect.X + cardRect.Width / 2f;
                using var arrowBrush = new SolidBrush(Color.FromArgb(100, 110, 140));
                using var arrowFont = new Font("Segoe UI Symbol", 14F, FontStyle.Bold);
                var arrowSz = g.MeasureString("↓", arrowFont);
                g.DrawString("↓", arrowFont, arrowBrush, midX - arrowSz.Width / 2f, y);
                y += (int)arrowSz.Height + 6;
            }
            else
            {
                y = cardRect.Bottom;
            }
        }

        // ── Summary ──
        y += 12;
        int summaryH = 290;
        var sumRect = new Rectangle(cardX, y, cardW, summaryH);
        using (var sumPath = RoundedRect(sumRect, 10))
        using (var sumBg = new System.Drawing.Drawing2D.LinearGradientBrush(
            sumRect, Color.FromArgb(28, 34, 52), Color.FromArgb(18, 22, 34), 90f))
        {
            g.FillPath(sumBg, sumPath);
            using var sumBorder = new Pen(Color.FromArgb(55, 65, 90), 1.2f);
            g.DrawPath(sumBorder, sumPath);
        }

        DrawSummarySection(g, sumRect, isEN,
            fontSummaryTitle, fontSummaryBody, fontSummaryArrow, fontFooter, accents);

        y += summaryH + pad;
    }

    private static void DrawVerticalCard(Graphics g, Rectangle r,
        string num, string title, string[] eqLines, string[] paramLines, string insight,
        Color accent, Font titleFont, Font eqFont, Font paramFont, Font paramTitleFont,
        Font insightFont, Font numFont)
    {
        // ── 阴影 ──
        using (var shadowPath = RoundedRect(new Rectangle(r.X + 2, r.Y + 3, r.Width, r.Height), 10))
        using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            g.FillPath(shadowBrush, shadowPath);

        // ── 卡片底板 ──
        using (var cardBg = new SolidBrush(Color.FromArgb(24, 27, 36)))
        using (var cardPath = RoundedRect(r, 10))
        using (var cardBorder = new Pen(Color.FromArgb(38, 42, 54), 1f))
        {
            g.FillPath(cardBg, cardPath);
            g.DrawPath(cardBorder, cardPath);
        }

        // ── 左侧 accent 竖条 ──
        using (var accentBrush = new SolidBrush(accent))
        using (var accentBarPath = RoundedRect(
            new Rectangle(r.X, r.Y + 14, 4, r.Height - 28), 2))
            g.FillPath(accentBrush, accentBarPath);

        // ── 顶部区域：编号 + 标题 ──
        int headerH = 48;
        var headerRect = new Rectangle(r.X + 6, r.Y, r.Width - 12, headerH);

        // 编号圆圈 (圆形 badge)
        int badgeSize = 36;
        var badgeRect = new Rectangle(r.X + 18, r.Y + 6, badgeSize, badgeSize);
        using (var badgeBg = new SolidBrush(Color.FromArgb(18, 20, 28)))
            g.FillEllipse(badgeBg, badgeRect);
        using (var badgePen = new Pen(accent, 2.5f))
            g.DrawEllipse(badgePen, badgeRect);

        var numSz = g.MeasureString(num, numFont);
        using (var numBrush = new SolidBrush(accent))
            g.DrawString(num, numFont, numBrush,
                badgeRect.X + (badgeRect.Width - numSz.Width) / 2,
                badgeRect.Y + (badgeRect.Height - numSz.Height) / 2);

        // 标题
        int titleX = badgeRect.Right + 14;
        using (var titleBrush = new SolidBrush(Color.FromArgb(220, 225, 238)))
            g.DrawString(title, titleFont, titleBrush, titleX, r.Y + 12);

        // 分割线
        int div1Y = r.Y + headerH;
        using (var divPen = new Pen(Color.FromArgb(38, 42, 54), 1f))
            g.DrawLine(divPen, r.X + 16, div1Y, r.Right - 16, div1Y);

        // ── 方程区域 ──
        int eqY = div1Y + 12;
        using var eqBrush = new SolidBrush(Color.FromArgb(200, 240, 255)); // bright cyan-white for equations
        foreach (var line in eqLines)
        {
            var sz = g.MeasureString(line, eqFont);
            g.DrawString(line, eqFont, eqBrush,
                r.X + (r.Width - sz.Width) / 2, eqY);
            eqY += (int)sz.Height + 2;
        }

        // ── 参数区域 ──
        int div2Y = eqY + 8;
        using (var divPen = new Pen(Color.FromArgb(33, 37, 47), 0.8f))
            g.DrawLine(divPen, r.X + 16, div2Y, r.Right - 16, div2Y);

        int paramY = div2Y + 6;
        using var paramBrush = new SolidBrush(Color.FromArgb(140, 148, 168));
        foreach (var line in paramLines)
        {
            g.DrawString(line, paramFont, paramBrush, r.X + 18, paramY);
            paramY += 17;
        }

        // ── 结论区域 ──
        int div3Y = paramY + 4;
        using (var divPen = new Pen(Color.FromArgb(33, 37, 47), 0.8f))
            g.DrawLine(divPen, r.X + 16, div3Y, r.Right - 16, div3Y);

        int insightY = div3Y + 6;

        // Insight 背景条
        var insightBg = new Rectangle(r.X + 16, insightY - 2, r.Width - 32, 30);
        using (var insightBgBrush = new SolidBrush(Color.FromArgb(accent.R / 6, accent.G / 6, accent.B / 6)))
        using (var insightBgPath = RoundedRect(insightBg, 4))
            g.FillPath(insightBgBrush, insightBgPath);

        using (var bulletBrush = new SolidBrush(accent))
        using (var insightTextBrush = new SolidBrush(Color.FromArgb(180, 188, 205)))
        {
            g.DrawString("▸", insightFont, bulletBrush, r.X + 22, insightY);
            g.DrawString(insight, insightFont, insightTextBrush, r.X + 36, insightY);
        }
    }

    private static void DrawSummarySection(Graphics g, Rectangle r, bool isEN,
        Font titleFont, Font bodyFont, Font arrowFont, Font footerFont,
        Color[] accents)
    {
        // ── 标题 ──
        string title = isEN ? "Signal Processing Pipeline" : "信号处理流水线";
        var titleSz = g.MeasureString(title, titleFont);
        using (var titleBrush = new SolidBrush(Color.FromArgb(200, 210, 230)))
            g.DrawString(title, titleFont, titleBrush,
                r.X + (r.Width - titleSz.Width) / 2, r.Y + 14);

        // ── 顶部分割线 ──
        int divY = r.Y + 38;
        using (var divPen = new Pen(Color.FromArgb(42, 48, 62), 0.8f))
            g.DrawLine(divPen, r.X + 24, divY, r.Right - 24, divY);

        // ── 流水线步骤 ──
        var steps = isEN
            ? new[] {
                ("①", "Chirp TX",    "Emit 60GHz chirp             λ = 5 mm"),
                ("②", "Reflection",  "Target reflects signal       Pᵣ ∝ 1/R⁴"),
                ("③", "Mix → IF",    "TX × RX → f_IF               FFT → Range"),
                ("④", "2D-FFT",       "Chirp-to-chirp phase          → Doppler → Velocity"),
                ("⑤", "Vital Signs", "Chest micro-motion            → Breath rate & HR"),
            }
            : new[] {
                ("①", "发射 Chirp",   "60GHz Chirp 信号发射                 λ = 5 mm"),
                ("②", "目标反射",     "电磁波反射回波                        Pᵣ ∝ 1/R⁴"),
                ("③", "混频 → 中频",   "TX × RX → f_IF                        FFT → 距离"),
                ("④", "二维 FFT",      "Chirp 间相位差                          → 多普勒 → 速度"),
                ("⑤", "体征提取",     "胸壁微动检测                            → 呼吸频率 & 心率"),
            };

        int marginX = r.X + 28;
        int stepY = divY + 12;
        int stepH = 38;

        using var labelFont = new Font("Microsoft YaHei", 10F, FontStyle.Bold);
        using var descFont = new Font("Microsoft YaHei", 8.5F);

        for (int i = 0; i < steps.Length; i++)
        {
            var (_, label, desc) = steps[i];

            // 编号圆点
            int dotR = 8;
            int dotX = marginX + 4;
            int dotY = stepY + (stepH - dotR * 2) / 2;
            using (var dotBrush = new SolidBrush(accents[i]))
                g.FillEllipse(dotBrush, dotX, dotY, dotR * 2, dotR * 2);

            // 数字在圆点内
            using (var numFont = new Font("Consolas", 8F, FontStyle.Bold))
            using (var numBrush = new SolidBrush(Color.White))
            {
                var numText = (i + 1).ToString();
                var numSize = g.MeasureString(numText, numFont);
                g.DrawString(numText, numFont, numBrush,
                    dotX + dotR - numSize.Width / 2f,
                    dotY + dotR - numSize.Height / 2f);
            }

            // 步骤名 (靠左)
            int textX = dotX + dotR * 2 + 14;
            using (var labelBrush = new SolidBrush(Color.FromArgb(220, 228, 242)))
                g.DrawString(label, labelFont, labelBrush, textX, stepY + 2);

            // 描述 (右对齐，浅色)
            using (var descBrush = new SolidBrush(Color.FromArgb(130, 140, 160)))
            {
                var descSz = g.MeasureString(desc, descFont);
                g.DrawString(desc, descFont, descBrush,
                    r.Right - 28 - descSz.Width, stepY + 3);
            }

            // 连接箭头
            if (i < steps.Length - 1)
            {
                using var arrBrush = new SolidBrush(Color.FromArgb(70, 78, 100));
                var arrSz = g.MeasureString("↓", arrowFont);
                g.DrawString("↓", arrowFont, arrBrush,
                    r.X + (r.Width - arrSz.Width) / 2f, stepY + stepH - 2);
            }

            stepY += stepH + (i < steps.Length - 1 ? 4 : 0);
        }

        // ── 底部分割线 ──
        int bottomDivY = stepY + 4;
        using (var divPen = new Pen(Color.FromArgb(42, 48, 62), 0.8f))
            g.DrawLine(divPen, r.X + 24, bottomDivY, r.Right - 24, bottomDivY);

        // ── 底部版权 ──
        string footer = isEN ? "SleepSightPro · R60ABD1 60GHz Bio-Radar" : "SleepSightPro · R60ABD1 60GHz 生物雷达";
        using var footerBrush = new SolidBrush(Color.FromArgb(90, 100, 120));
        var footerSz = g.MeasureString(footer, footerFont);
        g.DrawString(footer, footerFont, footerBrush,
            r.X + (r.Width - footerSz.Width) / 2, bottomDivY + 8);
    }

    /// <summary>创建圆角矩形 GraphicsPath (float)</summary>
    private static System.Drawing.Drawing2D.GraphicsPath RoundedRectF(RectangleF rect, float radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        float r2 = radius * 2;
        path.AddArc(rect.X, rect.Y, r2, r2, 180, 90);
        path.AddArc(rect.Right - r2, rect.Y, r2, r2, 270, 90);
        path.AddArc(rect.Right - r2, rect.Bottom - r2, r2, r2, 0, 90);
        path.AddArc(rect.X, rect.Bottom - r2, r2, r2, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>创建圆角矩形 GraphicsPath (int)</summary>
    private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle rect, int radius)
    {
        return RoundedRectF(rect, radius);
    }

    /// <summary>清空按钮点击（由 Designer 绑定）</summary>
    private void _btnClearResult_Click(object? sender, EventArgs e)
    {
        _settingsResultBox?.Clear();
    }

    // ========================================================================
    //  设计器绑定事件处理方法（严格设计器模式 - VS 属性面板可见）
    // ========================================================================

    #region 顶部工具栏

    private void _btnConnect_Click(object? sender, EventArgs e) => ToggleConnection();
    private void _btnRefresh_Click(object? sender, EventArgs e) => RefreshPorts();
    private void _btnLang_Click(object? sender, EventArgs e)
    {
        _loc.ToggleLanguage();
        bool isZH = _loc.CurrentLanguage == "zh-CN";
        if (_hasFlags)
        {
            _btnLang.Image = isZH ? _imgFlagCN : _imgFlagUS;
            _btnLang.Text = isZH ? _loc.Get("LanguageZH") : _loc.Get("LanguageEN");
        }
        else
        {
            _btnLang.Text = isZH
                ? $"{_loc.Get("LanguageZH")} | {_loc.Get("LanguageEN")}"
                : $"{_loc.Get("LanguageEN")} | {_loc.Get("LanguageZH")}";
        }
        ApplyLocalization();
    }

    #endregion

    #region 工具栏布局与绘制

    private void _rightToolbar_Layout(object? sender, LayoutEventArgs e) => ArrangeRightToolbar();
    private void _topPanel_Resize(object? sender, EventArgs e) => ArrangeRightToolbar();
    private void _mainTab_DrawItem(object? sender, DrawItemEventArgs e) => DrawTabItem(sender, e);

    #endregion

    #region 日志工具栏

    private void _btnClearLog_Click(object? sender, EventArgs e) => _logListView?.Items.Clear();
    private void _btnExportLog_Click(object? sender, EventArgs e) => ExportLog();
    private void _btnRawHexClear_Click(object? sender, EventArgs e)
    {
        if (_rawHexBox != null)
        {
            _rawHexBox.Clear();
            _rawByteCount = 0;
            _firstRawTime = DateTime.Now;
            _lblRawHexStatus.Text = string.Format(_loc.Get("RawHexStatus"), 0);
        }
    }

    private void _btnDiagnose_Click(object? sender, EventArgs e)
    {
        DiagnoseRawHex();
    }

    private void DiagnoseRawHex()
    {
        if (_rawHexBox == null || string.IsNullOrEmpty(_rawHexBox.Text))
        {
            MessageBox.Show(_loc.Get("DiagEmptyBuffer"), _loc.Get("DiagDialogTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var allBytes = ParseHexFromRawText(_rawHexBox.Text);
        if (allBytes.Count == 0)
        {
            MessageBox.Show(_loc.Get("DiagNoHexParsed"), _loc.Get("DiagDialogTitle"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var headBytes = new byte[] { 0x53, 0x59 };
        var tailBytes = new byte[] { 0x54, 0x43 };
        int headCount = 0, tailCount = 0;
        var headPositions = new List<int>();
        var tailPositions = new List<int>();
        var freqMap = new Dictionary<byte, int>();

        foreach (var b in allBytes)
        {
            freqMap[b] = freqMap.GetValueOrDefault(b, 0) + 1;
        }

        for (int i = 0; i < allBytes.Count - 1; i++)
        {
            if (allBytes[i] == headBytes[0] && allBytes[i + 1] == headBytes[1])
            {
                headCount++;
                headPositions.Add(i);
            }
            if (allBytes[i] == tailBytes[0] && allBytes[i + 1] == tailBytes[1])
            {
                tailCount++;
                tailPositions.Add(i);
            }
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine(_loc.Get("DiagReportBanner"));
        sb.AppendLine();
        sb.AppendLine(string.Format(_loc.Get("DiagTotalBytes"), allBytes.Count));
        sb.AppendLine(string.Format(_loc.Get("DiagFrameHeadCount"), headCount));
        sb.AppendLine(string.Format(_loc.Get("DiagFrameTailCount"), tailCount));
        sb.AppendLine();

        var baudRate = int.TryParse(_cmbBaudRate?.SelectedItem?.ToString(), out var br) ? br : 115200;
        var byteRate = _rawByteCount / Math.Max(1, (DateTime.Now - _firstRawTime).TotalSeconds);
        sb.AppendLine(string.Format(_loc.Get("DiagBaudRate"), baudRate));
        sb.AppendLine(string.Format(_loc.Get("DiagByteRate"), byteRate));
        sb.AppendLine();

        // 高频字节 TOP 10
        var topFreq = freqMap.OrderByDescending(kv => kv.Value).Take(10).ToList();
        sb.AppendLine(_loc.Get("DiagTopFreqHeader"));
        foreach (var kv in topFreq)
            sb.AppendLine(string.Format(_loc.Get("DiagFreqRow"), kv.Key, kv.Value));
        sb.AppendLine();

        // 诊断推断
        sb.AppendLine(_loc.Get("DiagInferenceHeader"));
        if (headCount > 0 && tailCount > 0)
        {
            var distances = new List<int>();
            for (int i = 0; i < Math.Min(headPositions.Count - 1, 10); i++)
                distances.Add(headPositions[i + 1] - headPositions[i]);
            double avgDist = distances.Count > 0 ? distances.Average() : 0;
            sb.AppendLine(string.Format(_loc.Get("DiagFrameInterval"), avgDist));

            if (Math.Abs(avgDist - 32) < 10) sb.AppendLine(_loc.Get("DiagInterval32"));
            else if (Math.Abs(avgDist - 64) < 10) sb.AppendLine(_loc.Get("DiagInterval64"));
            else sb.AppendLine(_loc.Get("DiagIntervalAbnormal"));
        }
        else if (headCount == 0)
        {
            sb.AppendLine(_loc.Get("DiagNoHeadTitle"));
            sb.AppendLine(_loc.Get("DiagNoHeadA"));
            sb.AppendLine(_loc.Get("DiagNoHeadB"));
            sb.AppendLine(_loc.Get("DiagNoHeadC"));
            sb.AppendLine(_loc.Get("DiagNoHeadD"));
        }

        if (headCount > 0 && tailCount == 0)
            sb.AppendLine(_loc.Get("DiagHeadNoTail"));

        if (headCount > 0 && headCount != tailCount)
            sb.AppendLine(_loc.Get("DiagHeadTailMismatch"));

        // 显示含 0x00 数量（全零可能是噪声）
        int zeroCount = freqMap.TryGetValue(0x00, out var zc) ? zc : 0;
        if (zeroCount > allBytes.Count * 0.5)
            sb.AppendLine(string.Format(_loc.Get("DiagZeroBytes"), zeroCount, allBytes.Count));

        // 显示原始数据的前 64 字节样本
        sb.AppendLine();
        sb.AppendLine(_loc.Get("DiagSampleHeader"));
        int sampleCount = Math.Min(64, allBytes.Count);
        for (int i = 0; i < sampleCount; i += 16)
        {
            var lineBytes = allBytes.Skip(i).Take(Math.Min(16, sampleCount - i)).ToArray();
            var hex = BitConverter.ToString(lineBytes).Replace("-", " ");
            sb.AppendLine($"  {i:D5}:  {hex}");
        }

        MessageBox.Show(sb.ToString(), _loc.Get("DiagReportTitle"),
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    /// <summary>从原始 Hex 日志文本中解析出所有字节</summary>
    private static List<byte> ParseHexFromRawText(string rawText)
    {
        var bytes = new List<byte>();
        // 匹配 [HH:mm:ss.fff] 后面的所有 hex 对
        var matches = System.Text.RegularExpressions.Regex.Matches(rawText, @"\]\s+([0-9A-Fa-f\s]+)", System.Text.RegularExpressions.RegexOptions.Multiline);
        foreach (System.Text.RegularExpressions.Match m in matches)
        {
            var hexStr = m.Groups[1].Value;
            foreach (var part in hexStr.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (byte.TryParse(part, System.Globalization.NumberStyles.HexNumber, null, out var b))
                    bytes.Add(b);
            }
        }
        return bytes;
    }

    private DateTime _firstRawTime = DateTime.Now;

    #endregion

    #region 状态栏

    private void _statusStrip_DoubleClick(object? sender, EventArgs e) => CaptureFormToClipboard();

    #endregion

    #region 人体存在控制

    private void _btnPresenceOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.SWITCH_SET, true);
    private void _btnPresenceOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.SWITCH_SET, false);
    private void _btnQueryPresence_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.PRESENCE_QUERY);
    private void _btnQueryMotion_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.MOTION_QUERY);
    private void _btnQueryDistance_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.DISTANCE_QUERY);
    private void _btnQueryPosition_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HUMAN_PRESENCE, RadarProtocol.HumanPresenceCmd.POSITION_QUERY);

    #endregion

    #region 呼吸监测控制

    private void _btnBreathOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.SWITCH_SET, true);
    private void _btnBreathOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.SWITCH_SET, false);
    private void _btnBreathWaveOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.WAVE_SWITCH_SET, true);
    private void _btnBreathWaveOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.WAVE_SWITCH_SET, false);
    private void _btnQueryBreathValue_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.BREATH_VALUE_QUERY);
    private void _btnQueryBreathState_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.BREATH_INFO_QUERY);
    private void _btnQueryBreathWaveSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.WAVE_SWITCH_QUERY);
    private void _btnQueryBreathSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.SWITCH_QUERY);
    private void _btnSetLowBreath_Click(object? sender, EventArgs e)
    {
        var input = Interaction.InputBox(_loc.Get("MsgInputLowBreath"), _loc.Get("BtnSetLowBreath"), "10");
        if (byte.TryParse(input, out byte val))
            SendData(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.LOW_BREATH_SET, new byte[] { val });
    }
    private void _btnQueryLowBreath_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.BREATH_DETECT, RadarProtocol.BreathDetectCmd.LOW_BREATH_QUERY);

    #endregion

    #region 心率监测控制

    private void _btnHeartOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.SWITCH_SET, true);
    private void _btnHeartOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.SWITCH_SET, false);
    private void _btnHeartWaveOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.WAVE_SWITCH_SET, true);
    private void _btnHeartWaveOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.WAVE_SWITCH_SET, false);
    private void _btnQueryHeartValue_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.HEART_VALUE_QUERY);
    private void _btnQueryHeartSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.SWITCH_QUERY);
    private void _btnQueryHeartWave_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.HEART_WAVE_QUERY);
    private void _btnQueryHeartWaveSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HEART_RATE, RadarProtocol.HeartRateCmd.WAVE_SWITCH_QUERY);

    #endregion

    #region 睡眠监测控制

    private void _btnSleepOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SWITCH_SET, true);
    private void _btnSleepOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SWITCH_SET, false);
    private void _btnQuerySleepComp_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_COMP_QUERY);
    private void _btnQuerySleepAnalysis_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_ANALYSIS_QUERY);
    private void _btnQuerySleepRating_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_RATING_QUERY);

    #endregion

    #region 睡眠参数设置

    private void _btnStruggleSwitchOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.STRUGGLE_SWITCH_SET, true);
    private void _btnStruggleSwitchOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.STRUGGLE_SWITCH_SET, false);
    private void _btnNoPersonSwitchOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.NO_PERSON_SWITCH_SET, true);
    private void _btnNoPersonSwitchOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.NO_PERSON_SWITCH_SET, false);
    private void _btnSetNoPersonDuration_Click(object? sender, EventArgs e)
    {
        var input = Interaction.InputBox(_loc.Get("MsgInputNoPersonDuration"), _loc.Get("BtnSetNoPersonDuration"), "30");
        if (byte.TryParse(input, out byte val) && val >= 30 && val <= 180)
            SendData(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.NO_PERSON_DURATION_SET, new byte[] { val });
    }
    private void _btnExtCtrlSwitchOn_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.EXT_SWITCH_SET, true);
    private void _btnExtCtrlSwitchOff_Click(object? sender, EventArgs e) => SendSwitch(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.EXT_SWITCH_SET, false);
    private void _btnSleepPeriodStart_Click(object? sender, EventArgs e) => SendData(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_PERIOD_SET, new byte[] { 0x01 });
    private void _btnSleepPeriodEnd_Click(object? sender, EventArgs e) => SendData(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_PERIOD_SET, new byte[] { 0x02 });
    private void _btnSetSleepDeadline_Click(object? sender, EventArgs e)
    {
        var input = Interaction.InputBox(_loc.Get("MsgInputSleepDeadline"), _loc.Get("BtnSetSleepDeadline"), "5");
        if (byte.TryParse(input, out byte val) && val >= 5 && val <= 120)
            SendData(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_DEADLINE_SET, new byte[] { val });
    }

    #endregion

    #region 睡眠参数查询

    private void _btnQueryBedState_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_STATE_QUERY);
    private void _btnQuerySleepState_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_DURATION_QUERY);
    private void _btnQueryAwakeDur_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.AWAKE_DURATION_QUERY);
    private void _btnQueryLightDur_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.LIGHT_DURATION_QUERY);
    private void _btnQueryDeepDur_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.DEEP_DURATION_QUERY);
    private void _btnQuerySleepScore_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_SCORE_QUERY);
    private void _btnQuerySleepAbnormal_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_ABNORMAL_QUERY);
    private void _btnQuerySleepStats_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.SLEEP_ANALYSIS_QUERY);
    private void _btnQueryStruggleSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.STRUGGLE_SWITCH_QUERY);
    private void _btnQueryNoPersonSwitch_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.SLEEP_MONITOR, RadarProtocol.SleepMonitorCmd.NO_PERSON_SWITCH_QUERY);

    #endregion

    #region 系统功能

    private void _btnHeartbeatQuery_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.HEARTBEAT, RadarProtocol.HeartbeatCmd.HEARTBEAT_QUERY);
    private void _btnInitQuery_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.WORK_STATUS, RadarProtocol.WorkStatusCmd.INIT_QUERY);
    private void _btnBoundaryQuery_Click(object? sender, EventArgs e) => SendQuerySafe(RadarProtocol.ControlWord.RADAR_RANGE, RadarProtocol.RadarRangeCmd.BOUNDARY_QUERY);
    private void _btnModuleReset_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show(_loc.Get("MsgResetConfirm"), _loc.Get("MsgWarning"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            SendData(RadarProtocol.ControlWord.HEARTBEAT, RadarProtocol.HeartbeatCmd.MODULE_RESET, new byte[] { 0x0F });
    }
    private void _btnQueryDev_Click(object? sender, EventArgs e)
    {
        if (!_serialService.IsConnected) return;
        StartDelayPause();
        AppendSettingsCmdResult($"Ctrl=0x{RadarProtocol.ControlWord.PRODUCT_INFO:X2} | Query Device Info (Model/ID/HW/FW)");
        _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.INFO_QUERY);
        AddTxLogEntry(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.INFO_QUERY, new byte[] { 0x0F });
        _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.ID_QUERY);
        AddTxLogEntry(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.ID_QUERY, new byte[] { 0x0F });
        _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.HARDWARE_QUERY);
        AddTxLogEntry(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.HARDWARE_QUERY, new byte[] { 0x0F });
        _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.FIRMWARE_QUERY);
        AddTxLogEntry(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.FIRMWARE_QUERY, new byte[] { 0x0F });
    }

    #endregion

    #region 窗体事件

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (MessageBox.Show(_loc.Get("MsgExitConfirm"), _loc.Get("MsgExitTitle"),
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            e.Cancel = true;
            return;
        }
        if (!_disposed)
        {
            _disposed = true;
            AppLogService.Instance.Info("[App] Closing...");
            lock (_timerLock) { _resultResumeTimer?.Dispose(); _resultResumeTimer = null; }
            _serialService.Dispose();
            _imgFlagCN?.Dispose();
            _imgFlagUS?.Dispose();
            AppLogService.Instance.Dispose();
        }
    }

    #endregion

    // ========================================================================
    //  事件 & 串口操作
    // ========================================================================
    private void InitEvents()
    {
        _serialService.StatusChanged += (s, msg) =>
        {
            AppLogService.Instance.Info($"[Status] {msg}");
            this.Invoke(() =>
            {
                _statusLabel.Text = msg;
                bool isConnected = _serialService.IsConnected;
                _statusLabel.ForeColor = isConnected ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);
                _btnConnect.Text = isConnected ? _loc.Get("BtnDisconnect") : _loc.Get("BtnConnect");
                _btnConnect.BackColor = isConnected ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113);

                if (!isConnected)
                {
                    _fwQueried = false;
                    _fwLabel.Text = _loc.Get("StatusFW") + "--";
                    _portLabel.Text = "";
                }
                else
                {
                    if (!_fwQueried)
                    {
                        try
                        {
                            _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.FIRMWARE_QUERY);
                            _fwQueried = true;
                        }
                        catch { /* retry on next frame or reconnect */ }
                    }
                }
            });
        };

        _serialService.FrameReceived += (s, frame) =>
        {
            this.Invoke(() =>
            {
                if (!_fwQueried && _serialService.IsConnected)
                {
                    try
                    {
                        _serialService.SendQuery(RadarProtocol.ControlWord.PRODUCT_INFO, RadarProtocol.ProductInfoCmd.FIRMWARE_QUERY);
                        _fwQueried = true;
                    }
                    catch { /* ignore, retry on next frame */ }
                }

                try
                {
                    _dataProcessor.ProcessFrame(frame);
                }
                catch (Exception ex) { AppLogService.Instance.Error(ex, "[ProcessFrame]"); }

                AddLogEntry("RX", frame);
                // 仪表盘卡片更新由 _dataProcessor.DataUpdated 事件驱动，不再此处重复调用

                if (_mainTab.SelectedIndex == 1 && frame.IsValid)
                {
                    if (_resultPaused) return;

                    try
                    {
                        string? parsed = ParseFrameData(frame);
                        var result = $"[{DateTime.Now:HH:mm:ss}] [RX] Ctrl=0x{frame.ControlWord:X2} Cmd=0x{frame.CommandWord:X2}";
                        if (!string.IsNullOrEmpty(parsed))
                            result += $" => {parsed}";
                        AppendSettingsCmdResultRaw(result, frame.ControlWord);
                    }
                    catch (Exception ex)
                    {
                        AppLogService.Instance.Error(ex, "[AppendSettingsResult]");
                    }
                }
            });
        };

        _dataProcessor.DataUpdated += (s, key) =>
        {
            switch (key)
            {
                case "ProductInfo":
                    if (!string.IsNullOrEmpty(_dataProcessor.FirmwareVersion))
                        this.Invoke(() => _fwLabel.Text = $"{_loc.Get("StatusFW")}{_dataProcessor.FirmwareVersion}");
                    break;

                case "BreathDetect":
                    _hasBreathData = _dataProcessor.BreathValue > 0;
                    if (_chartsInitialized && _dataProcessor.HasBreathWave)
                        this.Invoke(() => PushBreathWaveData());
                    break;

                case "HeartRate":
                    _hasHeartData = _dataProcessor.HeartValue > 0;
                    if (_chartsInitialized && _dataProcessor.HasHeartWave)
                        this.Invoke(() => PushHeartWaveData());
                    break;

                case "HumanPresence":
                    _hasDistanceData = _dataProcessor.Distance > 0;
                    break;

                case "SleepMonitor":
                    _hasSleepData = _dataProcessor.SleepScore > 0 || _dataProcessor.CompTurnOver > 0;
                    break;
            }

            // 所有数据变化时更新仪表盘卡片（DataUpdated 已在 UI 线程上触发）
            this.Invoke(UpdateDashboardCards);
        };

        _serialService.ErrorOccurred += (s, ex) =>
        {
            AppLogService.Instance.Error(ex, "[SerialError]");
        };

        _serialService.RawDataReceived += (s, rawData) =>
        {
            this.Invoke(() =>
            {
                if (_rawByteCount == 0) _firstRawTime = DateTime.Now;
                _rawByteCount += rawData.Length;
                _lblRawHexStatus.Text = string.Format(_loc.Get("RawHexStatusLine"), _rawByteCount, rawData.Length);

                if (_rawHexBox == null) return;

                // 生成带时间戳的 Hex 行
                var ts = DateTime.Now.ToString("HH:mm:ss.fff");
                var hex = BitConverter.ToString(rawData).Replace("-", " ");
                var line = $"[{ts}] {hex}\n";

                _rawHexBox.AppendText(line);

                // 超限裁剪前 1/3
                if (_rawHexBox.TextLength > MaxRawHexChars)
                {
                    var text = _rawHexBox.Text;
                    var cutIdx = text.Length / 3;
                    var newlineIdx = text.IndexOf('\n', cutIdx);
                    if (newlineIdx < 0) newlineIdx = cutIdx;
                    _rawHexBox.Text = text[(newlineIdx + 1)..];
                }

                // 自动滚到底部
                _rawHexBox.SelectionStart = _rawHexBox.TextLength;
                _rawHexBox.ScrollToCaret();
            });
        };

        _loc.LanguageChanged += (s, lang) => this.Invoke(ApplyLocalization);
    }

    private void ToggleConnection()
    {
        if (_serialService.IsConnected)
        {
            _serialService.Disconnect();
        }
        else
        {
            if (_cmbPort.SelectedItem == null)
            {
                MessageBox.Show(_loc.Get("MsgSelectPort"), _loc.Get("MsgWarning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var port = _cmbPort.SelectedItem.ToString()!;
                var baud = int.Parse(_cmbBaudRate.SelectedItem?.ToString() ?? "115200");
                _serialService.Connect(port, baud);
                _portLabel.Text = $"{port} @ {baud}bps";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{_loc.Get("MsgConnectFailed")}: {ex.Message}", _loc.Get("MsgError"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void RefreshPorts()
    {
        _cmbPort.Items.Clear();
        _cmbPort.Items.AddRange(_serialService.GetAvailablePorts());
        if (_cmbPort.Items.Count > 0) _cmbPort.SelectedIndex = 0;
        _cmbBaudRate.SelectedIndex = 0; // 默认选中 115200
    }

    /// <summary>发送开关命令 + 追加 TX 记录到 _settingsResultBox</summary>
    private void SendSwitch(byte ctrl, byte cmd, bool enable)
    {
        if (!_serialService.IsConnected) return;
        try
        {
            _serialService.SendSwitchCommand(ctrl, cmd, enable);
            StartDelayPause();
            AppendSettingsCmdResult($"Ctrl=0x{ctrl:X2} Cmd=0x{cmd:X2} | Switch={(enable ? "ON" : "OFF")}");
            AddTxLogEntry(ctrl, cmd, new byte[] { (byte)(enable ? 0x01 : 0x00) });
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, _loc.Get("MsgError"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    /// <summary>发送数据命令 + 追加 TX 记录到 _settingsResultBox</summary>
    private void SendData(byte ctrl, byte cmd, byte[] data)
    {
        if (!_serialService.IsConnected) return;
        try
        {
            _serialService.SendDataCommand(ctrl, cmd, data);
            StartDelayPause();
            var dataHex = BitConverter.ToString(data).Replace("-", " ");
            AppendSettingsCmdResult($"Ctrl=0x{ctrl:X2} Cmd=0x{cmd:X2} | Data=[{dataHex}]");
            AddTxLogEntry(ctrl, cmd, data);
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, _loc.Get("MsgError"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    /// <summary>发送查询命令 + 追加 TX 记录到 _settingsResultBox</summary>
    private void SendQuerySafe(byte ctrl, byte cmd)
    {
        if (!_serialService.IsConnected) return;
        try
        {
            _serialService.SendQuery(ctrl, cmd);
            StartDelayPause();
            AppendSettingsCmdResult($"Ctrl=0x{ctrl:X2} Cmd=0x{cmd:X2} | Query");
            AddTxLogEntry(ctrl, cmd, new byte[] { 0x0F });
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, _loc.Get("MsgError"), MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }

    /// <summary>
    /// _settingsResultBox 暂停控制：用户点击按钮时启动延迟暂停。
    ///   0..1s  窗口期（TX 和 RX 均可写入）
    ///   1..10s 暂停期（_resultPaused=true，跳过 RX 写入，仅保留 TX 记录）
    ///   10s后  自动恢复 RX 写入
    /// 重复点击重置计时器，从当前已暂停态不重置。
    /// </summary>
    private void StartDelayPause()
    {
        // 如果已经暂停，不重置计时器
        if (_resultPaused) return;

        lock (_timerLock)
        {
            _resultResumeTimer?.Dispose();
            _resultResumeTimer = new System.Threading.Timer(_ =>
            {
                if (_disposed) return;
                _resultPaused = true;
                lock (_timerLock)
                {
                    _resultResumeTimer?.Dispose();
                    // 10 秒后自动恢复
                    _resultResumeTimer = new System.Threading.Timer(_2 =>
                    {
                        if (_disposed) return;
                        _resultPaused = false;
                        lock (_timerLock)
                        {
                            _resultResumeTimer?.Dispose();
                            _resultResumeTimer = null;
                        }
                    }, null, 10000, Timeout.Infinite);
                }
            }, null, 1000, Timeout.Infinite);
        }
    }

    // ========================================================================
    //  设置页命令结果显示（左侧 - 用户点击按钮时显示命令和结果）
    // ========================================================================
    private void AddLogEntry(string direction, RadarFrame? frame)
    {
        if (_logListView == null) return;
        var time = DateTime.Now.ToString("HH:mm:ss.fff");
        var dir = direction;
        var ctrl = frame != null ? $"0x{frame.ControlWord:X2}" : "-";
        var cmd = frame != null ? $"0x{frame.CommandWord:X2}" : "-";
        var len = frame != null ? $"{frame.DataLength}" : "-";
        var data = frame != null ? BitConverter.ToString(frame.Data).Replace("-", " ") : "-";

        var item = new ListViewItem(new[] { time, dir, ctrl, cmd, len, data })
        {
            ForeColor = direction == "TX" ? Color.FromArgb(52, 152, 219) : Color.FromArgb(46, 204, 113)
        };

        _logListView.BeginUpdate();
        _logListView.Items.Add(item);
        // 批量裁剪：超过 1100 时一次删 100 行，避免逐条 RemoveAt(0) 的 O(n²) 开销
        if (_logListView.Items.Count > 1100)
        {
            for (int i = 0; i < 100; i++)
                _logListView.Items.RemoveAt(0);
        }
        _logListView.EndUpdate();
        _logListView.Items[_logListView.Items.Count - 1].EnsureVisible();
    }

    private void AddTxLogEntry(byte ctrl, byte cmd, byte[] data)
    {
        if (_logListView == null) return;
        var time = DateTime.Now.ToString("HH:mm:ss.fff");
        var dataStr = data.Length > 0 ? BitConverter.ToString(data).Replace("-", " ") : "-";

        var item = new ListViewItem(new[] { time, "TX", $"0x{ctrl:X2}", $"0x{cmd:X2}", $"{data.Length}", dataStr })
        {
            ForeColor = Color.FromArgb(52, 152, 219)
        };

        _logListView.BeginUpdate();
        _logListView.Items.Add(item);
        // 批量裁剪：超过 1100 时一次删 100 行
        if (_logListView.Items.Count > 1100)
        {
            for (int i = 0; i < 100; i++)
                _logListView.Items.RemoveAt(0);
        }
        _logListView.EndUpdate();
        _logListView.Items[_logListView.Items.Count - 1].EnsureVisible();
    }

    private void ExportLog()
    {
        if (_logListView == null || _logListView.Items.Count == 0) return;
        using var sfd = new SaveFileDialog
        {
            Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
            DefaultExt = "csv",
            FileName = $"SleepSightPro_Log_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
        };
        if (sfd.ShowDialog() == DialogResult.OK)
        {
            using var sw = new StreamWriter(sfd.FileName);
            sw.WriteLine("Time,Direction,Ctrl,Cmd,Len,Data");
            foreach (ListViewItem item in _logListView.Items)
            {
                var fields = item.SubItems.Cast<ListViewItem.ListViewSubItem>()
                    .Select(s => CsvEscape(s.Text));
                sw.WriteLine(string.Join(",", fields));
            }
            MessageBox.Show(_loc.Get("MsgLogExported"), _loc.Get("MsgInfo"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    /// <summary>
    /// CSV 字段转义：如果含逗号、引号或换行则用双引号包裹
    /// </summary>
    private static string CsvEscape(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            return $"\"{field.Replace("\"", "\"\"")}\"";
        return field;
    }

    // ========================================================================
    //  UI 数据更新
    // ========================================================================

    /// <summary>
    /// 刷新仪表盘 9 张卡片——所有数据均来自 RadarDataProcessor（由串口帧驱动）。
    /// 未收到模块数据时显示 "--" 避免误读为 0。
    /// </summary>
    private void UpdateDashboardCards()
    {
        var unitBreath = _loc.Get("UnitBreathRate");
        var unitScore = _loc.Get("UnitScore");
        var unitTimes = _loc.Get("UnitTimesCN");
        var unitBpmShort = _loc.Get("UnitBpmShort");
        var unitPercentShort = _loc.Get("UnitPercentShort");
        var unitCmShort = _loc.Get("UnitCmShort");
        var dash = "--";

        // --- 有人状态 ---
        bool present = _dataProcessor.IsPresence;
        _cardPresence.Value = present ? _loc.Get("StatePresent") : _loc.Get("StateAbsent");
        _cardPresence.AccentColor = present ? Color.FromArgb(46, 204, 113) : Color.FromArgb(127, 140, 141);

        // --- 呼吸数值 (0-35, 0 表示未收到) ---
        byte bv = _dataProcessor.BreathValue;
        _cardBreath.Value = _hasBreathData && bv > 0 ? $"{bv}{unitBreath}" : $"{dash}{unitBreath}";

        // --- 心率数值 (60-120 范围, 0 表示未收到) ---
        byte hv = _dataProcessor.HeartValue;
        _cardHeart.Value = _hasHeartData && hv > 0 ? $"{hv}{unitBpmShort}" : $"{dash}{unitBpmShort}";

        // --- 体动参数 (0-100, 0 是有效值) ---
        _cardBodyMove.Value = $"{_dataProcessor.BodyMoveParam}{unitPercentShort}";

        // --- 睡眠状态 ---
        _cardSleepState.Value = GetSleepStateText(_dataProcessor.SleepState);

        // --- 睡眠评分 ---
        byte score = _dataProcessor.SleepScore;
        _cardSleepScore.Value = _hasSleepData && score > 0 ? $"{score}{unitScore}" : $"{dash}{unitScore}";

        // --- 距离 (cm) ---
        ushort dist = _dataProcessor.Distance;
        _cardDistance.Value = _hasDistanceData && dist > 0 ? $"{dist}{unitCmShort}" : $"0{unitCmShort}";

        // --- 呼吸暂停次数 ---
        byte apnea = _dataProcessor.CompApneaCount;
        _cardApnea.Value = $"{apnea}{unitTimes}";

        // --- 翻身次数 ---
        byte turnover = _dataProcessor.CompTurnOver;
        _cardTurnOver.Value = $"{turnover}{unitTimes}";
    }


    private string GetSleepStateText(byte state) => state switch
    {
        RadarProtocol.SleepState.DEEP_SLEEP => _loc.Get("StateDeepSleep"),
        RadarProtocol.SleepState.LIGHT_SLEEP => _loc.Get("StateLightSleep"),
        RadarProtocol.SleepState.AWAKE => _loc.Get("StateAwake"),
        RadarProtocol.SleepState.OUT_OF_BED => _loc.Get("StateOutOfBed"),
        _ => _loc.Get("StateOutOfBed")
    };

    // ========================================================================
    //  _settingsResultBox 数据更新规则（简化版）
    // ========================================================================
    //
    //  【写入时机】
    //    1. TX（发送命令）: 用户在设置页点击任何按钮时，直接追加一行蓝色 TX 记录
    //       调用链: SendSwitchCommand/SendDataCommand/SendQuery → AppendSettingsCmdResult()
    //    2. RX（接收响应）: 雷达返回数据帧时，仅当用户在设置页（tab index==1）才追加
    //       调用链: FrameReceived → AppendSettingsCmdResultRaw()
    //
    //  【暂停机制 - 防止上行/下行相互覆盖】
    //    每次 TX 发出前调用 StartDelayPause()：
    //      0..1s : 窗口期（TX 刚发出，RX 可能立即返回，仍写入）
    //      1..10s: 暂停期（_resultPaused=true，跳过所有 RX 写入，保留 TX 结果给用户看）
    //      10s后 : 自动恢复，RX 继续写入
    //    再次点击按钮 → 重新计时 1s 窗口 + 10s 暂停
    //
    //  【超限清空 - TextChanged 事件驱动】
    //    OnSettingsResultBoxTextChanged 监听 TextChanged 事件，当 TextLength > 50000
    //    (约 50KB) 时自动调用 Clear()。_clearingResultBox 守卫防止重入。
    //
    //  【显示规则】
    //    TX 行: 浅蓝色 [HH:mm:ss] [TX] ...
    //    RX 行: 按 ControlWord 分 7 种颜色（存在/呼吸/心率/睡眠/产品信息/工作状态/测距/其他）
    //
    //  【清空】
    //    _btnClearResult 按钮 → RichTextBox.Clear()
    //
    //  关键字段: _resultPaused (volatile, 后台 Timer 写入, UI 线程读取)
    //           _clearingResultBox (TextChanged 重入保护)

    /// <summary>追加一条发送命令记录（TX - 浅蓝色）</summary>
    private void AppendSettingsCmdResult(string cmdText)
    {
        if (_settingsResultBox == null || _settingsResultBox.IsDisposed) return;

        var line = $"[{DateTime.Now:HH:mm:ss}] [TX] {cmdText}";
        _settingsResultBox.SelectionStart = _settingsResultBox.TextLength;
        _settingsResultBox.SelectionLength = 0;
        _settingsResultBox.SelectionColor = Color.FromArgb(100, 180, 255);
        _settingsResultBox.AppendText(line + "\n");
        _settingsResultBox.ScrollToCaret();
    }

    /// <summary>追加一条接收响应记录（RX - 直接追加，TextChanged 事件负责超限清空）</summary>
    private void AppendSettingsCmdResultRaw(string line, byte ctrlWord)
    {
        if (_settingsResultBox == null || _settingsResultBox.IsDisposed) return;
        if (_resultPaused) return;  // 暂停期跳过 RX

        Color c = ctrlWord switch
        {
            RadarProtocol.ControlWord.HUMAN_PRESENCE => Color.FromArgb(52, 152, 219),
            RadarProtocol.ControlWord.BREATH_DETECT => Color.FromArgb(46, 204, 113),
            RadarProtocol.ControlWord.HEART_RATE => Color.FromArgb(231, 76, 60),
            RadarProtocol.ControlWord.SLEEP_MONITOR => Color.FromArgb(155, 89, 182),
            RadarProtocol.ControlWord.PRODUCT_INFO => Color.FromArgb(243, 156, 18),
            RadarProtocol.ControlWord.WORK_STATUS => Color.FromArgb(26, 188, 156),
            RadarProtocol.ControlWord.RADAR_RANGE => Color.FromArgb(149, 165, 166),
            _ => Color.FromArgb(189, 195, 199)
        };

        _settingsResultBox.SelectionStart = _settingsResultBox.TextLength;
        _settingsResultBox.SelectionLength = 0;
        _settingsResultBox.SelectionColor = c;
        _settingsResultBox.AppendText(line + "\n");
        _settingsResultBox.ScrollToCaret();
    }

    /// <summary>TextChanged 事件：超过阈值 (50KB) 时自动清空，防止 RichEdit 内部状态膨胀</summary>
    private void OnSettingsResultBoxTextChanged(object? sender, EventArgs e)
    {
        if (_clearingResultBox || _settingsResultBox == null) return;
        if (_settingsResultBox.TextLength > 50000)
        {
            _clearingResultBox = true;
            _settingsResultBox.Clear();
            _clearingResultBox = false;
        }
    }

    private string? ParseFrameData(RadarFrame frame)
    {
        var d = frame.Data;
        var len = frame.DataLength;
        var l = _loc;

        switch (frame.ControlWord)
        {
            case RadarProtocol.ControlWord.HUMAN_PRESENCE:
                return frame.CommandWord switch
                {
                    RadarProtocol.HumanPresenceCmd.PRESENCE_REPORT or RadarProtocol.HumanPresenceCmd.PRESENCE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedPresence")}: {(d[0] == 0x01 ? l.Get("ParsedPresent") : l.Get("ParsedAbsent"))}" : null,
                    RadarProtocol.HumanPresenceCmd.MOTION_REPORT or RadarProtocol.HumanPresenceCmd.MOTION_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedMotion")}: {d[0] switch { 0 => l.Get("ParsedMotionNone"), 1 => l.Get("ParsedMotionStatic"), 2 => l.Get("ParsedMotionActive"), _ => d[0].ToString() }}" : null,
                    RadarProtocol.HumanPresenceCmd.DISTANCE_REPORT or RadarProtocol.HumanPresenceCmd.DISTANCE_QUERY =>
                        len >= 2 ? $"{l.Get("ParsedDistance")}: {(d[0] << 8 | d[1])}{l.Get("UnitCmShort")}" : null,
                    RadarProtocol.HumanPresenceCmd.POSITION_REPORT or RadarProtocol.HumanPresenceCmd.POSITION_QUERY =>
                        len >= 6 ? $"{l.Get("ParsedPosition")}: X={d[0]<<8|d[1]} Y={d[2]<<8|d[3]} Z={d[4]<<8|d[5]}{l.Get("UnitCmShort")}" : null,
                    RadarProtocol.HumanPresenceCmd.BODY_MOVE_REPORT or RadarProtocol.HumanPresenceCmd.BODY_MOVE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBodyMove")}: {d[0]}{l.Get("UnitPercentShort")}" : null,
                    RadarProtocol.HumanPresenceCmd.SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.BREATH_DETECT:
                return frame.CommandWord switch
                {
                    RadarProtocol.BreathDetectCmd.BREATH_VALUE_REPORT or RadarProtocol.BreathDetectCmd.BREATH_VALUE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBreathValue")}: {d[0]} {l.Get("UnitBreathRate")}" : null,
                    RadarProtocol.BreathDetectCmd.BREATH_INFO_REPORT or RadarProtocol.BreathDetectCmd.BREATH_INFO_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBreathState")}: {d[0] switch { 1 => l.Get("StateBreathNormal"), 2 => l.Get("StateBreathHigh"), 3 => l.Get("StateBreathLow"), 4 => l.Get("StateBreathNone"), _ => d[0].ToString() }}" : null,
                    RadarProtocol.BreathDetectCmd.SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBreathSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    RadarProtocol.BreathDetectCmd.WAVE_SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedWaveSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    RadarProtocol.BreathDetectCmd.LOW_BREATH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedLowBreath")}: {d[0]}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.HEART_RATE:
                return frame.CommandWord switch
                {
                    RadarProtocol.HeartRateCmd.HEART_VALUE_REPORT or RadarProtocol.HeartRateCmd.HEART_VALUE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedHeartRate")}: {d[0]}{l.Get("UnitBpmShort")}" : null,
                    RadarProtocol.HeartRateCmd.SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedHeartSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    RadarProtocol.HeartRateCmd.WAVE_SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedWaveSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.SLEEP_MONITOR:
                return frame.CommandWord switch
                {
                    RadarProtocol.SleepMonitorCmd.SLEEP_STATE_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_STATE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBedState")}: {d[0] switch { 0 => l.Get("ParsedOutBed"), 1 => l.Get("ParsedInBed"), 2 => l.Get("ParsedRealTime"), _ => d[0].ToString() }}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_DURATION_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_DURATION_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedSleepState")}: {d[0] switch { 0 => l.Get("StateDeepSleep"), 1 => l.Get("StateLightSleep"), 2 => l.Get("StateAwake"), 3 => l.Get("StateOutOfBed"), _ => d[0].ToString() }}" : null,
                    RadarProtocol.SleepMonitorCmd.AWAKE_DURATION_REPORT or RadarProtocol.SleepMonitorCmd.AWAKE_DURATION_QUERY =>
                        len >= 2 ? $"{l.Get("ParsedAwakeDur")}: {d[0]<<8|d[1]} {l.Get("UnitMin")}" : null,
                    RadarProtocol.SleepMonitorCmd.LIGHT_DURATION_REPORT or RadarProtocol.SleepMonitorCmd.LIGHT_DURATION_QUERY =>
                        len >= 2 ? $"{l.Get("ParsedLightDur")}: {d[0]<<8|d[1]} {l.Get("UnitMin")}" : null,
                    RadarProtocol.SleepMonitorCmd.DEEP_DURATION_REPORT or RadarProtocol.SleepMonitorCmd.DEEP_DURATION_QUERY =>
                        len >= 2 ? $"{l.Get("ParsedDeepDur")}: {d[0]<<8|d[1]} {l.Get("UnitMin")}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_SCORE_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_SCORE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedSleepScore")}: {d[0]} {l.Get("UnitScore")}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_RATING_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_RATING_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedRating")}: {d[0] switch { 1 => l.Get("ParsedRatingGood"), 2 => l.Get("ParsedRatingAvg"), 3 => l.Get("ParsedRatingPoor"), _ => l.Get("ParsedRatingNone") }}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_ABNORMAL_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_ABNORMAL_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedAbnormal")}: {d[0] switch { 0 => l.Get("ParsedAbnormalShort"), 1 => l.Get("ParsedAbnormalLong"), 2 => l.Get("ParsedAbnormalNoPerson"), 3 => l.Get("ParsedAbnormalNone"), _ => d[0].ToString() }}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_COMP_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_COMP_QUERY =>
                        len >= 8 ? $"{l.Get("ParsedComposite")}: Pres={d[0]==1} State={d[1]} Breath={d[2]} HR={d[3]} Turn={d[4]} BigMove={d[5]}% SmallMove={d[6]}% Apnea={d[7]}" : null,
                    RadarProtocol.SleepMonitorCmd.SLEEP_ANALYSIS_REPORT or RadarProtocol.SleepMonitorCmd.SLEEP_ANALYSIS_QUERY =>
                        len >= 12 ? $"{l.Get("ParsedAnalysis")}: Score={d[0]} Total={(d[1]<<8|d[2])}min Awake={d[3]}% Light={d[4]}% Deep={d[5]}% OutBed={d[6]}min OutTimes={d[7]} Turn={d[8]} AvgBreath={d[9]} AvgHR={d[10]} Apnea={d[11]}" : null,
                    RadarProtocol.SleepMonitorCmd.STRUGGLE_REPORT or RadarProtocol.SleepMonitorCmd.STRUGGLE_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedStruggle")}: {(d[0] == 0 ? l.Get("StateNormal") : l.Get("StateAbnormal"))}" : null,
                    RadarProtocol.SleepMonitorCmd.SWITCH_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedSleepSwitch")}: {(d[0] == 0x01 ? l.Get("ParsedOn") : l.Get("ParsedOff"))}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.PRODUCT_INFO:
                if (len >= 1)
                {
                    var str = System.Text.Encoding.ASCII.GetString(d, 0, len);
                    return frame.CommandWord switch
                    {
                        RadarProtocol.ProductInfoCmd.MODEL_REPORT => $"{l.Get("ParsedModel")}: {str}",
                        RadarProtocol.ProductInfoCmd.ID_REPORT => $"{l.Get("ParsedId")}: {str}",
                        RadarProtocol.ProductInfoCmd.HARDWARE_REPORT => $"{l.Get("ParsedHardware")}: {str}",
                        RadarProtocol.ProductInfoCmd.FIRMWARE_REPORT => $"{l.Get("ParsedFirmware")}: {str}",
                        _ => $"{l.Get("ParsedProductInfo")}: {str}"
                    };
                }
                return null;

            case RadarProtocol.ControlWord.WORK_STATUS:
                return frame.CommandWord switch
                {
                    RadarProtocol.WorkStatusCmd.INIT_COMPLETE_REPORT or RadarProtocol.WorkStatusCmd.INIT_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedInit")}: {(d[0] == 0x01 ? l.Get("ParsedInitDone") : l.Get("ParsedInitNotDone"))}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.RADAR_RANGE:
                return frame.CommandWord switch
                {
                    RadarProtocol.RadarRangeCmd.BOUNDARY_REPORT or RadarProtocol.RadarRangeCmd.BOUNDARY_QUERY =>
                        len >= 1 ? $"{l.Get("ParsedBoundary")}: {(d[0] == 0x01 ? l.Get("ParsedInside") : l.Get("ParsedOutside"))}" : null,
                    _ => null
                };

            case RadarProtocol.ControlWord.HEARTBEAT:
                return frame.CommandWord switch
                {
                    RadarProtocol.HeartbeatCmd.HEARTBEAT_REPORT or RadarProtocol.HeartbeatCmd.HEARTBEAT_QUERY =>
                        l.Get("ParsedHeartbeat"),
                    _ => null
                };

            default:
                return null;
        }
    }

    // ========================================================================
    //  波形图表
    // ========================================================================

    /// <summary>初始化呼吸和心率波形图表（护眼暗色主题，类似监护仪）</summary>
    private void InitWaveCharts()
    {
        if (_chartBreathWave == null || _chartHeartWave == null) return;
        _chartsInitialized = false;

        SetupWaveChart(_chartBreathWave, "BreathWave", "呼吸波形",
            Color.FromArgb(0, 255, 140), Color.FromArgb(0, 80, 50));
        SetupWaveChart(_chartHeartWave, "HeartWave", "心率波形",
            Color.FromArgb(255, 80, 80), Color.FromArgb(80, 0, 0));

        // 预填缓冲区：从 128（中轴）开始 5 秒的平直线
        for (int i = 0; i < WaveBufferSize; i++)
        {
            _breathWaveBuffer.Enqueue(128);
            _heartWaveBuffer.Enqueue(128);
        }
        _chartsInitialized = true;
    }

    /// <summary>配置单个波形图表的通用样式</summary>
    private static void SetupWaveChart(Chart chart, string seriesName, string title, Color line, Color fill)
    {
        // --- ChartArea ---
        var area = new ChartArea("MainArea")
        {
            BackColor = Color.FromArgb(25, 25, 30),
            BorderColor = Color.FromArgb(50, 50, 55)
        };

        // X 轴：显示最近 75 个点（15 秒）
        area.AxisX.Minimum = 0;
        area.AxisX.Maximum = 75;
        area.AxisX.MajorGrid.Enabled = false;
        area.AxisX.MinorGrid.Enabled = false;
        area.AxisX.LabelStyle.Enabled = false;
        area.AxisX.MajorTickMark.Enabled = false;
        area.AxisY.Minimum = 0;
        area.AxisY.Maximum = 255;

        chart.ChartAreas.Clear();
        chart.ChartAreas.Add(area);

        // Y 轴：波形值 0-255
        area.AxisY.LineColor = Color.FromArgb(60, 60, 65);
        area.AxisY.MajorGrid.LineColor = Color.FromArgb(40, 40, 45);
        area.AxisY.MinorGrid.Enabled = false;
        area.AxisY.LabelStyle.ForeColor = Color.FromArgb(120, 120, 130);
        area.AxisY.LabelStyle.Font = new Font("Consolas", 7F);
        area.AxisY.Interval = 64;
        area.AxisY.MajorTickMark.Enabled = false;
        area.AxisX.LineColor = Color.FromArgb(60, 60, 65);

        // 中轴参考线 (y=128)
        var refLine = new StripLine
        {
            BackColor = Color.FromArgb(20, 255, 255, 255),
            StripWidth = 0.5,
            Interval = 0,
            IntervalOffset = 128
        };
        area.AxisY.StripLines.Add(refLine);

        // --- Series ---
        var series = new Series(seriesName)
        {
            ChartType = SeriesChartType.FastLine,
            Color = line,
            BorderWidth = 2,
            ShadowOffset = 0
        };
        chart.Series.Clear();
        chart.Series.Add(series);

        // --- 整体外观 ---
        chart.BackColor = Color.FromArgb(25, 25, 30);
        chart.Palette = ChartColorPalette.None;
        chart.AntiAliasing = AntiAliasingStyles.Graphics;

        // 标题 (左上角悬浮标签)
        chart.Titles.Clear();
        chart.Titles.Add(new Title(title, Docking.Top,
            new Font("微软雅黑", 9F, FontStyle.Bold), Color.FromArgb(200, 200, 200)));
    }

    /// <summary>呼吸波形数据推送：每次收到呼吸数据时，将 5 个新采样点加入缓冲区</summary>
    private void PushBreathWaveData()
    {
        var wave = _dataProcessor.BreathWave;
        if (wave == null || wave.Length < 5) return;

        foreach (var v in wave)
        {
            _breathWaveBuffer.Dequeue();
            _breathWaveBuffer.Enqueue(v);
        }
        UpdateWaveChart(_chartBreathWave, "BreathWave", _breathWaveBuffer);
    }

    /// <summary>心率波形数据推送：每次收到心率数据时，将 5 个新采样点加入缓冲区</summary>
    private void PushHeartWaveData()
    {
        var wave = _dataProcessor.HeartWave;
        if (wave == null || wave.Length < 5) return;

        foreach (var v in wave)
        {
            _heartWaveBuffer.Dequeue();
            _heartWaveBuffer.Enqueue(v);
        }
        UpdateWaveChart(_chartHeartWave, "HeartWave", _heartWaveBuffer);
    }

    /// <summary>将环形缓冲区渲染到 Chart 的 Points 集合（第 0 点是旧的，第 N 点是新的）</summary>
    private static void UpdateWaveChart(Chart chart, string seriesName, Queue<int> buffer)
    {
        if (chart == null || chart.IsDisposed) return;
        var series = chart.Series[seriesName];

        series.Points.SuspendUpdates();
        try
        {
            var arr = buffer.ToArray();
            if (series.Points.Count == 0)
            {
                for (int i = 0; i < arr.Length; i++)
                    series.Points.AddXY(i, arr[i]);
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                    series.Points[i].SetValueXY(i, arr[i]);
            }
        }
        finally
        {
            series.Points.ResumeUpdates();
        }

        // 刷新图表
        chart.Invalidate();
    }

    // ========================================================================
    //  Tab 绘制
    // ========================================================================
    private void DrawTabItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tab) return;
        var g = e.Graphics;
        var tabText = tab.TabPages[e.Index].Text;
        var tabRect = tab.GetTabRect(e.Index);
        bool isSelected = e.Index == tab.SelectedIndex;

        using var bgBrush = new SolidBrush(isSelected ? Color.White : Color.FromArgb(236, 240, 241));
        g.FillRectangle(bgBrush, tabRect);

        if (isSelected)
        {
            using var accentPen = new Pen(Color.FromArgb(52, 152, 219), 3);
            g.DrawLine(accentPen, tabRect.Left, tabRect.Bottom - 2, tabRect.Right, tabRect.Bottom - 2);
        }

        TextRenderer.DrawText(g, tabText, tab.Font, tabRect,
            isSelected ? Color.FromArgb(44, 62, 80) : Color.FromArgb(127, 140, 141),
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    // ========================================================================
    //  多语言
    // ========================================================================
    private void RegisterLocStrings()
    {
        // Label 注册
        _locLabels["AppSubtitle"] = _lblSubtitle;
        _locLabels["LblPort"] = _lblPort;
        _locLabels["LogPanelTitle"] = _lblLogTitle;
        _locLabels["LblSettingsResult"] = _lblSettingsResult;
        _locLabels["SettingsRuleHint"] = _settingsRuleLabel;

        // 按钮注册（设计器创建的按钮）
        RegisterAllSettingsButtons();
    }

    private void RegisterAllSettingsButtons()
    {
        // 工具栏按钮
        _locButtons["BtnClearLog"] = _btnClearLog;
        _locButtons["BtnExportLog"] = _btnExportLog;
        _locButtons["BtnClearResult"] = _btnClearResult;
        _locButtons["BtnRawHexClear"] = _btnRawHexClear;

        // 人体存在
        _locButtons["BtnPresenceOn"] = _btnPresenceOn;
        _locButtons["BtnPresenceOff"] = _btnPresenceOff;
        _locButtons["BtnQueryPresence"] = _btnQueryPresence;
        _locButtons["BtnQueryMotion"] = _btnQueryMotion;
        _locButtons["BtnQueryDistance"] = _btnQueryDistance;
        _locButtons["BtnQueryPosition"] = _btnQueryPosition;

        // 呼吸监测
        _locButtons["BtnBreathOn"] = _btnBreathOn;
        _locButtons["BtnBreathOff"] = _btnBreathOff;
        _locButtons["BtnBreathWaveOn"] = _btnBreathWaveOn;
        _locButtons["BtnBreathWaveOff"] = _btnBreathWaveOff;
        _locButtons["BtnQueryBreathValue"] = _btnQueryBreathValue;
        _locButtons["BtnQueryBreathState"] = _btnQueryBreathState;
        _locButtons["BtnQueryBreathWaveSwitch"] = _btnQueryBreathWaveSwitch;
        _locButtons["BtnQueryBreathSwitch"] = _btnQueryBreathSwitch;
        _locButtons["BtnSetLowBreath"] = _btnSetLowBreath;
        _locButtons["BtnQueryLowBreath"] = _btnQueryLowBreath;

        // 心率监测
        _locButtons["BtnHeartOn"] = _btnHeartOn;
        _locButtons["BtnHeartOff"] = _btnHeartOff;
        _locButtons["BtnHeartWaveOn"] = _btnHeartWaveOn;
        _locButtons["BtnHeartWaveOff"] = _btnHeartWaveOff;
        _locButtons["BtnQueryHeartValue"] = _btnQueryHeartValue;
        _locButtons["BtnQueryHeartSwitch"] = _btnQueryHeartSwitch;
        _locButtons["BtnQueryHeartWave"] = _btnQueryHeartWave;
        _locButtons["BtnQueryHeartWaveSwitch"] = _btnQueryHeartWaveSwitch;

        // 睡眠监测
        _locButtons["BtnSleepOn"] = _btnSleepOn;
        _locButtons["BtnSleepOff"] = _btnSleepOff;
        _locButtons["BtnQuerySleepComp"] = _btnQuerySleepComp;
        _locButtons["BtnQuerySleepAnalysis"] = _btnQuerySleepAnalysis;
        _locButtons["BtnQuerySleepRating"] = _btnQuerySleepRating;

        // 睡眠参数设置
        _locButtons["BtnStruggleSwitchOn"] = _btnStruggleSwitchOn;
        _locButtons["BtnStruggleSwitchOff"] = _btnStruggleSwitchOff;
        _locButtons["BtnNoPersonSwitchOn"] = _btnNoPersonSwitchOn;
        _locButtons["BtnNoPersonSwitchOff"] = _btnNoPersonSwitchOff;
        _locButtons["BtnSetNoPersonDuration"] = _btnSetNoPersonDuration;
        _locButtons["BtnExtCtrlSwitchOn"] = _btnExtCtrlSwitchOn;
        _locButtons["BtnExtCtrlSwitchOff"] = _btnExtCtrlSwitchOff;
        _locButtons["BtnSleepPeriodStart"] = _btnSleepPeriodStart;
        _locButtons["BtnSleepPeriodEnd"] = _btnSleepPeriodEnd;
        _locButtons["BtnSetSleepDeadline"] = _btnSetSleepDeadline;

        // 睡眠参数查询
        _locButtons["BtnQueryBedState"] = _btnQueryBedState;
        _locButtons["BtnQuerySleepState"] = _btnQuerySleepState;
        _locButtons["BtnQueryAwakeDur"] = _btnQueryAwakeDur;
        _locButtons["BtnQueryLightDur"] = _btnQueryLightDur;
        _locButtons["BtnQueryDeepDur"] = _btnQueryDeepDur;
        _locButtons["BtnQuerySleepScore"] = _btnQuerySleepScore;
        _locButtons["BtnQuerySleepAbnormal"] = _btnQuerySleepAbnormal;
        _locButtons["BtnQuerySleepStats"] = _btnQuerySleepStats;
        _locButtons["BtnQueryStruggleSwitch"] = _btnQueryStruggleSwitch;
        _locButtons["BtnQueryNoPersonSwitch"] = _btnQueryNoPersonSwitch;

        // 系统功能
        _locButtons["BtnHeartbeatQuery"] = _btnHeartbeatQuery;
        _locButtons["BtnModuleReset"] = _btnModuleReset;
        _locButtons["BtnInitQuery"] = _btnInitQuery;
        _locButtons["BtnBoundaryQuery"] = _btnBoundaryQuery;
        _locButtons["BtnQueryDev"] = _btnQueryDev;
    }

    private void ApplyLocalization()
    {
        _mainTab.TabPages[0].Text = _loc.Get("TabDisplay");
        _mainTab.TabPages[1].Text = _loc.Get("TabSettings");
        _mainTab.TabPages[2].Text = _loc.Get("TabProtocol");
        _mainTab.TabPages[3].Text = _loc.Get("TabEquations");


        _cardPresence.Title = _loc.Get("CardPresence");
        _cardBreath.Title = _loc.Get("CardBreath");
        _cardHeart.Title = _loc.Get("CardHeartRate");
        _cardSleepState.Title = _loc.Get("CardSleepState");
        _cardBodyMove.Title = _loc.Get("CardBodyMove");
        _cardSleepScore.Title = _loc.Get("CardSleepScore");
        _cardDistance.Title = _loc.Get("CardDistance");
        _cardApnea.Title = _loc.Get("ApneaCount");
        _cardTurnOver.Title = _loc.Get("TurnOverCount");

        ApplyGroupLoc("GrpPresenceCtrl", "TabPresence");
        ApplyGroupLoc("GrpBreathCtrl", "TabBreath");
        ApplyGroupLoc("GrpHeartCtrl", "TabHeartRate");
        ApplyGroupLoc("GrpSleepCtrl", "TabSleep");
        ApplyGroupLoc("GrpSleepSettings", "GrpSleepSettings");
        ApplyGroupLoc("GrpSleepQuery", "GrpSleepQuery");
        ApplyGroupLoc("GrpSystemFunc", "GrpSystemFunc");

        ApplyLabelLoc("AppSubtitle", "AppSubtitle");
        ApplyLabelLoc("LblPort", "LblPort");
        ApplyLabelLoc("LogPanelTitle", "LogPanelTitle");
        ApplyLabelLoc("LblSettingsResult", "LblSettingsResult");
        ApplyLabelLoc("SettingsRuleHint", "SettingsRuleHint");

        // 窗口标题
        this.Text = _loc.Get("AppTitle");
        _lblTitle.Text = "SleepSightPro";
        _lblSubtitle.Text = $"V1.0.0.0 | {_loc.Get("AppSubtitle")}";

        string[] btnKeys = {
            "BtnClearLog", "BtnExportLog", "BtnClearResult", "BtnRawHexClear",
            "BtnPresenceOn", "BtnPresenceOff", "BtnQueryPresence", "BtnQueryMotion", "BtnQueryDistance", "BtnQueryPosition",
            "BtnBreathOn", "BtnBreathOff", "BtnBreathWaveOn", "BtnBreathWaveOff",
            "BtnQueryBreathValue", "BtnQueryBreathState", "BtnQueryBreathWaveSwitch", "BtnQueryBreathSwitch",
            "BtnSetLowBreath", "BtnQueryLowBreath",
            "BtnHeartOn", "BtnHeartOff", "BtnHeartWaveOn", "BtnHeartWaveOff",
            "BtnQueryHeartValue", "BtnQueryHeartSwitch", "BtnQueryHeartWave", "BtnQueryHeartWaveSwitch",
            "BtnSleepOn", "BtnSleepOff", "BtnQuerySleepComp", "BtnQuerySleepAnalysis", "BtnQuerySleepRating",
            "BtnStruggleSwitchOn", "BtnStruggleSwitchOff",
            "BtnNoPersonSwitchOn", "BtnNoPersonSwitchOff",
            "BtnExtCtrlSwitchOn", "BtnExtCtrlSwitchOff",
            "BtnSleepPeriodStart", "BtnSleepPeriodEnd",
            "BtnSetNoPersonDuration", "BtnSetSleepDeadline",
            "BtnQueryBedState", "BtnQuerySleepState", "BtnQueryAwakeDur", "BtnQueryLightDur", "BtnQueryDeepDur",
            "BtnQuerySleepScore", "BtnQuerySleepAbnormal", "BtnQuerySleepStats",
            "BtnQueryStruggleSwitch", "BtnQueryNoPersonSwitch",
            "BtnHeartbeatQuery", "BtnModuleReset", "BtnInitQuery", "BtnBoundaryQuery",
            "BtnQueryDev"
        };
        foreach (var key in btnKeys)
            ApplyButtonLoc(key, key);

        _btnConnect.Text = _serialService.IsConnected ? _loc.Get("BtnDisconnect") : _loc.Get("BtnConnect");
        if (!_serialService.IsConnected)
            _statusLabel.Text = _loc.Get("StatusDisconnected");

        _clipHintLabel.Text = _loc.Get("StatusDblClickHint");

        _fwLabel.Text = !string.IsNullOrEmpty(_dataProcessor.FirmwareVersion)
            ? $"{_loc.Get("StatusFW")}{_dataProcessor.FirmwareVersion}"
            : $"{_loc.Get("StatusFW")}--";

        if (_logListView != null && _logListView.Columns.Count >= 6)
        {
            _logListView.Columns[0].Text = _loc.Get("LogColTime");
            _logListView.Columns[1].Text = _loc.Get("LogColDir");
            _logListView.Columns[2].Text = _loc.Get("LogColCtrl");
            _logListView.Columns[3].Text = _loc.Get("LogColCmd");
            _logListView.Columns[4].Text = _loc.Get("LogColLen");
            _logListView.Columns[5].Text = _loc.Get("LogColData");
        }

        if (_logTab != null && _logTab.TabPages.Count >= 2)
        {
            _logTab.TabPages[0].Text = _loc.Get("LogTabParsed");
            _logTab.TabPages[1].Text = _loc.Get("LogTabRawHex");
        }

        if (_btnDiagnose != null) _btnDiagnose.Text = _loc.Get("BtnDiagnose");

        // 更新原始 Hex 状态标签
        if (_lblRawHexStatus != null)
            _lblRawHexStatus.Text = string.Format(_loc.Get("RawHexStatus"), _rawByteCount);

        UpdateDashboardCards();

        // 刷新通讯协议 Tab 语言
        RefreshProtocolTab();

        // 刷新雷达方程 Tab 语言
        RefreshEquationsTab();
    }

    private void ApplyGroupLoc(string groupKey, string locKey)
    {
        if (_locGroups.TryGetValue(groupKey, out var grp))
            grp.Text = _loc.Get(locKey);
    }

    private void ApplyLabelLoc(string labelKey, string locKey)
    {
        if (_locLabels.TryGetValue(labelKey, out var lbl))
            lbl.Text = _loc.Get(locKey);
    }

    private void ApplyButtonLoc(string btnKey, string locKey)
    {
        if (_locButtons.TryGetValue(btnKey, out var btn))
            btn.Text = _loc.Get(locKey);
    }

    // ========================================================================
    //  双击状态栏 → 将整个窗体截图复制到剪贴板
    // ========================================================================
    private void CaptureFormToClipboard()
    {
        try
        {
            using var bitmap = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmap, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bitmap);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{_loc.Get("MsgError")}: {ex.Message}",
                _loc.Get("MsgWarning"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
