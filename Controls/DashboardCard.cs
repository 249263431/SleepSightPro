using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace SleepSightPro.Controls;

/// <summary>
/// 仪表盘卡片控件 - 自适应尺寸的现代化卡片
/// </summary>
public class DashboardCard : UserControl
{
    private string _title = "Title";
    private string _value = "--";
    private Color _accentColor = Color.FromArgb(52, 152, 219);

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Title
    {
        get => _title;
        set { if (_title != value) { _title = value; Invalidate(); } }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Value
    {
        get => _value;
        set { if (_value != value) { _value = value; Invalidate(); } }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color AccentColor
    {
        get => _accentColor;
        set { if (_accentColor != value) { _accentColor = value; Invalidate(); } }
    }

    public DashboardCard()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                 ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
        MinimumSize = new Size(120, 90);
        Size = new Size(200, 130);
        BackColor = Color.White;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var rect = ClientRectangle;
        int w = rect.Width;
        int h = rect.Height;

        // ---- 卡片背景（圆角矩形） ----
        var cardRect = new Rectangle(2, 2, w - 4, h - 4);
        using var path = GetRoundedRect(cardRect, 8);
        using var bgBrush = new SolidBrush(Color.White);
        g.FillPath(bgBrush, path);

        // ---- 顶部彩色指示条 ----
        int accentLen = Math.Min(40, w / 5);
        int accentY = cardRect.Top + 6;
        using var accentPen = new Pen(_accentColor, 3);
        g.DrawLine(accentPen, cardRect.Left + 10, accentY, cardRect.Left + 10 + accentLen, accentY);

        // ---- 边框 ----
        using var borderPen = new Pen(Color.FromArgb(220, 220, 220), 1);
        g.DrawPath(borderPen, path);

        // ---- 根据卡片高度动态计算字体和布局 ----
        float titleFontSize = h > 150 ? 10F : h > 110 ? 9F : 8F;
        float valueFontSize = h > 150 ? 24F : h > 110 ? 18F : 14F;
        int titleY = cardRect.Top + (h > 120 ? 14 : 8);
        int valueY = cardRect.Top + (h > 120 ? 42 : h / 2 - 10);
        int leftMargin = Math.Min(14, w / 12);
        int rightMargin = leftMargin;

        // ---- 标题 ----
        using var titleFont = new Font("Microsoft YaHei", titleFontSize, FontStyle.Regular);
        using var titleBrush = new SolidBrush(Color.FromArgb(127, 140, 141));
        var titleMaxWidth = w - leftMargin - rightMargin - 4;
        var titleText = TruncateText(g, _title, titleFont, titleMaxWidth);
        g.DrawString(titleText, titleFont, titleBrush, leftMargin, titleY);

        // ---- 数值 ----
        using var valueFont = new Font("Microsoft YaHei", valueFontSize, FontStyle.Bold);
        using var valueBrush = new SolidBrush(Color.FromArgb(44, 62, 80));
        var valueMaxWidth = w - leftMargin - rightMargin - 4;
        var valueText = TruncateText(g, _value, valueFont, valueMaxWidth);
        var valueRect = new Rectangle(leftMargin, valueY, valueMaxWidth, h - valueY - 8);
        using var sf = new StringFormat { LineAlignment = StringAlignment.Near };
        g.DrawString(valueText, valueFont, valueBrush, valueRect, sf);

        // ---- 底部装饰圆点（仅在空间足够时绘制） ----
        if (w > 140 && h > 100)
        {
            using var dotBrush = new SolidBrush(_accentColor);
            int dotSize = h > 130 ? 8 : 6;
            g.FillEllipse(dotBrush, cardRect.Right - dotSize - 8, cardRect.Bottom - dotSize - 8, dotSize, dotSize);
        }
    }

    /// <summary>
    /// 截断文字并添加省略号（如果超出宽度）
    /// </summary>
    private static string TruncateText(Graphics g, string text, Font font, int maxWidth)
    {
        if (string.IsNullOrEmpty(text)) return "";
        var size = g.MeasureString(text, font);
        if (size.Width <= maxWidth) return text;

        // 逐字符截断
        for (int i = text.Length - 1; i > 0; i--)
        {
            var truncated = text[..i] + "…";
            if (g.MeasureString(truncated, font).Width <= maxWidth)
                return truncated;
        }
        return "…";
    }

    private static GraphicsPath GetRoundedRect(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.Left, rect.Top, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Top, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.Left, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
