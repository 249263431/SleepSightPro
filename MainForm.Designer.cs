using SleepSightPro.Controls;
using System.Windows.Forms.DataVisualization.Charting;

namespace SleepSightPro;

partial class MainForm
{
    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _mainLayout = new TableLayoutPanel();
        _topPanel = new Panel();
        _lblTitle = new Label();
        _lblSubtitle = new Label();
        _rightToolbar = new FlowLayoutPanel();
        _btnLang = new Button();
        _btnConnect = new Button();
        _btnRefresh = new Button();
        _cmbBaudRate = new ComboBox();
        _cmbPort = new ComboBox();
        _lblPort = new Label();
        _mainTab = new TabControl();
        _tabDisplay = new TabPage();
        _displayLayout = new TableLayoutPanel();
        _dashboardPanel = new TableLayoutPanel();
        _cardPresence = new DashboardCard();
        _cardBreath = new DashboardCard();
        _cardHeart = new DashboardCard();
        _cardSleepState = new DashboardCard();
        _cardBodyMove = new DashboardCard();
        _cardSleepScore = new DashboardCard();
        _cardDistance = new DashboardCard();
        _cardApnea = new DashboardCard();
        _cardTurnOver = new DashboardCard();
        _chartPanel = new TableLayoutPanel();
        _chartBreathWave = new Chart();
        _chartHeartWave = new Chart();
        _tabSettings = new TabPage();
        _settingsLayout = new TableLayoutPanel();
        _settingsScrollPanel = new FlowLayoutPanel();
        _grpPresenceCtrl = new GroupBox();
        _btnPresenceOn = new Button();
        _btnPresenceOff = new Button();
        _btnQueryPresence = new Button();
        _btnQueryMotion = new Button();
        _btnQueryDistance = new Button();
        _btnQueryPosition = new Button();
        _grpBreathCtrl = new GroupBox();
        _btnBreathOn = new Button();
        _btnBreathOff = new Button();
        _btnBreathWaveOn = new Button();
        _btnBreathWaveOff = new Button();
        _btnQueryBreathValue = new Button();
        _btnQueryBreathState = new Button();
        _btnQueryBreathWaveSwitch = new Button();
        _btnQueryBreathSwitch = new Button();
        _btnSetLowBreath = new Button();
        _btnQueryLowBreath = new Button();
        _grpHeartCtrl = new GroupBox();
        _btnHeartOn = new Button();
        _btnHeartOff = new Button();
        _btnHeartWaveOn = new Button();
        _btnHeartWaveOff = new Button();
        _btnQueryHeartValue = new Button();
        _btnQueryHeartSwitch = new Button();
        _btnQueryHeartWave = new Button();
        _btnQueryHeartWaveSwitch = new Button();
        _grpSleepCtrl = new GroupBox();
        _btnSleepOn = new Button();
        _btnSleepOff = new Button();
        _btnQuerySleepComp = new Button();
        _btnQuerySleepAnalysis = new Button();
        _btnQuerySleepRating = new Button();
        _grpSleepSettings = new GroupBox();
        _btnStruggleSwitchOn = new Button();
        _btnStruggleSwitchOff = new Button();
        _btnNoPersonSwitchOn = new Button();
        _btnNoPersonSwitchOff = new Button();
        _btnSetNoPersonDuration = new Button();
        _btnExtCtrlSwitchOn = new Button();
        _btnExtCtrlSwitchOff = new Button();
        _btnSleepPeriodStart = new Button();
        _btnSleepPeriodEnd = new Button();
        _btnSetSleepDeadline = new Button();
        _grpSleepQuery = new GroupBox();
        _btnQueryBedState = new Button();
        _btnQuerySleepState = new Button();
        _btnQueryAwakeDur = new Button();
        _btnQueryLightDur = new Button();
        _btnQueryDeepDur = new Button();
        _btnQuerySleepScore = new Button();
        _btnQuerySleepAbnormal = new Button();
        _btnQuerySleepStats = new Button();
        _btnQueryStruggleSwitch = new Button();
        _btnQueryNoPersonSwitch = new Button();
        _grpSystemFunc = new GroupBox();
        _btnHeartbeatQuery = new Button();
        _btnInitQuery = new Button();
        _btnBoundaryQuery = new Button();
        _btnModuleReset = new Button();
        _btnQueryDev = new Button();
        _settingsResultToolbar = new Panel();
        _lblSettingsResult = new Label();
        _settingsRuleLabel = new Label();
        _btnClearResult = new Button();
        _settingsResultBox = new RichTextBox();
        _tabProtocol = new TabPage();
        _protocolTextBox = new RichTextBox();
        _tabEquations = new TabPage();
        _equationsPanel = new Panel();
        _bottomLogPanel = new Panel();
        _logPanel = new Panel();
        _logTab = new TabControl();
        _tabParsed = new TabPage();
        _logListView = new ListView();
        _logColTime = new ColumnHeader();
        _logColDir = new ColumnHeader();
        _logColCtrl = new ColumnHeader();
        _logColCmd = new ColumnHeader();
        _logColLen = new ColumnHeader();
        _logColData = new ColumnHeader();
        _tabRawHex = new TabPage();
        _rawHexToolbar = new Panel();
        _lblRawHexStatus = new Label();
        _btnRawHexClear = new Button();
        _btnDiagnose = new Button();
        _rawHexBox = new RichTextBox();
        _logToolbar = new Panel();
        _lblLogTitle = new Label();
        _btnClearLog = new Button();
        _btnExportLog = new Button();
        _statusStrip = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel();
        _portLabel = new ToolStripStatusLabel();
        _fwLabel = new ToolStripStatusLabel();
        _clipHintLabel = new ToolStripStatusLabel();
        _versionLabel = new ToolStripStatusLabel();
        _mainLayout.SuspendLayout();
        _topPanel.SuspendLayout();
        _rightToolbar.SuspendLayout();
        _mainTab.SuspendLayout();
        _tabDisplay.SuspendLayout();
        _displayLayout.SuspendLayout();
        _dashboardPanel.SuspendLayout();
        _chartPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)_chartBreathWave).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_chartHeartWave).BeginInit();
        _tabSettings.SuspendLayout();
        _settingsLayout.SuspendLayout();
        _settingsScrollPanel.SuspendLayout();
        _grpPresenceCtrl.SuspendLayout();
        _grpBreathCtrl.SuspendLayout();
        _grpHeartCtrl.SuspendLayout();
        _grpSleepCtrl.SuspendLayout();
        _grpSleepSettings.SuspendLayout();
        _grpSleepQuery.SuspendLayout();
        _grpSystemFunc.SuspendLayout();
        _settingsResultToolbar.SuspendLayout();
        _tabProtocol.SuspendLayout();
        _tabEquations.SuspendLayout();
        _bottomLogPanel.SuspendLayout();
        _logPanel.SuspendLayout();
        _logTab.SuspendLayout();
        _tabParsed.SuspendLayout();
        _tabRawHex.SuspendLayout();
        _rawHexToolbar.SuspendLayout();
        _logToolbar.SuspendLayout();
        _statusStrip.SuspendLayout();
        SuspendLayout();
        // 
        // _mainLayout
        // 
        _mainLayout.ColumnCount = 1;
        _mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        _mainLayout.Controls.Add(_topPanel, 0, 0);
        _mainLayout.Controls.Add(_mainTab, 0, 1);
        _mainLayout.Controls.Add(_bottomLogPanel, 0, 2);
        _mainLayout.Controls.Add(_statusStrip, 0, 3);
        _mainLayout.Dock = DockStyle.Fill;
        _mainLayout.Location = new Point(0, 0);
        _mainLayout.Name = "_mainLayout";
        _mainLayout.RowCount = 4;
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200F));
        _mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        _mainLayout.Size = new Size(1341, 1033);
        _mainLayout.TabIndex = 0;
        // 
        // _topPanel
        // 
        _topPanel.BackColor = Color.FromArgb(44, 62, 80);
        _topPanel.Controls.Add(_lblTitle);
        _topPanel.Controls.Add(_lblSubtitle);
        _topPanel.Controls.Add(_rightToolbar);
        _topPanel.Dock = DockStyle.Fill;
        _topPanel.Location = new Point(3, 3);
        _topPanel.Name = "_topPanel";
        _topPanel.Padding = new Padding(12, 0, 12, 0);
        _topPanel.Size = new Size(1335, 42);
        _topPanel.TabIndex = 0;
        _topPanel.Resize += _topPanel_Resize;
        // 
        // _lblTitle
        // 
        _lblTitle.AutoSize = true;
        _lblTitle.Font = new Font("微软雅黑", 14F, FontStyle.Bold);
        _lblTitle.ForeColor = Color.White;
        _lblTitle.Location = new Point(12, 11);
        _lblTitle.Name = "_lblTitle";
        _lblTitle.Size = new Size(146, 26);
        _lblTitle.TabIndex = 0;
        _lblTitle.Text = "SleepSightPro";
        // 
        // _lblSubtitle
        // 
        _lblSubtitle.AutoSize = true;
        _lblSubtitle.Font = new Font("微软雅黑", 8F);
        _lblSubtitle.ForeColor = Color.FromArgb(189, 195, 199);
        _lblSubtitle.Location = new Point(170, 16);
        _lblSubtitle.Name = "_lblSubtitle";
        _lblSubtitle.Size = new Size(95, 16);
        _lblSubtitle.TabIndex = 1;
        _lblSubtitle.Text = "智能睡眠监测系统";
        // 
        // _rightToolbar
        // 
        _rightToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _rightToolbar.AutoSize = true;
        _rightToolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _rightToolbar.BackColor = Color.Transparent;
        _rightToolbar.Controls.Add(_btnLang);
        _rightToolbar.Controls.Add(_btnConnect);
        _rightToolbar.Controls.Add(_btnRefresh);
        _rightToolbar.Controls.Add(_cmbBaudRate);
        _rightToolbar.Controls.Add(_cmbPort);
        _rightToolbar.Controls.Add(_lblPort);
        _rightToolbar.FlowDirection = FlowDirection.RightToLeft;
        _rightToolbar.Location = new Point(917, 0);
        _rightToolbar.Margin = new Padding(0);
        _rightToolbar.Name = "_rightToolbar";
        _rightToolbar.Padding = new Padding(0, 10, 0, 10);
        _rightToolbar.Size = new Size(418, 48);
        _rightToolbar.TabIndex = 2;
        _rightToolbar.WrapContents = false;
        _rightToolbar.Layout += _rightToolbar_Layout;
        // 
        // _btnLang
        // 
        _btnLang.BackColor = Color.FromArgb(52, 73, 94);
        _btnLang.Cursor = Cursors.Hand;
        _btnLang.FlatAppearance.BorderSize = 0;
        _btnLang.FlatStyle = FlatStyle.Flat;
        _btnLang.Font = new Font("Segoe UI", 9F);
        _btnLang.ForeColor = Color.FromArgb(236, 240, 241);
        _btnLang.ImageAlign = ContentAlignment.MiddleLeft;
        _btnLang.Location = new Point(333, 10);
        _btnLang.Margin = new Padding(0);
        _btnLang.Name = "_btnLang";
        _btnLang.Padding = new Padding(6, 0, 6, 0);
        _btnLang.Size = new Size(85, 28);
        _btnLang.TabIndex = 0;
        _btnLang.Text = "中文";
        _btnLang.TextImageRelation = TextImageRelation.ImageBeforeText;
        _btnLang.UseVisualStyleBackColor = false;
        _btnLang.Click += _btnLang_Click;
        // 
        // _btnConnect
        // 
        _btnConnect.BackColor = Color.FromArgb(46, 204, 113);
        _btnConnect.FlatAppearance.BorderSize = 0;
        _btnConnect.FlatStyle = FlatStyle.Flat;
        _btnConnect.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _btnConnect.ForeColor = Color.White;
        _btnConnect.Location = new Point(263, 10);
        _btnConnect.Margin = new Padding(6, 0, 0, 0);
        _btnConnect.Name = "_btnConnect";
        _btnConnect.Size = new Size(70, 28);
        _btnConnect.TabIndex = 1;
        _btnConnect.Text = "连接";
        _btnConnect.UseVisualStyleBackColor = false;
        _btnConnect.Click += _btnConnect_Click;
        // 
        // _btnRefresh
        // 
        _btnRefresh.BackColor = Color.FromArgb(52, 73, 94);
        _btnRefresh.FlatAppearance.BorderSize = 0;
        _btnRefresh.FlatStyle = FlatStyle.Flat;
        _btnRefresh.Font = new Font("微软雅黑", 10F);
        _btnRefresh.ForeColor = Color.White;
        _btnRefresh.Location = new Point(229, 10);
        _btnRefresh.Margin = new Padding(6, 0, 0, 0);
        _btnRefresh.Name = "_btnRefresh";
        _btnRefresh.Size = new Size(28, 28);
        _btnRefresh.TabIndex = 2;
        _btnRefresh.Text = "⟳";
        _btnRefresh.UseVisualStyleBackColor = false;
        _btnRefresh.Click += _btnRefresh_Click;
        // 
        // _cmbBaudRate
        // 
        _cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbBaudRate.FlatStyle = FlatStyle.Flat;
        _cmbBaudRate.Font = new Font("微软雅黑", 9F);
        _cmbBaudRate.Items.AddRange(new object[] { "115200", "57600", "38400", "19200", "9600" });
        _cmbBaudRate.Location = new Point(143, 10);
        _cmbBaudRate.Margin = new Padding(6, 0, 0, 0);
        _cmbBaudRate.Name = "_cmbBaudRate";
        _cmbBaudRate.Size = new Size(80, 25);
        _cmbBaudRate.TabIndex = 3;
        // 
        // 
        // _cmbPort
        // 
        _cmbPort.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbPort.FlatStyle = FlatStyle.Flat;
        _cmbPort.Font = new Font("微软雅黑", 9F);
        _cmbPort.Location = new Point(47, 10);
        _cmbPort.Margin = new Padding(6, 0, 0, 0);
        _cmbPort.Name = "_cmbPort";
        _cmbPort.Size = new Size(90, 25);
        _cmbPort.TabIndex = 4;
        // 
        // _lblPort
        // 
        _lblPort.AutoSize = true;
        _lblPort.Font = new Font("微软雅黑", 9F);
        _lblPort.ForeColor = Color.FromArgb(236, 240, 241);
        _lblPort.Location = new Point(6, 15);
        _lblPort.Margin = new Padding(6, 5, 0, 0);
        _lblPort.Name = "_lblPort";
        _lblPort.Size = new Size(35, 17);
        _lblPort.TabIndex = 5;
        _lblPort.Text = "串口:";
        // 
        // _mainTab
        // 
        _mainTab.Controls.Add(_tabDisplay);
        _mainTab.Controls.Add(_tabSettings);
        _mainTab.Controls.Add(_tabProtocol);
        _mainTab.Controls.Add(_tabEquations);
        _mainTab.Dock = DockStyle.Fill;
        _mainTab.Font = new Font("微软雅黑", 10F);
        _mainTab.ItemSize = new Size(100, 36);
        _mainTab.Location = new Point(3, 51);
        _mainTab.Name = "_mainTab";
        _mainTab.Padding = new Point(0, 0);
        _mainTab.SelectedIndex = 0;
        _mainTab.Size = new Size(1335, 757);
        _mainTab.TabIndex = 1;
        _mainTab.DrawItem += _mainTab_DrawItem;
        // 
        // _tabDisplay
        // 
        _tabDisplay.BackColor = Color.FromArgb(245, 247, 250);
        _tabDisplay.Controls.Add(_displayLayout);
        _tabDisplay.Location = new Point(4, 40);
        _tabDisplay.Name = "_tabDisplay";
        _tabDisplay.Size = new Size(1327, 713);
        _tabDisplay.TabIndex = 0;
        _tabDisplay.Text = "显示";
        // 
        // _displayLayout
        // 
        _displayLayout.ColumnCount = 1;
        _displayLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _displayLayout.Controls.Add(_dashboardPanel, 0, 0);
        _displayLayout.Controls.Add(_chartPanel, 0, 1);
        _displayLayout.Dock = DockStyle.Fill;
        _displayLayout.Location = new Point(0, 0);
        _displayLayout.Name = "_displayLayout";
        _displayLayout.Padding = new Padding(8);
        _displayLayout.RowCount = 2;
        _displayLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 38F));
        _displayLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 62F));
        _displayLayout.Size = new Size(1327, 713);
        _displayLayout.TabIndex = 0;
        // 
        // _dashboardPanel
        // 
        _dashboardPanel.ColumnCount = 3;
        _dashboardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.Controls.Add(_cardPresence, 0, 0);
        _dashboardPanel.Controls.Add(_cardBreath, 1, 0);
        _dashboardPanel.Controls.Add(_cardHeart, 2, 0);
        _dashboardPanel.Controls.Add(_cardSleepState, 0, 1);
        _dashboardPanel.Controls.Add(_cardBodyMove, 1, 1);
        _dashboardPanel.Controls.Add(_cardSleepScore, 2, 1);
        _dashboardPanel.Controls.Add(_cardDistance, 0, 2);
        _dashboardPanel.Controls.Add(_cardApnea, 1, 2);
        _dashboardPanel.Controls.Add(_cardTurnOver, 2, 2);
        _dashboardPanel.Dock = DockStyle.Fill;
        _dashboardPanel.Location = new Point(8, 8);
        _dashboardPanel.Margin = new Padding(0, 0, 0, 4);
        _dashboardPanel.Name = "_dashboardPanel";
        _dashboardPanel.RowCount = 3;
        _dashboardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
        _dashboardPanel.Size = new Size(1311, 260);
        _dashboardPanel.TabIndex = 0;
        // 
        // _cardPresence
        // 
        _cardPresence.BackColor = Color.White;
        _cardPresence.Dock = DockStyle.Fill;
        _cardPresence.Location = new Point(3, 3);
        _cardPresence.MinimumSize = new Size(120, 60);
        _cardPresence.Name = "_cardPresence";
        _cardPresence.Size = new Size(431, 80);
        _cardPresence.TabIndex = 0;
        // 
        // _cardBreath
        // 
        _cardBreath.BackColor = Color.White;
        _cardBreath.Dock = DockStyle.Fill;
        _cardBreath.Location = new Point(440, 3);
        _cardBreath.MinimumSize = new Size(120, 60);
        _cardBreath.Name = "_cardBreath";
        _cardBreath.Size = new Size(431, 80);
        _cardBreath.TabIndex = 1;
        // 
        // _cardHeart
        // 
        _cardHeart.BackColor = Color.White;
        _cardHeart.Dock = DockStyle.Fill;
        _cardHeart.Location = new Point(877, 3);
        _cardHeart.MinimumSize = new Size(120, 60);
        _cardHeart.Name = "_cardHeart";
        _cardHeart.Size = new Size(431, 80);
        _cardHeart.TabIndex = 2;
        // 
        // _cardSleepState
        // 
        _cardSleepState.BackColor = Color.White;
        _cardSleepState.Dock = DockStyle.Fill;
        _cardSleepState.Location = new Point(3, 89);
        _cardSleepState.MinimumSize = new Size(120, 60);
        _cardSleepState.Name = "_cardSleepState";
        _cardSleepState.Size = new Size(431, 80);
        _cardSleepState.TabIndex = 3;
        // 
        // _cardBodyMove
        // 
        _cardBodyMove.BackColor = Color.White;
        _cardBodyMove.Dock = DockStyle.Fill;
        _cardBodyMove.Location = new Point(440, 89);
        _cardBodyMove.MinimumSize = new Size(120, 60);
        _cardBodyMove.Name = "_cardBodyMove";
        _cardBodyMove.Size = new Size(431, 80);
        _cardBodyMove.TabIndex = 4;
        // 
        // _cardSleepScore
        // 
        _cardSleepScore.BackColor = Color.White;
        _cardSleepScore.Dock = DockStyle.Fill;
        _cardSleepScore.Location = new Point(877, 89);
        _cardSleepScore.MinimumSize = new Size(120, 60);
        _cardSleepScore.Name = "_cardSleepScore";
        _cardSleepScore.Size = new Size(431, 80);
        _cardSleepScore.TabIndex = 5;
        // 
        // _cardDistance
        // 
        _cardDistance.BackColor = Color.White;
        _cardDistance.Dock = DockStyle.Fill;
        _cardDistance.Location = new Point(3, 175);
        _cardDistance.MinimumSize = new Size(120, 60);
        _cardDistance.Name = "_cardDistance";
        _cardDistance.Size = new Size(431, 82);
        _cardDistance.TabIndex = 6;
        // 
        // _cardApnea
        // 
        _cardApnea.BackColor = Color.White;
        _cardApnea.Dock = DockStyle.Fill;
        _cardApnea.Location = new Point(440, 175);
        _cardApnea.MinimumSize = new Size(120, 60);
        _cardApnea.Name = "_cardApnea";
        _cardApnea.Size = new Size(431, 82);
        _cardApnea.TabIndex = 7;
        // 
        // _cardTurnOver
        // 
        _cardTurnOver.BackColor = Color.White;
        _cardTurnOver.Dock = DockStyle.Fill;
        _cardTurnOver.Location = new Point(877, 175);
        _cardTurnOver.MinimumSize = new Size(120, 60);
        _cardTurnOver.Name = "_cardTurnOver";
        _cardTurnOver.Size = new Size(431, 82);
        _cardTurnOver.TabIndex = 8;
        // 
        // _chartPanel
        // 
        _chartPanel.BackColor = Color.FromArgb(30, 30, 30);
        _chartPanel.ColumnCount = 2;
        _chartPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _chartPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        _chartPanel.Controls.Add(_chartBreathWave, 0, 0);
        _chartPanel.Controls.Add(_chartHeartWave, 1, 0);
        _chartPanel.Dock = DockStyle.Fill;
        _chartPanel.Location = new Point(8, 276);
        _chartPanel.Margin = new Padding(0, 4, 0, 0);
        _chartPanel.Name = "_chartPanel";
        _chartPanel.RowCount = 1;
        _chartPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _chartPanel.Size = new Size(1311, 429);
        _chartPanel.TabIndex = 1;
        // 
        // _chartBreathWave
        // 
        _chartBreathWave.Dock = DockStyle.Fill;
        _chartBreathWave.Location = new Point(3, 3);
        _chartBreathWave.Margin = new Padding(3, 3, 2, 3);
        _chartBreathWave.Name = "_chartBreathWave";
        _chartBreathWave.Size = new Size(650, 423);
        _chartBreathWave.TabIndex = 0;
        _chartBreathWave.Text = "BreathWave";
        // 
        // _chartHeartWave
        // 
        _chartHeartWave.Dock = DockStyle.Fill;
        _chartHeartWave.Location = new Point(657, 3);
        _chartHeartWave.Margin = new Padding(2, 3, 3, 3);
        _chartHeartWave.Name = "_chartHeartWave";
        _chartHeartWave.Size = new Size(651, 423);
        _chartHeartWave.TabIndex = 1;
        _chartHeartWave.Text = "HeartWave";
        // 
        // _tabSettings
        // 
        _tabSettings.BackColor = Color.FromArgb(245, 247, 250);
        _tabSettings.Controls.Add(_settingsLayout);
        _tabSettings.Location = new Point(4, 40);
        _tabSettings.Name = "_tabSettings";
        _tabSettings.Size = new Size(1327, 713);
        _tabSettings.TabIndex = 1;
        _tabSettings.Text = "设置";
        // 
        // _settingsLayout
        // 
        _settingsLayout.ColumnCount = 1;
        _settingsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _settingsLayout.Controls.Add(_settingsScrollPanel, 0, 0);
        _settingsLayout.Controls.Add(_settingsResultToolbar, 0, 1);
        _settingsLayout.Controls.Add(_settingsResultBox, 0, 2);
        _settingsLayout.Dock = DockStyle.Fill;
        _settingsLayout.Location = new Point(0, 0);
        _settingsLayout.Name = "_settingsLayout";
        _settingsLayout.RowCount = 3;
        _settingsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        _settingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        _settingsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        _settingsLayout.Size = new Size(1327, 713);
        _settingsLayout.TabIndex = 0;
        // 
        // _settingsScrollPanel
        // 
        _settingsScrollPanel.AutoScroll = true;
        _settingsScrollPanel.BackColor = Color.FromArgb(245, 247, 250);
        _settingsScrollPanel.Controls.Add(_grpPresenceCtrl);
        _settingsScrollPanel.Controls.Add(_grpBreathCtrl);
        _settingsScrollPanel.Controls.Add(_grpHeartCtrl);
        _settingsScrollPanel.Controls.Add(_grpSleepCtrl);
        _settingsScrollPanel.Controls.Add(_grpSleepSettings);
        _settingsScrollPanel.Controls.Add(_grpSleepQuery);
        _settingsScrollPanel.Controls.Add(_grpSystemFunc);
        _settingsScrollPanel.Dock = DockStyle.Fill;
        _settingsScrollPanel.Location = new Point(3, 3);
        _settingsScrollPanel.Name = "_settingsScrollPanel";
        _settingsScrollPanel.Padding = new Padding(10);
        _settingsScrollPanel.Size = new Size(1321, 533);
        _settingsScrollPanel.TabIndex = 0;
        // 
        // _grpPresenceCtrl
        // 
        _grpPresenceCtrl.AutoSize = true;
        _grpPresenceCtrl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpPresenceCtrl.BackColor = Color.White;
        _grpPresenceCtrl.Controls.Add(_btnPresenceOn);
        _grpPresenceCtrl.Controls.Add(_btnPresenceOff);
        _grpPresenceCtrl.Controls.Add(_btnQueryPresence);
        _grpPresenceCtrl.Controls.Add(_btnQueryMotion);
        _grpPresenceCtrl.Controls.Add(_btnQueryDistance);
        _grpPresenceCtrl.Controls.Add(_btnQueryPosition);
        _grpPresenceCtrl.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpPresenceCtrl.ForeColor = Color.FromArgb(44, 62, 80);
        _grpPresenceCtrl.Location = new Point(10, 10);
        _grpPresenceCtrl.Margin = new Padding(0, 0, 6, 6);
        _grpPresenceCtrl.MinimumSize = new Size(260, 0);
        _grpPresenceCtrl.Name = "_grpPresenceCtrl";
        _grpPresenceCtrl.Size = new Size(303, 141);
        _grpPresenceCtrl.TabIndex = 10;
        _grpPresenceCtrl.TabStop = false;
        _grpPresenceCtrl.Text = "人体存在";
        // 
        // _btnPresenceOn
        // 
        _btnPresenceOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnPresenceOn.FlatAppearance.BorderSize = 0;
        _btnPresenceOn.FlatStyle = FlatStyle.Flat;
        _btnPresenceOn.Font = new Font("微软雅黑", 8F);
        _btnPresenceOn.ForeColor = Color.White;
        _btnPresenceOn.Location = new Point(12, 22);
        _btnPresenceOn.Margin = new Padding(2);
        _btnPresenceOn.Name = "_btnPresenceOn";
        _btnPresenceOn.Size = new Size(140, 30);
        _btnPresenceOn.TabIndex = 0;
        _btnPresenceOn.Text = "开启人体存在";
        _btnPresenceOn.UseVisualStyleBackColor = false;
        _btnPresenceOn.Click += _btnPresenceOn_Click;
        // 
        // _btnPresenceOff
        // 
        _btnPresenceOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnPresenceOff.FlatAppearance.BorderSize = 0;
        _btnPresenceOff.FlatStyle = FlatStyle.Flat;
        _btnPresenceOff.Font = new Font("微软雅黑", 8F);
        _btnPresenceOff.ForeColor = Color.White;
        _btnPresenceOff.Location = new Point(158, 22);
        _btnPresenceOff.Margin = new Padding(2);
        _btnPresenceOff.Name = "_btnPresenceOff";
        _btnPresenceOff.Size = new Size(140, 30);
        _btnPresenceOff.TabIndex = 1;
        _btnPresenceOff.Text = "关闭人体存在";
        _btnPresenceOff.UseVisualStyleBackColor = false;
        _btnPresenceOff.Click += _btnPresenceOff_Click;
        // 
        // _btnQueryPresence
        // 
        _btnQueryPresence.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryPresence.FlatAppearance.BorderSize = 0;
        _btnQueryPresence.FlatStyle = FlatStyle.Flat;
        _btnQueryPresence.Font = new Font("微软雅黑", 8F);
        _btnQueryPresence.ForeColor = Color.White;
        _btnQueryPresence.Location = new Point(12, 56);
        _btnQueryPresence.Margin = new Padding(2);
        _btnQueryPresence.Name = "_btnQueryPresence";
        _btnQueryPresence.Size = new Size(140, 30);
        _btnQueryPresence.TabIndex = 2;
        _btnQueryPresence.Text = "查询状态";
        _btnQueryPresence.UseVisualStyleBackColor = false;
        _btnQueryPresence.Click += _btnQueryPresence_Click;
        // 
        // _btnQueryMotion
        // 
        _btnQueryMotion.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryMotion.FlatAppearance.BorderSize = 0;
        _btnQueryMotion.FlatStyle = FlatStyle.Flat;
        _btnQueryMotion.Font = new Font("微软雅黑", 8F);
        _btnQueryMotion.ForeColor = Color.White;
        _btnQueryMotion.Location = new Point(158, 56);
        _btnQueryMotion.Margin = new Padding(2);
        _btnQueryMotion.Name = "_btnQueryMotion";
        _btnQueryMotion.Size = new Size(140, 30);
        _btnQueryMotion.TabIndex = 3;
        _btnQueryMotion.Text = "查询运动信息";
        _btnQueryMotion.UseVisualStyleBackColor = false;
        _btnQueryMotion.Click += _btnQueryMotion_Click;
        // 
        // _btnQueryDistance
        // 
        _btnQueryDistance.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryDistance.FlatAppearance.BorderSize = 0;
        _btnQueryDistance.FlatStyle = FlatStyle.Flat;
        _btnQueryDistance.Font = new Font("微软雅黑", 8F);
        _btnQueryDistance.ForeColor = Color.White;
        _btnQueryDistance.Location = new Point(12, 90);
        _btnQueryDistance.Margin = new Padding(2);
        _btnQueryDistance.Name = "_btnQueryDistance";
        _btnQueryDistance.Size = new Size(140, 30);
        _btnQueryDistance.TabIndex = 4;
        _btnQueryDistance.Text = "查询距离";
        _btnQueryDistance.UseVisualStyleBackColor = false;
        _btnQueryDistance.Click += _btnQueryDistance_Click;
        // 
        // _btnQueryPosition
        // 
        _btnQueryPosition.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryPosition.FlatAppearance.BorderSize = 0;
        _btnQueryPosition.FlatStyle = FlatStyle.Flat;
        _btnQueryPosition.Font = new Font("微软雅黑", 8F);
        _btnQueryPosition.ForeColor = Color.White;
        _btnQueryPosition.Location = new Point(158, 90);
        _btnQueryPosition.Margin = new Padding(2);
        _btnQueryPosition.Name = "_btnQueryPosition";
        _btnQueryPosition.Size = new Size(140, 30);
        _btnQueryPosition.TabIndex = 5;
        _btnQueryPosition.Text = "查询方位";
        _btnQueryPosition.UseVisualStyleBackColor = false;
        _btnQueryPosition.Click += _btnQueryPosition_Click;
        // 
        // _grpBreathCtrl
        // 
        _grpBreathCtrl.AutoSize = true;
        _grpBreathCtrl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpBreathCtrl.BackColor = Color.White;
        _grpBreathCtrl.Controls.Add(_btnBreathOn);
        _grpBreathCtrl.Controls.Add(_btnBreathOff);
        _grpBreathCtrl.Controls.Add(_btnBreathWaveOn);
        _grpBreathCtrl.Controls.Add(_btnBreathWaveOff);
        _grpBreathCtrl.Controls.Add(_btnQueryBreathValue);
        _grpBreathCtrl.Controls.Add(_btnQueryBreathState);
        _grpBreathCtrl.Controls.Add(_btnQueryBreathWaveSwitch);
        _grpBreathCtrl.Controls.Add(_btnQueryBreathSwitch);
        _grpBreathCtrl.Controls.Add(_btnSetLowBreath);
        _grpBreathCtrl.Controls.Add(_btnQueryLowBreath);
        _grpBreathCtrl.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpBreathCtrl.ForeColor = Color.FromArgb(44, 62, 80);
        _grpBreathCtrl.Location = new Point(319, 10);
        _grpBreathCtrl.Margin = new Padding(0, 0, 6, 6);
        _grpBreathCtrl.MinimumSize = new Size(260, 0);
        _grpBreathCtrl.Name = "_grpBreathCtrl";
        _grpBreathCtrl.Size = new Size(303, 209);
        _grpBreathCtrl.TabIndex = 11;
        _grpBreathCtrl.TabStop = false;
        _grpBreathCtrl.Text = "呼吸监测";
        // 
        // _btnBreathOn
        // 
        _btnBreathOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnBreathOn.FlatAppearance.BorderSize = 0;
        _btnBreathOn.FlatStyle = FlatStyle.Flat;
        _btnBreathOn.Font = new Font("微软雅黑", 8F);
        _btnBreathOn.ForeColor = Color.White;
        _btnBreathOn.Location = new Point(12, 22);
        _btnBreathOn.Margin = new Padding(2);
        _btnBreathOn.Name = "_btnBreathOn";
        _btnBreathOn.Size = new Size(140, 30);
        _btnBreathOn.TabIndex = 0;
        _btnBreathOn.Text = "开启呼吸监测";
        _btnBreathOn.UseVisualStyleBackColor = false;
        _btnBreathOn.Click += _btnBreathOn_Click;
        // 
        // _btnBreathOff
        // 
        _btnBreathOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnBreathOff.FlatAppearance.BorderSize = 0;
        _btnBreathOff.FlatStyle = FlatStyle.Flat;
        _btnBreathOff.Font = new Font("微软雅黑", 8F);
        _btnBreathOff.ForeColor = Color.White;
        _btnBreathOff.Location = new Point(158, 22);
        _btnBreathOff.Margin = new Padding(2);
        _btnBreathOff.Name = "_btnBreathOff";
        _btnBreathOff.Size = new Size(140, 30);
        _btnBreathOff.TabIndex = 1;
        _btnBreathOff.Text = "关闭呼吸监测";
        _btnBreathOff.UseVisualStyleBackColor = false;
        _btnBreathOff.Click += _btnBreathOff_Click;
        // 
        // _btnBreathWaveOn
        // 
        _btnBreathWaveOn.BackColor = Color.FromArgb(52, 152, 219);
        _btnBreathWaveOn.FlatAppearance.BorderSize = 0;
        _btnBreathWaveOn.FlatStyle = FlatStyle.Flat;
        _btnBreathWaveOn.Font = new Font("微软雅黑", 8F);
        _btnBreathWaveOn.ForeColor = Color.White;
        _btnBreathWaveOn.Location = new Point(12, 56);
        _btnBreathWaveOn.Margin = new Padding(2);
        _btnBreathWaveOn.Name = "_btnBreathWaveOn";
        _btnBreathWaveOn.Size = new Size(140, 30);
        _btnBreathWaveOn.TabIndex = 2;
        _btnBreathWaveOn.Text = "开启波形上报";
        _btnBreathWaveOn.UseVisualStyleBackColor = false;
        _btnBreathWaveOn.Click += _btnBreathWaveOn_Click;
        // 
        // _btnBreathWaveOff
        // 
        _btnBreathWaveOff.BackColor = Color.FromArgb(149, 165, 166);
        _btnBreathWaveOff.FlatAppearance.BorderSize = 0;
        _btnBreathWaveOff.FlatStyle = FlatStyle.Flat;
        _btnBreathWaveOff.Font = new Font("微软雅黑", 8F);
        _btnBreathWaveOff.ForeColor = Color.White;
        _btnBreathWaveOff.Location = new Point(158, 56);
        _btnBreathWaveOff.Margin = new Padding(2);
        _btnBreathWaveOff.Name = "_btnBreathWaveOff";
        _btnBreathWaveOff.Size = new Size(140, 30);
        _btnBreathWaveOff.TabIndex = 3;
        _btnBreathWaveOff.Text = "关闭波形上报";
        _btnBreathWaveOff.UseVisualStyleBackColor = false;
        _btnBreathWaveOff.Click += _btnBreathWaveOff_Click;
        // 
        // _btnQueryBreathValue
        // 
        _btnQueryBreathValue.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryBreathValue.FlatAppearance.BorderSize = 0;
        _btnQueryBreathValue.FlatStyle = FlatStyle.Flat;
        _btnQueryBreathValue.Font = new Font("微软雅黑", 8F);
        _btnQueryBreathValue.ForeColor = Color.White;
        _btnQueryBreathValue.Location = new Point(12, 90);
        _btnQueryBreathValue.Margin = new Padding(2);
        _btnQueryBreathValue.Name = "_btnQueryBreathValue";
        _btnQueryBreathValue.Size = new Size(140, 30);
        _btnQueryBreathValue.TabIndex = 4;
        _btnQueryBreathValue.Text = "查询呼吸值";
        _btnQueryBreathValue.UseVisualStyleBackColor = false;
        _btnQueryBreathValue.Click += _btnQueryBreathValue_Click;
        // 
        // _btnQueryBreathState
        // 
        _btnQueryBreathState.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryBreathState.FlatAppearance.BorderSize = 0;
        _btnQueryBreathState.FlatStyle = FlatStyle.Flat;
        _btnQueryBreathState.Font = new Font("微软雅黑", 8F);
        _btnQueryBreathState.ForeColor = Color.White;
        _btnQueryBreathState.Location = new Point(158, 90);
        _btnQueryBreathState.Margin = new Padding(2);
        _btnQueryBreathState.Name = "_btnQueryBreathState";
        _btnQueryBreathState.Size = new Size(140, 30);
        _btnQueryBreathState.TabIndex = 5;
        _btnQueryBreathState.Text = "查询呼吸状态";
        _btnQueryBreathState.UseVisualStyleBackColor = false;
        _btnQueryBreathState.Click += _btnQueryBreathState_Click;
        // 
        // _btnQueryBreathWaveSwitch
        // 
        _btnQueryBreathWaveSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryBreathWaveSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryBreathWaveSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryBreathWaveSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryBreathWaveSwitch.ForeColor = Color.White;
        _btnQueryBreathWaveSwitch.Location = new Point(12, 124);
        _btnQueryBreathWaveSwitch.Margin = new Padding(2);
        _btnQueryBreathWaveSwitch.Name = "_btnQueryBreathWaveSwitch";
        _btnQueryBreathWaveSwitch.Size = new Size(140, 30);
        _btnQueryBreathWaveSwitch.TabIndex = 6;
        _btnQueryBreathWaveSwitch.Text = "查询波形开关";
        _btnQueryBreathWaveSwitch.UseVisualStyleBackColor = false;
        _btnQueryBreathWaveSwitch.Click += _btnQueryBreathWaveSwitch_Click;
        // 
        // _btnQueryBreathSwitch
        // 
        _btnQueryBreathSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryBreathSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryBreathSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryBreathSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryBreathSwitch.ForeColor = Color.White;
        _btnQueryBreathSwitch.Location = new Point(158, 124);
        _btnQueryBreathSwitch.Margin = new Padding(2);
        _btnQueryBreathSwitch.Name = "_btnQueryBreathSwitch";
        _btnQueryBreathSwitch.Size = new Size(140, 30);
        _btnQueryBreathSwitch.TabIndex = 7;
        _btnQueryBreathSwitch.Text = "查询呼吸开关";
        _btnQueryBreathSwitch.UseVisualStyleBackColor = false;
        _btnQueryBreathSwitch.Click += _btnQueryBreathSwitch_Click;
        // 
        // _btnSetLowBreath
        // 
        _btnSetLowBreath.BackColor = Color.FromArgb(243, 156, 18);
        _btnSetLowBreath.FlatAppearance.BorderSize = 0;
        _btnSetLowBreath.FlatStyle = FlatStyle.Flat;
        _btnSetLowBreath.Font = new Font("微软雅黑", 8F);
        _btnSetLowBreath.ForeColor = Color.White;
        _btnSetLowBreath.Location = new Point(12, 158);
        _btnSetLowBreath.Margin = new Padding(2);
        _btnSetLowBreath.Name = "_btnSetLowBreath";
        _btnSetLowBreath.Size = new Size(140, 30);
        _btnSetLowBreath.TabIndex = 8;
        _btnSetLowBreath.Text = "设置低缓判读";
        _btnSetLowBreath.UseVisualStyleBackColor = false;
        _btnSetLowBreath.Click += _btnSetLowBreath_Click;
        // 
        // _btnQueryLowBreath
        // 
        _btnQueryLowBreath.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryLowBreath.FlatAppearance.BorderSize = 0;
        _btnQueryLowBreath.FlatStyle = FlatStyle.Flat;
        _btnQueryLowBreath.Font = new Font("微软雅黑", 8F);
        _btnQueryLowBreath.ForeColor = Color.White;
        _btnQueryLowBreath.Location = new Point(158, 158);
        _btnQueryLowBreath.Margin = new Padding(2);
        _btnQueryLowBreath.Name = "_btnQueryLowBreath";
        _btnQueryLowBreath.Size = new Size(140, 30);
        _btnQueryLowBreath.TabIndex = 9;
        _btnQueryLowBreath.Text = "查询低缓判读";
        _btnQueryLowBreath.UseVisualStyleBackColor = false;
        _btnQueryLowBreath.Click += _btnQueryLowBreath_Click;
        // 
        // _grpHeartCtrl
        // 
        _grpHeartCtrl.AutoSize = true;
        _grpHeartCtrl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpHeartCtrl.BackColor = Color.White;
        _grpHeartCtrl.Controls.Add(_btnHeartOn);
        _grpHeartCtrl.Controls.Add(_btnHeartOff);
        _grpHeartCtrl.Controls.Add(_btnHeartWaveOn);
        _grpHeartCtrl.Controls.Add(_btnHeartWaveOff);
        _grpHeartCtrl.Controls.Add(_btnQueryHeartValue);
        _grpHeartCtrl.Controls.Add(_btnQueryHeartSwitch);
        _grpHeartCtrl.Controls.Add(_btnQueryHeartWave);
        _grpHeartCtrl.Controls.Add(_btnQueryHeartWaveSwitch);
        _grpHeartCtrl.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpHeartCtrl.ForeColor = Color.FromArgb(44, 62, 80);
        _grpHeartCtrl.Location = new Point(628, 10);
        _grpHeartCtrl.Margin = new Padding(0, 0, 6, 6);
        _grpHeartCtrl.MinimumSize = new Size(260, 0);
        _grpHeartCtrl.Name = "_grpHeartCtrl";
        _grpHeartCtrl.Size = new Size(303, 175);
        _grpHeartCtrl.TabIndex = 12;
        _grpHeartCtrl.TabStop = false;
        _grpHeartCtrl.Text = "心率监测";
        // 
        // _btnHeartOn
        // 
        _btnHeartOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnHeartOn.FlatAppearance.BorderSize = 0;
        _btnHeartOn.FlatStyle = FlatStyle.Flat;
        _btnHeartOn.Font = new Font("微软雅黑", 8F);
        _btnHeartOn.ForeColor = Color.White;
        _btnHeartOn.Location = new Point(12, 22);
        _btnHeartOn.Margin = new Padding(2);
        _btnHeartOn.Name = "_btnHeartOn";
        _btnHeartOn.Size = new Size(140, 30);
        _btnHeartOn.TabIndex = 0;
        _btnHeartOn.Text = "开启心率监测";
        _btnHeartOn.UseVisualStyleBackColor = false;
        _btnHeartOn.Click += _btnHeartOn_Click;
        // 
        // _btnHeartOff
        // 
        _btnHeartOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnHeartOff.FlatAppearance.BorderSize = 0;
        _btnHeartOff.FlatStyle = FlatStyle.Flat;
        _btnHeartOff.Font = new Font("微软雅黑", 8F);
        _btnHeartOff.ForeColor = Color.White;
        _btnHeartOff.Location = new Point(158, 22);
        _btnHeartOff.Margin = new Padding(2);
        _btnHeartOff.Name = "_btnHeartOff";
        _btnHeartOff.Size = new Size(140, 30);
        _btnHeartOff.TabIndex = 1;
        _btnHeartOff.Text = "关闭心率监测";
        _btnHeartOff.UseVisualStyleBackColor = false;
        _btnHeartOff.Click += _btnHeartOff_Click;
        // 
        // _btnHeartWaveOn
        // 
        _btnHeartWaveOn.BackColor = Color.FromArgb(52, 152, 219);
        _btnHeartWaveOn.FlatAppearance.BorderSize = 0;
        _btnHeartWaveOn.FlatStyle = FlatStyle.Flat;
        _btnHeartWaveOn.Font = new Font("微软雅黑", 8F);
        _btnHeartWaveOn.ForeColor = Color.White;
        _btnHeartWaveOn.Location = new Point(12, 56);
        _btnHeartWaveOn.Margin = new Padding(2);
        _btnHeartWaveOn.Name = "_btnHeartWaveOn";
        _btnHeartWaveOn.Size = new Size(140, 30);
        _btnHeartWaveOn.TabIndex = 2;
        _btnHeartWaveOn.Text = "开启波形上报";
        _btnHeartWaveOn.UseVisualStyleBackColor = false;
        _btnHeartWaveOn.Click += _btnHeartWaveOn_Click;
        // 
        // _btnHeartWaveOff
        // 
        _btnHeartWaveOff.BackColor = Color.FromArgb(149, 165, 166);
        _btnHeartWaveOff.FlatAppearance.BorderSize = 0;
        _btnHeartWaveOff.FlatStyle = FlatStyle.Flat;
        _btnHeartWaveOff.Font = new Font("微软雅黑", 8F);
        _btnHeartWaveOff.ForeColor = Color.White;
        _btnHeartWaveOff.Location = new Point(158, 56);
        _btnHeartWaveOff.Margin = new Padding(2);
        _btnHeartWaveOff.Name = "_btnHeartWaveOff";
        _btnHeartWaveOff.Size = new Size(140, 30);
        _btnHeartWaveOff.TabIndex = 3;
        _btnHeartWaveOff.Text = "关闭波形上报";
        _btnHeartWaveOff.UseVisualStyleBackColor = false;
        _btnHeartWaveOff.Click += _btnHeartWaveOff_Click;
        // 
        // _btnQueryHeartValue
        // 
        _btnQueryHeartValue.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryHeartValue.FlatAppearance.BorderSize = 0;
        _btnQueryHeartValue.FlatStyle = FlatStyle.Flat;
        _btnQueryHeartValue.Font = new Font("微软雅黑", 8F);
        _btnQueryHeartValue.ForeColor = Color.White;
        _btnQueryHeartValue.Location = new Point(12, 90);
        _btnQueryHeartValue.Margin = new Padding(2);
        _btnQueryHeartValue.Name = "_btnQueryHeartValue";
        _btnQueryHeartValue.Size = new Size(140, 30);
        _btnQueryHeartValue.TabIndex = 4;
        _btnQueryHeartValue.Text = "查询心率值";
        _btnQueryHeartValue.UseVisualStyleBackColor = false;
        _btnQueryHeartValue.Click += _btnQueryHeartValue_Click;
        // 
        // _btnQueryHeartSwitch
        // 
        _btnQueryHeartSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryHeartSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryHeartSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryHeartSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryHeartSwitch.ForeColor = Color.White;
        _btnQueryHeartSwitch.Location = new Point(158, 90);
        _btnQueryHeartSwitch.Margin = new Padding(2);
        _btnQueryHeartSwitch.Name = "_btnQueryHeartSwitch";
        _btnQueryHeartSwitch.Size = new Size(140, 30);
        _btnQueryHeartSwitch.TabIndex = 5;
        _btnQueryHeartSwitch.Text = "查询心率开关";
        _btnQueryHeartSwitch.UseVisualStyleBackColor = false;
        _btnQueryHeartSwitch.Click += _btnQueryHeartSwitch_Click;
        // 
        // _btnQueryHeartWave
        // 
        _btnQueryHeartWave.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryHeartWave.FlatAppearance.BorderSize = 0;
        _btnQueryHeartWave.FlatStyle = FlatStyle.Flat;
        _btnQueryHeartWave.Font = new Font("微软雅黑", 8F);
        _btnQueryHeartWave.ForeColor = Color.White;
        _btnQueryHeartWave.Location = new Point(12, 124);
        _btnQueryHeartWave.Margin = new Padding(2);
        _btnQueryHeartWave.Name = "_btnQueryHeartWave";
        _btnQueryHeartWave.Size = new Size(140, 30);
        _btnQueryHeartWave.TabIndex = 6;
        _btnQueryHeartWave.Text = "查询心率波形";
        _btnQueryHeartWave.UseVisualStyleBackColor = false;
        _btnQueryHeartWave.Click += _btnQueryHeartWave_Click;
        // 
        // _btnQueryHeartWaveSwitch
        // 
        _btnQueryHeartWaveSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryHeartWaveSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryHeartWaveSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryHeartWaveSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryHeartWaveSwitch.ForeColor = Color.White;
        _btnQueryHeartWaveSwitch.Location = new Point(158, 124);
        _btnQueryHeartWaveSwitch.Margin = new Padding(2);
        _btnQueryHeartWaveSwitch.Name = "_btnQueryHeartWaveSwitch";
        _btnQueryHeartWaveSwitch.Size = new Size(140, 30);
        _btnQueryHeartWaveSwitch.TabIndex = 7;
        _btnQueryHeartWaveSwitch.Text = "查询波形开关";
        _btnQueryHeartWaveSwitch.UseVisualStyleBackColor = false;
        _btnQueryHeartWaveSwitch.Click += _btnQueryHeartWaveSwitch_Click;
        // 
        // _grpSleepCtrl
        // 
        _grpSleepCtrl.AutoSize = true;
        _grpSleepCtrl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpSleepCtrl.BackColor = Color.White;
        _grpSleepCtrl.Controls.Add(_btnSleepOn);
        _grpSleepCtrl.Controls.Add(_btnSleepOff);
        _grpSleepCtrl.Controls.Add(_btnQuerySleepComp);
        _grpSleepCtrl.Controls.Add(_btnQuerySleepAnalysis);
        _grpSleepCtrl.Controls.Add(_btnQuerySleepRating);
        _grpSleepCtrl.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpSleepCtrl.ForeColor = Color.FromArgb(44, 62, 80);
        _grpSleepCtrl.Location = new Point(937, 10);
        _grpSleepCtrl.Margin = new Padding(0, 0, 6, 6);
        _grpSleepCtrl.MinimumSize = new Size(260, 0);
        _grpSleepCtrl.Name = "_grpSleepCtrl";
        _grpSleepCtrl.Size = new Size(303, 141);
        _grpSleepCtrl.TabIndex = 13;
        _grpSleepCtrl.TabStop = false;
        _grpSleepCtrl.Text = "睡眠监测";
        // 
        // _btnSleepOn
        // 
        _btnSleepOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnSleepOn.FlatAppearance.BorderSize = 0;
        _btnSleepOn.FlatStyle = FlatStyle.Flat;
        _btnSleepOn.Font = new Font("微软雅黑", 8F);
        _btnSleepOn.ForeColor = Color.White;
        _btnSleepOn.Location = new Point(12, 22);
        _btnSleepOn.Margin = new Padding(2);
        _btnSleepOn.Name = "_btnSleepOn";
        _btnSleepOn.Size = new Size(140, 30);
        _btnSleepOn.TabIndex = 0;
        _btnSleepOn.Text = "开启睡眠监测";
        _btnSleepOn.UseVisualStyleBackColor = false;
        _btnSleepOn.Click += _btnSleepOn_Click;
        // 
        // _btnSleepOff
        // 
        _btnSleepOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnSleepOff.FlatAppearance.BorderSize = 0;
        _btnSleepOff.FlatStyle = FlatStyle.Flat;
        _btnSleepOff.Font = new Font("微软雅黑", 8F);
        _btnSleepOff.ForeColor = Color.White;
        _btnSleepOff.Location = new Point(158, 22);
        _btnSleepOff.Margin = new Padding(2);
        _btnSleepOff.Name = "_btnSleepOff";
        _btnSleepOff.Size = new Size(140, 30);
        _btnSleepOff.TabIndex = 1;
        _btnSleepOff.Text = "关闭睡眠监测";
        _btnSleepOff.UseVisualStyleBackColor = false;
        _btnSleepOff.Click += _btnSleepOff_Click;
        // 
        // _btnQuerySleepComp
        // 
        _btnQuerySleepComp.BackColor = Color.FromArgb(52, 152, 219);
        _btnQuerySleepComp.FlatAppearance.BorderSize = 0;
        _btnQuerySleepComp.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepComp.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepComp.ForeColor = Color.White;
        _btnQuerySleepComp.Location = new Point(12, 56);
        _btnQuerySleepComp.Margin = new Padding(2);
        _btnQuerySleepComp.Name = "_btnQuerySleepComp";
        _btnQuerySleepComp.Size = new Size(140, 30);
        _btnQuerySleepComp.TabIndex = 2;
        _btnQuerySleepComp.Text = "查询综合状态";
        _btnQuerySleepComp.UseVisualStyleBackColor = false;
        _btnQuerySleepComp.Click += _btnQuerySleepComp_Click;
        // 
        // _btnQuerySleepAnalysis
        // 
        _btnQuerySleepAnalysis.BackColor = Color.FromArgb(155, 89, 182);
        _btnQuerySleepAnalysis.FlatAppearance.BorderSize = 0;
        _btnQuerySleepAnalysis.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepAnalysis.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepAnalysis.ForeColor = Color.White;
        _btnQuerySleepAnalysis.Location = new Point(158, 56);
        _btnQuerySleepAnalysis.Margin = new Padding(2);
        _btnQuerySleepAnalysis.Name = "_btnQuerySleepAnalysis";
        _btnQuerySleepAnalysis.Size = new Size(140, 30);
        _btnQuerySleepAnalysis.TabIndex = 3;
        _btnQuerySleepAnalysis.Text = "查询睡眠分析";
        _btnQuerySleepAnalysis.UseVisualStyleBackColor = false;
        _btnQuerySleepAnalysis.Click += _btnQuerySleepAnalysis_Click;
        // 
        // _btnQuerySleepRating
        // 
        _btnQuerySleepRating.BackColor = Color.FromArgb(26, 188, 156);
        _btnQuerySleepRating.FlatAppearance.BorderSize = 0;
        _btnQuerySleepRating.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepRating.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepRating.ForeColor = Color.White;
        _btnQuerySleepRating.Location = new Point(12, 90);
        _btnQuerySleepRating.Margin = new Padding(2);
        _btnQuerySleepRating.Name = "_btnQuerySleepRating";
        _btnQuerySleepRating.Size = new Size(140, 30);
        _btnQuerySleepRating.TabIndex = 4;
        _btnQuerySleepRating.Text = "查询睡眠评级";
        _btnQuerySleepRating.UseVisualStyleBackColor = false;
        _btnQuerySleepRating.Click += _btnQuerySleepRating_Click;
        // 
        // _grpSleepSettings
        // 
        _grpSleepSettings.AutoSize = true;
        _grpSleepSettings.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpSleepSettings.BackColor = Color.White;
        _grpSleepSettings.Controls.Add(_btnStruggleSwitchOn);
        _grpSleepSettings.Controls.Add(_btnStruggleSwitchOff);
        _grpSleepSettings.Controls.Add(_btnNoPersonSwitchOn);
        _grpSleepSettings.Controls.Add(_btnNoPersonSwitchOff);
        _grpSleepSettings.Controls.Add(_btnSetNoPersonDuration);
        _grpSleepSettings.Controls.Add(_btnExtCtrlSwitchOn);
        _grpSleepSettings.Controls.Add(_btnExtCtrlSwitchOff);
        _grpSleepSettings.Controls.Add(_btnSleepPeriodStart);
        _grpSleepSettings.Controls.Add(_btnSleepPeriodEnd);
        _grpSleepSettings.Controls.Add(_btnSetSleepDeadline);
        _grpSleepSettings.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpSleepSettings.ForeColor = Color.FromArgb(44, 62, 80);
        _grpSleepSettings.Location = new Point(10, 225);
        _grpSleepSettings.Margin = new Padding(0, 0, 6, 6);
        _grpSleepSettings.MinimumSize = new Size(260, 0);
        _grpSleepSettings.Name = "_grpSleepSettings";
        _grpSleepSettings.Size = new Size(303, 209);
        _grpSleepSettings.TabIndex = 14;
        _grpSleepSettings.TabStop = false;
        _grpSleepSettings.Text = "睡眠参数设置";
        // 
        // _btnStruggleSwitchOn
        // 
        _btnStruggleSwitchOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnStruggleSwitchOn.FlatAppearance.BorderSize = 0;
        _btnStruggleSwitchOn.FlatStyle = FlatStyle.Flat;
        _btnStruggleSwitchOn.Font = new Font("微软雅黑", 8F);
        _btnStruggleSwitchOn.ForeColor = Color.White;
        _btnStruggleSwitchOn.Location = new Point(12, 22);
        _btnStruggleSwitchOn.Margin = new Padding(2);
        _btnStruggleSwitchOn.Name = "_btnStruggleSwitchOn";
        _btnStruggleSwitchOn.Size = new Size(140, 30);
        _btnStruggleSwitchOn.TabIndex = 0;
        _btnStruggleSwitchOn.Text = "开启挣扎检测";
        _btnStruggleSwitchOn.UseVisualStyleBackColor = false;
        _btnStruggleSwitchOn.Click += _btnStruggleSwitchOn_Click;
        // 
        // _btnStruggleSwitchOff
        // 
        _btnStruggleSwitchOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnStruggleSwitchOff.FlatAppearance.BorderSize = 0;
        _btnStruggleSwitchOff.FlatStyle = FlatStyle.Flat;
        _btnStruggleSwitchOff.Font = new Font("微软雅黑", 8F);
        _btnStruggleSwitchOff.ForeColor = Color.White;
        _btnStruggleSwitchOff.Location = new Point(158, 22);
        _btnStruggleSwitchOff.Margin = new Padding(2);
        _btnStruggleSwitchOff.Name = "_btnStruggleSwitchOff";
        _btnStruggleSwitchOff.Size = new Size(140, 30);
        _btnStruggleSwitchOff.TabIndex = 1;
        _btnStruggleSwitchOff.Text = "关闭挣扎检测";
        _btnStruggleSwitchOff.UseVisualStyleBackColor = false;
        _btnStruggleSwitchOff.Click += _btnStruggleSwitchOff_Click;
        // 
        // _btnNoPersonSwitchOn
        // 
        _btnNoPersonSwitchOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnNoPersonSwitchOn.FlatAppearance.BorderSize = 0;
        _btnNoPersonSwitchOn.FlatStyle = FlatStyle.Flat;
        _btnNoPersonSwitchOn.Font = new Font("微软雅黑", 8F);
        _btnNoPersonSwitchOn.ForeColor = Color.White;
        _btnNoPersonSwitchOn.Location = new Point(12, 56);
        _btnNoPersonSwitchOn.Margin = new Padding(2);
        _btnNoPersonSwitchOn.Name = "_btnNoPersonSwitchOn";
        _btnNoPersonSwitchOn.Size = new Size(140, 30);
        _btnNoPersonSwitchOn.TabIndex = 2;
        _btnNoPersonSwitchOn.Text = "开启无人计时";
        _btnNoPersonSwitchOn.UseVisualStyleBackColor = false;
        _btnNoPersonSwitchOn.Click += _btnNoPersonSwitchOn_Click;
        // 
        // _btnNoPersonSwitchOff
        // 
        _btnNoPersonSwitchOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnNoPersonSwitchOff.FlatAppearance.BorderSize = 0;
        _btnNoPersonSwitchOff.FlatStyle = FlatStyle.Flat;
        _btnNoPersonSwitchOff.Font = new Font("微软雅黑", 8F);
        _btnNoPersonSwitchOff.ForeColor = Color.White;
        _btnNoPersonSwitchOff.Location = new Point(158, 56);
        _btnNoPersonSwitchOff.Margin = new Padding(2);
        _btnNoPersonSwitchOff.Name = "_btnNoPersonSwitchOff";
        _btnNoPersonSwitchOff.Size = new Size(140, 30);
        _btnNoPersonSwitchOff.TabIndex = 3;
        _btnNoPersonSwitchOff.Text = "关闭无人计时";
        _btnNoPersonSwitchOff.UseVisualStyleBackColor = false;
        _btnNoPersonSwitchOff.Click += _btnNoPersonSwitchOff_Click;
        // 
        // _btnSetNoPersonDuration
        // 
        _btnSetNoPersonDuration.BackColor = Color.FromArgb(243, 156, 18);
        _btnSetNoPersonDuration.FlatAppearance.BorderSize = 0;
        _btnSetNoPersonDuration.FlatStyle = FlatStyle.Flat;
        _btnSetNoPersonDuration.Font = new Font("微软雅黑", 8F);
        _btnSetNoPersonDuration.ForeColor = Color.White;
        _btnSetNoPersonDuration.Location = new Point(12, 90);
        _btnSetNoPersonDuration.Margin = new Padding(2);
        _btnSetNoPersonDuration.Name = "_btnSetNoPersonDuration";
        _btnSetNoPersonDuration.Size = new Size(140, 30);
        _btnSetNoPersonDuration.TabIndex = 4;
        _btnSetNoPersonDuration.Text = "设置无人时长";
        _btnSetNoPersonDuration.UseVisualStyleBackColor = false;
        _btnSetNoPersonDuration.Click += _btnSetNoPersonDuration_Click;
        // 
        // _btnExtCtrlSwitchOn
        // 
        _btnExtCtrlSwitchOn.BackColor = Color.FromArgb(46, 204, 113);
        _btnExtCtrlSwitchOn.FlatAppearance.BorderSize = 0;
        _btnExtCtrlSwitchOn.FlatStyle = FlatStyle.Flat;
        _btnExtCtrlSwitchOn.Font = new Font("微软雅黑", 8F);
        _btnExtCtrlSwitchOn.ForeColor = Color.White;
        _btnExtCtrlSwitchOn.Location = new Point(158, 90);
        _btnExtCtrlSwitchOn.Margin = new Padding(2);
        _btnExtCtrlSwitchOn.Name = "_btnExtCtrlSwitchOn";
        _btnExtCtrlSwitchOn.Size = new Size(140, 30);
        _btnExtCtrlSwitchOn.TabIndex = 5;
        _btnExtCtrlSwitchOn.Text = "开启外部控制";
        _btnExtCtrlSwitchOn.UseVisualStyleBackColor = false;
        _btnExtCtrlSwitchOn.Click += _btnExtCtrlSwitchOn_Click;
        // 
        // _btnExtCtrlSwitchOff
        // 
        _btnExtCtrlSwitchOff.BackColor = Color.FromArgb(231, 76, 60);
        _btnExtCtrlSwitchOff.FlatAppearance.BorderSize = 0;
        _btnExtCtrlSwitchOff.FlatStyle = FlatStyle.Flat;
        _btnExtCtrlSwitchOff.Font = new Font("微软雅黑", 8F);
        _btnExtCtrlSwitchOff.ForeColor = Color.White;
        _btnExtCtrlSwitchOff.Location = new Point(12, 124);
        _btnExtCtrlSwitchOff.Margin = new Padding(2);
        _btnExtCtrlSwitchOff.Name = "_btnExtCtrlSwitchOff";
        _btnExtCtrlSwitchOff.Size = new Size(140, 30);
        _btnExtCtrlSwitchOff.TabIndex = 6;
        _btnExtCtrlSwitchOff.Text = "关闭外部控制";
        _btnExtCtrlSwitchOff.UseVisualStyleBackColor = false;
        _btnExtCtrlSwitchOff.Click += _btnExtCtrlSwitchOff_Click;
        // 
        // _btnSleepPeriodStart
        // 
        _btnSleepPeriodStart.BackColor = Color.FromArgb(46, 204, 113);
        _btnSleepPeriodStart.FlatAppearance.BorderSize = 0;
        _btnSleepPeriodStart.FlatStyle = FlatStyle.Flat;
        _btnSleepPeriodStart.Font = new Font("微软雅黑", 8F);
        _btnSleepPeriodStart.ForeColor = Color.White;
        _btnSleepPeriodStart.Location = new Point(158, 124);
        _btnSleepPeriodStart.Margin = new Padding(2);
        _btnSleepPeriodStart.Name = "_btnSleepPeriodStart";
        _btnSleepPeriodStart.Size = new Size(140, 30);
        _btnSleepPeriodStart.TabIndex = 7;
        _btnSleepPeriodStart.Text = "睡眠周期开始";
        _btnSleepPeriodStart.UseVisualStyleBackColor = false;
        _btnSleepPeriodStart.Click += _btnSleepPeriodStart_Click;
        // 
        // _btnSleepPeriodEnd
        // 
        _btnSleepPeriodEnd.BackColor = Color.FromArgb(231, 76, 60);
        _btnSleepPeriodEnd.FlatAppearance.BorderSize = 0;
        _btnSleepPeriodEnd.FlatStyle = FlatStyle.Flat;
        _btnSleepPeriodEnd.Font = new Font("微软雅黑", 8F);
        _btnSleepPeriodEnd.ForeColor = Color.White;
        _btnSleepPeriodEnd.Location = new Point(12, 158);
        _btnSleepPeriodEnd.Margin = new Padding(2);
        _btnSleepPeriodEnd.Name = "_btnSleepPeriodEnd";
        _btnSleepPeriodEnd.Size = new Size(140, 30);
        _btnSleepPeriodEnd.TabIndex = 8;
        _btnSleepPeriodEnd.Text = "睡眠周期结束";
        _btnSleepPeriodEnd.UseVisualStyleBackColor = false;
        _btnSleepPeriodEnd.Click += _btnSleepPeriodEnd_Click;
        // 
        // _btnSetSleepDeadline
        // 
        _btnSetSleepDeadline.BackColor = Color.FromArgb(243, 156, 18);
        _btnSetSleepDeadline.FlatAppearance.BorderSize = 0;
        _btnSetSleepDeadline.FlatStyle = FlatStyle.Flat;
        _btnSetSleepDeadline.Font = new Font("微软雅黑", 8F);
        _btnSetSleepDeadline.ForeColor = Color.White;
        _btnSetSleepDeadline.Location = new Point(158, 158);
        _btnSetSleepDeadline.Margin = new Padding(2);
        _btnSetSleepDeadline.Name = "_btnSetSleepDeadline";
        _btnSetSleepDeadline.Size = new Size(140, 30);
        _btnSetSleepDeadline.TabIndex = 9;
        _btnSetSleepDeadline.Text = "设置睡眠截止";
        _btnSetSleepDeadline.UseVisualStyleBackColor = false;
        _btnSetSleepDeadline.Click += _btnSetSleepDeadline_Click;
        // 
        // _grpSleepQuery
        // 
        _grpSleepQuery.AutoSize = true;
        _grpSleepQuery.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpSleepQuery.BackColor = Color.White;
        _grpSleepQuery.Controls.Add(_btnQueryBedState);
        _grpSleepQuery.Controls.Add(_btnQuerySleepState);
        _grpSleepQuery.Controls.Add(_btnQueryAwakeDur);
        _grpSleepQuery.Controls.Add(_btnQueryLightDur);
        _grpSleepQuery.Controls.Add(_btnQueryDeepDur);
        _grpSleepQuery.Controls.Add(_btnQuerySleepScore);
        _grpSleepQuery.Controls.Add(_btnQuerySleepAbnormal);
        _grpSleepQuery.Controls.Add(_btnQuerySleepStats);
        _grpSleepQuery.Controls.Add(_btnQueryStruggleSwitch);
        _grpSleepQuery.Controls.Add(_btnQueryNoPersonSwitch);
        _grpSleepQuery.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpSleepQuery.ForeColor = Color.FromArgb(44, 62, 80);
        _grpSleepQuery.Location = new Point(319, 225);
        _grpSleepQuery.Margin = new Padding(0, 0, 6, 6);
        _grpSleepQuery.MinimumSize = new Size(260, 0);
        _grpSleepQuery.Name = "_grpSleepQuery";
        _grpSleepQuery.Size = new Size(303, 209);
        _grpSleepQuery.TabIndex = 15;
        _grpSleepQuery.TabStop = false;
        _grpSleepQuery.Text = "睡眠参数查询";
        // 
        // _btnQueryBedState
        // 
        _btnQueryBedState.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryBedState.FlatAppearance.BorderSize = 0;
        _btnQueryBedState.FlatStyle = FlatStyle.Flat;
        _btnQueryBedState.Font = new Font("微软雅黑", 8F);
        _btnQueryBedState.ForeColor = Color.White;
        _btnQueryBedState.Location = new Point(12, 22);
        _btnQueryBedState.Margin = new Padding(2);
        _btnQueryBedState.Name = "_btnQueryBedState";
        _btnQueryBedState.Size = new Size(140, 30);
        _btnQueryBedState.TabIndex = 0;
        _btnQueryBedState.Text = "查询入离床";
        _btnQueryBedState.UseVisualStyleBackColor = false;
        _btnQueryBedState.Click += _btnQueryBedState_Click;
        // 
        // _btnQuerySleepState
        // 
        _btnQuerySleepState.BackColor = Color.FromArgb(52, 152, 219);
        _btnQuerySleepState.FlatAppearance.BorderSize = 0;
        _btnQuerySleepState.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepState.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepState.ForeColor = Color.White;
        _btnQuerySleepState.Location = new Point(158, 22);
        _btnQuerySleepState.Margin = new Padding(2);
        _btnQuerySleepState.Name = "_btnQuerySleepState";
        _btnQuerySleepState.Size = new Size(140, 30);
        _btnQuerySleepState.TabIndex = 1;
        _btnQuerySleepState.Text = "查询睡眠状态";
        _btnQuerySleepState.UseVisualStyleBackColor = false;
        _btnQuerySleepState.Click += _btnQuerySleepState_Click;
        // 
        // _btnQueryAwakeDur
        // 
        _btnQueryAwakeDur.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryAwakeDur.FlatAppearance.BorderSize = 0;
        _btnQueryAwakeDur.FlatStyle = FlatStyle.Flat;
        _btnQueryAwakeDur.Font = new Font("微软雅黑", 8F);
        _btnQueryAwakeDur.ForeColor = Color.White;
        _btnQueryAwakeDur.Location = new Point(12, 56);
        _btnQueryAwakeDur.Margin = new Padding(2);
        _btnQueryAwakeDur.Name = "_btnQueryAwakeDur";
        _btnQueryAwakeDur.Size = new Size(140, 30);
        _btnQueryAwakeDur.TabIndex = 2;
        _btnQueryAwakeDur.Text = "查询清醒时长";
        _btnQueryAwakeDur.UseVisualStyleBackColor = false;
        _btnQueryAwakeDur.Click += _btnQueryAwakeDur_Click;
        // 
        // _btnQueryLightDur
        // 
        _btnQueryLightDur.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryLightDur.FlatAppearance.BorderSize = 0;
        _btnQueryLightDur.FlatStyle = FlatStyle.Flat;
        _btnQueryLightDur.Font = new Font("微软雅黑", 8F);
        _btnQueryLightDur.ForeColor = Color.White;
        _btnQueryLightDur.Location = new Point(158, 56);
        _btnQueryLightDur.Margin = new Padding(2);
        _btnQueryLightDur.Name = "_btnQueryLightDur";
        _btnQueryLightDur.Size = new Size(140, 30);
        _btnQueryLightDur.TabIndex = 3;
        _btnQueryLightDur.Text = "查询浅睡时长";
        _btnQueryLightDur.UseVisualStyleBackColor = false;
        _btnQueryLightDur.Click += _btnQueryLightDur_Click;
        // 
        // _btnQueryDeepDur
        // 
        _btnQueryDeepDur.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryDeepDur.FlatAppearance.BorderSize = 0;
        _btnQueryDeepDur.FlatStyle = FlatStyle.Flat;
        _btnQueryDeepDur.Font = new Font("微软雅黑", 8F);
        _btnQueryDeepDur.ForeColor = Color.White;
        _btnQueryDeepDur.Location = new Point(12, 90);
        _btnQueryDeepDur.Margin = new Padding(2);
        _btnQueryDeepDur.Name = "_btnQueryDeepDur";
        _btnQueryDeepDur.Size = new Size(140, 30);
        _btnQueryDeepDur.TabIndex = 4;
        _btnQueryDeepDur.Text = "查询深睡时长";
        _btnQueryDeepDur.UseVisualStyleBackColor = false;
        _btnQueryDeepDur.Click += _btnQueryDeepDur_Click;
        // 
        // _btnQuerySleepScore
        // 
        _btnQuerySleepScore.BackColor = Color.FromArgb(52, 152, 219);
        _btnQuerySleepScore.FlatAppearance.BorderSize = 0;
        _btnQuerySleepScore.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepScore.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepScore.ForeColor = Color.White;
        _btnQuerySleepScore.Location = new Point(158, 90);
        _btnQuerySleepScore.Margin = new Padding(2);
        _btnQuerySleepScore.Name = "_btnQuerySleepScore";
        _btnQuerySleepScore.Size = new Size(140, 30);
        _btnQuerySleepScore.TabIndex = 5;
        _btnQuerySleepScore.Text = "查询睡眠评分";
        _btnQuerySleepScore.UseVisualStyleBackColor = false;
        _btnQuerySleepScore.Click += _btnQuerySleepScore_Click;
        // 
        // _btnQuerySleepAbnormal
        // 
        _btnQuerySleepAbnormal.BackColor = Color.FromArgb(52, 152, 219);
        _btnQuerySleepAbnormal.FlatAppearance.BorderSize = 0;
        _btnQuerySleepAbnormal.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepAbnormal.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepAbnormal.ForeColor = Color.White;
        _btnQuerySleepAbnormal.Location = new Point(12, 124);
        _btnQuerySleepAbnormal.Margin = new Padding(2);
        _btnQuerySleepAbnormal.Name = "_btnQuerySleepAbnormal";
        _btnQuerySleepAbnormal.Size = new Size(140, 30);
        _btnQuerySleepAbnormal.TabIndex = 6;
        _btnQuerySleepAbnormal.Text = "查询睡眠异常";
        _btnQuerySleepAbnormal.UseVisualStyleBackColor = false;
        _btnQuerySleepAbnormal.Click += _btnQuerySleepAbnormal_Click;
        // 
        // _btnQuerySleepStats
        // 
        _btnQuerySleepStats.BackColor = Color.FromArgb(52, 152, 219);
        _btnQuerySleepStats.FlatAppearance.BorderSize = 0;
        _btnQuerySleepStats.FlatStyle = FlatStyle.Flat;
        _btnQuerySleepStats.Font = new Font("微软雅黑", 8F);
        _btnQuerySleepStats.ForeColor = Color.White;
        _btnQuerySleepStats.Location = new Point(158, 124);
        _btnQuerySleepStats.Margin = new Padding(2);
        _btnQuerySleepStats.Name = "_btnQuerySleepStats";
        _btnQuerySleepStats.Size = new Size(140, 30);
        _btnQuerySleepStats.TabIndex = 7;
        _btnQuerySleepStats.Text = "查询睡眠统计";
        _btnQuerySleepStats.UseVisualStyleBackColor = false;
        _btnQuerySleepStats.Click += _btnQuerySleepStats_Click;
        // 
        // _btnQueryStruggleSwitch
        // 
        _btnQueryStruggleSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryStruggleSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryStruggleSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryStruggleSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryStruggleSwitch.ForeColor = Color.White;
        _btnQueryStruggleSwitch.Location = new Point(12, 158);
        _btnQueryStruggleSwitch.Margin = new Padding(2);
        _btnQueryStruggleSwitch.Name = "_btnQueryStruggleSwitch";
        _btnQueryStruggleSwitch.Size = new Size(140, 30);
        _btnQueryStruggleSwitch.TabIndex = 8;
        _btnQueryStruggleSwitch.Text = "查询挣扎开关";
        _btnQueryStruggleSwitch.UseVisualStyleBackColor = false;
        _btnQueryStruggleSwitch.Click += _btnQueryStruggleSwitch_Click;
        // 
        // _btnQueryNoPersonSwitch
        // 
        _btnQueryNoPersonSwitch.BackColor = Color.FromArgb(52, 152, 219);
        _btnQueryNoPersonSwitch.FlatAppearance.BorderSize = 0;
        _btnQueryNoPersonSwitch.FlatStyle = FlatStyle.Flat;
        _btnQueryNoPersonSwitch.Font = new Font("微软雅黑", 8F);
        _btnQueryNoPersonSwitch.ForeColor = Color.White;
        _btnQueryNoPersonSwitch.Location = new Point(158, 158);
        _btnQueryNoPersonSwitch.Margin = new Padding(2);
        _btnQueryNoPersonSwitch.Name = "_btnQueryNoPersonSwitch";
        _btnQueryNoPersonSwitch.Size = new Size(140, 30);
        _btnQueryNoPersonSwitch.TabIndex = 9;
        _btnQueryNoPersonSwitch.Text = "查询无人开关";
        _btnQueryNoPersonSwitch.UseVisualStyleBackColor = false;
        _btnQueryNoPersonSwitch.Click += _btnQueryNoPersonSwitch_Click;
        // 
        // _grpSystemFunc
        // 
        _grpSystemFunc.AutoSize = true;
        _grpSystemFunc.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        _grpSystemFunc.BackColor = Color.White;
        _grpSystemFunc.Controls.Add(_btnHeartbeatQuery);
        _grpSystemFunc.Controls.Add(_btnInitQuery);
        _grpSystemFunc.Controls.Add(_btnBoundaryQuery);
        _grpSystemFunc.Controls.Add(_btnModuleReset);
        _grpSystemFunc.Controls.Add(_btnQueryDev);
        _grpSystemFunc.Font = new Font("微软雅黑", 9F, FontStyle.Bold);
        _grpSystemFunc.ForeColor = Color.FromArgb(44, 62, 80);
        _grpSystemFunc.Location = new Point(628, 225);
        _grpSystemFunc.Margin = new Padding(0, 0, 6, 6);
        _grpSystemFunc.MinimumSize = new Size(260, 0);
        _grpSystemFunc.Name = "_grpSystemFunc";
        _grpSystemFunc.Size = new Size(303, 141);
        _grpSystemFunc.TabIndex = 16;
        _grpSystemFunc.TabStop = false;
        _grpSystemFunc.Text = "系统功能";
        // 
        // _btnHeartbeatQuery
        // 
        _btnHeartbeatQuery.BackColor = Color.FromArgb(52, 152, 219);
        _btnHeartbeatQuery.FlatAppearance.BorderSize = 0;
        _btnHeartbeatQuery.FlatStyle = FlatStyle.Flat;
        _btnHeartbeatQuery.Font = new Font("微软雅黑", 8F);
        _btnHeartbeatQuery.ForeColor = Color.White;
        _btnHeartbeatQuery.Location = new Point(12, 22);
        _btnHeartbeatQuery.Margin = new Padding(2);
        _btnHeartbeatQuery.Name = "_btnHeartbeatQuery";
        _btnHeartbeatQuery.Size = new Size(140, 30);
        _btnHeartbeatQuery.TabIndex = 0;
        _btnHeartbeatQuery.Text = "心跳包查询";
        _btnHeartbeatQuery.UseVisualStyleBackColor = false;
        _btnHeartbeatQuery.Click += _btnHeartbeatQuery_Click;
        // 
        // _btnInitQuery
        // 
        _btnInitQuery.BackColor = Color.FromArgb(52, 152, 219);
        _btnInitQuery.FlatAppearance.BorderSize = 0;
        _btnInitQuery.FlatStyle = FlatStyle.Flat;
        _btnInitQuery.Font = new Font("微软雅黑", 8F);
        _btnInitQuery.ForeColor = Color.White;
        _btnInitQuery.Location = new Point(158, 22);
        _btnInitQuery.Margin = new Padding(2);
        _btnInitQuery.Name = "_btnInitQuery";
        _btnInitQuery.Size = new Size(140, 30);
        _btnInitQuery.TabIndex = 1;
        _btnInitQuery.Text = "初始化查询";
        _btnInitQuery.UseVisualStyleBackColor = false;
        _btnInitQuery.Click += _btnInitQuery_Click;
        // 
        // _btnBoundaryQuery
        // 
        _btnBoundaryQuery.BackColor = Color.FromArgb(52, 152, 219);
        _btnBoundaryQuery.FlatAppearance.BorderSize = 0;
        _btnBoundaryQuery.FlatStyle = FlatStyle.Flat;
        _btnBoundaryQuery.Font = new Font("微软雅黑", 8F);
        _btnBoundaryQuery.ForeColor = Color.White;
        _btnBoundaryQuery.Location = new Point(12, 56);
        _btnBoundaryQuery.Margin = new Padding(2);
        _btnBoundaryQuery.Name = "_btnBoundaryQuery";
        _btnBoundaryQuery.Size = new Size(140, 30);
        _btnBoundaryQuery.TabIndex = 2;
        _btnBoundaryQuery.Text = "查询越界状态";
        _btnBoundaryQuery.UseVisualStyleBackColor = false;
        _btnBoundaryQuery.Click += _btnBoundaryQuery_Click;
        // 
        // _btnModuleReset
        // 
        _btnModuleReset.BackColor = Color.FromArgb(231, 76, 60);
        _btnModuleReset.FlatAppearance.BorderSize = 0;
        _btnModuleReset.FlatStyle = FlatStyle.Flat;
        _btnModuleReset.Font = new Font("微软雅黑", 8F);
        _btnModuleReset.ForeColor = Color.White;
        _btnModuleReset.Location = new Point(158, 56);
        _btnModuleReset.Margin = new Padding(2);
        _btnModuleReset.Name = "_btnModuleReset";
        _btnModuleReset.Size = new Size(140, 30);
        _btnModuleReset.TabIndex = 3;
        _btnModuleReset.Text = "模组复位";
        _btnModuleReset.UseVisualStyleBackColor = false;
        _btnModuleReset.Click += _btnModuleReset_Click;
        // 
        // _btnQueryDev
        // 
        _btnQueryDev.BackColor = Color.FromArgb(243, 156, 18);
        _btnQueryDev.FlatAppearance.BorderSize = 0;
        _btnQueryDev.FlatStyle = FlatStyle.Flat;
        _btnQueryDev.Font = new Font("微软雅黑", 8F);
        _btnQueryDev.ForeColor = Color.White;
        _btnQueryDev.Location = new Point(12, 90);
        _btnQueryDev.Margin = new Padding(2);
        _btnQueryDev.Name = "_btnQueryDev";
        _btnQueryDev.Size = new Size(140, 30);
        _btnQueryDev.TabIndex = 4;
        _btnQueryDev.Text = "查询设备信息";
        _btnQueryDev.UseVisualStyleBackColor = false;
        _btnQueryDev.Click += _btnQueryDev_Click;
        // 
        // _settingsResultToolbar
        // 
        _settingsResultToolbar.BackColor = Color.FromArgb(40, 40, 40);
        _settingsResultToolbar.Controls.Add(_lblSettingsResult);
        _settingsResultToolbar.Controls.Add(_settingsRuleLabel);
        _settingsResultToolbar.Controls.Add(_btnClearResult);
        _settingsResultToolbar.Dock = DockStyle.Fill;
        _settingsResultToolbar.Location = new Point(0, 539);
        _settingsResultToolbar.Margin = new Padding(0);
        _settingsResultToolbar.Name = "_settingsResultToolbar";
        _settingsResultToolbar.Size = new Size(1327, 44);
        _settingsResultToolbar.TabIndex = 1;
        // 
        // _lblSettingsResult
        // 
        _lblSettingsResult.AutoSize = true;
        _lblSettingsResult.Font = new Font("Consolas", 8F, FontStyle.Bold);
        _lblSettingsResult.ForeColor = Color.FromArgb(0, 200, 0);
        _lblSettingsResult.Location = new Point(8, 4);
        _lblSettingsResult.Name = "_lblSettingsResult";
        _lblSettingsResult.Size = new Size(59, 13);
        _lblSettingsResult.TabIndex = 0;
        _lblSettingsResult.Text = "返回结果";
        // 
        // _settingsRuleLabel
        // 
        _settingsRuleLabel.AutoSize = true;
        _settingsRuleLabel.Font = new Font("微软雅黑", 7.5F);
        _settingsRuleLabel.ForeColor = Color.FromArgb(127, 140, 141);
        _settingsRuleLabel.Location = new Point(8, 22);
        _settingsRuleLabel.Name = "_settingsRuleLabel";
        _settingsRuleLabel.Size = new Size(268, 16);
        _settingsRuleLabel.TabIndex = 2;
        _settingsRuleLabel.Text = "单击按钮显示返回结果 | 约10秒无操作后自动恢复动态更新";
        // 
        // _btnClearResult
        // 
        _btnClearResult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnClearResult.BackColor = Color.FromArgb(60, 60, 60);
        _btnClearResult.FlatAppearance.BorderSize = 0;
        _btnClearResult.FlatStyle = FlatStyle.Flat;
        _btnClearResult.Font = new Font("微软雅黑", 7F);
        _btnClearResult.ForeColor = Color.FromArgb(200, 200, 200);
        _btnClearResult.Location = new Point(1271, 2);
        _btnClearResult.Name = "_btnClearResult";
        _btnClearResult.Size = new Size(50, 24);
        _btnClearResult.TabIndex = 1;
        _btnClearResult.Text = "清空";
        _btnClearResult.UseVisualStyleBackColor = false;
        _btnClearResult.Click += _btnClearResult_Click;
        // 
        // _settingsResultBox
        // 
        _settingsResultBox.BackColor = Color.FromArgb(30, 30, 30);
        _settingsResultBox.BorderStyle = BorderStyle.None;
        _settingsResultBox.Dock = DockStyle.Fill;
        _settingsResultBox.Font = new Font("Consolas", 9F);
        _settingsResultBox.ForeColor = Color.FromArgb(0, 255, 0);
        _settingsResultBox.Location = new Point(3, 586);
        _settingsResultBox.Name = "_settingsResultBox";
        _settingsResultBox.ReadOnly = true;
        _settingsResultBox.Size = new Size(1321, 124);
        _settingsResultBox.TabIndex = 0;
        _settingsResultBox.Text = "";
        _settingsResultBox.WordWrap = false;
        _settingsResultBox.TextChanged += OnSettingsResultBoxTextChanged;
        // 
        // _tabProtocol
        // 
        _tabProtocol.BackColor = Color.FromArgb(245, 247, 250);
        _tabProtocol.Controls.Add(_protocolTextBox);
        _tabProtocol.Location = new Point(4, 40);
        _tabProtocol.Name = "_tabProtocol";
        _tabProtocol.Size = new Size(1327, 713);
        _tabProtocol.TabIndex = 2;
        _tabProtocol.Text = "通讯协议";
        // 
        // _protocolTextBox
        // 
        _protocolTextBox.BackColor = Color.FromArgb(30, 30, 30);
        _protocolTextBox.BorderStyle = BorderStyle.None;
        _protocolTextBox.Dock = DockStyle.Fill;
        _protocolTextBox.Font = new Font("Consolas", 9F);
        _protocolTextBox.ForeColor = Color.FromArgb(200, 200, 200);
        _protocolTextBox.Location = new Point(0, 0);
        _protocolTextBox.Name = "_protocolTextBox";
        _protocolTextBox.ReadOnly = true;
        _protocolTextBox.Size = new Size(1327, 713);
        _protocolTextBox.TabIndex = 0;
        _protocolTextBox.Text = "";
        _protocolTextBox.WordWrap = false;
        // 
        // _tabEquations
        // 
        _tabEquations.BackColor = Color.FromArgb(245, 247, 250);
        _tabEquations.Controls.Add(_equationsPanel);
        _tabEquations.Location = new Point(4, 40);
        _tabEquations.Name = "_tabEquations";
        _tabEquations.Size = new Size(1327, 713);
        _tabEquations.TabIndex = 3;
        _tabEquations.Text = "雷达方程";
        // 
        // _equationsPanel
        // 
        _equationsPanel.AutoScroll = true;
        _equationsPanel.BackColor = Color.FromArgb(18, 20, 26);
        _equationsPanel.Dock = DockStyle.Fill;
        _equationsPanel.Location = new Point(0, 0);
        _equationsPanel.Name = "_equationsPanel";
        _equationsPanel.Size = new Size(1327, 713);
        _equationsPanel.TabIndex = 0;
        _equationsPanel.Paint += _equationsPanel_Paint;
        // 
        // _bottomLogPanel
        // 
        _bottomLogPanel.BackColor = Color.FromArgb(245, 247, 250);
        _bottomLogPanel.Controls.Add(_logPanel);
        _bottomLogPanel.Dock = DockStyle.Fill;
        _bottomLogPanel.Location = new Point(3, 814);
        _bottomLogPanel.Name = "_bottomLogPanel";
        _bottomLogPanel.Size = new Size(1335, 194);
        _bottomLogPanel.TabIndex = 2;
        // 
        // _logPanel
        // 
        _logPanel.BackColor = Color.White;
        _logPanel.Controls.Add(_logTab);
        _logPanel.Controls.Add(_logToolbar);
        _logPanel.Dock = DockStyle.Fill;
        _logPanel.Location = new Point(0, 0);
        _logPanel.Name = "_logPanel";
        _logPanel.Size = new Size(1335, 194);
        _logPanel.TabIndex = 0;
        // 
        // _logTab
        // 
        _logTab.Controls.Add(_tabParsed);
        _logTab.Controls.Add(_tabRawHex);
        _logTab.Dock = DockStyle.Fill;
        _logTab.Font = new Font("微软雅黑", 9F);
        _logTab.ItemSize = new Size(100, 26);
        _logTab.Location = new Point(0, 28);
        _logTab.Name = "_logTab";
        _logTab.SelectedIndex = 0;
        _logTab.Size = new Size(1335, 166);
        _logTab.TabIndex = 2;
        // 
        // _tabParsed
        // 
        _tabParsed.BackColor = Color.White;
        _tabParsed.Controls.Add(_logListView);
        _tabParsed.Location = new Point(4, 30);
        _tabParsed.Name = "_tabParsed";
        _tabParsed.Size = new Size(1327, 132);
        _tabParsed.TabIndex = 0;
        _tabParsed.Text = "解析数据";
        // 
        // _logListView
        // 
        _logListView.BackColor = Color.White;
        _logListView.Columns.AddRange(new ColumnHeader[] { _logColTime, _logColDir, _logColCtrl, _logColCmd, _logColLen, _logColData });
        _logListView.Dock = DockStyle.Fill;
        _logListView.Font = new Font("Consolas", 8F);
        _logListView.FullRowSelect = true;
        _logListView.GridLines = true;
        _logListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        _logListView.Location = new Point(0, 0);
        _logListView.Name = "_logListView";
        _logListView.Size = new Size(1327, 132);
        _logListView.TabIndex = 0;
        _logListView.UseCompatibleStateImageBehavior = false;
        _logListView.View = View.Details;
        // 
        // _tabRawHex
        // 
        _tabRawHex.BackColor = Color.FromArgb(30, 30, 30);
        _tabRawHex.Controls.Add(_rawHexBox);
        _tabRawHex.Controls.Add(_rawHexToolbar);
        _tabRawHex.Location = new Point(4, 30);
        _tabRawHex.Name = "_tabRawHex";
        _tabRawHex.Size = new Size(1327, 132);
        _tabRawHex.TabIndex = 1;
        _tabRawHex.Text = "原始数据 (Hex)";
        // 
        // _rawHexToolbar
        // 
        _rawHexToolbar.BackColor = Color.FromArgb(40, 40, 40);
        _rawHexToolbar.Controls.Add(_lblRawHexStatus);
        _rawHexToolbar.Controls.Add(_btnDiagnose);
        _rawHexToolbar.Controls.Add(_btnRawHexClear);
        _rawHexToolbar.Dock = DockStyle.Top;
        _rawHexToolbar.Location = new Point(0, 0);
        _rawHexToolbar.Name = "_rawHexToolbar";
        _rawHexToolbar.Size = new Size(1327, 26);
        _rawHexToolbar.TabIndex = 0;
        // 
        // _lblRawHexStatus
        // 
        _lblRawHexStatus.AutoSize = true;
        _lblRawHexStatus.Font = new Font("Consolas", 8F);
        _lblRawHexStatus.ForeColor = Color.FromArgb(160, 160, 160);
        _lblRawHexStatus.Location = new Point(8, 5);
        _lblRawHexStatus.Name = "_lblRawHexStatus";
        _lblRawHexStatus.Size = new Size(89, 13);
        _lblRawHexStatus.TabIndex = 0;
        _lblRawHexStatus.Text = "已接收: 0 字节";
        // 
        // _btnRawHexClear
        // 
        _btnRawHexClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnRawHexClear.BackColor = Color.FromArgb(60, 60, 60);
        _btnRawHexClear.FlatAppearance.BorderSize = 0;
        _btnRawHexClear.FlatStyle = FlatStyle.Flat;
        _btnRawHexClear.Font = new Font("微软雅黑", 7F);
        _btnRawHexClear.ForeColor = Color.FromArgb(200, 200, 200);
        _btnRawHexClear.Location = new Point(1271, 1);
        _btnRawHexClear.Name = "_btnRawHexClear";
        _btnRawHexClear.Size = new Size(50, 22);
        _btnRawHexClear.TabIndex = 1;
        _btnRawHexClear.Text = "清空";
        _btnRawHexClear.UseVisualStyleBackColor = false;
        _btnRawHexClear.Click += _btnRawHexClear_Click;
        // 
        // _btnDiagnose
        // 
        _btnDiagnose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnDiagnose.BackColor = Color.FromArgb(60, 60, 60);
        _btnDiagnose.FlatAppearance.BorderSize = 0;
        _btnDiagnose.FlatStyle = FlatStyle.Flat;
        _btnDiagnose.Font = new Font("微软雅黑", 7F);
        _btnDiagnose.ForeColor = Color.FromArgb(241, 196, 15);
        _btnDiagnose.Location = new Point(1215, 1);
        _btnDiagnose.Name = "_btnDiagnose";
        _btnDiagnose.Size = new Size(50, 22);
        _btnDiagnose.TabIndex = 2;
        _btnDiagnose.Text = "诊断";
        _btnDiagnose.UseVisualStyleBackColor = false;
        _btnDiagnose.Click += _btnDiagnose_Click;
        // 
        // _rawHexBox
        // 
        _rawHexBox.BackColor = Color.FromArgb(30, 30, 30);
        _rawHexBox.BorderStyle = BorderStyle.None;
        _rawHexBox.Dock = DockStyle.Fill;
        _rawHexBox.Font = new Font("Consolas", 9F);
        _rawHexBox.ForeColor = Color.FromArgb(0, 255, 0);
        _rawHexBox.Location = new Point(0, 26);
        _rawHexBox.Name = "_rawHexBox";
        _rawHexBox.ReadOnly = true;
        _rawHexBox.Size = new Size(1327, 106);
        _rawHexBox.TabIndex = 1;
        _rawHexBox.Text = "";
        _rawHexBox.WordWrap = false;
        // 
        // _logColTime
        // 
        _logColTime.Text = "时间";
        _logColTime.Width = 120;
        // 
        // _logColDir
        // 
        _logColDir.Text = "方向";
        _logColDir.Width = 40;
        // 
        // _logColCtrl
        // 
        _logColCtrl.Text = "控制字";
        _logColCtrl.Width = 55;
        // 
        // _logColCmd
        // 
        _logColCmd.Text = "命令";
        _logColCmd.Width = 55;
        // 
        // _logColLen
        // 
        _logColLen.Text = "长度";
        _logColLen.Width = 35;
        // 
        // _logColData
        // 
        _logColData.Text = "数据";
        _logColData.Width = 500;
        // 
        // _logToolbar
        // 
        _logToolbar.BackColor = Color.FromArgb(250, 250, 250);
        _logToolbar.Controls.Add(_lblLogTitle);
        _logToolbar.Controls.Add(_btnClearLog);
        _logToolbar.Controls.Add(_btnExportLog);
        _logToolbar.Dock = DockStyle.Top;
        _logToolbar.Location = new Point(0, 0);
        _logToolbar.Name = "_logToolbar";
        _logToolbar.Size = new Size(1335, 28);
        _logToolbar.TabIndex = 0;
        // 
        // _lblLogTitle
        // 
        _lblLogTitle.AutoSize = true;
        _lblLogTitle.Font = new Font("微软雅黑", 8F, FontStyle.Bold);
        _lblLogTitle.ForeColor = Color.FromArgb(44, 62, 80);
        _lblLogTitle.Location = new Point(8, 5);
        _lblLogTitle.Name = "_lblLogTitle";
        _lblLogTitle.Size = new Size(51, 16);
        _lblLogTitle.TabIndex = 0;
        _lblLogTitle.Text = "通信日志";
        // 
        // _btnClearLog
        // 
        _btnClearLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnClearLog.BackColor = Color.FromArgb(231, 76, 60);
        _btnClearLog.FlatAppearance.BorderSize = 0;
        _btnClearLog.FlatStyle = FlatStyle.Flat;
        _btnClearLog.Font = new Font("微软雅黑", 7F);
        _btnClearLog.ForeColor = Color.White;
        _btnClearLog.Location = new Point(1245, 1);
        _btnClearLog.Name = "_btnClearLog";
        _btnClearLog.Size = new Size(50, 22);
        _btnClearLog.TabIndex = 1;
        _btnClearLog.Text = "清空";
        _btnClearLog.UseVisualStyleBackColor = false;
        _btnClearLog.Click += _btnClearLog_Click;
        // 
        // _btnExportLog
        // 
        _btnExportLog.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _btnExportLog.BackColor = Color.FromArgb(52, 152, 219);
        _btnExportLog.FlatAppearance.BorderSize = 0;
        _btnExportLog.FlatStyle = FlatStyle.Flat;
        _btnExportLog.Font = new Font("微软雅黑", 7F);
        _btnExportLog.ForeColor = Color.White;
        _btnExportLog.Location = new Point(1169, 1);
        _btnExportLog.Name = "_btnExportLog";
        _btnExportLog.Size = new Size(50, 22);
        _btnExportLog.TabIndex = 2;
        _btnExportLog.Text = "导出";
        _btnExportLog.UseVisualStyleBackColor = false;
        _btnExportLog.Click += _btnExportLog_Click;
        // 
        // _statusStrip
        // 
        _statusStrip.BackColor = Color.FromArgb(236, 240, 241);
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _portLabel, _fwLabel, _clipHintLabel, _versionLabel });
        _statusStrip.Location = new Point(0, 1011);
        _statusStrip.Name = "_statusStrip";
        _statusStrip.Size = new Size(1341, 22);
        _statusStrip.TabIndex = 3;
        _statusStrip.DoubleClick += _statusStrip_DoubleClick;
        // 
        // _statusLabel
        // 
        _statusLabel.ForeColor = Color.FromArgb(231, 76, 60);
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new Size(44, 17);
        _statusLabel.Text = "未连接";
        // 
        // _portLabel
        // 
        _portLabel.ForeColor = Color.FromArgb(127, 140, 141);
        _portLabel.Name = "_portLabel";
        _portLabel.Size = new Size(0, 17);
        // 
        // _fwLabel
        // 
        _fwLabel.ForeColor = Color.FromArgb(46, 204, 113);
        _fwLabel.Name = "_fwLabel";
        _fwLabel.Size = new Size(43, 17);
        _fwLabel.Text = "FW: --";
        // 
        // _clipHintLabel
        // 
        _clipHintLabel.ForeColor = Color.FromArgb(127, 140, 141);
        _clipHintLabel.Name = "_clipHintLabel";
        _clipHintLabel.Size = new Size(152, 17);
        _clipHintLabel.Text = "双击状态栏可截图到剪贴板";
        // 
        // _versionLabel
        // 
        _versionLabel.Alignment = ToolStripItemAlignment.Right;
        _versionLabel.ForeColor = Color.FromArgb(127, 140, 141);
        _versionLabel.Name = "_versionLabel";
        _versionLabel.Size = new Size(183, 17);
        _versionLabel.Text = "V1.0.0.0 | 249263431@qq.com";
        // 
        // MainForm
        // 
        BackColor = Color.FromArgb(245, 247, 250);
        ClientSize = new Size(1341, 1033);
        Controls.Add(_mainLayout);
        Font = new Font("微软雅黑", 9F);
        MinimumSize = new Size(960, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SleepSightPro";
        WindowState = FormWindowState.Maximized;
        FormClosing += MainForm_FormClosing;
        _mainLayout.ResumeLayout(false);
        _mainLayout.PerformLayout();
        _topPanel.ResumeLayout(false);
        _topPanel.PerformLayout();
        _rightToolbar.ResumeLayout(false);
        _rightToolbar.PerformLayout();
        _mainTab.ResumeLayout(false);
        _tabDisplay.ResumeLayout(false);
        _displayLayout.ResumeLayout(false);
        _dashboardPanel.ResumeLayout(false);
        _chartPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)_chartBreathWave).EndInit();
        ((System.ComponentModel.ISupportInitialize)_chartHeartWave).EndInit();
        _tabSettings.ResumeLayout(false);
        _settingsLayout.ResumeLayout(false);
        _settingsScrollPanel.ResumeLayout(false);
        _settingsScrollPanel.PerformLayout();
        _grpPresenceCtrl.ResumeLayout(false);
        _grpBreathCtrl.ResumeLayout(false);
        _grpHeartCtrl.ResumeLayout(false);
        _grpSleepCtrl.ResumeLayout(false);
        _grpSleepSettings.ResumeLayout(false);
        _grpSleepQuery.ResumeLayout(false);
        _grpSystemFunc.ResumeLayout(false);
        _settingsResultToolbar.ResumeLayout(false);
        _settingsResultToolbar.PerformLayout();
        _tabProtocol.ResumeLayout(false);
        _tabEquations.ResumeLayout(false);
        _bottomLogPanel.ResumeLayout(false);
        _logPanel.ResumeLayout(false);
        _logTab.ResumeLayout(false);
        _tabParsed.ResumeLayout(false);
        _tabRawHex.ResumeLayout(false);
        _rawHexToolbar.ResumeLayout(false);
        _rawHexToolbar.PerformLayout();
        _logToolbar.ResumeLayout(false);
        _logToolbar.PerformLayout();
        _statusStrip.ResumeLayout(false);
        _statusStrip.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    // ========================================================================
    //  控件字段声明
    // ========================================================================

    // 主布局
    private TableLayoutPanel _mainLayout = null!;

    // 顶部面板
    private Panel _topPanel = null!;
    private Label _lblTitle = null!;
    private Label _lblSubtitle = null!;
    private FlowLayoutPanel _rightToolbar = null!;
    private ComboBox _cmbPort = null!;
    private ComboBox _cmbBaudRate = null!;
    private Button _btnConnect = null!;
    private Button _btnRefresh = null!;
    private Button _btnLang = null!;
    private Label _lblPort = null!;

    // 主 Tab
    private TabControl _mainTab = null!;
    private TabPage _tabDisplay = null!;
    private TabPage _tabSettings = null!;
    private TabPage _tabProtocol = null!;
    private RichTextBox _protocolTextBox = null!;
    private TabPage _tabEquations = null!;
    private Panel _equationsPanel = null!;

    // 显示页 - 布局
    private TableLayoutPanel _displayLayout = null!;
    private TableLayoutPanel _dashboardPanel = null!;
    private TableLayoutPanel _chartPanel = null!;

    // 波形图表
    private Chart _chartBreathWave = null!;
    private Chart _chartHeartWave = null!;

    // 仪表盘卡片
    private DashboardCard _cardPresence = null!;
    private DashboardCard _cardBreath = null!;
    private DashboardCard _cardHeart = null!;
    private DashboardCard _cardSleepState = null!;
    private DashboardCard _cardBodyMove = null!;
    private DashboardCard _cardSleepScore = null!;
    private DashboardCard _cardDistance = null!;
    private DashboardCard _cardApnea = null!;
    private DashboardCard _cardTurnOver = null!;

    // 设置页
    private TableLayoutPanel _settingsLayout = null!;
    private FlowLayoutPanel _settingsScrollPanel = null!;
    private RichTextBox _settingsResultBox = null!;
    private Panel _settingsResultToolbar = null!;
    private Label _lblSettingsResult = null!;
    private Label _settingsRuleLabel = null!;
    private Button _btnClearResult = null!;

    // 设置页 GroupBox
    private GroupBox _grpPresenceCtrl = null!;
    private GroupBox _grpBreathCtrl = null!;
    private GroupBox _grpHeartCtrl = null!;
    private GroupBox _grpSleepCtrl = null!;
    private GroupBox _grpSleepSettings = null!;
    private GroupBox _grpSleepQuery = null!;
    private GroupBox _grpSystemFunc = null!;

    // 人体存在控制 按钮
    private Button _btnPresenceOn = null!;
    private Button _btnPresenceOff = null!;
    private Button _btnQueryPresence = null!;
    private Button _btnQueryMotion = null!;
    private Button _btnQueryDistance = null!;
    private Button _btnQueryPosition = null!;

    // 呼吸监测控制 按钮
    private Button _btnBreathOn = null!;
    private Button _btnBreathOff = null!;
    private Button _btnBreathWaveOn = null!;
    private Button _btnBreathWaveOff = null!;
    private Button _btnQueryBreathValue = null!;
    private Button _btnQueryBreathState = null!;
    private Button _btnQueryBreathWaveSwitch = null!;
    private Button _btnQueryBreathSwitch = null!;
    private Button _btnSetLowBreath = null!;
    private Button _btnQueryLowBreath = null!;

    // 心率监测控制 按钮
    private Button _btnHeartOn = null!;
    private Button _btnHeartOff = null!;
    private Button _btnHeartWaveOn = null!;
    private Button _btnHeartWaveOff = null!;
    private Button _btnQueryHeartValue = null!;
    private Button _btnQueryHeartSwitch = null!;
    private Button _btnQueryHeartWave = null!;
    private Button _btnQueryHeartWaveSwitch = null!;

    // 睡眠监测控制 按钮
    private Button _btnSleepOn = null!;
    private Button _btnSleepOff = null!;
    private Button _btnQuerySleepComp = null!;
    private Button _btnQuerySleepAnalysis = null!;
    private Button _btnQuerySleepRating = null!;

    // 睡眠参数设置 按钮
    private Button _btnStruggleSwitchOn = null!;
    private Button _btnStruggleSwitchOff = null!;
    private Button _btnNoPersonSwitchOn = null!;
    private Button _btnNoPersonSwitchOff = null!;
    private Button _btnSetNoPersonDuration = null!;
    private Button _btnExtCtrlSwitchOn = null!;
    private Button _btnExtCtrlSwitchOff = null!;
    private Button _btnSleepPeriodStart = null!;
    private Button _btnSleepPeriodEnd = null!;
    private Button _btnSetSleepDeadline = null!;

    // 睡眠参数查询 按钮
    private Button _btnQueryBedState = null!;
    private Button _btnQuerySleepState = null!;
    private Button _btnQueryAwakeDur = null!;
    private Button _btnQueryLightDur = null!;
    private Button _btnQueryDeepDur = null!;
    private Button _btnQuerySleepScore = null!;
    private Button _btnQuerySleepAbnormal = null!;
    private Button _btnQuerySleepStats = null!;
    private Button _btnQueryStruggleSwitch = null!;
    private Button _btnQueryNoPersonSwitch = null!;

    // 系统功能 按钮
    private Button _btnHeartbeatQuery = null!;
    private Button _btnModuleReset = null!;
    private Button _btnInitQuery = null!;
    private Button _btnBoundaryQuery = null!;
    private Button _btnQueryDev = null!;

    // 底部日志面板
    private Panel _bottomLogPanel = null!;
    private Panel _logPanel = null!;
    private TabControl _logTab = null!;
    private TabPage _tabParsed = null!;
    private TabPage _tabRawHex = null!;
    private Panel _rawHexToolbar = null!;
    private Label _lblRawHexStatus = null!;
    private Button _btnRawHexClear = null!;
    private Button _btnDiagnose = null!;
    private RichTextBox _rawHexBox = null!;
    private Panel _logToolbar = null!;
    private Label _lblLogTitle = null!;
    private Button _btnClearLog = null!;
    private Button _btnExportLog = null!;
    private ListView _logListView = null!;
    private ColumnHeader _logColTime = null!;
    private ColumnHeader _logColDir = null!;
    private ColumnHeader _logColCtrl = null!;
    private ColumnHeader _logColCmd = null!;
    private ColumnHeader _logColLen = null!;
    private ColumnHeader _logColData = null!;

    // 状态栏
    private StatusStrip _statusStrip = null!;
    private ToolStripStatusLabel _statusLabel = null!;
    private ToolStripStatusLabel _portLabel = null!;
    private ToolStripStatusLabel _fwLabel = null!;
    private ToolStripStatusLabel _versionLabel = null!;
    private ToolStripStatusLabel _clipHintLabel = null!;
}
