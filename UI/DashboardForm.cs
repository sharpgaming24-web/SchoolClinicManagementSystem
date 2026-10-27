using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Models;
using SchoolClinicManagementSystem.Services;
using SchoolClinicManagementSystem.UI.Controls;

namespace SchoolClinicManagementSystem.UI
{
    public class DashboardForm : Form
    {
        private readonly User _currentUser;

        // Layout
        private Panel _sidebar;
        private Panel _contentArea;

        // Sidebar buttons
        private Button _btnNavDashboard;
        private Button _btnNavPatients;
        private Button _btnNavPharmacy;
        private Button _activeNavButton;

        // Sidebar footer
        private Label _lblSidebarRole;
        private Label _lblSidebarDuty;
        private Button _btnSignOut;

        // View panels
        private Panel _viewDashboard;
        private Panel _viewPatients;
        private Panel _viewPharmacy;

        // Dashboard cards
        private StatCard _cardPatients;
        private StatCard _cardConsultToday;
        private StatCard _cardResting;
        private StatCard _cardLowStock;

        // Dashboard grids
        private DataGridView _gridRecentConsult;
        private DataGridView _gridLowStock;

        // Patients view
        private DataGridView _gridPatients;
        private TextBox _txtSearchPatient;

        // Pharmacy view
        private DataGridView _gridPharmacy;
        private TextBox _txtSearchMed;
        private ComboBox _cboFilterMed;

        public DashboardForm(User user)
        {
            _currentUser = user;
            InitializeComponent();
        }

        private static string GetResourcePath(string fileName)
        {
            foreach (var dir in new[] {
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                AppDomain.CurrentDomain.BaseDirectory,
                Directory.GetCurrentDirectory()
            })
            {
                try
                {
                    if (!string.IsNullOrEmpty(dir))
                    {
                        string p = Path.Combine(dir, "Resources", fileName);
                        if (File.Exists(p)) return p;
                    }
                }
                catch { }
            }
            return null;
        }

        private void InitializeComponent()
        {
            Text = "School Clinic Management System (SCMS)";
            ClientSize = new Size(1100, 640);
            MinimumSize = new Size(960, 580);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = UITheme.WindowBg;

            try
            {
                string ico = GetResourcePath("app.ico");
                if (!string.IsNullOrEmpty(ico)) Icon = new Icon(ico);
            }
            catch { }

            BuildContent();
            BuildSidebar();

            SwitchView(_btnNavDashboard, _viewDashboard);
            RefreshAll();
        }

        // ─────── SIDEBAR ───────
        private void BuildSidebar()
        {
            _sidebar = new Panel { Dock = DockStyle.Left, Width = 175, BackColor = UITheme.SidebarBg };

            // Top logo area
            var pnlLogo = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = UITheme.MaroonBanner };

            var picLogo = new PictureBox { Size = new Size(28, 34), Location = new Point(8, 10), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.Transparent };
            try { string lp = GetResourcePath("cct_logo.png"); if (!string.IsNullOrEmpty(lp)) picLogo.Image = Image.FromFile(lp); } catch { }

            var lblPortal = new Label { Text = "SCMS PORTAL", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(40, 8), AutoSize = true, BackColor = Color.Transparent };
            var lblSubPortal = new Label { Text = "School Clinic Mgmt", Font = new Font("Segoe UI", 7.5F), ForeColor = UITheme.SubtitleText, Location = new Point(40, 26), AutoSize = true, BackColor = Color.Transparent };

            pnlLogo.Controls.AddRange(new Control[] { picLogo, lblPortal, lblSubPortal });
            _sidebar.Controls.Add(pnlLogo);

            // Nav buttons
            int y = 60;
            _btnNavDashboard = MakeNavBtn("   Dashboard", ref y);
            _btnNavPatients = MakeNavBtn("   Patients & Students", ref y);
            _btnNavPharmacy = MakeNavBtn("   Medicine Inventory", ref y);

