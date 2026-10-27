using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Models;

namespace SchoolClinicManagementSystem.UI
{
    public class PatientDialog : Form
    {
        public Patient PatientRecord { get; private set; }

        private TextBox _txtIdNumber, _txtName, _txtContact, _txtEmergencyName, _txtEmergencyContact, _txtAllergies, _txtConditions;
        private ComboBox _cboType, _cboCourse, _cboYear, _cboGender, _cboBlood;
        private NumericUpDown _numAge;
        private Button _btnSave, _btnCancel;

        public PatientDialog(Patient existing = null)
        {
            PatientRecord = existing != null ? new Patient
            {
                Id = existing.Id, StudentIdNumber = existing.StudentIdNumber, FullName = existing.FullName,
                PatientType = existing.PatientType, CourseOrDepartment = existing.CourseOrDepartment,
                YearLevel = existing.YearLevel, Gender = existing.Gender, Age = existing.Age,
                DateOfBirth = existing.DateOfBirth, ContactNumber = existing.ContactNumber,
                EmergencyContactName = existing.EmergencyContactName, EmergencyContactNumber = existing.EmergencyContactNumber,
                BloodType = existing.BloodType, Allergies = existing.Allergies, MedicalConditions = existing.MedicalConditions,
                DateRegistered = existing.DateRegistered
            } : new Patient();

            InitializeComponent(existing != null);
        }

        private void InitializeComponent(bool isEdit)
        {
            Text = isEdit ? "Edit Patient Record - SCMS" : "Register New Patient - SCMS";
            ClientSize = new Size(540, 570);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = Color.White;

            var header = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = UITheme.MaroonBanner };
            header.Controls.Add(new Label { Text = isEdit ? "Update Patient Profile" : "Register New Patient", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 8), AutoSize = true });
            header.Controls.Add(new Label { Text = "City College of Tagaytay School Clinic", Font = UITheme.SmallFont, ForeColor = UITheme.SubtitleText, Location = new Point(20, 32), AutoSize = true });
            Controls.Add(header);

            var body = new Panel { Location = new Point(20, 68), Size = new Size(500, 430), AutoScroll = true };
            int y = 0;

            AddRow(body, "Student / Employee ID:", _txtIdNumber = new TextBox { Width = 210, Text = PatientRecord.StudentIdNumber ?? "CCT-" + DateTime.Now.Year + "-" }, ref y);
            AddRow(body, "Full Name:", _txtName = new TextBox { Width = 460, Text = PatientRecord.FullName ?? "" }, ref y);

