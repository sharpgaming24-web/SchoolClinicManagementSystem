using System;
using System.Windows.Forms;
using SchoolClinicManagementSystem.UI;

namespace SchoolClinicManagementSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}
