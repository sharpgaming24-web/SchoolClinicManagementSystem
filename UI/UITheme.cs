using System.Drawing;

namespace SchoolClinicManagementSystem.UI
{
    public static class UITheme
    {
        // City College of Tagaytay Brand Colors
        public static readonly Color MaroonBanner = Color.FromArgb(36, 12, 18);    // #240C12
        public static readonly Color PrimaryRed = Color.FromArgb(136, 14, 30);     // #880E1E (Crimson button)
        public static readonly Color PrimaryRedHover = Color.FromArgb(163, 20, 38); // Slightly lighter on hover
        public static readonly Color PrimaryRedClick = Color.FromArgb(115, 10, 24); // Darker on click

        // Accent & Status Colors
        public static readonly Color FocusBlue = Color.FromArgb(13, 110, 253);     // #0D6EFD / #0078D7
        public static readonly Color BorderDefault = Color.FromArgb(204, 204, 204); // #CCCCCC
        public static readonly Color WindowBg = Color.FromArgb(250, 248, 249);     // #FAF8F9 (Outer form bg)
        public static readonly Color CardBg = Color.White;
        public static readonly Color CardBorder = Color.FromArgb(226, 221, 223);   // #E2DDDF
        
        public static readonly Color TextDark = Color.FromArgb(25, 25, 25);
        public static readonly Color TextMuted = Color.FromArgb(108, 117, 125);
        public static readonly Color SubtitleText = Color.FromArgb(192, 180, 175);  // Off-white/cream for banner

        // Status Indicators
        public static readonly Color Success = Color.FromArgb(25, 135, 84);
        public static readonly Color Warning = Color.FromArgb(255, 193, 7);
        public static readonly Color Danger = Color.FromArgb(220, 53, 69);
        public static readonly Color Info = Color.FromArgb(13, 202, 240);

        // Sidebar & Dashboard
        public static readonly Color SidebarBg = Color.FromArgb(28, 10, 15);
        public static readonly Color SidebarActive = Color.FromArgb(136, 14, 30);
        public static readonly Color SidebarHover = Color.FromArgb(50, 18, 25);

        // Fonts
        public static readonly Font HeaderFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font SubHeaderFont = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font TitleFont = new Font("Segoe UI", 17F, FontStyle.Bold);
        public static readonly Font LabelFont = new Font("Segoe UI", 9F, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font ButtonFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font SmallFont = new Font("Segoe UI", 8.25F, FontStyle.Regular);
    }
}