            // Type + Course side by side
            var lblT = new Label { Text = "Patient Type:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboType = new ComboBox { Location = new Point(0, y + 18), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboType.Items.AddRange(new object[] { "Student", "Faculty", "Staff" });
            _cboType.SelectedItem = PatientRecord.PatientType ?? "Student";
            var lblC = new Label { Text = "Course / Department:", Font = UITheme.LabelFont, Location = new Point(240, y), AutoSize = true };
            _cboCourse = new ComboBox { Location = new Point(240, y + 18), Width = 220, DropDownStyle = ComboBoxStyle.DropDown, Font = UITheme.BodyFont };
            _cboCourse.Items.AddRange(new object[] { "BSIT", "BSCS", "BSHM", "BSTM", "BSBA", "BSED", "Senior High", "College of Computer Studies", "Admin Office" });
            _cboCourse.Text = PatientRecord.CourseOrDepartment ?? "BSIT";
            body.Controls.AddRange(new Control[] { lblT, _cboType, lblC, _cboCourse });
            y += 50;

            // Year + Gender + Age
            var lblY = new Label { Text = "Year Level:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboYear = new ComboBox { Location = new Point(0, y + 18), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboYear.Items.AddRange(new object[] { "1st Year", "2nd Year", "3rd Year", "4th Year", "Grade 11", "Grade 12", "N/A" });
            _cboYear.SelectedItem = PatientRecord.YearLevel ?? "1st Year";
            var lblG = new Label { Text = "Gender:", Font = UITheme.LabelFont, Location = new Point(150, y), AutoSize = true };
            _cboGender = new ComboBox { Location = new Point(150, y + 18), Width = 130, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboGender.Items.AddRange(new object[] { "Male", "Female" });
            _cboGender.SelectedItem = PatientRecord.Gender ?? "Male";
            var lblA = new Label { Text = "Age:", Font = UITheme.LabelFont, Location = new Point(300, y), AutoSize = true };
            _numAge = new NumericUpDown { Location = new Point(300, y + 18), Width = 160, Minimum = 5, Maximum = 100, Value = PatientRecord.Age > 0 ? PatientRecord.Age : 19, Font = UITheme.BodyFont };
            body.Controls.AddRange(new Control[] { lblY, _cboYear, lblG, _cboGender, lblA, _numAge });
            y += 50;

            // Contact + Blood
            var lblCon = new Label { Text = "Contact Number:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _txtContact = new TextBox { Location = new Point(0, y + 18), Width = 220, Text = PatientRecord.ContactNumber ?? "09", Font = UITheme.BodyFont };
            var lblB = new Label { Text = "Blood Type:", Font = UITheme.LabelFont, Location = new Point(240, y), AutoSize = true };
            _cboBlood = new ComboBox { Location = new Point(240, y + 18), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboBlood.Items.AddRange(new object[] { "O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-", "Unknown" });
            _cboBlood.SelectedItem = PatientRecord.BloodType ?? "O+";
            body.Controls.AddRange(new Control[] { lblCon, _txtContact, lblB, _cboBlood });
            y += 50;

            AddRow(body, "Emergency Contact (Name):", _txtEmergencyName = new TextBox { Width = 460, Text = PatientRecord.EmergencyContactName ?? "" }, ref y);
            AddRow(body, "Emergency Contact Phone:", _txtEmergencyContact = new TextBox { Width = 460, Text = PatientRecord.EmergencyContactNumber ?? "" }, ref y);
            AddRow(body, "Known Allergies:", _txtAllergies = new TextBox { Width = 460, Text = PatientRecord.Allergies ?? "None" }, ref y);
            AddRow(body, "Medical Conditions:", _txtConditions = new TextBox { Width = 460, Text = PatientRecord.MedicalConditions ?? "None" }, ref y);
            Controls.Add(body);

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 55, BackColor = UITheme.WindowBg };
            _btnSave = new Button { Text = isEdit ? "Update Record" : "Register Patient", Font = UITheme.ButtonFont, BackColor = UITheme.PrimaryRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(150, 36), Location = new Point(230, 10), Cursor = Cursors.Hand };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;
            _btnCancel = new Button { Text = "Cancel", Font = UITheme.BodyFont, BackColor = Color.White, ForeColor = UITheme.TextDark, FlatStyle = FlatStyle.Flat, Size = new Size(90, 36), Location = new Point(390, 10), Cursor = Cursors.Hand };
            _btnCancel.FlatAppearance.BorderColor = UITheme.BorderDefault;
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;
            footer.Controls.Add(_btnSave); footer.Controls.Add(_btnCancel);
            Controls.Add(footer);
            AcceptButton = _btnSave; CancelButton = _btnCancel;
        }

        private void AddRow(Panel c, string label, Control ctrl, ref int y)
        {
            c.Controls.Add(new Label { Text = label, Font = UITheme.LabelFont, ForeColor = UITheme.TextDark, Location = new Point(0, y), AutoSize = true });
            ctrl.Location = new Point(0, y + 18); ctrl.Font = UITheme.BodyFont;
            c.Controls.Add(ctrl); y += 48;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtIdNumber.Text)) { MessageBox.Show("Please enter the ID number.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(_txtName.Text)) { MessageBox.Show("Please enter the full name.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            PatientRecord.StudentIdNumber = _txtIdNumber.Text.Trim();
            PatientRecord.FullName = _txtName.Text.Trim();
            PatientRecord.PatientType = _cboType.SelectedItem?.ToString() ?? "Student";
            PatientRecord.CourseOrDepartment = _cboCourse.Text.Trim();
            PatientRecord.YearLevel = _cboYear.SelectedItem?.ToString() ?? "1st Year";
            PatientRecord.Gender = _cboGender.SelectedItem?.ToString() ?? "Male";
            PatientRecord.Age = (int)_numAge.Value;
            PatientRecord.ContactNumber = _txtContact.Text.Trim();
            PatientRecord.BloodType = _cboBlood.SelectedItem?.ToString() ?? "Unknown";
            PatientRecord.EmergencyContactName = _txtEmergencyName.Text.Trim();
            PatientRecord.EmergencyContactNumber = _txtEmergencyContact.Text.Trim();
            PatientRecord.Allergies = _txtAllergies.Text.Trim();
            PatientRecord.MedicalConditions = _txtConditions.Text.Trim();
            DialogResult = DialogResult.OK; Close();
        }
    }
}
