using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Wisej.Web;
using WJ_HustleForProfit_003.Forms;
using WJ_HustleForProfit_003.Shared;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using WJ_HustleForProfit_003.Test;
using WJ_HustleForProfit_003.Services;
using WJ_HustleForProfit_003.Models;

namespace WJ_HustleForProfit_003
{
    public partial class MyDesktop : Desktop
    {
        private static MyDesktop _instance; // needed to access the email label from other forms/windows
        private EventLog eventLog;
        private static string connectionString = clsConnectionString.GetConnectionString();
        private UserProfile userProfile;
        public MyDesktop()
        {
            InitializeComponent();
            InitializeEventLog();
            _instance = this; // Assign the instance
        }
        public static MyDesktop Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MyDesktop();
                }
                return _instance;
            }
        }
        public Label lblPanelLoginNameInstance // Ensure this is public or has a public getter
        {
            get { return this.lblPanelLoginName; }
        }
        private void MyDesktop_Load(object sender, EventArgs e)
        {
            this.menuItemTwitterTest.Visible = true;
            //Application.ShowConsole = true;
            //pnlLogin.Visible = true;
            // Embed JavaScript directly to disable the right-click context menu
            string script = @"
                document.addEventListener('contextmenu', function(e) {
                    e.preventDefault();
                });
            ";
            Application.Call("eval", new object[] { script });
            Log("WJ_HustleForProfit_003 MyDesktop_Load() started successfully...", EventLogEntryType.Information, EventIds.AppStart, EventCat.ApplicationEvents);
        }
        
        #region StartBar
        private void btnStart_Click(object sender, EventArgs e)
        {
            //var screenMousePosition = Control.MousePosition;
            //var formMousePosition = this.PointToClient(screenMousePosition);
            //contextMenuStart.Show(this, new Point(5, formMousePosition.Y + 10));
            if (pnlLogin.Visible == true)
                pnlLogin.Visible = false;
            if (pnlRegister.Visible == true)
                pnlRegister.Visible = false;
            contextMenuStart.Show(this, new Point(5, 5));
        }
        private void btnPanelLogin_Click(object sender, EventArgs e)
        {
            if (pnlRegister.Visible == true)
                pnlRegister.Visible = false;

            pnlLogin.Visible = !pnlLogin.Visible;

            if (pnlLogin.Visible)
            {
                // Calculate the center position
                pnlLogin.Left = (this.ClientSize.Width - pnlLogin.Width) / 2;
                pnlLogin.Top = (this.ClientSize.Height - pnlLogin.Height) / 2;
                // Focus txtLoginEMail
                txtLoginEmail.Focus();
            }
        }
        private void btnPanelRegister_Click(object sender, EventArgs e)
        {
            if (pnlLogin.Visible == true)
                pnlLogin.Visible = false;

            pnlRegister.Visible = !pnlRegister.Visible;

            if (pnlRegister.Visible)
            {
                // Calculate the center position
                pnlRegister.Left = (this.ClientSize.Width - pnlRegister.Width) / 2;
                pnlRegister.Top = (this.ClientSize.Height - pnlRegister.Height) / 2;
                // Focus txtRegisterFirstName
                txtRegisterFirstName.Focus();
            }
        }
        #endregion

        #region StartButton
        private void menuItemStart1000Hustles_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartMemberDashboard_Click(object sender, EventArgs e)
        {
            // Check if frmMemberDashboard is already open
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm is frmMemberDashboard)
                {
                    // Bring the form to the front and make it active
                    openForm.Activate();
                    openForm.BringToFront();
                    return;
                }
            }

            // frmMemberDashboard is not open, create a new instance and show it
            frmMemberDashboard frm = new frmMemberDashboard();
            frm.Show();
        }
        private void menuItemStartFacebookGroup_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartFaceBookPage_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartInstagram_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartLoginRegister_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartTwitter_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartWelcomeVideo_Click(object sender, EventArgs e)
        {

        }
        private void menuItemStartYouTubeChannel_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region EventLog
        private void InitializeEventLog()
        {
            // Create a new EventLog instance
            eventLog = new EventLog();

            // Check if the source exists, if not create it
            if (!EventLog.SourceExists("WJ_HustleForProfit_003"))
            {
                EventLog.CreateEventSource("WJ_HustleForProfit_003", "Application");
            }

            // Set the source and log name
            eventLog.Source = "WJ_HustleForProfit_003";
            eventLog.Log = "Application";

            // Subscribe to the EntryWritten event if you want to handle it
            eventLog.EntryWritten += new EntryWrittenEventHandler(EventLog_EntryWritten);
            eventLog.EnableRaisingEvents = true; // Enable the event to be raised
        }
        private void EventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            // Display the new event log entry in a message box or any other control
            MessageBox.Show($"New Event Log Entry: {e.Entry.Message}");
        }
        public void Log(string message, EventLogEntryType type = EventLogEntryType.Information, int eventID = 0, int category = 0)
        {
            // Create an EventInstance to include the EventID and Category
            EventInstance eventInstance = new EventInstance(eventID, category, type);
            eventLog.WriteEvent(eventInstance, message);

            //Log("Application started.", EventLogEntryType.Information, EventIds.AppStart, EventCat.ApplicationEvents);
            //Log("User logged in.", EventLogEntryType.Information, EventIds.UserLogin, EventCat.UserActions);
            //Log("User logged out.", EventLogEntryType.Information, EventIds.UserLogout, EventCat.UserActions);
            //Log("Application stopped.", EventLogEntryType.Information, EventIds.AppStop, EventCat.ApplicationEvents);
        }

        #endregion

        #region LoginEvents
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var email = this.txtLoginEmail.Text;
            var password = this.txtLoginPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string userIP = GetUserIP();
            string referer = GetReferer();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("usp_LoginUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", HashPassword(password));
                        command.Parameters.AddWithValue("@UserIP", userIP);
                        command.Parameters.AddWithValue("@Referer", referer);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblPanelLoginName.Text = reader["EmailAddress"].ToString();
                                pnlLogin.Visible = false;
                                if (chkLaunchDashboard.Checked==true)
                                {
                                    // Load user profile from the database
                                    UserProfile userprofile = LoadUserSettingsFromDatabase(reader["EmailAddress"].ToString());
                                    // Store the user profile in the session
                                    Application.Session["UserSettings"] = userprofile;
                                    userProfile = (UserProfile)Application.Session["UserSettings"];
                                    menuItemStartMemberDashboard.Enabled = true;
                                    menuItemStartMemberDashboard_Click(this, EventArgs.Empty);
                                }
                            }
                            else
                            {
                                // Show error message or handle invalid login
                                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void pnlLogin_ToolClick(object sender, ToolClickEventArgs e)
        {
            int toolIndex = pnlLogin.Tools.IndexOf(e.Tool);
            switch (toolIndex)
            {
                case 0:
                    pnlLogin.Visible = false;
                    break;
            }
        }
        private UserProfile LoadUserSettingsFromDatabase(string userEmail)
        {
            UserProfile userProfile = new UserProfile(userEmail);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("usp_UserGetSettings", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmailAddress", userEmail);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string settingName = reader.GetString(0);
                            bool isEnabled = reader.GetBoolean(1);
                            userProfile.HustlePoints = Convert.ToInt32(reader.GetInt32(3));
                            //userProfile.UserSettingsList[settingName] = isEnabled;
                            userProfile.UserType = reader.GetString(4);
                            userProfile.ID = Convert.ToInt32(reader.GetInt32(5));
                        }
                    }
                }
            }
            return userProfile;
        }
        #endregion

        #region RegistrationEvents
        private void btnRegistration_Click(object sender, EventArgs e)
        {
            statusBarRegistration.ForeColor = Color.Black;
            statusBarRegistration.Text = "Processing...";

            if (!IsValidEmail(txtLoginEmail.Text))
            {
                statusBarRegistration.ForeColor = Color.Red;
                statusBarRegistration.Text = "Invalid email address format...";
                return;
            }
            if (!AreAllFieldsFilled())
            {
                statusBarRegistration.ForeColor = Color.Red;
                statusBarRegistration.Text = "All fields are required...";
                return;
            }

            if (txtRegisterPassword.Text != txtRegisterPasswordVerify.Text)
            {
                statusBarRegistration.ForeColor = Color.Red;
                statusBarRegistration.Text = "Passwords do not match...";
                return;
            }

            if (!IsValidPassword(txtRegisterPassword.Text))
            {
                statusBarRegistration.ForeColor = Color.Red;
                statusBarRegistration.Text = "Password at least 8 chars, must not contain spaces or HTML reserved characters...";
                return;
            }

            string hashedPassword = HashPassword(txtRegisterPassword.Text);
            string userIP = GetUserIP();
            string referer = GetReferer();

            RegisterUser(txtRegisterFirstName.Text, txtRegisterLastName.Text, txtRegisterEmail.Text, hashedPassword, userIP, referer);
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use Regex to validate the email format
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                Regex regex = new Regex(pattern);
                return regex.IsMatch(email);
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool AreAllFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(txtRegisterFirstName.Text) &&
                   !string.IsNullOrWhiteSpace(txtRegisterLastName.Text) &&
                   !string.IsNullOrWhiteSpace(txtRegisterEmail.Text) &&
                   !string.IsNullOrWhiteSpace(txtRegisterPassword.Text) &&
                   !string.IsNullOrWhiteSpace(txtRegisterPasswordVerify.Text);
        }
        private bool IsValidPassword(string password)
        {
            if (password.Length < 8)
                return false;

            string htmlReservedCharacters = @"<>&'""";
            foreach (char c in htmlReservedCharacters)
            {
                if (password.Contains(c.ToString()))
                    return false;
            }

            if (password.Contains(" "))
                return false;

            return true;
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private string GetUserIP()
        {
            //return Application.Session.ClientIP;
            return Application.UserHostAddress;
        }
        private string GetReferer()
        {
            return Application.ServerVariables["HTTP_REFERER"];
        }
        private void RegisterUser(string firstName, string lastName, string email, string passwordHash, string userIP, string referer)
        {
            statusBarRegistration.ForeColor = Color.Black;
            statusBarRegistration.Text = "Registering...";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_UserInsertWithFeatures", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        command.Parameters.AddWithValue("@HustlePoints", 30000);
                        command.Parameters.AddWithValue("@UserIP", userIP);
                        command.Parameters.AddWithValue("@Referer", referer);                        
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    statusBarRegistration.ForeColor = Color.Green;
                    statusBarRegistration.Text = "Registration successful, please Login now...";
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000) // Custom error thrown by the stored procedure
                    {
                        MessageBox.Show(ex.Message);
                    }
                    else
                        MessageBox.Show(ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString() + "An error occurred during registration. Please try again.");
                }
            }
        }
        private void pnlRegister_ToolClick(object sender, ToolClickEventArgs e)
        {
            int toolIndex = pnlRegister.Tools.IndexOf(e.Tool);
            switch (toolIndex)
            {
                case 0:
                    pnlRegister.Visible = false;
                    break;
            }
        }



        #endregion

        private void menuItemTwitterTest_Click(object sender, EventArgs e)
        {
            //frmTwitterAPI tw = new frmTwitterAPI();
            //tw.ShowDialog();    

            //frmAudiobook frm = new frmAudiobook();
            //frm.ShowDialog();

            //frmVideoPlayer vid = new frmVideoPlayer();
            //vid.ShowDialog();
            frmStrategy30 strat = new frmStrategy30();
            strat.ShowDialog();
        }
    }
}
