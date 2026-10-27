using System;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolClinicManagementSystem.UI.Controls
{
    public class ModernTextBox : UserControl
    {
        private readonly TextBox _textBox;
        private bool _isFocused;
        private Color _borderColor = UITheme.BorderDefault;
        private Color _focusBorderColor = UITheme.FocusBlue;
        private int _borderThickness = 1;

        public event EventHandler TextChangedCustom;

        public ModernTextBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            
            BackColor = Color.White;
            Padding = new Padding(8, 6, 8, 6);
            Size = new Size(330, 32);

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = UITheme.TextDark,
                BackColor = Color.White,
                Dock = DockStyle.Fill
            };

            _textBox.Enter += (s, e) =>
            {
                _isFocused = true;
                _borderThickness = 2;
                Invalidate();
            };

            _textBox.Leave += (s, e) =>
            {
                _isFocused = false;
                _borderThickness = 1;
                Invalidate();
            };

            _textBox.TextChanged += (s, e) =>
            {
                TextChangedCustom?.Invoke(this, EventArgs.Empty);
            };

            _textBox.KeyDown += (s, e) =>
            {
                OnKeyDown(e);
            };

            Controls.Add(_textBox);
        }

        public string TextValue
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        public bool UseSystemPasswordChar
        {
            get => _textBox.UseSystemPasswordChar;
            set => _textBox.UseSystemPasswordChar = value;
        }

        public char PasswordChar
        {
            get => _textBox.PasswordChar;
            set => _textBox.PasswordChar = value;
        }

        public TextBox InnerTextBox => _textBox;

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        public new void Focus()
        {
            _textBox.Focus();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Color currentBorder = _isFocused ? _focusBorderColor : _borderColor;
            using (var pen = new Pen(currentBorder, _borderThickness))
            {
                int offset = _borderThickness / 2;
                e.Graphics.DrawRectangle(pen, offset, offset, Width - _borderThickness, Height - _borderThickness);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Center the textbox vertically within padding
            if (_textBox != null)
            {
                int topPadding = Math.Max(4, (Height - _textBox.PreferredHeight) / 2);
                Padding = new Padding(8, topPadding, 8, 4);
            }
        }
    }
}
