using System;
using Wisej.Web;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Services;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmMemberDashboard : Form
    {
        private UserProfile userProfile;
        public frmMemberDashboard()
        {
            InitializeComponent();
            UserBalancePointService.Instance.BalanceUpdated += OnBalanceUpdated; // Subscribe to the event
        }
        private void frmMemberDashboard_Load(object sender, EventArgs e)
        {
            // check for valid session
            SessionCheck sessionCheck = new SessionCheck();
            sessionCheck.CheckSession();
            // initialize session variable
            userProfile = (UserProfile)Application.Session["UserSettings"];
            // assign session variables to form values
            this.lblUserEMail.Text = userProfile.UserEmail.ToString();
            this.label908.Text = userProfile.HustlePoints.ToString();
            this.label907.Text = userProfile.UserType.ToString();
        }
        private void OnBalanceUpdated(int newBalance)
        {
            this.label908.Text = $"Balance: {newBalance}"; // Update the label
        }
        private void btnProfile_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<frmUserProfile>();
        }

        private void btnHustles_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<frmHustleListing>();
        }
        private void btnEBooks_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<frmEBookCreation>();
        }
        private void btnSocialMedia_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<frmSocialMarketing>();
        }

        private void btnAudiobookCreation_Click(object sender, EventArgs e)
        {
            ShowOrActivateForm<frmAudiobookCreation>();
        }

        public void ShowOrActivateForm<T>() where T : Form, new()
        {
            T existingForm = null;

            // Find if the form is already open
            foreach (Form child in this.MdiChildren)
            {
                if (child is T typedChild)
                {
                    existingForm = typedChild;
                    break;
                }
            }

            if (existingForm == null)
            {
                // The form is not open, create a new instance and show it
                existingForm = new T();
                existingForm.MdiParent = this;
                existingForm.Show();
            }
            else
            {
                // The form is already open, activate it
                existingForm.Activate();

                // If the form is minimized, restore it
                if (existingForm.WindowState == FormWindowState.Minimized)
                {
                    existingForm.WindowState = FormWindowState.Normal;
                }

                // Ensure it's brought to the front
                existingForm.BringToFront();
            }
        }

        private void frmMemberDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserBalancePointService.Instance.BalanceUpdated -= OnBalanceUpdated;
        }

        
    }
}
