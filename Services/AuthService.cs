using System;
using System.Collections.Generic;
using SchoolClinicManagementSystem.Models;

namespace SchoolClinicManagementSystem.Services
{
    public class AuthService
    {
        private static AuthService _instance;
        public static AuthService Instance => _instance ?? (_instance = new AuthService());

        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Username = "admin",
                Password = "admin123",
                FullName = "System Administrator",
                Role = "Administrator",
                Email = "admin@cct.edu.ph",
                LastLogin = DateTime.Now
            },
            new User
            {
                Username = "nurse",
                Password = "nurse123",
                FullName = "Maria Santos, RN",
                Role = "School Nurse",
                Email = "clinic.nurse@cct.edu.ph",
                LastLogin = DateTime.Now
            }
        };

        public User CurrentUser { get; private set; }

        public bool Login(string username, string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage = "Please enter your username.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Please enter your password.";
                return false;
            }

            var user = _users.Find(u => u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase) && u.Password == password);
            if (user != null)
            {
                user.LastLogin = DateTime.Now;
                CurrentUser = user;
                return true;
            }

            errorMessage = "Invalid username or password. Please check your credentials.";
            return false;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
