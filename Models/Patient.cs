using System;

namespace SchoolClinicManagementSystem.Models
{
    public class Patient
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string StudentIdNumber { get; set; }
        public string FullName { get; set; }
        public string PatientType { get; set; } // "Student", "Faculty", "Staff"
        public string CourseOrDepartment { get; set; }
        public string YearLevel { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string BloodType { get; set; }
        public string Allergies { get; set; }
        public string MedicalConditions { get; set; }
        public DateTime DateRegistered { get; set; } = DateTime.Now;

        public string DisplaySummary => $"{StudentIdNumber} - {FullName} ({CourseOrDepartment})";

        public override string ToString()
        {
            return $"{StudentIdNumber} - {FullName} ({CourseOrDepartment})";
        }
    }
}
