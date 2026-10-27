using System;

namespace SchoolClinicManagementSystem.Models
{
    public class ClinicVisit
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string VisitNumber { get; set; } // e.g. "VST-2026-0007"
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientType { get; set; }
        public string CourseOrDepartment { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;

        // Vital Signs
        public string BloodPressure { get; set; }
        public string BodyTemperature { get; set; }
        public string PulseRate { get; set; }

        // Consultation
        public string ChiefComplaint { get; set; }
        public string Diagnosis { get; set; }
        public string TreatmentGiven { get; set; }
        public string MedicineDispensed { get; set; }
        public int QuantityDispensed { get; set; }

        public string Disposition { get; set; } // "Returned to Class", "Bed Rest in Clinic", "Sent Home", "Referred to Hospital"
        public string AttendingStaff { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            return $"{VisitNumber} - {PatientName} ({ChiefComplaint})";
        }
    }
}
