using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchoolClinicManagementSystem.UI.Controls
{
    public class CardPanel : Panel
    {
        public Color BorderColor { get; set; } = UITheme.CardBorder;
        public int BorderWidth { get; set; } = 1;

        public CardPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (BorderWidth > 0)
            {
                using (var pen = new Pen(BorderColor, BorderWidth))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }

    public class StatCard : Panel
    {
        private readonly Label _lblTitle;
        private readonly Label _lblValue;
        private readonly Label _lblSub;
        private readonly Panel _colorBar;

        public StatCard(string title, string value, string sub, Color accentColor)
        {
            Size = new Size(220, 100);
            BackColor = Color.White;
            Margin = new Padding(8);

            _colorBar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = accentColor
            };

            _lblTitle = new Label
            {
                Text = title.ToUpperInvariant(),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = UITheme.TextMuted,
                Location = new Point(16, 12),
                AutoSize = true
            };

            _lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = UITheme.TextDark,
                Location = new Point(14, 30),
                AutoSize = true
            };

            _lblSub = new Label
            {
                Text = sub,
                Font = new Font("Segoe UI", 8.25F, FontStyle.Regular),
                ForeColor = accentColor,
                Location = new Point(16, 70),
                AutoSize = true
            };

            Controls.Add(_colorBar);
            Controls.Add(_lblTitle);
            Controls.Add(_lblValue);
            Controls.Add(_lblSub);
        }

        public void UpdateValue(string value, string sub = null)
        {
            _lblValue.Text = value;
            if (sub != null) _lblSub.Text = sub;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var pen = new Pen(UITheme.CardBorder, 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }
        }
    }
}
