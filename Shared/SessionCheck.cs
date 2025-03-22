using System;
using Wisej.Web;

namespace WJ_HustleForProfit_003.Shared
{
    public class SessionCheck
    {
        public void CheckSession()
        {
            // Assuming "UserSession" is the session variable you want to check
            var userSession = Application.Session["UserSettings"];

            // Check if the session variable is null or expired
            if (userSession == null)
            {
                // Session has expired
                CloseAllOpenWindows();
                ShowSessionExpiredMessage();
            }
            else
            {
                // Session is valid
                // You can add more logic here if needed
                Console.WriteLine("Session is valid.");
            }
        }

        private void CloseAllOpenWindows()
        {
            // Close all open windows
            foreach (Form openForm in Application.OpenForms)
            {
                openForm.Close();
            }
        }

        private void ShowSessionExpiredMessage()
        {
            MessageBox.Show("Your session has expired. Please log in again.", "Session Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Redirect to login page or take appropriate action
            Application.Navigate("/login");
        }
    }
}