            _btnNavDashboard.Click += (s, e) => SwitchView(_btnNavDashboard, _viewDashboard);
            _btnNavPatients.Click += (s, e) => SwitchView(_btnNavPatients, _viewPatients);
            _btnNavPharmacy.Click += (s, e) => SwitchView(_btnNavPharmacy, _viewPharmacy);

            // Sidebar footer
            var pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 80, BackColor = Color.FromArgb(22, 8, 12) };
            _lblSidebarRole = new Label { Text = _currentUser.Role, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(8, 8), AutoSize = true };
            _lblSidebarDuty = new Label { Text = "On Duty • " + _currentUser.Username, Font = new Font("Segoe UI", 7.5F), ForeColor = Color.Gray, Location = new Point(8, 26), AutoSize = true };
            _btnSignOut = new Button { Text = "🚪 Sign Out", Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), BackColor = UITheme.PrimaryRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(120, 28), Location = new Point(8, 48), Cursor = Cursors.Hand };
            _btnSignOut.FlatAppearance.BorderSize = 0;
            _btnSignOut.Click += (s, e) => { AuthService.Instance.Logout(); Close(); };
            pnlFooter.Controls.AddRange(new Control[] { _lblSidebarRole, _lblSidebarDuty, _btnSignOut });
            _sidebar.Controls.Add(pnlFooter);

            Controls.Add(_sidebar);
        }

        private Button MakeNavBtn(string text, ref int y)
        {
            var btn = new Button
            {
                Text = text,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.FromArgb(200, 200, 200),
                BackColor = UITheme.SidebarBg,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(175, 34),
                Location = new Point(0, y),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = UITheme.SidebarHover;
            _sidebar.Controls.Add(btn);
            y += 34;
            return btn;
        }

        // ─────── CONTENT VIEWS ───────
        private void BuildContent()
        {
            _contentArea = new Panel { Dock = DockStyle.Fill, BackColor = UITheme.WindowBg, Padding = new Padding(18, 12, 18, 12) };

            _viewDashboard = BuildDashboardView();
            _viewPatients = BuildPatientsView();
            _viewPharmacy = BuildPharmacyView();

            _contentArea.Controls.AddRange(new Control[] { _viewDashboard, _viewPatients, _viewPharmacy });
            Controls.Add(_contentArea);
        }

        private void SwitchView(Button nav, Panel view)
        {
            if (_activeNavButton != null) { _activeNavButton.BackColor = UITheme.SidebarBg; _activeNavButton.Font = new Font("Segoe UI", 8.5F); }
            _activeNavButton = nav;
            _activeNavButton.BackColor = UITheme.SidebarActive;
            _activeNavButton.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);

            _viewDashboard.Visible = _viewPatients.Visible = _viewPharmacy.Visible = false;
            view.Visible = true;
            view.BringToFront();
            RefreshAll();
        }

        // ─────── DASHBOARD VIEW ───────
        private Panel BuildDashboardView()
        {
            var pnl = new Panel { Dock = DockStyle.Fill, AutoScroll = true };

            // Title row
            pnl.Controls.Add(new Label { Text = "Clinic Dashboard Overview", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = UITheme.TextDark, Location = new Point(0, 0), AutoSize = true });
            pnl.Controls.Add(new Label { Text = "Real-time summary of consultations, patients, and inventory alerts", Font = UITheme.SmallFont, ForeColor = UITheme.TextMuted, Location = new Point(0, 28), AutoSize = true });

            // ── ACTION BUTTONS ──
            var btnConsult = new Button { Text = "+ Record Consultation", Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = UITheme.PrimaryRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(170, 32), Location = new Point(0, 52), Cursor = Cursors.Hand };
            btnConsult.FlatAppearance.BorderSize = 0;
            btnConsult.Click += (s, e) => OpenConsultationDialog();

            var btnRegister = new Button { Text = "📋 Register New Patient", Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = Color.White, ForeColor = UITheme.TextDark, FlatStyle = FlatStyle.Flat, Size = new Size(170, 32), Location = new Point(180, 52), Cursor = Cursors.Hand };
            btnRegister.FlatAppearance.BorderColor = UITheme.BorderDefault;
            btnRegister.Click += (s, e) => OpenRegisterPatientDialog();

            var btnPharmacy = new Button { Text = "Pharmacy & Inventory", Font = new Font("Segoe UI", 9F, FontStyle.Bold), BackColor = Color.FromArgb(255, 153, 0), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(165, 32), Location = new Point(360, 52), Cursor = Cursors.Hand };
            btnPharmacy.FlatAppearance.BorderSize = 0;
            btnPharmacy.Click += (s, e) => SwitchView(_btnNavPharmacy, _viewPharmacy);

            pnl.Controls.AddRange(new Control[] { btnConsult, btnRegister, btnPharmacy });

            // ── STAT CARDS ──
            var cardsPanel = new FlowLayoutPanel { Location = new Point(0, 95), Size = new Size(880, 80), WrapContents = false };
            _cardPatients = new StatCard("TOTAL REGISTERED PATIENTS", "0", "", UITheme.FocusBlue) { Size = new Size(205, 72) };
            _cardConsultToday = new StatCard("CONSULTATIONS TODAY", "0", "", UITheme.Success) { Size = new Size(205, 72) };
            _cardResting = new StatCard("RESTING IN CLINIC NOW", "0", "", UITheme.Warning) { Size = new Size(205, 72) };
            _cardLowStock = new StatCard("LOW STOCK / EXPIRED ALERTS", "0", "", UITheme.Danger) { Size = new Size(205, 72) };
            cardsPanel.Controls.AddRange(new Control[] { _cardPatients, _cardConsultToday, _cardResting, _cardLowStock });
            pnl.Controls.Add(cardsPanel);

            // ── RECENT CONSULTATIONS TABLE ──
            var lblRecent = new Label { Text = "Recent Consultations", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = UITheme.TextDark, Location = new Point(0, 185), AutoSize = true };
            var lblHint = new Label { Text = "(Double-click to view / print slip)", Font = UITheme.SmallFont, ForeColor = UITheme.TextMuted, Location = new Point(165, 189), AutoSize = true };
            pnl.Controls.AddRange(new Control[] { lblRecent, lblHint });

            _gridRecentConsult = MakeGrid();
            _gridRecentConsult.Location = new Point(0, 210);
            _gridRecentConsult.Size = new Size(520, 295);
            _gridRecentConsult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            _gridRecentConsult.Columns.Add("VisitNo", "Visit ID");
            _gridRecentConsult.Columns.Add("Name", "Patient Name");
            _gridRecentConsult.Columns.Add("Complaint", "Chief Complaint");
            _gridRecentConsult.Columns.Add("Disposition", "Disposition");
            _gridRecentConsult.Columns.Add("Time", "Time");
            pnl.Controls.Add(_gridRecentConsult);

            // ── LOW STOCK ATTENTION TABLE ──
            var lblLow = new Label { Text = "⚠ Low Stock Attention", Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = UITheme.Danger, Location = new Point(535, 185), AutoSize = true };
            pnl.Controls.Add(lblLow);

            _gridLowStock = MakeGrid();
            _gridLowStock.Location = new Point(535, 210);
            _gridLowStock.Size = new Size(330, 295);
            _gridLowStock.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            _gridLowStock.Columns.Add("Medicine", "Medicine Name");
            _gridLowStock.Columns.Add("Stock", "Stock");
            _gridLowStock.Columns.Add("Reorder", "Reorder Level");
            pnl.Controls.Add(_gridLowStock);

            return pnl;
        }

        // ─────── PATIENTS VIEW ───────
        private Panel BuildPatientsView()
        {
            var pnl = new Panel { Dock = DockStyle.Fill };
            pnl.Controls.Add(new Label { Text = "Patients & Students", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = UITheme.TextDark, Location = new Point(0, 0), AutoSize = true });

            var lblS = new Label { Text = "Search:", Font = UITheme.LabelFont, Location = new Point(0, 38), AutoSize = true };
            _txtSearchPatient = new TextBox { Location = new Point(55, 35), Width = 250, Font = UITheme.BodyFont };
            _txtSearchPatient.TextChanged += (s, e) => RefreshPatients();

            var btnAdd = MakeActionBtn("➕ Register Patient", 320, 35, 150, (s, e) => OpenRegisterPatientDialog());
            var btnEdit = MakeActionBtn("✏️ Edit", 480, 35, 80, (s, e) => EditSelectedPatient());
            var btnDel = MakeActionBtn("🗑️ Delete", 570, 35, 80, (s, e) => DeleteSelectedPatient());

            pnl.Controls.AddRange(new Control[] { lblS, _txtSearchPatient, btnAdd, btnEdit, btnDel });

            _gridPatients = MakeGrid();
            _gridPatients.Location = new Point(0, 68);
            _gridPatients.Size = new Size(860, 440);
            _gridPatients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridPatients.Columns.Add("ID", "ID Number");
            _gridPatients.Columns.Add("Name", "Full Name");
            _gridPatients.Columns.Add("Type", "Type");
            _gridPatients.Columns.Add("Course", "Course/Dept");
            _gridPatients.Columns.Add("Year", "Year");
            _gridPatients.Columns.Add("Gender", "Gender");
            _gridPatients.Columns.Add("Age", "Age");
            _gridPatients.Columns.Add("Blood", "Blood");
            _gridPatients.Columns.Add("Contact", "Contact");
            _gridPatients.Columns.Add("Allergies", "Allergies");
            pnl.Controls.Add(_gridPatients);

            return pnl;
        }

        // ─────── PHARMACY VIEW ───────
        private Panel BuildPharmacyView()
        {
            var pnl = new Panel { Dock = DockStyle.Fill };
            pnl.Controls.Add(new Label { Text = "Medicine Inventory", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = UITheme.TextDark, Location = new Point(0, 0), AutoSize = true });

            var lblS = new Label { Text = "Search:", Font = UITheme.LabelFont, Location = new Point(0, 38), AutoSize = true };
            _txtSearchMed = new TextBox { Location = new Point(55, 35), Width = 200, Font = UITheme.BodyFont };
            _txtSearchMed.TextChanged += (s, e) => RefreshPharmacy();

            _cboFilterMed = new ComboBox { Location = new Point(265, 35), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboFilterMed.Items.AddRange(new object[] { "All Items", "Low / Out of Stock", "In Stock" });
            _cboFilterMed.SelectedIndex = 0;
            _cboFilterMed.SelectedIndexChanged += (s, e) => RefreshPharmacy();

            var btnAdd = MakeActionBtn("➕ Add Medicine", 430, 35, 130, (s, e) => OpenAddMedicineDialog());
            var btnDisp = MakeActionBtn("💊 Dispense", 570, 35, 100, (s, e) => OpenDispenseFromPharmacy());
            var btnEdit = MakeActionBtn("✏️ Edit", 680, 35, 70, (s, e) => EditSelectedMedicine());
            var btnDel = MakeActionBtn("🗑️ Del", 760, 35, 60, (s, e) => DeleteSelectedMedicine());

            pnl.Controls.AddRange(new Control[] { lblS, _txtSearchMed, _cboFilterMed, btnAdd, btnDisp, btnEdit, btnDel });

            _gridPharmacy = MakeGrid();
            _gridPharmacy.Location = new Point(0, 68);
            _gridPharmacy.Size = new Size(860, 440);
            _gridPharmacy.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _gridPharmacy.Columns.Add("Brand", "Brand");
            _gridPharmacy.Columns.Add("Generic", "Generic Name");
            _gridPharmacy.Columns.Add("Category", "Category");
            _gridPharmacy.Columns.Add("Dosage", "Dosage");
            _gridPharmacy.Columns.Add("Stock", "Stock");
            _gridPharmacy.Columns.Add("Unit", "Unit");
            _gridPharmacy.Columns.Add("Reorder", "Reorder Lvl");
            _gridPharmacy.Columns.Add("Expiry", "Expiry Date");
            _gridPharmacy.Columns.Add("Status", "Status");
            pnl.Controls.Add(_gridPharmacy);

            return pnl;
        }

        // ─────── GRID FACTORY ───────
       
    
       
            // ... (other methods and code)

            // ─────── GRID FACTORY ───────
            private DataGridView MakeGrid()
            {
                var g = new DataGridView
                {
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                    GridColor = Color.FromArgb(240, 240, 240),
                    RowHeadersVisible = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    AllowUserToResizeRows = false,
                    ReadOnly = true,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells, // <--- Make sure this is AllCells
                    EnableHeadersVisualStyles = false,
                    Font = new Font("Segoe UI", 8.5F)
                };
                g.ColumnHeadersDefaultCellStyle.BackColor = UITheme.MaroonBanner;
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                g.ColumnHeadersHeight = 30;
                g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(254, 237, 240);
                g.DefaultCellStyle.SelectionForeColor = UITheme.PrimaryRed;
                g.DefaultCellStyle.Padding = new Padding(3);
                g.RowTemplate.Height = 28;
                return g;
            }

            // ... (rest of your code)
        
    

    private Button MakeActionBtn(string text, int x, int y, int w, EventHandler onClick)
        {
            var b = new Button { Text = text, Font = new Font("Segoe UI", 8.5F, FontStyle.Bold), BackColor = UITheme.PrimaryRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(w, 28), Location = new Point(x, y), Cursor = Cursors.Hand };
            b.FlatAppearance.BorderSize = 0;
            b.Click += onClick;
            return b;
        }

        // ─────── DATA REFRESH ───────
        private void RefreshAll()
        {
            var ds = DataService.Instance;

            // Dashboard cards
            _cardPatients.UpdateValue(ds.Patients.Count.ToString());
            _cardConsultToday.UpdateValue(ds.GetTodayVisits().Count.ToString());
            _cardResting.UpdateValue(ds.GetRestingInClinicCount().ToString());
            _cardLowStock.UpdateValue(ds.GetLowStockOrExpiredMedicines().Count.ToString());

            // Recent Consultations rows cleared intentionally
            _gridRecentConsult.Rows.Clear();

            // Low Stock grid (dashboard)
            _gridLowStock.Rows.Clear();
            foreach (var m in ds.GetLowStockOrExpiredMedicines())
            {
                string status = m.Status;
                string stockStr = m.StockQuantity + " " + m.Unit;
                if (status == "Expired") stockStr += " (Expired)";
                int r = _gridLowStock.Rows.Add(m.BrandName, stockStr, m.ReorderLevel + " " + m.Unit);
                _gridLowStock.Rows[r].DefaultCellStyle.ForeColor = UITheme.Danger;
            }

            RefreshPatients();
            RefreshPharmacy();
        }

        private void RefreshPatients()
        {
            string q = (_txtSearchPatient?.Text ?? "").Trim().ToLowerInvariant();
            var list = DataService.Instance.Patients.Where(p =>
                string.IsNullOrEmpty(q) ||
                (p.FullName ?? "").ToLowerInvariant().Contains(q) ||
                (p.StudentIdNumber ?? "").ToLowerInvariant().Contains(q) ||
                (p.CourseOrDepartment ?? "").ToLowerInvariant().Contains(q)
            ).ToList();

            _gridPatients.Rows.Clear();
            foreach (var p in list)
            {
                int r = _gridPatients.Rows.Add(p.StudentIdNumber, p.FullName, p.PatientType, p.CourseOrDepartment, p.YearLevel, p.Gender, p.Age, p.BloodType, p.ContactNumber, p.Allergies);
                _gridPatients.Rows[r].Tag = p;
            }
        }

        private void RefreshPharmacy()
        {
            string q = (_txtSearchMed?.Text ?? "").Trim().ToLowerInvariant();
            string filter = _cboFilterMed?.SelectedItem?.ToString() ?? "All Items";

            var list = DataService.Instance.Medicines.Where(m =>
                (string.IsNullOrEmpty(q) || (m.BrandName ?? "").ToLowerInvariant().Contains(q) || (m.GenericName ?? "").ToLowerInvariant().Contains(q) || (m.Category ?? "").ToLowerInvariant().Contains(q)) &&
                (filter == "All Items" ||
                 (filter == "Low / Out of Stock" && (m.StockQuantity <= m.ReorderLevel || m.StockQuantity == 0 || DateTime.Now > m.ExpirationDate)) ||
                 (filter == "In Stock" && m.StockQuantity > m.ReorderLevel && DateTime.Now <= m.ExpirationDate))
            ).ToList();

            _gridPharmacy.Rows.Clear();
            foreach (var m in list)
            {
                int r = _gridPharmacy.Rows.Add(m.BrandName, m.GenericName, m.Category, m.DosageForm, m.StockQuantity, m.Unit, m.ReorderLevel, m.ExpirationDate.ToString("yyyy-MM-dd"), m.Status);
                _gridPharmacy.Rows[r].Tag = m;
                if (m.Status != "In Stock") { _gridPharmacy.Rows[r].Cells["Status"].Style.ForeColor = UITheme.Danger; _gridPharmacy.Rows[r].Cells["Status"].Style.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold); }
                else { _gridPharmacy.Rows[r].Cells["Status"].Style.ForeColor = UITheme.Success; }
            }
        }

        // ─────── ACTIONS ───────
        private void OpenConsultationDialog()
        {
            using (var dlg = new ConsultationDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    DataService.Instance.AddVisit(dlg.VisitRecord);
                    RefreshAll();
                    MessageBox.Show("Consultation recorded!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OpenRegisterPatientDialog()
        {
            using (var dlg = new PatientDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    DataService.Instance.AddPatient(dlg.PatientRecord);
                    RefreshAll();
                    MessageBox.Show("Patient registered!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EditSelectedPatient()
        {
            if (_gridPatients.CurrentRow?.Tag is Patient p)
            {
                using (var dlg = new PatientDialog(p))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        DataService.Instance.UpdatePatient(dlg.PatientRecord);
                        RefreshAll();
                    }
                }
            }
            else MessageBox.Show("Select a patient first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteSelectedPatient()
        {
            if (_gridPatients.CurrentRow?.Tag is Patient p)
            {
                if (MessageBox.Show($"Delete '{p.FullName}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataService.Instance.DeletePatient(p.Id);
                    RefreshAll();
                }
            }
        }

        private void OpenAddMedicineDialog()
        {
            using (var dlg = new MedicineDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    DataService.Instance.AddMedicine(dlg.MedicineRecord);
                    RefreshAll();
                    MessageBox.Show("Medicine added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EditSelectedMedicine()
        {
            if (_gridPharmacy.CurrentRow?.Tag is Medicine m)
            {
                using (var dlg = new MedicineDialog(m))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        DataService.Instance.UpdateMedicine(dlg.MedicineRecord);
                        RefreshAll();
                    }
                }
            }
            else MessageBox.Show("Select a medicine first.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void DeleteSelectedMedicine()
        {
            if (_gridPharmacy.CurrentRow?.Tag is Medicine m)
            {
                if (MessageBox.Show($"Delete '{m.BrandName}'?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataService.Instance.DeleteMedicine(m.Id);
                    RefreshAll();
                }
            }
        }

        private void OpenDispenseFromPharmacy()
        {
            Medicine preselected = _gridPharmacy.CurrentRow?.Tag as Medicine;
            using (var dlg = new DispenseDialog(preselected))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshAll();
                    MessageBox.Show("Medicine dispensed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}