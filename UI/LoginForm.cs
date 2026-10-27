using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Services;
using SchoolClinicManagementSystem.UI.Controls;

namespace SchoolClinicManagementSystem.UI
{
    public class LoginForm : Form
    {
        private CardPanel _cardPanel;
        private Panel _headerPanel;
        private PictureBox _picLogo;
        private Label _lblPortalTitle;
        private Label _lblPortalSubtitle;

        private Label _lblWelcome;
        private Label _lblSignInDesc;
        private Label _lblUsername;
        private ModernTextBox _txtUsername;
        private Label _lblPassword;
        private ModernTextBox _txtPassword;
        private Label _lblError;
        private Button _btnLogin;
        private Label _lblFooterHelp;

        public LoginForm()
        {
            InitializeComponent();
        }

        private static string GetResourcePath(string fileName)
        {
            try
            {
                string baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (!string.IsNullOrEmpty(baseDir))
                {
                    string p1 = Path.Combine(baseDir, "Resources", fileName);
                    if (File.Exists(p1)) return p1;
                }
            }
            catch { }

            try
            {
                string p2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", fileName);
                if (File.Exists(p2)) return p2;
            }
            catch { }

            try
            {
                string p3 = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
                if (File.Exists(p3)) return p3;
            }
            catch { }

            return null;
        }

        private void InitializeComponent()
        {
            Text = "Login - School Clinic Management System";
            BackColor = UITheme.WindowBg;
            ClientSize = new Size(464, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            // Try loading form icon
            try
            {
                string iconPath = GetResourcePath("app.ico");
                if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                {
                    Icon = new Icon(iconPath);
                }
            }
            catch { }

            // Card Container
            _cardPanel = new CardPanel
            {
                Size = new Size(402, 474),
                Location = new Point(31, 38),
                BackColor = Color.White,
                BorderColor = UITheme.CardBorder,
                BorderWidth = 1
            };

            // Top Header Banner
            _headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = UITheme.MaroonBanner
            };

            // Logo
            _picLogo = new PictureBox
            {
                Size = new Size(42, 50),
                Location = new Point((402 - 42) / 2, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                string logoPath = GetResourcePath("cct_logo.png");
                if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                {
                    _picLogo.Image = Image.FromFile(logoPath);
                }
            }
            catch { }

            // "SCMS PORTAL"
            _lblPortalTitle = new Label
            {
                Text = "SCMS PORTAL",
                Font = UITheme.HeaderFont,
                ForeColor = Color.White,
                Location = new Point(0, 66),
                Size = new Size(402, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Subtitle
            _lblPortalSubtitle = new Label
            {
                Text = "City College of Tagaytay Clinic Management",
                Font = UITheme.SubHeaderFont,
                ForeColor = UITheme.SubtitleText,
                Location = new Point(0, 93),
                Size = new Size(402, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            _headerPanel.Controls.Add(_picLogo);
            _headerPanel.Controls.Add(_lblPortalTitle);
            _headerPanel.Controls.Add(_lblPortalSubtitle);
            _cardPanel.Controls.Add(_headerPanel);

            // Card Body Controls
            // "Welcome Back"
            _lblWelcome = new Label
            {
                Text = "Welcome Back",
                Font = UITheme.TitleFont,
                ForeColor = UITheme.TextDark,
                Location = new Point(36, 146),
                AutoSize = true
            };

            // Subtitle
            _lblSignInDesc = new Label
            {
                Text = "Sign in to access the school clinic dashboard.",
                Font = UITheme.BodyFont,
                ForeColor = UITheme.TextMuted,
                Location = new Point(36, 180),
                AutoSize = true
            };

            // Username Label
            _lblUsername = new Label
            {
                Text = "Username",
                Font = UITheme.LabelFont,
                ForeColor = UITheme.TextDark,
                Location = new Point(36, 212),
                AutoSize = true
            };

            // Username TextBox
            _txtUsername = new ModernTextBox
            {
                Location = new Point(36, 234),
                Size = new Size(330, 32),
                TextValue = "admin"
            };

            // Password Label
            _lblPassword = new Label
            {
                Text = "Password",
                Font = UITheme.LabelFont,
                ForeColor = UITheme.TextDark,
                Location = new Point(36, 276),
                AutoSize = true
            };

            // Password TextBox
            _txtPassword = new ModernTextBox
            {
                Location = new Point(36, 298),
                Size = new Size(330, 32),
                UseSystemPasswordChar = true,
                TextValue = "admin123"
            };

            // Error Label
            _lblError = new Label
            {
                Location = new Point(36, 342),
                Size = new Size(330, 18),
                Font = UITheme.SmallFont,
                ForeColor = UITheme.Danger,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false
            };

            // "Log In" Button
            _btnLogin = new Button
            {
                Text = "Log In",
                Location = new Point(36, 368),
                Size = new Size(330, 42),
                BackColor = UITheme.PrimaryRed,
                ForeColor = Color.White,
                Font = UITheme.ButtonFont,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _btnLogin.FlatAppearance.BorderSize = 0;
            _btnLogin.FlatAppearance.MouseOverBackColor = UITheme.PrimaryRedHover;
            _btnLogin.FlatAppearance.MouseDownBackColor = UITheme.PrimaryRedClick;
            _btnLogin.Click += BtnLogin_Click;

            // Footer hint
            _lblFooterHelp = new Label
            {
                Text = "Default: admin / admin123  •  nurse / nurse123",
                Font = UITheme.SmallFont,
                ForeColor = UITheme.TextMuted,
                Location = new Point(0, 426),
                Size = new Size(402, 22),
                TextAlign = ContentAlignment.MiddleCenter
            };

            _cardPanel.Controls.Add(_lblWelcome);
            _cardPanel.Controls.Add(_lblSignInDesc);
            _cardPanel.Controls.Add(_lblUsername);
            _cardPanel.Controls.Add(_txtUsername);
            _cardPanel.Controls.Add(_lblPassword);
            _cardPanel.Controls.Add(_txtPassword);
            _cardPanel.Controls.Add(_lblError);
            _cardPanel.Controls.Add(_btnLogin);
            _cardPanel.Controls.Add(_lblFooterHelp);

            Controls.Add(_cardPanel);

            AcceptButton = _btnLogin;

            Shown += (s, e) =>
            {
                _txtUsername.Focus();
                _txtUsername.SelectAll();
            };
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = _txtUsername.TextValue;
            string password = _txtPassword.TextValue;

            if (AuthService.Instance.Login(username, password, out string error))
            {
                _lblError.Visible = false;
                Hide();

                var dashboard = new DashboardForm(AuthService.Instance.CurrentUser);
                dashboard.FormClosed += (s, args) =>
                {
                    if (AuthService.Instance.CurrentUser == null)
                    {
                        // Logged out
                        _txtPassword.TextValue = "";
                        _lblError.Visible = false;
                        Show();
                        _txtUsername.Focus();
                    }
                    else
                    {
                        // Closed window
                        Application.Exit();
                    }
                };
                dashboard.Show();
            }
            else
            {
                _lblError.Text = error;
                _lblError.Visible = true;
            }
        }
    }
}
