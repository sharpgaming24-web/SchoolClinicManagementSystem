using System;

namespace SchoolClinicManagementSystem.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // "Administrator", "School Nurse"
        public string Email { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
