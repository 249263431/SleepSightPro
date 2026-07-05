namespace SleepSightPro.Models;

/// <summary>
/// R60ABD1 雷达协议定义
/// </summary>
public static class RadarProtocol
{
    // 帧结构常量
    public const byte FRAME_HEAD1 = 0x53;
    public const byte FRAME_HEAD2 = 0x59;
    public const byte FRAME_TAIL1 = 0x54;
    public const byte FRAME_TAIL2 = 0x43;
    public const byte DEFAULT_QUERY_DATA = 0x0F;

    // 控制字
    public static class ControlWord
    {
        public const byte HEARTBEAT = 0x01;       // 心跳包
        public const byte PRODUCT_INFO = 0x02;     // 产品信息
        public const byte OTA = 0x03;              // OTA升级
        public const byte WORK_STATUS = 0x05;       // 工作状态
        public const byte RADAR_RANGE = 0x07;       // 雷达探测范围
        public const byte HUMAN_PRESENCE = 0x80;    // 人体存在
        public const byte BREATH_DETECT = 0x81;     // 呼吸检测
        public const byte SLEEP_MONITOR = 0x84;     // 睡眠监测
        public const byte HEART_RATE = 0x85;        // 心率监测
    }

    // 命令字 - 人体存在 (0x80)
    public static class HumanPresenceCmd
    {
        public const byte SWITCH_SET = 0x00;        // 开关设置
        public const byte PRESENCE_REPORT = 0x01;   // 存在信息主动上报
        public const byte MOTION_REPORT = 0x02;     // 运动信息主动上报
        public const byte BODY_MOVE_REPORT = 0x03;  // 体动参数主动上报
        public const byte DISTANCE_REPORT = 0x04;   // 人体距离主动上报
        public const byte POSITION_REPORT = 0x05;   // 人体方位主动上报
        public const byte SWITCH_QUERY = 0x80;      // 查询人体存在开关
        public const byte PRESENCE_QUERY = 0x81;    // 存在信息查询
        public const byte MOTION_QUERY = 0x82;      // 运动信息查询
        public const byte BODY_MOVE_QUERY = 0x83;   // 体动参数查询
        public const byte DISTANCE_QUERY = 0x84;    // 人体距离查询
        public const byte POSITION_QUERY = 0x85;    // 人体方位查询
    }

    // 命令字 - 呼吸检测 (0x81)
    public static class BreathDetectCmd
    {
        public const byte SWITCH_SET = 0x00;        // 开关设置
        public const byte BREATH_INFO_REPORT = 0x01;// 呼吸信息主动上报
        public const byte BREATH_VALUE_REPORT = 0x02;// 呼吸数值主动上报
        public const byte BREATH_WAVE_REPORT = 0x05;// 呼吸波形主动上报
        public const byte LOW_BREATH_SET = 0x0B;    // 低缓呼吸判读设置
        public const byte WAVE_SWITCH_SET = 0x0C;   // 呼吸波形上报开关
        public const byte TIME_DOMAIN_REPORT = 0x0D;// 时域呼吸值上报
        public const byte SWITCH_QUERY = 0x80;      // 查询开关
        public const byte BREATH_INFO_QUERY = 0x81; // 呼吸信息查询
        public const byte BREATH_VALUE_QUERY = 0x82;// 呼吸数值查询
        public const byte BREATH_WAVE_QUERY = 0x85; // 呼吸波形查询
        public const byte LOW_BREATH_QUERY = 0x8B;  // 低缓呼吸判读查询
        public const byte WAVE_SWITCH_QUERY = 0x8C; // 呼吸波形上报开关查询
    }

