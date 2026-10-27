using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Models;
using SchoolClinicManagementSystem.Services;

namespace SchoolClinicManagementSystem.UI
{
    public class DispenseDialog : Form
    {
        private ComboBox _cboMedicine;
        private Label _lblStockInfo;
        private NumericUpDown _numQty;
        private TextBox _txtRecipient;
        private ComboBox _cboType;
        private TextBox _txtCourse;
        private ComboBox _cboReason;
        private TextBox _txtDispensedBy;
        private TextBox _txtNotes;

        private Button _btnDispense;
        private Button _btnCancel;

        public DispenseDialog(Medicine preselected = null)
        {
            InitializeComponent(preselected);
        }

        private void InitializeComponent(Medicine preselected)
        {
            Text = "Dispense Medicine - CCT School Clinic";
            ClientSize = new Size(520, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Header Banner
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = UITheme.MaroonBanner
            };
            var lblTitle = new Label
            {
                Text = "Dispense Clinic Medicine / Supplies",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true
            };
            var lblSubtitle = new Label
            {
                Text = "City College of Tagaytay School Clinic Pharmacy",
                Font = UITheme.SmallFont,
                ForeColor = UITheme.SubtitleText,
                Location = new Point(20, 34),
                AutoSize = true
            };
            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSubtitle);
            Controls.Add(header);

            var body = new Panel
            {
                Location = new Point(20, 75),
                Size = new Size(480, 415),
                AutoScroll = true
            };

            int y = 5;

            // Medicine Selection
            var lblMed = new Label { Text = "Select Medicine:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboMedicine = new ComboBox { Location = new Point(0, y + 20), Width = 450, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            foreach (var m in DataService.Instance.Medicines)
            {
                _cboMedicine.Items.Add(m);
            }
            if (_cboMedicine.Items.Count > 0)
            {
                if (preselected != null)
                {
                    var match = DataService.Instance.Medicines.FirstOrDefault(m => m.Id == preselected.Id);
                    _cboMedicine.SelectedItem = match ?? _cboMedicine.Items[0];
                }
                else
                {
                    _cboMedicine.SelectedIndex = 0;
                }
            }
            body.Controls.Add(lblMed);
            body.Controls.Add(_cboMedicine);
            y += 55;

            // Quantity & Stock info
            var lblQty = new Label { Text = "Quantity to Dispense:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _numQty = new NumericUpDown { Location = new Point(0, y + 20), Width = 140, Minimum = 1, Maximum = 100, Value = 1, Font = UITheme.BodyFont };

            _lblStockInfo = new Label { Text = "", Font = UITheme.LabelFont, Location = new Point(160, y + 22), AutoSize = true };

            _cboMedicine.SelectedIndexChanged += (s, e) =>
            {
                if (_cboMedicine.SelectedItem is Medicine m)
                {
                    _lblStockInfo.Text = $"Available Stock: {m.StockQuantity} {m.Unit}";
                    _lblStockInfo.ForeColor = m.StockQuantity <= m.ReorderLevel ? UITheme.Danger : UITheme.Success;
                    _numQty.Maximum = Math.Max(1, m.StockQuantity);
                }
            };
            // Trigger once
            if (_cboMedicine.SelectedItem is Medicine initial)
            {
                _lblStockInfo.Text = $"Available Stock: {initial.StockQuantity} {initial.Unit}";
                _lblStockInfo.ForeColor = initial.StockQuantity <= initial.ReorderLevel ? UITheme.Danger : UITheme.Success;
                _numQty.Maximum = Math.Max(1, initial.StockQuantity);
            }

            body.Controls.AddRange(new Control[] { lblQty, _numQty, _lblStockInfo });
            y += 55;

            // Recipient Name
            AddLabelAndControl(body, "Recipient Name (Student or Staff):", _txtRecipient = new TextBox { Width = 450, Text = "" }, ref y);

            // Recipient Type & Course
            var lblType = new Label { Text = "Recipient Role:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboType = new ComboBox { Location = new Point(0, y + 20), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboType.Items.AddRange(new object[] { "Student", "Faculty", "Staff", "Visitor" });
            _cboType.SelectedIndex = 0;

            var lblCourse = new Label { Text = "Course / Department / Section:", Font = UITheme.LabelFont, Location = new Point(200, y), AutoSize = true };
            _txtCourse = new TextBox { Location = new Point(200, y + 20), Width = 250, Font = UITheme.BodyFont, Text = "BSIT 3-A" };

            body.Controls.AddRange(new Control[] { lblType, _cboType, lblCourse, _txtCourse });
            y += 55;

            // Reason / Chief Complaint
            var lblReason = new Label { Text = "Reason / Symptoms:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboReason = new ComboBox { Location = new Point(0, y + 20), Width = 450, DropDownStyle = ComboBoxStyle.DropDown, Font = UITheme.BodyFont };
            _cboReason.Items.AddRange(new object[] {
                "Headache / Migraine",
                "Fever / Chills / Elevated Body Temp",
                "Stomachache / Abdominal Pain / Hyperacidity",
                "Dysmenorrhea / Menstrual Cramps",
                "Cough, Cold & Sneezing",
                "Dizziness / Vertigo / Lightheadedness",
                "Allergic Reaction / Skin Rash",
                "Minor Wound / First Aid Cleaning"
            });
            _cboReason.SelectedIndex = 0;
            body.Controls.Add(lblReason);
            body.Controls.Add(_cboReason);
            y += 55;

            // Dispensed By
            AddLabelAndControl(body, "Dispensing Nurse / Clinic Staff:", _txtDispensedBy = new TextBox { Width = 450, Text = AuthService.Instance.CurrentUser?.FullName ?? "School Nurse, RN" }, ref y);

            // Notes
            AddLabelAndControl(body, "Directions / Instructions Given:", _txtNotes = new TextBox { Width = 450, Text = "Take 1 tablet after meals with plenty of water. Advised clinic rest." }, ref y);

            Controls.Add(body);

            // Footer
            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = UITheme.WindowBg
            };

            _btnDispense = new Button
            {
                Text = "Confirm & Dispense",
                Font = UITheme.ButtonFont,
                BackColor = UITheme.PrimaryRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 36),
                Location = new Point(190, 12),
                Cursor = Cursors.Hand
            };
            _btnDispense.FlatAppearance.BorderSize = 0;
            _btnDispense.Click += BtnDispense_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                Font = UITheme.BodyFont,
                BackColor = Color.White,
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 36),
                Location = new Point(380, 12),
                Cursor = Cursors.Hand
            };
            _btnCancel.FlatAppearance.BorderColor = UITheme.BorderDefault;
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            footer.Controls.Add(_btnDispense);
            footer.Controls.Add(_btnCancel);
            Controls.Add(footer);

