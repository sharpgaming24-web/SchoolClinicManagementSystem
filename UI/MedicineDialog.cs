using System;
using System.Drawing;
using System.Windows.Forms;
using SchoolClinicManagementSystem.Models;

namespace SchoolClinicManagementSystem.UI
{
    public class MedicineDialog : Form
    {
        public Medicine MedicineRecord { get; private set; }

        private TextBox _txtBrand;
        private TextBox _txtGeneric;
        private ComboBox _cboCategory;
        private TextBox _txtDosage;
        private NumericUpDown _numStock;
        private TextBox _txtUnit;
        private NumericUpDown _numReorder;
        private DateTimePicker _dtpExpiry;

        private Button _btnSave;
        private Button _btnCancel;

        public MedicineDialog(Medicine existing = null)
        {
            MedicineRecord = existing != null ? new Medicine
            {
                Id = existing.Id,
                BrandName = existing.BrandName,
                GenericName = existing.GenericName,
                Category = existing.Category,
                DosageForm = existing.DosageForm,
                StockQuantity = existing.StockQuantity,
                Unit = existing.Unit,
                ReorderLevel = existing.ReorderLevel,
                ExpirationDate = existing.ExpirationDate
            } : new Medicine
            {
                ExpirationDate = DateTime.Today.AddYears(1)
            };

            InitializeComponent(existing != null);
        }

        private void InitializeComponent(bool isEdit)
        {
            Text = isEdit ? "Update Medicine / Supply - SCMS" : "Add Medicine / Medical Supply - SCMS";
            ClientSize = new Size(500, 520);
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
                Text = isEdit ? "Update Pharmacy Inventory Item" : "New Medicine / Medical Supply Item",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 10),
                AutoSize = true
            };
            var lblSubtitle = new Label
            {
                Text = "City College of Tagaytay School Clinic Pharmacy Registry",
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
                Size = new Size(460, 370)
            };

            int y = 5;
            AddLabelAndControl(body, "Brand Name:", _txtBrand = new TextBox { Width = 430, Text = MedicineRecord.BrandName ?? "" }, ref y);
            AddLabelAndControl(body, "Generic Name :", _txtGeneric = new TextBox { Width = 430, Text = MedicineRecord.GenericName ?? "" }, ref y);

            var lblCat = new Label { Text = "Category:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _cboCategory = new ComboBox { Location = new Point(0, y + 20), Width = 430, DropDownStyle = ComboBoxStyle.DropDown, Font = UITheme.BodyFont };
            _cboCategory.Items.AddRange(new object[] {
                "Analgesic / Antipyretic (Pain & Fever)",
                "Antacid / Antiflatulent (Stomach)",
                "Antihistamine / Allergy Relief",
                "Antibiotic",
                "Antispasmodic (Cramps)",
                "Decongestant / Cold Relief",
                "First Aid Supplies & Antiseptics",
                "Vitamins & Supplements"
            });
            _cboCategory.SelectedItem = MedicineRecord.Category ?? "Analgesic / Antipyretic (Pain & Fever)";
            body.Controls.Add(lblCat);
            body.Controls.Add(_cboCategory);
            y += 55;

            AddLabelAndControl(body, "Dosage Form:", _txtDosage = new TextBox { Width = 430, Text = MedicineRecord.DosageForm ?? "" }, ref y);

            // Stock & Unit
            var lblStock = new Label { Text = "Current Stock:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _numStock = new NumericUpDown { Location = new Point(0, y + 20), Width = 130, Minimum = 0, Maximum = 10000, Value = MedicineRecord.StockQuantity, Font = UITheme.BodyFont };

            var lblUnit = new Label { Text = "Unit:", Font = UITheme.LabelFont, Location = new Point(150, y), AutoSize = true };
            _txtUnit = new TextBox { Location = new Point(150, y + 20), Width = 130, Text = MedicineRecord.Unit ?? "tablets", Font = UITheme.BodyFont };

            var lblReorder = new Label { Text = "Low Stock Alert Level:", Font = UITheme.LabelFont, Location = new Point(300, y), AutoSize = true };
            _numReorder = new NumericUpDown { Location = new Point(300, y + 20), Width = 130, Minimum = 1, Maximum = 500, Value = MedicineRecord.ReorderLevel > 0 ? MedicineRecord.ReorderLevel : 15, Font = UITheme.BodyFont };

            body.Controls.AddRange(new Control[] { lblStock, _numStock, lblUnit, _txtUnit, lblReorder, _numReorder });
            y += 55;

            // Expiry Date
            var lblExp = new Label { Text = "Expiration Date:", Font = UITheme.LabelFont, Location = new Point(0, y), AutoSize = true };
            _dtpExpiry = new DateTimePicker { Location = new Point(0, y + 20), Width = 250, Font = UITheme.BodyFont, Format = DateTimePickerFormat.Short, Value = MedicineRecord.ExpirationDate > DateTime.MinValue ? MedicineRecord.ExpirationDate : DateTime.Today.AddYears(1) };
            body.Controls.Add(lblExp);
            body.Controls.Add(_dtpExpiry);

            Controls.Add(body);

            // Footer
            var footer = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = UITheme.WindowBg
            };

            _btnSave = new Button
            {
                Text = "Save Item",
                Font = UITheme.ButtonFont,
                BackColor = UITheme.PrimaryRed,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(130, 36),
                Location = new Point(220, 12),
                Cursor = Cursors.Hand
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                Font = UITheme.BodyFont,
                BackColor = Color.White,
                ForeColor = UITheme.TextDark,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 36),
                Location = new Point(360, 12),
                Cursor = Cursors.Hand
            };
            _btnCancel.FlatAppearance.BorderColor = UITheme.BorderDefault;
            _btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            footer.Controls.Add(_btnSave);
            footer.Controls.Add(_btnCancel);
            Controls.Add(footer);

            AcceptButton = _btnSave;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtBrand.Text))
            {
                MessageBox.Show("Please enter the brand name.", "Required Field", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _txtBrand.Focus();
                return;
            }

            MedicineRecord.BrandName = _txtBrand.Text.Trim();
            MedicineRecord.GenericName = _txtGeneric.Text.Trim();
            MedicineRecord.Category = _cboCategory.Text.Trim();
            MedicineRecord.DosageForm = _txtDosage.Text.Trim();
            MedicineRecord.StockQuantity = (int)_numStock.Value;
            MedicineRecord.Unit = _txtUnit.Text.Trim();
            MedicineRecord.ReorderLevel = (int)_numReorder.Value;
            MedicineRecord.ExpirationDate = _dtpExpiry.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
