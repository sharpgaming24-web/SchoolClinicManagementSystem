using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using SchoolClinicManagementSystem.Models;

namespace SchoolClinicManagementSystem.Services
{
    public class ClinicDataStorage
    {
        public List<Patient> Patients { get; set; } = new List<Patient>();
        public List<ClinicVisit> Visits { get; set; } = new List<ClinicVisit>();
        public List<Medicine> Medicines { get; set; } = new List<Medicine>();
        public List<DispenseRecord> DispenseRecords { get; set; } = new List<DispenseRecord>();
        public int NextVisitNumber { get; set; } = 1;
    }

    public class DataService
    {
        private static DataService _instance;
        public static DataService Instance => _instance ?? (_instance = new DataService());

        private readonly string _dataFilePath;
        private readonly JavaScriptSerializer _serializer;
        private ClinicDataStorage _storage;

        public List<Patient> Patients => _storage.Patients;
        public List<ClinicVisit> Visits => _storage.Visits;
        public List<Medicine> Medicines => _storage.Medicines;
        public List<DispenseRecord> DispenseRecords => _storage.DispenseRecords;

        public DataService()
        {
            _serializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };

            string baseDir = null;
            try
            {
                baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch { }

            if (string.IsNullOrEmpty(baseDir) || !Directory.Exists(baseDir))
                baseDir = AppDomain.CurrentDomain.BaseDirectory;

            string appData = Path.Combine(baseDir, "Data");
            try
            {
                if (!Directory.Exists(appData))
                    Directory.CreateDirectory(appData);
            }
            catch
            {
                appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCT_SCMS", "Data");
                if (!Directory.Exists(appData))
                    Directory.CreateDirectory(appData);
            }

            _dataFilePath = Path.Combine(appData, "clinic_data.json");
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    string json = File.ReadAllText(_dataFilePath);
                    _storage = _serializer.Deserialize<ClinicDataStorage>(json);
                }
            }
            catch { _storage = null; }

            // Initialize clean, empty storage if file doesn't exist or failed to load
            if (_storage == null)
            {
                _storage = new ClinicDataStorage();
                SaveData();
            }
        }

        public void SaveData()
        {
            try
            {
                string json = _serializer.Serialize(_storage);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error saving: " + ex.Message);
            }
        }

        public string GenerateVisitNumber()
        {
            string num = $"VST-{DateTime.Now.Year}-{_storage.NextVisitNumber:D4}";
            _storage.NextVisitNumber++;
            SaveData();
            return num;
        }

        // Patient CRUD
        public void AddPatient(Patient p) { if (p != null) { _storage.Patients.Add(p); SaveData(); } }
        public void UpdatePatient(Patient p) { if (p == null) return; var i = _storage.Patients.FindIndex(x => x.Id == p.Id); if (i >= 0) { _storage.Patients[i] = p; SaveData(); } }
        public void DeletePatient(string id) { _storage.Patients.RemoveAll(x => x.Id == id); SaveData(); }
        public Patient GetPatientById(string id) { return _storage.Patients.FirstOrDefault(x => x.Id == id); }

        // Visit CRUD
        public void AddVisit(ClinicVisit v)
        {
            if (v == null) return;
            _storage.Visits.Insert(0, v);

            // Deduct medicine stock
            if (!string.IsNullOrEmpty(v.MedicineDispensed) && v.QuantityDispensed > 0 && v.MedicineDispensed != "None")
            {
                var med = _storage.Medicines.FirstOrDefault(m =>
                    m.BrandName.Equals(v.MedicineDispensed, StringComparison.OrdinalIgnoreCase) ||
                    m.GenericName.Equals(v.MedicineDispensed, StringComparison.OrdinalIgnoreCase));
                if (med != null)
                    med.StockQuantity = Math.Max(0, med.StockQuantity - v.QuantityDispensed);
            }
            SaveData();
        }

        public List<ClinicVisit> GetTodayVisits()
        {
            return _storage.Visits.Where(v => v.VisitDate.Date == DateTime.Today).ToList();
        }

        public int GetRestingInClinicCount()
        {
            return _storage.Visits.Count(v => v.VisitDate.Date == DateTime.Today && v.Disposition == "Bed Rest in Clinic");
        }

        // Medicine CRUD
        public void AddMedicine(Medicine m) { if (m != null) { _storage.Medicines.Add(m); SaveData(); } }
        public void UpdateMedicine(Medicine m) { if (m == null) return; var i = _storage.Medicines.FindIndex(x => x.Id == m.Id); if (i >= 0) { _storage.Medicines[i] = m; SaveData(); } }
        public void DeleteMedicine(string id) { _storage.Medicines.RemoveAll(x => x.Id == id); SaveData(); }

        public bool DispenseMedicine(string medId, int qty, string recipient, string recipientType, string course, string reason, string dispensedBy, string notes)
        {
            var med = _storage.Medicines.FirstOrDefault(m => m.Id == medId);
            if (med != null && med.StockQuantity >= qty)
            {
                med.StockQuantity -= qty;
                _storage.DispenseRecords.Insert(0, new DispenseRecord
                {
                    MedicineId = med.Id,
                    BrandName = med.BrandName,
                    GenericName = med.GenericName,
                    DosageForm = med.DosageForm,
                    Quantity = qty,
                    Unit = med.Unit,
                    RecipientName = recipient,
                    RecipientType = recipientType,
                    CourseOrDepartment = course,
                    ReasonOrComplaint = reason,
                    DispensedBy = dispensedBy,
                    DispensedAt = DateTime.Now,
                    Notes = notes
                });
                SaveData();
                return true;
            }
            return false;
        }

        public List<Medicine> GetLowStockOrExpiredMedicines()
        {
            return _storage.Medicines.Where(m => m.StockQuantity <= m.ReorderLevel || m.StockQuantity == 0 || DateTime.Now > m.ExpirationDate).ToList();
        }
    }
}