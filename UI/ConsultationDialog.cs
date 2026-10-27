using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Models;
using SchoolClinicManagementSystem.Services;

namespace SchoolClinicManagementSystem.UI
{
    public class ConsultationDialog : Form
    {
        public ClinicVisit VisitRecord { get; private set; }

        private ComboBox _cboPatient;
        private TextBox _txtBP, _txtTemp, _txtPulse;
        private ComboBox _cboComplaint;
        private TextBox _txtDiagnosis, _txtTreatment;
        private ComboBox _cboMedicine;
        private NumericUpDown _numQty;
        private Label _lblStock;
        private ComboBox _cboDisposition;
        private TextBox _txtStaff, _txtNotes;
        private Button _btnSave, _btnCancel;

        public ConsultationDialog(Patient preselect = null)
        {
            VisitRecord = new ClinicVisit { VisitNumber = DataService.Instance.GenerateVisitNumber() };
            InitializeComponent(preselect);
        }

        private void InitializeComponent(Patient preselect)
        {
            Text = "Record New Consultation - SCMS";
            ClientSize = new Size(560, 640);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;
            BackColor = Color.White;

            var header = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = UITheme.MaroonBanner };
            header.Controls.Add(new Label { Text = "Record Clinic Consultation", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 8), AutoSize = true });
            header.Controls.Add(new Label { Text = "Visit No: " + VisitRecord.VisitNumber, Font = UITheme.SmallFont, ForeColor = UITheme.SubtitleText, Location = new Point(20, 32), AutoSize = true });
            Controls.Add(header);

            var body = new Panel { Location = new Point(20, 65), Size = new Size(520, 505), AutoScroll = true };
            int y = 0;

