using Microsoft.Ajax.Utilities;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using Wisej.Web;
using Wisej.Web.Ext.TinyEditor;
using Wisej.Ext.ClientClipboard;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Services;
using WJ_HustleForProfit_003.Shared;
using System.Text;
using System.Threading.Tasks;
using NAudio.SoundFont;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmSocialMarketing : Form
    {
        private static string connectionString = clsConnectionString.GetConnectionString();
        private UserProfile userProfile;
        private TransactionModel transaction;
        private Timer checkEditorTimer;
        private bool isTrialMember = true;
        private string memberType = "";

        public frmSocialMarketing()
        {
            InitializeComponent();
            clsShared.InitializeAPI();
        }

        private void frmSocialMedia_Load(object sender, EventArgs e)
        {            
            // check for valid session
            SessionCheck sessionCheck = new SessionCheck();
            sessionCheck.CheckSession();
            // initialize session variable
            userProfile = (UserProfile)Application.Session["UserSettings"];
            memberType = userProfile.UserType;
            if (memberType == "Trial Member")
            {
                isTrialMember = true;
                initializeTrialMemberControlsState();
            }
        }


        #region form events
        private void initializeTrialMemberControlsState()
        {
            if (isTrialMember)
            {
                // disable tinyEditor for Trial Members
                InitializeCheckEditorTimer();

                this.tinyEditor1.Selectable = false;
                this.tinyEditor1.Enabled = false;
                this.tinyEditor1.ShowFooter = false;

                this.btnGenerateFinal.Enabled = false;
                this.btnGenerateFinal.BackColor = Color.Red;
            }
            else
            {
                this.tinyEditor1.Selectable = true;
                this.tinyEditor1.Enabled = true;
                this.tinyEditor1.ShowFooter = true;

                this.btnGenerateFinal.Enabled = true;
                this.btnGenerateFinal.BackColor = Color.FromName("@table-row-background-focused");
            }
        }
        private void InitializeCheckEditorTimer()
        {
            // Initialize the timer
            checkEditorTimer = new Timer();
            checkEditorTimer.Interval = 500; // Check every 500ms
            checkEditorTimer.Tick += CheckEditorTimer_Tick;
            checkEditorTimer.Start();
        }
        private void CheckEditorTimer_Tick(object sender, EventArgs e)
        {
            if (tinyEditor1.IsHandleCreated && tinyEditor1.Visible)
            {
                // TinyEditor is ready, inject the JavaScript
                string script = $@"
                    var interval = setInterval(function() {{
                        var editorElement = document.querySelector('[name=""{tinyEditor1.Name}""]');
                        if (editorElement) {{
                            var iframe = editorElement.querySelector('iframe');
                            if (iframe) {{
                                var editorDocument = iframe.contentDocument || iframe.contentWindow.document;
                                if (editorDocument) {{
                                    clearInterval(interval);
                                    editorDocument.addEventListener('contextmenu', function(e) {{
                                        e.preventDefault();
                                    }});
                                    editorDocument.addEventListener('keydown', function(e) {{
                                        if (e.ctrlKey && e.key === 'c') {{
                                            e.preventDefault();
                                        }}
                                    }});
                                }}
                            }}
                        }}
                    }}, 500);
                ";
                Application.Call("eval", new object[] { script });

                // Stop the timer
                checkEditorTimer.Stop();
                checkEditorTimer.Tick -= CheckEditorTimer_Tick;
                checkEditorTimer.Dispose();
            }
        }
        private void splitButtonSocialMedia_Click(object sender, EventArgs e)
        {
            this.pnlSocialMedia.Visible = true;
        }


        #endregion

        #region Campaign Details
        private void btnTwitter_Click(object sender, EventArgs e)
        {
            MessageBox.Show(memberType);
            this.txtCampaignTitle.Text = "Twitter Campaign ";
            this.txtCampaignNarrative.Text = $"Write engaging tweets for {this.numericUpDownWeeks.Value} " +
                $"weeks with {this.numericUpDownPostsPerDay.Value} tweets per day this campaign ";
            this.txtTargetAudience.Text = "people looking to make extra income with any of our side jobs ";
            this.txtCampaignKeywords.Text = "#hustles #sidejobs #makeextramoney ";
            getListBoxYouTubeVideoLinks();
        }
        private async void btnGenerateDraft_Click(object sender, EventArgs e)
        {
            int estimatedPoints = (int)this.numericUpDownWeeks.Value * 7 * (int)this.numericUpDownPostsPerDay.Value * 140;

            // Show the message box and handle the response
            DialogResult result = MessageBox.Show(
                $"Your Estimated Points will be deducted by {estimatedPoints.ToString()}, do you want to proceed with this transaction?",
                "Confirm Transaction",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            // Every tweet will have a link with the following link: https://twitter.com/intent/tweet?text=hello
            if (result == DialogResult.Yes)
            {
                Application.ShowLoader = true;

                int realPointAmount = 0;
                string resultAPI = "";
                string promptTemplate = $@"Your role is to craft relevant and higly engaging tweets {this.txtCampaignTitle.Text} 
                    with narrative {this.txtCampaignNarrative.Text} for the audience defined as {this.txtTargetAudience.Text} 
                    using these hashtags {this.txtCampaignKeywords.Text}. 
                    Follow the instructions per week count, {this.numericUpDownWeeks.Value} weeks with each week having 7 days.
                    Each day having {this.numericUpDownPostsPerDay.Value} tweets.
                    Output in HTML ordered by week number in <h1> tag then Day number in <h2> tag and the messages in <ol> list per day.
                    Use emojis. Put each tweet in its own div single border, rounded corners, light blue border color. ";
                string prompt = promptTemplate;
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = VerifyBalance(userProfile.UserEmail, estimatedPoints);
                    if (precheck.isTransactionPossible)
                    {
                        if (chkYoutubeTweets.Checked)
                        {
                            resultAPI = await getYutubeTweets(Convert.ToInt32(this.numericUpDownWeeks.Value) * 7);
                            realPointAmount = StripHtmlTags(resultAPI).Length;
                        }
                        else
                        {
                            resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                            realPointAmount = StripHtmlTags(resultAPI).Length;
                        }
                        
                        this.tinyEditor1.Text = resultAPI;
                        this.tinyEditor1.Update();

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.SocialMediaTransaction,
                            TransactionDescription = "frmSocialMedia points deducted: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        string retval = setUserBalancePointService(transaction);
                        MessageBox.Show(retval);
                        this.statusBar1.Text = precheck.message;
                    }
                    else
                    {
                        MessageBox.Show(precheck.message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.statusBar1.Text = precheck.message;
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }

                Application.ShowLoader = false;
                Application.Update(this);

            }
            else
            {
                MessageBox.Show("Operation canceled by user.");
            }
        }
        private async void btnGenerateFinal_Click(object sender, EventArgs e)
        {
            if (this.txtCampaignName.Text == "")
            {
                MessageBox.Show("Campaign Name Required");
                return;
            }
            if (this.tinyEditor1.Text == "")
            {
                MessageBox.Show("Tweet messages required, Generate Draft before submitting as final.");
                return;
            }
            Application.ShowLoader = true;

            bool buttonExists = false;
            string newButtonText = this.txtCampaignName.Text + Environment.NewLine + "(Start " + dateTimeCampaignStart.Value.ToShortDateString() + ")"; // The text for the new button

            // Check all existing controls in the FlowLayoutPanel
            foreach (Control control in this.flowLayoutPanelSocialMarketing.Controls)
            {
                // Check if the control is a Button and has the same text
                if (control is Button existingButton && existingButton.Text == newButtonText)
                {
                    buttonExists = true;
                    break; // Exit the loop if a matching button is found
                }
            }

            try
            {
                var precheck = VerifyBalance(MyDesktop.Instance.lblPanelLoginNameInstance.Text, 50000);
                if (precheck.isTransactionPossible)
                {
                    // Only add the new button if no matching button was found
                    if (!buttonExists)
                    {
                        Button btn = new Button();
                        btn.Height = 100;
                        btn.Width = 100;
                        btn.Text = newButtonText;
                        this.flowLayoutPanelSocialMarketing.Controls.Add(btn);
                        //setUserBalancePointService();
                        MessageBox.Show("Campaign saved...");
                    }
                }
                else
                {
                    MessageBox.Show(precheck.message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }



            Application.ShowLoader = false;
            Application.Update(this);
        }

        public void getListBoxYouTubeVideoLinks()
        {
            string query = "SELECT YouTubeDomain, YouTubeFileName FROM tblHustles WHERE YouTubeFileName IS NOT NULL ORDER BY ID DESC";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        lbYouTubeVideoLinks.Items.Clear();
                        while (reader.Read())
                        {
                            string domain = reader["YouTubeDomain"].ToString();
                            string fileName = reader["YouTubeFileName"].ToString();
                            string fullLink = $"{domain}{fileName}";
                            lbYouTubeVideoLinks.Items.Add(fullLink);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public async Task<string> getYutubeTweets(int numberoftweets)
        {
            StringBuilder result = new StringBuilder();
            string query = "SELECT TOP (@NumberOfTweets) HustleTitle, HustleDescriptionText, YouTubeDomain, YouTubeFileName FROM tblHustles WHERE active = 1 ORDER BY NEWID();";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NumberOfTweets", numberoftweets); // Add parameter to avoid SQL injection

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string hustleTitle = reader["HustleTitle"].ToString();
                            string hustleDescription = reader["HustleDescriptionText"].ToString();
                            string youtubeDomain = reader["YouTubeDomain"].ToString();
                            string youtubeFileName = reader["YouTubeFileName"].ToString();
                            string prompt = $"Create a highly relevant and engaging tweet of max 130 characters about: {hustleTitle}. Description: {hustleDescription}. Add some emojis.";
                            string tweet = await clsShared.GenerateResponseFromGPT(prompt);
                            string youtubeLink = $"{youtubeDomain}{youtubeFileName}";
                            result.AppendLine($"<p>{tweet} How? {youtubeLink}</p>");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the database operation
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty; // Return an empty string in case of an error
                }
            }

            // Return the generated HTML tweets as a string
            return result.ToString();
        }
        #endregion


        #region Campaign Messages



        #endregion

        #region Points Accounting

        private string StripHtmlTags(string html)
        {
            // Remove HTML tags using a regular expression
            return Regex.Replace(html, "<.*?>", string.Empty);
        }
        public (bool isTransactionPossible, string message) VerifyBalance(string email, int estimateAmount)
        {
            var result = UserTransactionPointService.Instance.VerifyBalance(email, estimateAmount);

            //if (result.IsTransactionPossible)
            //    Console.WriteLine("Transaction can proceed.");
            //else
            //    Console.WriteLine("Transaction cannot proceed.");
            //Console.WriteLine($"Message: {result.Message}");

            return (result.IsTransactionPossible, result.Message);
        }
        private string setUserBalancePointService(TransactionModel transaction)
        {
            UserBalancePointService transactionService = UserBalancePointService.Instance;
            //transactionService.BalanceUpdated += OnBalanceUpdated;
            try
            {
                var result = transactionService.ExecuteTransaction(transaction);
                //Console.WriteLine($"Transaction Result: {result.Message}");
                //if (result.UpdatedBalance.HasValue)
                //    Console.WriteLine($"Updated Balance: {result.UpdatedBalance.Value}");
                return result.Message;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "error occurred";
            }

        }
        private void OnBalanceUpdated(int updatedBalance)
        {
            //MessageBox.Show($"Balance Updated to: {updatedBalance}");
        }




        #endregion

        private void lbYouTubeVideoLinks_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (lbYouTubeVideoLinks.SelectedItem != null)
            {
                // Get the text of the selected item
                string selectedText = lbYouTubeVideoLinks.SelectedItem.ToString();

                // Copy the selected text to the clipboard
                ClientClipboard.WriteText(selectedText);

                // Optionally, show a message to the user
                MessageBox.Show($"Copied to clipboard: {selectedText}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
