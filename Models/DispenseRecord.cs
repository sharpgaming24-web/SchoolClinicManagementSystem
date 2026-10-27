using System;

namespace SchoolClinicManagementSystem.Models
{
    public class DispenseRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string MedicineId { get; set; }
        public string BrandName { get; set; }
        public string GenericName { get; set; }
        public string DosageForm { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string RecipientName { get; set; }     // e.g. "Juan Dela Cruz"
        public string RecipientType { get; set; }     // "Student", "Faculty", "Staff"
        public string CourseOrDepartment { get; set; } // e.g. "BSIT 3-A"
        public string ReasonOrComplaint { get; set; } // e.g. "Headache / Fever"
        public string DispensedBy { get; set; }       // e.g. "Maria Santos, RN"
        public DateTime DispensedAt { get; set; } = DateTime.Now;
        public string Notes { get; set; }
    }
}
