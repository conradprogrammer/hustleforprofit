using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using Wisej.Web;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Shared;
using WJ_HustleForProfit_003.Services;
using System.Diagnostics;
using System.Collections.Generic;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmUserProfile : Form
    {
        private static string connectionString = clsConnectionString.GetConnectionString();
        private UserProfile userProfile;
        private EventLog eventLog;
        private UserProfileService _userProfileService = new UserProfileService();

        
        public frmUserProfile()
        {
            InitializeComponent();
        }

        private void frmProfile_Load(object sender, EventArgs e)
        {
            userProfile = (UserProfile)Application.Session["UserSettings"];
            UserGetProfile(userProfile.UserEmail);
            UserGetSettingsCheckboxes(userProfile.UserEmail);
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            var userProfile = new UserProfile("user@example.com")
            {
                FirstName = "John",
                LastName = "Doe",
                PrimaryPhone = "123-456-7890",
                UserTypeID = 1
            };

            string passwordHash = "hashedPassword"; // Replace with actual hashed password
            string userIP = "192.168.1.1"; // Replace with actual user IP
            string referer = "someReferer"; // Replace with actual referrer

            UpdateUserProfile(userProfile, passwordHash, userIP, referer);
        }
        private void UserGetProfile(string email)
        {
            try
            {
                UserProfile userProfile = _userProfileService.UserGetProfileByEmail(email); 
                if (userProfile != null)
                {
                    this.txtFirstname.Text = userProfile.FirstName;
                    this.txtLastname.Text = userProfile.LastName;
                    this.txtPrimaryPhone.Text = userProfile.PrimaryPhone;
                }
                else
                {
                    MessageBox.Show("User profile not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while fetching the user profile: {ex.Message}");
            }
        }
        private void UserGetSettingsCheckboxes(string email)
        {
            try
            {
                var settings = _userProfileService.UserGetSettingsByEmail(email);
                var settingsSet = new HashSet<string>(settings);

                chkEBooks.Checked = settingsSet.Contains("Enable eBooks");
                chkBusinessPlans.Checked = settingsSet.Contains("Enable Business Plans");
                chkMarketingPlans.Checked = settingsSet.Contains("Enable Marketing Plans");
                chkFlipbooks.Checked = settingsSet.Contains("Enable Flipbooks");
                chkAudiobooks.Checked = settingsSet.Contains("Enable Audiobooks");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the user settings checkboxes: {ex.Message}");
            }
        }
        private void UpdateUserProfile(UserProfile userProfile, string passwordHash, string userIP, string referer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("usp_UpdateUserProfileWithFeatures", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@FirstName", userProfile.FirstName);
                        command.Parameters.AddWithValue("@LastName", userProfile.LastName);
                        command.Parameters.AddWithValue("@PrimaryPhone", userProfile.PrimaryPhone);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
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
                    MessageBox.Show(ex.ToString() + "An error occurred while updating the profile. Please try again.");
                }
            }
        }
    }
}