    // 命令字 - 睡眠监测 (0x84)
    public static class SleepMonitorCmd
    {
        public const byte SWITCH_SET = 0x00;        // 开关设置
        public const byte SLEEP_STATE_REPORT = 0x01;// 入床/离床状态上报
        public const byte SLEEP_DURATION_REPORT = 0x02;// 睡眠状态（清醒/浅睡/深睡）上报
        public const byte AWAKE_DURATION_REPORT = 0x03;// 清醒时长上报
        public const byte LIGHT_DURATION_REPORT = 0x04;// 浅睡时长上报
        public const byte DEEP_DURATION_REPORT = 0x05;// 深睡时长上报
        public const byte SLEEP_SCORE_REPORT = 0x06;// 睡眠质量评分上报
        public const byte SLEEP_COMP_REPORT = 0x0C; // 睡眠综合状态上报
        public const byte SLEEP_ANALYSIS_REPORT = 0x0D;// 睡眠质量分析上报
        public const byte SLEEP_ABNORMAL_REPORT = 0x0E;// 睡眠异常上报
        public const byte SLEEP_RATING_REPORT = 0x10;// 睡眠质量评级上报
        public const byte STRUGGLE_REPORT = 0x11;   // 异常挣扎上报
        public const byte NO_PERSON_TIME_REPORT = 0x12;// 无人计时上报
        // 设置类命令
        public const byte STRUGGLE_SWITCH_SET = 0x13;// 异常挣扎状态开关设置
        public const byte NO_PERSON_SWITCH_SET = 0x14;// 无人计时功能开关设置
        public const byte NO_PERSON_DURATION_SET = 0x15;// 无人计时时长设置
        public const byte SLEEP_DEADLINE_SET = 0x16;// 睡眠截止时长设置
        public const byte STRUGGLE_LEVEL_SET = 0x1A;// 挣扎状态判读设置
        public const byte EXT_SWITCH_SET = 0x1C;    // 睡眠周期外部控制开关
        public const byte SLEEP_PERIOD_SET = 0x1D;  // 睡眠周期开始截止设置
        // 查询类命令
        public const byte SWITCH_QUERY = 0x80;      // 查询开关
        public const byte SLEEP_STATE_QUERY = 0x81; // 入床/离床状态查询
        public const byte SLEEP_DURATION_QUERY = 0x82;// 睡眠状态查询
        public const byte AWAKE_DURATION_QUERY = 0x83;// 清醒时长查询
        public const byte LIGHT_DURATION_QUERY = 0x84;// 浅睡时长查询
        public const byte DEEP_DURATION_QUERY = 0x85;// 深睡时长查询
        public const byte SLEEP_SCORE_QUERY = 0x86; // 睡眠质量评分查询
        public const byte SLEEP_COMP_QUERY = 0x8D;  // 睡眠综合状态查询
        public const byte SLEEP_ABNORMAL_QUERY = 0x8E;// 睡眠异常查询
        public const byte SLEEP_ANALYSIS_QUERY = 0x8F;// 睡眠质量分析查询
        public const byte SLEEP_RATING_QUERY = 0x90; // 睡眠质量评级查询
        public const byte STRUGGLE_QUERY = 0x91;    // 异常挣扎状态查询
        public const byte NO_PERSON_TIME_QUERY = 0x92;// 无人计时状态查询
        public const byte STRUGGLE_SWITCH_QUERY = 0x93;// 异常挣扎开关查询
        public const byte NO_PERSON_SWITCH_QUERY = 0x94;// 无人计时开关查询
        public const byte NO_PERSON_DURATION_QUERY = 0x95;// 无人计时时长查询
        public const byte SLEEP_DEADLINE_QUERY = 0x96;// 睡眠截止时间查询
        public const byte STRUGGLE_LEVEL_QUERY = 0x9A;// 挣扎状态判读查询
        public const byte EXT_SWITCH_QUERY = 0x9C;  // 外部控制睡眠开关查询
        public const byte SLEEP_PERIOD_QUERY = 0x9D; // 睡眠周期开始截止查询
    }

    // 命令字 - 心率监测 (0x85)
    public static class HeartRateCmd
    {
        public const byte SWITCH_SET = 0x00;        // 开关设置
        public const byte HEART_VALUE_REPORT = 0x02;// 心率数值主动上报
        public const byte HEART_WAVE_REPORT = 0x05; // 心率波形主动上报
        public const byte WAVE_SWITCH_SET = 0x0A;   // 心率波形上报开关
        public const byte SWITCH_QUERY = 0x80;      // 查询开关
        public const byte HEART_VALUE_QUERY = 0x82; // 心率数值查询
        public const byte HEART_WAVE_QUERY = 0x85;  // 心率波形查询
        public const byte WAVE_SWITCH_QUERY = 0x8A; // 心率波形上报开关查询
    }