            // Patient
            body.Controls.Add(new Label { Text = "Select Patient:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true });
            _cboPatient = new ComboBox { Location = new Point(0, y + 18), Width = 500, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            foreach (var p in DataService.Instance.Patients) _cboPatient.Items.Add(p);
            if (_cboPatient.Items.Count > 0)
            {
                if (preselect != null) { var m = DataService.Instance.Patients.FirstOrDefault(p => p.Id == preselect.Id); _cboPatient.SelectedItem = m ?? _cboPatient.Items[0]; }
                else _cboPatient.SelectedIndex = 0;
            }
            body.Controls.Add(_cboPatient); y += 50;

            // Vital Signs Group
            var grp = new GroupBox { Text = " Vital Signs ", Font = UITheme.LabelFont, ForeColor = UITheme.PrimaryRed, Location = new Point(0, y), Size = new Size(500, 70) };
            grp.Controls.Add(new Label { Text = "BP (mmHg):", Font = UITheme.SmallFont, ForeColor = UITheme.TextDark, Location = new Point(15, 22), AutoSize = true });
            _txtBP = new TextBox { Location = new Point(15, 40), Width = 100, Text = "120/80", Font = UITheme.BodyFont };
            grp.Controls.Add(_txtBP);
            grp.Controls.Add(new Label { Text = "Temp (°C):", Font = UITheme.SmallFont, ForeColor = UITheme.TextDark, Location = new Point(135, 22), AutoSize = true });
            _txtTemp = new TextBox { Location = new Point(135, 40), Width = 100, Text = "36.8", Font = UITheme.BodyFont };
            grp.Controls.Add(_txtTemp);
            grp.Controls.Add(new Label { Text = "Pulse (bpm):", Font = UITheme.SmallFont, ForeColor = UITheme.TextDark, Location = new Point(255, 22), AutoSize = true });
            _txtPulse = new TextBox { Location = new Point(255, 40), Width = 100, Text = "78", Font = UITheme.BodyFont };
            grp.Controls.Add(_txtPulse);
            body.Controls.Add(grp); y += 80;

            // Chief Complaint
            body.Controls.Add(new Label { Text = "Chief Complaint:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true });
            _cboComplaint = new ComboBox { Location = new Point(0, y + 18), Width = 500, DropDownStyle = ComboBoxStyle.DropDown, Font = UITheme.BodyFont };
            _cboComplaint.Items.AddRange(new object[] { "Headache / Migraine", "High Fever", "Dysmenorrhea", "Elevated BP & Headache", "Epigastric Pain / Acid Reflux", "Abrasion on right knee", "Allergic rhinitis", "Cough, Cold & Sneezing", "Dizziness / Vertigo", "Nausea / Vomiting", "Sprain / Strain (PE)", "General Malaise / Fatigue" });
            _cboComplaint.SelectedIndex = 0;
            body.Controls.Add(_cboComplaint); y += 50;

            AddRow(body, "Diagnosis:", _txtDiagnosis = new TextBox { Width = 500 }, ref y);
            AddRow(body, "Treatment Given:", _txtTreatment = new TextBox { Width = 500, Text = "Vital signs taken. Rest advised." }, ref y);

            // Medicine Group
            var grpM = new GroupBox { Text = " Dispense Medicine (Optional) ", Font = UITheme.LabelFont, ForeColor = UITheme.PrimaryRed, Location = new Point(0, y), Size = new Size(500, 70) };
            grpM.Controls.Add(new Label { Text = "Medicine:", Font = UITheme.SmallFont, ForeColor = UITheme.TextDark, Location = new Point(15, 22), AutoSize = true });
            _cboMedicine = new ComboBox { Location = new Point(15, 40), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboMedicine.Items.Add("-- None --");
            foreach (var m in DataService.Instance.Medicines) _cboMedicine.Items.Add(m);
            _cboMedicine.SelectedIndex = 0;
            grpM.Controls.Add(_cboMedicine);
            grpM.Controls.Add(new Label { Text = "Qty:", Font = UITheme.SmallFont, ForeColor = UITheme.TextDark, Location = new Point(280, 22), AutoSize = true });
            _numQty = new NumericUpDown { Location = new Point(280, 40), Width = 60, Minimum = 1, Maximum = 50, Value = 1, Font = UITheme.BodyFont };
            grpM.Controls.Add(_numQty);
            _lblStock = new Label { Text = "", Font = UITheme.SmallFont, Location = new Point(350, 42), AutoSize = true };
            grpM.Controls.Add(_lblStock);
            _cboMedicine.SelectedIndexChanged += (s, e) => { if (_cboMedicine.SelectedItem is Medicine med) { _lblStock.Text = $"Stock: {med.StockQuantity} {med.Unit}"; _lblStock.ForeColor = med.StockQuantity <= med.ReorderLevel ? UITheme.Danger : UITheme.Success; } else _lblStock.Text = ""; };
            body.Controls.Add(grpM); y += 80;

            // Disposition
            body.Controls.Add(new Label { Text = "Disposition:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true });
            _cboDisposition = new ComboBox { Location = new Point(0, y + 18), Width = 240, DropDownStyle = ComboBoxStyle.DropDownList, Font = UITheme.BodyFont };
            _cboDisposition.Items.AddRange(new object[] { "Returned to Class", "Bed Rest in Clinic", "Sent Home", "Referred to Hospital" });
            _cboDisposition.SelectedIndex = 0;
            body.Controls.Add(_cboDisposition);
            body.Controls.Add(new Label { Text = "Attending Staff:", Font = UITheme.LabelFont, Location = new Point(260, y), AutoSize = true });
            _txtStaff = new TextBox { Location = new Point(260, y + 18), Width = 240, Text = AuthService.Instance.CurrentUser?.FullName ?? "School Nurse, RN", Font = UITheme.BodyFont };
            body.Controls.Add(_txtStaff); y += 50;

            AddRow(body, "Notes:", _txtNotes = new TextBox { Width = 500, Text = "Patient advised to return if symptoms persist." }, ref y);

            Controls.Add(body);

            var footer = new Panel { Dock = DockStyle.Bottom, Height = 55, BackColor = UITheme.WindowBg };
            _btnSave = new Button { Text = "Record Consultation", Font = UITheme.ButtonFont, BackColor = UITheme.PrimaryRed, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(170, 36), Location = new Point(230, 10), Cursor = Cursors.Hand };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;
            _btnCancel = new Button { Text = "Cancel", Font = UITheme.BodyFont, BackColor = Color.White, ForeColor = UITheme.TextDark, FlatStyle = FlatStyle.Flat, Size = new Size(90, 36), Location = new Point(410, 10), Cursor = Cursors.Hand };
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
            if (!(_cboPatient.SelectedItem is Patient p)) { MessageBox.Show("Select a patient.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            VisitRecord.PatientId = p.Id;
            VisitRecord.PatientName = p.FullName;
            VisitRecord.PatientType = p.PatientType;
            VisitRecord.CourseOrDepartment = p.CourseOrDepartment;
            VisitRecord.VisitDate = DateTime.Now;
            VisitRecord.BloodPressure = _txtBP.Text.Trim();
            VisitRecord.BodyTemperature = _txtTemp.Text.Trim();
            VisitRecord.PulseRate = _txtPulse.Text.Trim();
            VisitRecord.ChiefComplaint = _cboComplaint.Text.Trim();
            VisitRecord.Diagnosis = _txtDiagnosis.Text.Trim();
            VisitRecord.TreatmentGiven = _txtTreatment.Text.Trim();
            if (_cboMedicine.SelectedItem is Medicine med) { VisitRecord.MedicineDispensed = med.BrandName; VisitRecord.QuantityDispensed = (int)_numQty.Value; }
            else { VisitRecord.MedicineDispensed = "None"; VisitRecord.QuantityDispensed = 0; }
            VisitRecord.Disposition = _cboDisposition.SelectedItem?.ToString() ?? "Returned to Class";
            VisitRecord.AttendingStaff = _txtStaff.Text.Trim();
            VisitRecord.Notes = _txtNotes.Text.Trim();
            DialogResult = DialogResult.OK; Close();
        }
    }
}
