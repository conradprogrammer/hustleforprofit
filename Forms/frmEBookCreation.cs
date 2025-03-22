using iText.StyledXmlParser.Jsoup.Select;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wisej.Web;
using Wisej.Web.Ext.TinyEditor;

using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;



using WJ_HustleForProfit_003.Extensions;
using WJ_HustleForProfit_003.Models;
using WJ_HustleForProfit_003.Services;
using WJ_HustleForProfit_003.Shared;
using iText.Kernel.Utils;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Data;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmEBookCreation : Form
    {
        private string connectionString = clsConnectionString.GetConnectionString();
        private UserProfile userProfile;
        private TransactionModel transaction;
        private UserBalancePointService pointService;

        private bool isTrialMember = false;
        private string memberType = "";
        private const int PanelHeaderHeight = 28; // Height of each collapsed panel header
        private int realPointAmount = 0;
        private int estimatedPointsPerChapterDefault = 5000;

        public frmEBookCreation()
        {
            InitializeComponent();
            clsShared.InitializeAPI();
        }
        private void frmEBook_Load(object sender, EventArgs e)
        {
            // check for valid session
            SessionCheck sessionCheck = new SessionCheck();
            sessionCheck.CheckSession();
            // initialize session variable
            userProfile = (UserProfile)Application.Session["UserSettings"];
            memberType = userProfile.UserType;
            if (memberType == "Trial Member")
                isTrialMember = true;
            this.txtEBookCoverTitle.Text = "Side gig of selling your digital art online.";

            initializeEBookPanels();
            initializeEBookTabPages();
            initializeAllCheckboxes();
            initializeAllTinyEditors();
            initializeTrialMemberControlsState();

            EBookLoadButtons(userProfile.ID);

            // DEBUG
            //pnlSideEBookFlipbook.Enabled = true;
            this.webBrowser1.Url = new Uri("https://www.apiroaming.com");
        }

        #region frmEBookCreation Events
        private void frmEBookCreation_Resize(object sender, EventArgs e)
        {
            AdjustPanelSize();
        }
        private void initializeTrialMemberControlsState()
        {
            if (isTrialMember)
            {
                // disable non-Trial Member functions
                this.chkSaveSideEBookCover.Enabled = false;
                this.chkSaveSideEBookWriter.Enabled = false;
                this.uploadEBookCover.Enabled = false;

                this.chkChapter2.Enabled = false;
                this.chkChapter3.Enabled = false;   
                this.chkChapter4.Enabled = false;   
                this.chkChapter5.Enabled = false;
                this.chkChapter6.Enabled = false;

                this.chkChapter2.Checked = false;
                this.chkChapter3.Checked = false;
                this.chkChapter4.Checked = false;
                this.chkChapter5.Checked = false;
                this.chkChapter6.Checked = false;

                this.btnGenBookChapter01.Enabled = false;
                this.btnGenBookChapter02.Enabled = false;
                this.btnGenBookChapter03.Enabled = false;
                this.btnGenBookChapter04.Enabled = false;
                this.btnGenBookChapter05.Enabled = false;
                this.btnGenBookChapter06.Enabled = false;

                this.numericUpDownChapters.Enabled = false;
                this.numericUpDownChapters.Value = 1;

                // color the controls red
                this.uploadEBookCover.BackColor = Color.Red;

                this.btnGenBookChapter01.BackColor = Color.Red;
                this.btnGenBookChapter02.BackColor = Color.Red;
                this.btnGenBookChapter03.BackColor = Color.Red;
                this.btnGenBookChapter04.BackColor = Color.Red;
                this.btnGenBookChapter05.BackColor = Color.Red;
                this.btnGenBookChapter06.BackColor = Color.Red;
            }
            else
            {
                // enable non-Trial Member functions
                this.chkSaveSideEBookCover.Enabled = true;
                this.chkSaveSideEBookWriter.Enabled = true;
                this.uploadEBookCover.Enabled = true;

                this.chkChapter2.Enabled = true;
                this.chkChapter3.Enabled = true;
                this.chkChapter4.Enabled = true;
                this.chkChapter5.Enabled = true;
                this.chkChapter6.Enabled = true;

                this.chkChapter2.Checked = true;
                this.chkChapter3.Checked = true;
                this.chkChapter4.Checked = true;
                this.chkChapter5.Checked = true;
                this.chkChapter6.Checked = true;

                this.btnGenBookChapter01.Enabled = true;
                this.btnGenBookChapter02.Enabled = true;
                this.btnGenBookChapter03.Enabled = true;
                this.btnGenBookChapter04.Enabled = true;
                this.btnGenBookChapter05.Enabled = true;
                this.btnGenBookChapter06.Enabled = true;

                this.numericUpDownChapters.Enabled = true;
                this.numericUpDownChapters.Value = 6;

                // color the controls blue
                this.uploadEBookCover.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter01.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter02.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter03.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter04.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter05.BackColor = Color.FromName("@table-row-background-focused");
                this.btnGenBookChapter06.BackColor = Color.FromName("@table-row-background-focused");
            }
        }
        private void formClearAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Clear(); // Clear textboxes
                }
                else if (control is CheckBox checkBox)
                {
                    // Check if the current checkbox is chkChapter1
                    if (checkBox.Name == "chkChapter1")
                    {
                        checkBox.Checked = true; // Set chkChapter1 to checked
                    }
                    else
                    {
                        checkBox.Checked = false; // Uncheck all other checkboxes
                    }
                }
                else if (control is RadioButton radioButton)
                {
                    radioButton.Checked = false; // Uncheck radio buttons
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.SelectedIndex = -1; // Reset comboboxes
                }
                else if (control is ListBox listBox)
                {
                    listBox.ClearSelected(); // Clear listbox selections
                }
                else if (control is NumericUpDown numericUpDown)
                {
                    
                    // Ensure Minimum and Maximum are set correctly before setting Value
                    if (numericUpDown.Minimum != 1 || numericUpDown.Maximum != 6)
                    {
                        numericUpDown.Minimum = 1;
                        numericUpDown.Maximum = 6;
                    }

                    // Directly set the Value to 1 to avoid out-of-range errors
                    numericUpDown.Value = 1;
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    dateTimePicker.Value = DateTime.Now; // Reset datetime picker to current date
                }
                else if (control.GetType().Name.Contains("tinyEditor")) // Assuming the tiny editor has a specific control type
                {
                    control.Text = string.Empty; // Clear tiny editors by setting text to empty
                }
                // Continue with more control types if necessary

                // Recursively clear controls within containers like GroupBoxes, Panels, TabPages, etc.
                if (control.HasChildren)
                {
                    formClearAllControls(control);
                }
            }
        }
        private void splitButtonEBook_Click(object sender, EventArgs e)
        {
            pnlSideEBookCover.Visible = true;
            pnlSideEBookWriter.Visible = true;
            pnlSideEBookPDFViewer.Visible = true;
            pnlSideEBookFlipbook.Visible = true;

            formClearAllControls(this);
            InitializeTabPageChapter1();
            // After clearing all controls, simulate the PanelExpanded event
            pnlSideEBookCover_PanelExpanded(this.pnlSideEBookCover, EventArgs.Empty);
        }

        #endregion

        #region SidePanels
        private void ExpandPanel(Panel expandedPanel)
        {
            int totalCollapsedWidth = 0;

            Panel[] panels = { pnlSideEBookCover, pnlSideEBookWriter, pnlSideEBookPDFViewer, pnlSideEBookFlipbook };

            // Calculate total width taken by collapsed panels
            foreach (var panel in panels)
            {
                if (panel != expandedPanel)
                {
                    panel.Collapsed = true; // Collapse other panels
                    totalCollapsedWidth += PanelHeaderHeight;
                }
            }

            // Calculate available width for the expanded panel
            int availableWidth = ClientSize.Width - totalCollapsedWidth;
            expandedPanel.Width = availableWidth;
        }
        private void AdjustPanelSize()
        {
            Panel expandedPanel = null;
            Panel[] panels = { pnlSideEBookCover, pnlSideEBookWriter, pnlSideEBookPDFViewer, pnlSideEBookFlipbook };

            // Find the expanded panel
            foreach (var panel in panels)
            {
                if (!panel.Collapsed)
                {
                    expandedPanel = panel;
                    break;
                }
            }

            if (expandedPanel != null)
            {
                int totalCollapsedWidth = 0;

                // Calculate total width taken by collapsed panels
                foreach (var panel in panels)
                {
                    if (panel != expandedPanel)
                    {
                        panel.Collapsed = true; // Collapse other panels
                        totalCollapsedWidth += PanelHeaderHeight;
                    }
                }

                // Calculate available width for the expanded panel
                int availableWidth = ClientSize.Width - totalCollapsedWidth;
                expandedPanel.Width = availableWidth;
            }
        }
        private void pnlSideEBookCover_PanelExpanded(object sender, EventArgs e)
        {
            Panel expandedPanel = sender as Panel;
            if (expandedPanel != null)
            {
                ExpandPanel(expandedPanel);
            }
        }
        private void pnlSideEBookWriter_PanelExpanded(object sender, EventArgs e)
        {
            Panel expandedPanel = sender as Panel;
            if (expandedPanel != null)
            {
                ExpandPanel(expandedPanel);
            }
        }
        private void pnlSideEBookPDFViewer_PanelExpanded(object sender, EventArgs e)
        {
            Panel expandedPanel = sender as Panel;
            if (expandedPanel != null)
            {
                ExpandPanel(expandedPanel);
            }
        }
        private void pnlSideEBookFlipbook_PanelExpanded(object sender, EventArgs e)
        {
            Panel expandedPanel = sender as Panel;
            if (expandedPanel != null)
            {
                ExpandPanel(expandedPanel);
            }
        }

        #endregion

        #region EBookCover
        private async void txtEBookCoverTitle_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            int clickedIndex = txtEBookCoverTitle.Tools.IndexOf(e.Tool);

            switch (clickedIndex)
            {
                case 0:
                    {
                        await GenerateResponseForTextBoxesAsync(txtEBookCoverTitle, txtEBookCoverTitle, "Write a relevant eBook title using {0}.", 5);
                    }
                    break;
            }
        }
        private async void txtEBookCoverSaveAs_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            int clickedIndex = ((TextBox)sender).Tools.IndexOf(e.Tool);

            if (clickedIndex == 0) // Assuming the "Save As" tool is at index 0
            {
                var result = MessageBox.Show("Do you want to save this project?", "Save As", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await SaveProjectAsync(); // Replace with your actual save method
                }
            }
        }
        private async void txtEBookCoverByline_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            if (sender is TextBox textBox)
            {
                int clickedIndex = textBox.Tools.IndexOf(e.Tool);

                switch (clickedIndex)
                {
                    case 0:
                        {
                            await GenerateResponseForTextBoxesAsync(txtEBookCoverTitle, txtEBookCoverByline, "Write a relevant ebook byline  using title {0}. Just a one sentence with no quotes in the result.", 30);
                        }
                        break;
                }
            }
        }
        private async void txtEBookCoverShortDescription_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            if (sender is TextBox textBox)
            {
                int clickedIndex = textBox.Tools.IndexOf(e.Tool);

                switch (clickedIndex)
                {
                    case 0:
                        {
                            await GenerateResponseForTextBoxesAsync(txtEBookCoverTitle, txtEBookCoverShortDescription, "Write a short ebook description using title {0}. Just one complete paragraph with no quotes in the result.", 75);
                        }
                        break;
                }
            }
        }
        private void txtEBookCoverShortDescription_TextChanged(object sender, EventArgs e)
        {
            this.txtEBookWriterBookSummary.Text = this.txtEBookCoverShortDescription.Text;
        }
        public async Task GenerateResponseForTextBoxesAsync(TextBox inputTextBox, TextBox resultTextBox, string promptTemplate, int desiredCompletionTokens)
        {
            try
            {
                Application.ShowLoader = true;

                string prompt = string.Format(promptTemplate, inputTextBox.Text);
                //var (response, totalTokens, completionTokens, continuations) = await clsShared.GenerateResponseFromGPT3(prompt, desiredCompletionTokens);
                string response = await clsShared.GenerateResponseFromGPT(prompt);
                resultTextBox.Text = response;
                resultTextBox.Update();

                Application.ShowLoader = false;
                Application.Update(this);
            }
            catch (Exception ex)
            {
                Application.ShowLoader = false;
                MessageBox.Show($"Error generating response: {ex.Message}");
            }
        }
        private async Task SaveProjectAsync()
        {
            // Check if a button with the same text already exists
            foreach (Control control in this.flowLayoutPanelEBook.Controls)
            {
                if (control is Button existingButton && existingButton.Text == txtEBookCoverSaveAs.Text)
                {
                    MessageBox.Show("A button with this name already exists. Please choose a different name.", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit the method without saving
                }
            }

            // Add a button with txtEBookCoverSaveAs.Text as the text to the FlowLayoutPanel
            Button newButton = new Button();
            newButton.Text = txtEBookCoverSaveAs.Text;
            newButton.Size = new Size(100, 100);

            // Assuming flowLayoutPanelEBook is your FlowLayoutPanel
            this.flowLayoutPanelEBook.Controls.Add(newButton);

            MessageBox.Show("Project saved successfully!", "Save As", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await Task.CompletedTask; // If you have an actual async operation, replace this with the actual async call
        }
        private void btnEBookCoverSaveAndNextStep_Click(object sender, EventArgs e)
        {
               
            if (chkSaveSideEBookCover.Checked)
            {
                try
                {
                    EBookUpsert();
                    MessageBox.Show("eBook UPSERT OK");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                // Expand the panel programmatically
                pnlSideEBookWriter.Collapsed = false;
                pnlSideEBookWriter.Enabled = true;
                pnlSideEBookWriter.HeaderBackColor = Color.FromName("@primary");
                // Trigger the PanelExpanded event handler manually
                pnlSideEBookWriter_PanelExpanded(pnlSideEBookWriter, EventArgs.Empty);
            }
            else
            {
                // Expand the panel programmatically
                pnlSideEBookWriter.Collapsed = false;
                pnlSideEBookWriter.Enabled = true;
                pnlSideEBookWriter.HeaderBackColor = Color.FromName("@primary");
                // Trigger the PanelExpanded event handler manually
                pnlSideEBookWriter_PanelExpanded(pnlSideEBookWriter, EventArgs.Empty);
            }
        }
        private void EBookUpsert()
        {
            // Initialize the EBookService
            var ebookService = new EBookService();
            // Convert PictureBox image to byte array
            byte[] eBookCoverImage = null;
            if (pictureBoxEBookCoverImage.Image != null)
            {
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    pictureBoxEBookCoverImage.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png); // Always use a fixed format
                    eBookCoverImage = memoryStream.ToArray();
                }
            }
            if (this.txtEBookID.Text == "0" || this.txtEBookID.Text == "")
            {
                EBook newEBook = new EBook
                {
                    UserID = userProfile.ID,
                    EBookName = this.txtEBookCoverSaveAs.Text,
                    EBookTitle = this.txtEBookCoverTitle.Text,
                    EBookByline = this.txtEBookCoverByline.Text,
                    EBookDescription = this.txtEBookCoverShortDescription.Text,
                    EBookAuthor = this.txtEBookCoverAuthor.Text,
                    EBookPublisher = this.txtEBookCoverPublisher.Text,
                    EBookFormat = this.cboEBookCoverBookFormat.Text,
                    EBookSerial = this.txtEBookCoverSerial.Text,
                    EBookVideoURL = this.txtEBookCoverVideoURL.Text,
                    EBookCoverImage = eBookCoverImage // Include image data
                };

                // Upsert the new eBook (this will perform an INSERT because ID is null)
                int newEBookID = ebookService.EBookUpsert(newEBook);
                this.txtEBookID.Text = newEBookID.ToString();
            }
            else
            {
                EBook existingEBook = new EBook
                {
                    ID = Convert.ToInt32(this.txtEBookID.Text),
                    UserID = userProfile.ID,
                    EBookName = this.txtEBookCoverSaveAs.Text,
                    EBookTitle = this.txtEBookCoverTitle.Text,
                    EBookByline = this.txtEBookCoverByline.Text,
                    EBookDescription = this.txtEBookCoverShortDescription.Text,
                    EBookAuthor = this.txtEBookCoverAuthor.Text,
                    EBookPublisher = this.txtEBookCoverPublisher.Text,
                    EBookFormat = this.cboEBookCoverBookFormat.Text,
                    EBookSerial = this.txtEBookCoverSerial.Text,
                    EBookVideoURL = this.txtEBookCoverVideoURL.Text,
                    EBookCoverImage = eBookCoverImage // Include image data
                };

                // Upsert the existing eBook (this will perform an update because ID is set)
                int updatedEBookID = ebookService.EBookUpsert(existingEBook);
            }
        }
        public void EBookGetByUserID(int userID, int ebookID)
        {
            string connectionString = clsConnectionString.GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookGetByUserID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", userID);
                command.Parameters.AddWithValue("@EBookID", ebookID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Populate form fields with retrieved data
                        this.txtEBookID.Text = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty;
                        this.txtEBookCoverSaveAs.Text = reader["eBookName"] != DBNull.Value ? reader["eBookName"].ToString() : string.Empty;
                        this.txtEBookCoverTitle.Text = reader["eBookTitle"] != DBNull.Value ? reader["eBookTitle"].ToString() : string.Empty;
                        this.txtEBookCoverByline.Text = reader["eBookByline"] != DBNull.Value ? reader["eBookByline"].ToString() : string.Empty;
                        this.txtEBookCoverShortDescription.Text = reader["eBookDescription"] != DBNull.Value ? reader["eBookDescription"].ToString() : string.Empty;
                        this.txtEBookCoverAuthor.Text = reader["eBookAuthor"] != DBNull.Value ? reader["eBookAuthor"].ToString() : string.Empty;
                        this.txtEBookCoverPublisher.Text = reader["eBookPublisher"] != DBNull.Value ? reader["eBookPublisher"].ToString() : string.Empty;
                        this.cboEBookCoverBookFormat.Text = reader["eBookFormat"] != DBNull.Value ? reader["eBookFormat"].ToString() : string.Empty;
                        this.txtEBookCoverSerial.Text = reader["eBookSerial"] != DBNull.Value ? reader["eBookSerial"].ToString() : string.Empty;
                        this.txtEBookCoverVideoURL.Text = reader["eBookVideoURL"] != DBNull.Value ? reader["eBookVideoURL"].ToString() : string.Empty;

                        // Load image into PictureBox
                        if (reader["eBookCoverImage"] != DBNull.Value)
                        {
                            byte[] imageBytes = (byte[])reader["eBookCoverImage"];
                            using (var ms = new System.IO.MemoryStream(imageBytes))
                            {
                                this.pictureBoxEBookCoverImage.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }
                        else
                        {
                            this.pictureBoxEBookCoverImage.Image = null; // Clear image if no image is present
                        }
                    }
                    else
                    {
                        // Handle case where no eBook record is found for the given UserID
                        MessageBox.Show("No eBook records found for this user.");
                    }
                }
            }
        }
        public void EBookLoadButtons(int userID)
        {
            string connectionString = clsConnectionString.GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("usp_EBookGetTotalsByUserID", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Clear existing controls
                    this.flowLayoutPanelEBook.Controls.Clear();

                    while (reader.Read())
                    {
                        // Read EBookID and EBookName
                        int eBookID = Convert.ToInt32(reader["ID"]);
                        string eBookName = reader["eBookName"].ToString();

                        // Create a new button for each eBook
                        Button ebookButton = new Button
                        {
                            Height = 100,
                            Width = 100,
                            Text = eBookName,
                            Tag = eBookID // Store the eBook ID in the Tag property for reference in the click event
                        };

                        // Add a click event handler for the button
                        ebookButton.Click += (sender, e) => EbookButton_Click(sender, e, eBookID);

                        // Add the button to the FlowLayoutPanel
                        this.flowLayoutPanelEBook.Controls.Add(ebookButton);
                    }
                }
            }
        }
        private void EbookButton_Click(object sender, EventArgs e, int eBookID)
        {
            // Logic to handle eBook button click, e.g., load eBook details in another form or panel
            //MessageBox.Show($"EBook ID {eBookID} button clicked.");

            EBookGetByUserID(userProfile.ID, eBookID);
            EBookLoadChapters(eBookID);

            pnlSideEBookCover.Visible = true;
            pnlSideEBookWriter.Visible = true;
            pnlSideEBookPDFViewer.Visible = true;
            pnlSideEBookFlipbook.Visible = true;
        }

        #endregion

        #region EBookWriter
        private void setAllCheckBoxes(object sender, EventArgs e)
        {
            CheckBox[] checkBoxes = { chkChapter1, chkChapter2, chkChapter3, chkChapter4, chkChapter5, chkChapter6 };
            TextBox[] textBoxes = { txtBookChapter01, txtBookChapter02, txtBookChapter03, txtBookChapter04, txtBookChapter05, txtBookChapter06 };

            CheckBox currentCheckBox = sender as CheckBox;
            int currentIndex = Array.IndexOf(checkBoxes, currentCheckBox);

            if (currentCheckBox.Checked)
            {
                for (int i = 0; i <= currentIndex; i++)
                {
                    checkBoxes[i].Checked = true;
                    textBoxes[i].Enabled = true;
                }
            }
            else
            {
                for (int i = currentIndex; i < checkBoxes.Length; i++)
                {
                    checkBoxes[i].Checked = false;
                    textBoxes[i].Enabled = false;
                    textBoxes[i].Text = string.Empty;
                }
            }

            // Update the numericUpDownChapters value to reflect the number of checked chapters
            numericUpDownChapters.ValueChanged -= numericUpDownChapters_ValueChanged; // Prevent recursion
            numericUpDownChapters.Value = checkBoxes.Count(cb => cb.Checked);
            numericUpDownChapters.ValueChanged += numericUpDownChapters_ValueChanged; // Reattach the event
        }
        private void numericUpDownChapters_ValueChanged(object sender, EventArgs e)
        {
            // Get the current NumericUpDown control
            NumericUpDown numericUpDown = sender as NumericUpDown;

            // Check if the NumericUpDown is valid and within range
            if (numericUpDown != null && numericUpDown.Value >= 1 && numericUpDown.Value <= 6)
            {
                CheckBox[] checkBoxes = { chkChapter1, chkChapter2, chkChapter3, chkChapter4, chkChapter5, chkChapter6 };
                TextBox[] textBoxes = { txtBookChapter01, txtBookChapter02, txtBookChapter03, txtBookChapter04, txtBookChapter05, txtBookChapter06 };

                int newValue = (int)numericUpDown.Value;

                for (int i = 0; i < checkBoxes.Length; i++)
                {
                    if (i < newValue)
                    {
                        checkBoxes[i].Checked = true;
                        textBoxes[i].Enabled = true;
                    }
                    else
                    {
                        checkBoxes[i].Checked = false;
                        textBoxes[i].Enabled = false;
                        textBoxes[i].Text = string.Empty;
                    }
                }
            }
        }

        private void initializeEBookTabPages()
        {
            //this.tabControlEBookChapters.TabPages.Remove(tabPageChapter2);
            //this.tabControlEBookChapters.TabPages.Remove(tabPageChapter3);
            //this.tabControlEBookChapters.TabPages.Remove(tabPageChapter4);
            //this.tabControlEBookChapters.TabPages.Remove(tabPageChapter5);
            //this.tabControlEBookChapters.TabPages.Remove(tabPageChapter6);
            // Hide all tab pages except the first one on form load
            tabPageChapter2.Visible = false;
            tabPageChapter3.Visible = false;
            tabPageChapter4.Visible = false;
            tabPageChapter5.Visible = false;
            tabPageChapter6.Visible = false;

            // Optionally, select the first tab page
            this.tabControlEBookChapters.SelectedTab = tabPageChapter1;
        }
        private void initializeEBookPanels()
        {
            this.pnlSideEBookCover.Enabled = true;
            this.pnlSideEBookCover.Collapsed = false;
            ExpandPanel(pnlSideEBookCover);
            this.pnlSideEBookWriter.Enabled = false;
            this.pnlSideEBookWriter.Collapsed = true;
            this.pnlSideEBookWriter.HeaderBackColor = Color.LightSlateGray;
            this.pnlSideEBookPDFViewer.Enabled = false;
            this.pnlSideEBookPDFViewer.Collapsed = true;
            this.pnlSideEBookPDFViewer.HeaderBackColor = Color.LightSlateGray;
            this.pnlSideEBookFlipbook.Enabled = false;
            this.pnlSideEBookFlipbook.Collapsed = true;
            this.pnlSideEBookFlipbook.HeaderBackColor = Color.LightSlateGray;
        }
        private void ShowNextTabPage(TabPage tabPage)
        {
            if (!this.tabControlEBookChapters.TabPages.Contains(tabPage))
            {
                //this.tabControlEBookChapters.TabPages.Add(tabPage);
                tabPage.Visible = true;
            }
            this.tabControlEBookChapters.SelectedTab = tabPage; // Switch to the newly added tab
        }
        private void initializeAllCheckboxes()
        {
            CheckBox[] checkBoxes = { chkChapter1, chkChapter2, chkChapter3, chkChapter4, chkChapter5, chkChapter6 };
            foreach (CheckBox checkBox in checkBoxes)
            {
                checkBox.CheckedChanged += setAllCheckBoxes;
            }
        }
        private void initializeAllTinyEditors()
        {
            TinyEditor[] editors = { tinyEditorChapter1, tinyEditorChapter2, tinyEditorChapter3, tinyEditorChapter4, tinyEditorChapter5, tinyEditorChapter6 };
            foreach (TinyEditor editor in editors)
            {
                editor.Enabled = true;
            }
        }
        #endregion

        #region GenerateDraft
        private async void btnGenerateChapterTitles_Click(object sender, EventArgs e)
        {
            Application.ShowLoader = true;

            this.btnGenerateChapterTitles.Text = "Processing...";
            await generateEBookChapters();
            this.btnGenerateChapterTitles.Text = "(Re)Generate Chapter Titles";            

            this.lblEBookStatus.ForeColor = Color.Lime;
            this.lblEBookStatus.Text = "Book Chapters generation completed!";
            this.lblEBookStatus.Update();

            this.btnGenBookLayout.Visible = true;

            Application.ShowLoader = false;
            Application.Update(this);
        }
        private async void btnGenBookLayout_Click(object sender, EventArgs e)
        {
            Application.ShowLoader = true;

            await getResponseFromGPTChapter01();
            if (chkChapter2.Checked)
                await getResponseFromGPTChapter02();
            if (chkChapter3.Checked)
                await getResponseFromGPTChapter03();
            if (chkChapter4.Checked)
                await getResponseFromGPTChapter04();
            if (chkChapter5.Checked)
                await getResponseFromGPTChapter05();
            if (chkChapter6.Checked)
                await getResponseFromGPTChapter06();

            if (!isTrialMember)
            {
                this.btnGenBookNextStepPDF.Visible = true;
                this.chkSaveSideEBookWriter.Visible = true;
            }
            
            Application.ShowLoader = false;
            Application.Update(this);
        }
        private async Task generateEBookChapters()
        {
            this.lblEBookStatus.ForeColor = Color.Lime;
            this.lblEBookStatus.Text = "Start generating Book Chapters...";
            this.lblEBookStatus.Update();
            this.btnGenerateChapterTitles.Enabled = false;

            Application.ShowLoader = true;

            try
            {
                // Count the number of checked checkboxes
                int chapterCount = 0;
                for (int i = 1; i <= 6; i++)
                {
                    string checkBoxName = $"chkChapter{i}";
                    Wisej.Web.Control[] foundCheckBoxes = this.Controls.Find(checkBoxName, true);
                    if (foundCheckBoxes.Length > 0 && foundCheckBoxes[0] is Wisej.Web.CheckBox checkBox && checkBox.Checked)
                    {
                        chapterCount++;
                    }
                }

                if (chapterCount == 0)
                {
                    this.lblEBookStatus.Text = "Please check at least one chapter.";
                    return;
                }

                string prompt = "";
                string result = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string promptTemplate = $@"Your role is to craft an ebook with {chapterCount} chapters, each chapter title succinctly described in a single line. 
            As an administrative expert, ensure clarity and precision in your writing. The ebook's overarching topic is {bookTitle}.
            Add the words 'Chapter' and then the number for each line. Format each chapter title with a '|' at the end. 
            This concise structure will maintain a clean and professional response, aligning with the expectations of your response.";

                prompt = promptTemplate;

                try
                {
                    result = await clsShared.GenerateResponseFromGPT(prompt);
                    this.txtEBookWriterAIPrompt.Text = result;
                }
                catch (Exception ex)
                {
                    result = ex.ToString();
                }                
            }
            finally
            {
                this.btnGenerateChapterTitles.Enabled = true;
            }

            AssignChaptersToTextBoxes();

            Application.ShowLoader = false;
            Application.Update(this);
        }
        public void AssignChaptersToTextBoxes()
        {
            string chapters = this.txtEBookWriterAIPrompt.Text;

            // Split the chapters string by the '|' delimiter.
            string[] chapterArray = chapters.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            int chapterIndex = 0;

            // Loop through each checkbox and corresponding TextBox and Label.
            for (int i = 1; i <= 6; i++)
            {
                // Construct the checkbox name
                string checkBoxName = $"chkChapter{i}";

                // Find the checkbox control by name
                Wisej.Web.Control[] foundCheckBoxes = this.Controls.Find(checkBoxName, true);

                if (foundCheckBoxes.Length > 0 && foundCheckBoxes[0] is Wisej.Web.CheckBox checkBox && checkBox.Checked)
                {
                    // Construct the names for TextBox and Label
                    string textBoxName = $"txtBookChapter{i.ToString("D2")}";
                    string textLabelName = $"lblBookChapter{i.ToString("D2")}";

                    // Only process if there are more chapters left in the array
                    if (chapterIndex < chapterArray.Length)
                    {
                        // Find the TextBox by name and assign the chapter title
                        Wisej.Web.Control[] foundTextBoxes = this.Controls.Find(textBoxName, true);
                        if (foundTextBoxes.Length > 0 && foundTextBoxes[0] is Wisej.Web.TextBox textBox)
                        {
                            textBox.Text = chapterArray[chapterIndex].Trim();
                        }

                        // Find the Label by name and assign the chapter title
                        Wisej.Web.Control[] foundLabels = this.Controls.Find(textLabelName, true);
                        if (foundLabels.Length > 0 && foundLabels[0] is Wisej.Web.Label label)
                        {
                            label.Text = chapterArray[chapterIndex].Trim();
                        }

                        chapterIndex++; // Move to the next chapter title
                    }
                }
            }
        }
        private void UpdateControlText(Control control, string text)
        {
            if (control != null)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new Action(() =>
                    {
                        control.Text = text;
                        control.Refresh(); // Ensure repaint
                    }));
                }
                else
                {
                    control.Text = text;
                    control.Refresh(); // Ensure repaint
                }
            }
            Application.Update(this);
        }
        private async Task getResponseFromGPTChapter01()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 1...");
            btnGenBookChapter01.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter01.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.

                    Your task is to craft this ebook first chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'. Do not change the title. 
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter1.Text = resultAPI;
                        this.tinyEditorChapter1.Update();

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 1 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1,transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                        
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }                
            }
            finally
            {
                btnGenBookChapter01.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 1 completed, {realPointAmount} points used...");
        }
        private async Task getResponseFromGPTChapter02()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 2...");
            btnGenBookChapter02.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter02.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.

                    Your task is to craft this ebook second chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'. Do not change the title.
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter2.Text = resultAPI;
                        this.tinyEditorChapter2.Update();
                        ShowNextTabPage(tabPageChapter2);
                        this.lblBookChapter02.Text = txtBookChapter02.Text;

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 2 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1, transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }
            }
            finally
            {
                btnGenBookChapter02.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 2 completed, {realPointAmount} points used...");
        }
        private async Task getResponseFromGPTChapter03()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 3...");
            btnGenBookChapter03.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter03.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.

                    Your task is to craft this ebook third chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'. Do not change the title. 
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter3.Text = resultAPI;
                        this.tinyEditorChapter3.Update();
                        ShowNextTabPage(tabPageChapter3);
                        this.lblBookChapter03.Text = txtBookChapter03.Text;

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 3 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1, transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }
            }
            finally
            {
                btnGenBookChapter03.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 3 completed, {realPointAmount} points used...");
        }
        private async Task getResponseFromGPTChapter04()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 4...");
            btnGenBookChapter04.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter04.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.

                    Your task is to craft this ebook fourth chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'.  Do not change the title.
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter4.Text = resultAPI;
                        this.tinyEditorChapter4.Update();
                        ShowNextTabPage(tabPageChapter4);
                        this.lblBookChapter04.Text = txtBookChapter04.Text;

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 4 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1, transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }
            }
            finally
            {
                btnGenBookChapter04.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 4 completed, {realPointAmount} points used...");
        }
        private async Task getResponseFromGPTChapter05()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 5...");
            btnGenBookChapter05.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter05.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.

                    Your task is to craft this ebook fifth chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'. Do not change the title.
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter5.Text = resultAPI;
                        this.tinyEditorChapter5.Update();
                        ShowNextTabPage(tabPageChapter5);
                        this.lblBookChapter05.Text = txtBookChapter05.Text;

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 5 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1, transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }
            }
            finally
            {
                btnGenBookChapter05.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 5 completed, {realPointAmount} points used...");
        }
        private async Task getResponseFromGPTChapter06()
        {
            UpdateControlText(this.lblEBookStatus, "Generating book Chapter 6...");
            btnGenBookChapter06.Enabled = false;
            try
            {
                string resultAPI = "";
                string prompt = "";
                string bookTitle = this.txtEBookCoverTitle.Text;
                string chapterTitle = this.txtBookChapter06.Text;
                string AllChapters = this.txtEBookWriterAIPrompt.Text;
                string promptTemplate = @"This task is part of an ebook writing effort structured in 6 chapters: {{AllChapters}}.
                    Your task is to craft this ebook sixth and last chapter titled '{{ChapterTitle}}' for the book '{{BookTitle}}'. Do not change the title.
                    This chapter should provide comprehensive insights on the topic, blending business expertise with specialized writing techniques. 
                    To maintain a cohesive narrative and seamless transition to subsequent chapters, please adhere to the following HTML formatting guidelines:
                    1. Structure the chapter as an integral part of the book, avoiding the style of a standalone article.
                    2. Present key points or steps in bullet list format using <ul> tags.
                    3. Emphasize significant concepts using <strong> or <b> tags.
                    4. Limit the chapter content to 4000 characters to ensure reader engagement.
                    5. ALWAYS Utilize <h1> tags for every chapter title on the first line and <h2> tags for subsection headings and <p> tags for paragraphs.

                    Please note: Ensure the chapter concludes without traditional article conclusions such as 'In Conclusion', 'Summary', or any equivalent phrases. 
                    Maintain a smooth transition between chapters without introducing unnecessary endings.

                    Integrate these instructions seamlessly into your writing process to ensure the generated content aligns with the specified HTML formatting requirements.
                    ";
                prompt = promptTemplate
                    .Replace("{{ChapterTitle}}", chapterTitle)
                    .Replace("{{BookTitle}}", bookTitle)
                    .Replace("{{AllChapters}}", bookTitle);
                try
                {
                    // precheck is to verify estimated balance with no transaction registered
                    var precheck = UserTransactionPointService.Instance.VerifyBalance(userProfile.UserEmail, estimatedPointsPerChapterDefault);
                    if (precheck.IsTransactionPossible)
                    {
                        resultAPI = await clsShared.GenerateResponseFromGPT(prompt);
                        realPointAmount = resultAPI.CharCountNoHTML();
                        this.tinyEditorChapter6.Text = resultAPI;
                        this.tinyEditorChapter6.Update();
                        ShowNextTabPage(tabPageChapter6);
                        this.lblBookChapter06.Text = txtBookChapter06.Text;

                        transaction = new TransactionModel
                        {
                            UserEmail = userProfile.UserEmail,
                            RealPointAmount = -realPointAmount,
                            RealCreditAmount = 0,
                            TransactionTypeID = GlobalEnums.TransactionTypeID.eBookTransaction,
                            TransactionDescription = "frmEBook Chapter 6 points: -" + realPointAmount.ToString()
                        };

                        // final balance with transaction submitted, still with all the verifications
                        // perhaps some additional messaging can be done here in case the audits and verifications fail
                        pointService = UserBalancePointService.Instance;
                        var retval = pointService.ExecuteTransaction(transaction);
                        //MessageBox.Show(retval.Message);
                        UpdateControlText(this.statusBar1, transaction.TransactionDescription);
                    }
                    else
                    {
                        MessageBox.Show(precheck.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    resultAPI = ex.ToString();
                }
            }
            finally
            {
                btnGenBookChapter06.Enabled = true;
            }
            UpdateControlText(this.lblEBookStatus, $"Generating book Chapter 6 completed, {realPointAmount} points used...");
        }
        private void btnGenBookNextStepPDF_Click(object sender, EventArgs e)
        {
            if (chkSaveSideEBookWriter.Checked)
            {
                EBookChapterUPSERT();

                // Expand the panel programmatically
                pnlSideEBookPDFViewer.Collapsed = false;
                pnlSideEBookPDFViewer.Enabled = true;
                pnlSideEBookPDFViewer.HeaderBackColor = Color.FromName("@primary");
                // Trigger the PanelExpanded event handler manually
                pnlSideEBookWriter_PanelExpanded(pnlSideEBookPDFViewer, EventArgs.Empty);
            }
            else
            {
                // Expand the panel programmatically
                pnlSideEBookPDFViewer.Collapsed = false;
                pnlSideEBookPDFViewer.Enabled = true;
                pnlSideEBookPDFViewer.HeaderBackColor = Color.FromName("@primary");
                // Trigger the PanelExpanded event handler manually
                pnlSideEBookWriter_PanelExpanded(pnlSideEBookPDFViewer, EventArgs.Empty);
            }

        }
        private void EBookChapterUPSERT()
        {
            // Initialize the EBookService
            var ebookService = new EBookService();

            // Loop through each checkbox (chkChapter1 to chkChapter6)
            for (int i = 1; i <= 6; i++)
            {
                // Find the checkbox control by name
                var checkBox = this.Controls.Find($"chkChapter{i}", true).FirstOrDefault() as CheckBox;

                if (checkBox != null && checkBox.Checked)
                {
                    // Find the title text box and the tiny editor for the chapter text by name
                    var chapterTitleTextBox = this.Controls.Find($"txtBookChapter{i:D2}", true).FirstOrDefault() as TextBox;
                    var chapterTextEditor = this.Controls.Find($"tinyEditorChapter{i}", true).FirstOrDefault() as Control; // Adjust if you have a specific control type for the editor

                    if (chapterTitleTextBox != null && chapterTextEditor != null)
                    {
                        // Get the text from the title textbox and the tiny editor
                        string chapterTitle = chapterTitleTextBox.Text;
                        string chapterText = chapterTextEditor.Text; // Assuming the editor has a Text property; adjust if different

                        // Create a new EBookChapter object with the actual values
                        EBookChapter chapter = new EBookChapter
                        {
                            EBookID = Convert.ToInt32(this.txtEBookID.Text),
                            EBookChapterID = i,
                            EBookChapterTitle = chapterTitle,
                            EBookChapterText = chapterText
                        };

                        // Perform the Upsert operation
                        int chapterID = ebookService.EBookChapterUpsert(chapter);
                        Console.WriteLine($"Chapter {i} ID after upsert: {chapterID}");
                    }
                    else
                    {
                        Console.WriteLine($"Could not find controls for Chapter {i}");
                    }
                }
            }

            // Optionally display a success message after processing all chapters
            MessageBox.Show("Chapters saved successfully.");
        }
        private void EBookMatchChaptersToControls(List<EBookChapter> chapters)
        {
            if (chapters.Count == 0)
            {
                // If no chapters exist, only show tabPageChapter1
                tabPageChapter2.Visible = false;
                tabPageChapter3.Visible = false;
                tabPageChapter4.Visible = false;
                tabPageChapter5.Visible = false;
                tabPageChapter6.Visible = false;

                // Clear controls for a new chapter
                InitializeTabPageChapter1();
            }
            else
            {
                // Make only the relevant tab pages visible based on chapters count
                for (int i = 0; i < chapters.Count; i++)
                {
                    EBookChapter chapter = chapters[i];

                    switch (i)
                    {
                        case 0:
                            // Initialize the first tab (tabPageChapter1)
                            tabPageChapter1.Visible = true;
                            chkChapter1.Checked = true;
                            txtBookChapter01.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter1.Text = chapter.EBookChapterText;
                            break;
                        case 1:
                            tabPageChapter2.Visible = true;
                            chkChapter2.Checked = true;
                            txtBookChapter02.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter2.Text = chapter.EBookChapterText;
                            break;
                        case 2:
                            tabPageChapter3.Visible = true;
                            chkChapter3.Checked = true;
                            txtBookChapter03.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter3.Text = chapter.EBookChapterText;
                            break;
                        case 3:
                            tabPageChapter4.Visible = true;
                            chkChapter4.Checked = true;
                            txtBookChapter04.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter4.Text = chapter.EBookChapterText;
                            break;
                        case 4:
                            tabPageChapter5.Visible = true;
                            chkChapter5.Checked = true;
                            txtBookChapter05.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter5.Text = chapter.EBookChapterText;
                            break;
                        case 5:
                            tabPageChapter6.Visible = true;
                            chkChapter6.Checked = true;
                            txtBookChapter06.Text = chapter.EBookChapterTitle;
                            tinyEditorChapter6.Text = chapter.EBookChapterText;
                            break;
                        default:
                            break;
                    }
                }

                // Hide any remaining unused tab pages
                if (chapters.Count < 6) tabPageChapter6.Visible = false;
                if (chapters.Count < 5) tabPageChapter5.Visible = false;
                if (chapters.Count < 4) tabPageChapter4.Visible = false;
                if (chapters.Count < 3) tabPageChapter3.Visible = false;
                if (chapters.Count < 2) tabPageChapter2.Visible = false;
            }

            // Optionally, select the first visible tab page
            this.tabControlEBookChapters.SelectedTab = tabPageChapter1;
        }
        // Method to initialize or clear tabPageChapter1 when creating a new eBook
        private void InitializeTabPageChapter1()
        {
            // Ensure tabPageChapter1 is reset properly
            chkChapter1.Checked = true;            
            txtBookChapter01.Enabled = true;
            if (isTrialMember)
            {
                numericUpDownChapters.Value = 1;
                chkChapter2.Checked = false;
                chkChapter3.Checked = false;
                chkChapter4.Checked = false;
                chkChapter5.Checked = false;
                chkChapter6.Checked = false;
                txtBookChapter02.Enabled = false;
                txtBookChapter03.Enabled = false;
                txtBookChapter04.Enabled = false;
                txtBookChapter05.Enabled = false;
                txtBookChapter06.Enabled = false;
            }
            else
            {
                numericUpDownChapters.Value = 6;
                chkChapter2.Checked = true;
                chkChapter3.Checked = true;
                chkChapter4.Checked = true;
                chkChapter5.Checked = true;
                chkChapter6.Checked = true;
                txtBookChapter02.Enabled = true;
                txtBookChapter03.Enabled = true;
                txtBookChapter04.Enabled = true;
                txtBookChapter05.Enabled = true;
                txtBookChapter06.Enabled = true;
            }
            txtBookChapter01.Text = string.Empty;
            txtBookChapter02.Text = string.Empty;
            txtBookChapter03.Text = string.Empty;
            txtBookChapter04.Text = string.Empty;
            txtBookChapter05.Text = string.Empty;
            txtBookChapter06.Text = string.Empty;

            tinyEditorChapter1.Text = string.Empty;
            tinyEditorChapter2.Text = string.Empty;
            tinyEditorChapter3.Text = string.Empty;
            tinyEditorChapter4.Text = string.Empty;
            tinyEditorChapter5.Text = string.Empty;
            tinyEditorChapter6.Text = string.Empty;

            // Hide all tab pages except the first one on form load
            tabPageChapter2.Visible = false;
            tabPageChapter3.Visible = false;
            tabPageChapter4.Visible = false;
            tabPageChapter5.Visible = false;
            tabPageChapter6.Visible = false;

            // Optionally, select the first tab page
            this.tabControlEBookChapters.SelectedTab = tabPageChapter1;

        }
        private void EBookLoadChapters(int eBookID)
        {
            var ebookService = new EBookService();
            List<EBookChapter> chapters = ebookService.GetEBookChaptersByEBookID(eBookID);
            EBookMatchChaptersToControls(chapters);
        }
        #endregion

        #region Generate PDF

        private void btnGenBookFinalPDF_Click(object sender, EventArgs e)
        {
            Application.ShowLoader = true;

            string filename1 = clsRandomFilenameGenerator.Generate();
            MakePDF($@"C:\Temp\{filename1}.pdf");
            LoadPDF($@"C:\Temp\{filename1}.pdf");
            this.btnNextStepFlipbook.Visible = true;

            Application.ShowLoader = false;
        }
        private void MakePDF(string pdfFilename)
        {
            List<byte[]> pdfPages = new List<byte[]>();
            //pdfPages.Add(ConvertImageToPdf(@"C:\Temp\Book_001.jpg"));
            pdfPages.Add(ConvertImageToPdf(this.pictureBoxEBookCoverImage.Image));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter1.Text));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter2.Text));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter3.Text));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter4.Text));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter5.Text));
            pdfPages.Add(ConvertHtmlToPdf(this.tinyEditorChapter6.Text));

            CreateEBookPdf(pdfPages, pdfFilename);
        }
        private void LoadPDF(string pdfFilename)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream f = new FileStream(pdfFilename, FileMode.Open, FileAccess.Read))
                f.CopyTo(ms);
            this.pdfViewer1.PdfStream = ms;
        }
        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Initialize PDF writer and document
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document document = new Document(pdfDoc);

                // Convert HTML to PDF
                HtmlConverter.ConvertToPdf(htmlContent, pdfDoc, new ConverterProperties());

                // Close the document
                document.Close();

                return stream.ToArray();
            }
        }
        public void CreateEBookPdf(IEnumerable<byte[]> pdfBytes, string outputPdfPath)
        {
            // Initialize the PDF document for output
            using (PdfWriter writer = new PdfWriter(outputPdfPath))
            {
                using (PdfDocument pdfDoc = new PdfDocument(writer))
                {
                    // Initialize a PdfMerger to merge the documents
                    PdfMerger merger = new PdfMerger(pdfDoc);

                    foreach (var pdfByte in pdfBytes)
                    {
                        // Create a PdfDocument for each byte array
                        using (PdfDocument pdfToMerge = new PdfDocument(new PdfReader(new MemoryStream(pdfByte))))
                        {
                            // Merge the document into the output PDF
                            merger.Merge(pdfToMerge, 1, pdfToMerge.GetNumberOfPages());
                        }
                    }
                }
            }
        }
        public byte[] ConvertImageToPdf(System.Drawing.Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Save the image to a memory stream in a format supported by iText7
                using (MemoryStream imageStream = new MemoryStream())
                {
                    image.Save(imageStream, ImageFormat.Png);
                    imageStream.Position = 0;

                    // Initialize PDF writer
                    PdfWriter writer = new PdfWriter(stream);

                    // Initialize PDF document
                    PdfDocument pdfDoc = new PdfDocument(writer);

                    // Initialize document
                    Document document = new Document(pdfDoc);

                    // Add image to document
                    ImageData imageData = ImageDataFactory.Create(imageStream.ToArray());
                    iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(imageData);
                    document.Add(pdfImage);

                    // Close document
                    document.Close();
                }

                return stream.ToArray();
            }
        }
        public byte[] ConvertImageLocationToPdf(string imagePath)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                // Initialize PDF writer
                PdfWriter writer = new PdfWriter(stream);

                // Initialize PDF document
                PdfDocument pdfDoc = new PdfDocument(writer);

                // Initialize document
                Document document = new Document(pdfDoc);

                // Add image to document
                ImageData imageData = ImageDataFactory.Create(imagePath);
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData);
                document.Add(image);

                // Close document
                document.Close();

                return stream.ToArray();
            }
        }


        #endregion

        #region Flipbook
        private void btnNextStepFlipbook_Click(object sender, EventArgs e)
        {
            // Expand the panel programmatically
            pnlSideEBookFlipbook.Collapsed = false;
            pnlSideEBookFlipbook.Enabled = true;
            pnlSideEBookFlipbook.HeaderBackColor = Color.FromName("@primary");
            // Trigger the PanelExpanded event handler manually
            pnlSideEBookWriter_PanelExpanded(pnlSideEBookFlipbook, EventArgs.Empty);

            this.webBrowser1.Url = new Uri("https://www.apiroaming.com");
        }



        #endregion

        

        private void uploadEBookCover_Uploaded(object sender, UploadedEventArgs e)
        {
            try
            {
                // Ensure a file was uploaded
                if (e.Files.Count > 0)
                {
                    // Dispose of the old image properly
                    if (this.pictureBoxEBookCoverImage.Image != null)
                    {
                        this.pictureBoxEBookCoverImage.Image.Dispose();
                        this.pictureBoxEBookCoverImage.Image = null;  // Clear the old image reference completely
                    }

                    // Force PictureBox to invalidate and clear any cached state
                    this.pictureBoxEBookCoverImage.Invalidate();
                    this.pictureBoxEBookCoverImage.Update();

                    // Get the uploaded file stream
                    var uploadedFile = e.Files[0];

                    using (var stream = new MemoryStream())
                    {
                        // Copy the uploaded file stream to a MemoryStream
                        uploadedFile.InputStream.CopyTo(stream);
                        stream.Position = 0; // Reset stream position to the beginning

                        try
                        {
                            // Use Image.FromStream instead of Bitmap to support different formats better
                            using (var img = System.Drawing.Image.FromStream(stream))
                            {
                                var newImage = new Bitmap(img); // Ensure Bitmap is created from the general Image
                                this.pictureBoxEBookCoverImage.Image = newImage;  // Set the new image
                            }

                            // Force UI to refresh the PictureBox
                            this.pictureBoxEBookCoverImage.Refresh();

                            // Optionally update the entire form or application to force a full refresh
                            Application.Update(this);
                        }
                        catch (Exception imgEx)
                        {
                            Console.WriteLine($"Error creating image from stream: {imgEx.Message}");
                            MessageBox.Show($"Error loading image: {imgEx.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No files were uploaded.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the upload or display process
                MessageBox.Show($"Error uploading image: {ex.Message}");
                Console.WriteLine($"General error: {ex.Message}");
            }
        }






    }
}