    // 命令字 - 心跳包/系统 (0x01)
    public static class HeartbeatCmd
    {
        public const byte HEARTBEAT_REPORT = 0x01;  // 心跳包上报
        public const byte MODULE_RESET = 0x02;      // 模组复位
        public const byte HEARTBEAT_QUERY = 0x80;   // 心跳包查询
    }

    // 命令字 - OTA (0x03)
    public static class OtaCmd
    {
        public const byte OTA_START = 0x01;         // 开始OTA升级
        public const byte OTA_TRANSFER = 0x02;      // 升级包传输
        public const byte OTA_END = 0x03;           // 结束OTA升级
    }

    // 命令字 - 产品信息 (0x02)
    public static class ProductInfoCmd
    {
        public const byte MODEL_REPORT = 0x01;      // 产品型号上报
        public const byte ID_REPORT = 0x02;         // 产品ID上报
        public const byte HARDWARE_REPORT = 0x03;   // 硬件型号上报
        public const byte FIRMWARE_REPORT = 0x04;   // 固件版本上报
        public const byte INFO_QUERY = 0xA1;        // 信息查询
        public const byte ID_QUERY = 0xA2;          // 产品ID查询
        public const byte HARDWARE_QUERY = 0xA3;    // 硬件型号查询
        public const byte FIRMWARE_QUERY = 0xA4;    // 固件版本查询
    }

    // 命令字 - 工作状态 (0x05)
    public static class WorkStatusCmd
    {
        public const byte INIT_COMPLETE_REPORT = 0x01;// 初始化完成上报
        public const byte INIT_QUERY = 0x81;          // 初始化完成查询
    }

    // 命令字 - 雷达探测范围 (0x07)
    public static class RadarRangeCmd
    {
        public const byte BOUNDARY_REPORT = 0x07;   // 位置越界上报
        public const byte BOUNDARY_QUERY = 0x87;    // 位置越界查询
    }

    // 人体存在状态
    public static class PresenceState
    {
        public const byte NONE = 0x00;
        public const byte PRESENT = 0x01;
    }

    // 运动状态
    public static class MotionState
    {
        public const byte NONE = 0x00;
        public const byte STATIC = 0x01;
        public const byte ACTIVE = 0x02;
    }

    // 呼吸状态
    public static class BreathState
    {
        public const byte NORMAL = 0x01;
        public const byte HIGH = 0x02;
        public const byte LOW = 0x03;
        public const byte NONE = 0x04;
    }

    // 睡眠状态
    public static class SleepState
    {
        public const byte DEEP_SLEEP = 0x00;
        public const byte LIGHT_SLEEP = 0x01;
        public const byte AWAKE = 0x02;
        public const byte OUT_OF_BED = 0x03;
    }

    // 睡眠质量评级
    public static class SleepRating
    {
        public const byte NONE = 0x00;
        public const byte GOOD = 0x01;
        public const byte AVERAGE = 0x02;
        public const byte POOR = 0x03;
    }

    // 睡眠异常状态
    public static class SleepAbnormalState
    {
        public const byte SLEEP_TOO_SHORT = 0x00;  // 睡眠时长不足4小时
        public const byte SLEEP_TOO_LONG = 0x01;   // 睡眠时长大于12小时
        public const byte LONG_NO_PERSON = 0x02;   // 长时间异常无人
        public const byte NONE = 0x03;              // 无异常
    }

    // 睡眠周期状态
    public static class SleepPeriodState
    {
        public const byte NONE = 0x00;   // 无
        public const byte START = 0x01;  // 开始
        public const byte END = 0x02;    // 结束
    }

    // 挣扎判读灵敏度
    public static class StruggleSensitivity
    {
        public const byte LOW = 0x00;
        public const byte MEDIUM = 0x01;
        public const byte HIGH = 0x02;
    }

    // 入床/离床状态
    public static class BedState
    {
        public const byte OUT_OF_BED = 0x00;
        public const byte IN_BED = 0x01;
        public const byte NONE = 0x02;  // 实时探测模式
    }

    // 越界状态
    public static class BoundaryState
    {
        public const byte OUTSIDE = 0x00;
        public const byte INSIDE = 0x01;
    }

    // 初始化状态
    public static class InitState
    {
        public const byte NOT_COMPLETED = 0x00;
        public const byte COMPLETED = 0x01;
    }
}