            AcceptButton = _btnDispense;
            CancelButton = _btnCancel;
        }

        private void AddLabelAndControl(Panel container, string labelText, Control ctrl, ref int y)
        {
            var lbl = new Label
            {
                Text = labelText,
                Font = UITheme.LabelFont,
                ForeColor = UITheme.TextDark,
                Location = new Point(0, y),
                AutoSize = true
            };
            ctrl.Location = new Point(0, y + 20);
            ctrl.Font = UITheme.BodyFont;
            container.Controls.Add(lbl);
            container.Controls.Add(ctrl);
            y += 55;
        }

        private void BtnDispense_Click(object sender, EventArgs e)
        {
            if (!(_cboMedicine.SelectedItem is Medicine med))
            {
                MessageBox.Show("Please select a medicine to dispense.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int qty = (int)_numQty.Value;
            if (qty > med.StockQuantity)
            {
                MessageBox.Show($"Cannot dispense {qty} {med.Unit}. Only {med.StockQuantity} available in stock.", "Insufficient Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_txtRecipient.Text))
            {
                MessageBox.Show("Please enter the recipient's name (student or staff).", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtRecipient.Focus();
                return;
            }

            bool success = DataService.Instance.DispenseMedicine(
                med.Id,
                qty,
                _txtRecipient.Text.Trim(),
                _cboType.SelectedItem?.ToString() ?? "Student",
                _txtCourse.Text.Trim(),
                _cboReason.Text.Trim(),
                _txtDispensedBy.Text.Trim(),
                _txtNotes.Text.Trim()
            );

            if (success)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Failed to dispense medicine. Check available stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
