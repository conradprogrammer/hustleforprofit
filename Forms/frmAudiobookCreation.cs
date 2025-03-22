using System;
using System.Threading.Tasks;
using Wisej.Web;
using System.IO;
using WJ_HustleForProfit_003.Extensions;    
using WJ_HustleForProfit_003.Shared;
using System.Drawing;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmAudiobookCreation : Form
    {
        private clsOpenAITTS openAITTS;
        private int ChapterDisplayOrder = 1;
        private string audioFilename = "";

        public frmAudiobookCreation()
        {
            InitializeComponent();
            clsShared.InitializeAPI();
            openAITTS = new clsOpenAITTS("");
        }
        private void frmAudiobookCreation_Load(object sender, EventArgs e)
        {
            initializeListView();
        }

        #region Generate Audiobook
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
                lblCharCount.Text = "Current Char Count: " + response.Length.ToString();
                Application.ShowLoader = false;
                Application.Update(this);
            }
            catch (Exception ex)
            {
                Application.ShowLoader = false;
                MessageBox.Show($"Error generating response: {ex.Message}");
            }
        }
        private async void txtAudiobookSource_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            int clickedIndex = this.txtAudiobookSource.Tools.IndexOf(e.Tool);

            switch (clickedIndex)
            {
                case 0:
                    {
                        await GenerateResponseForTextBoxesAsync(txtAudiobookDescription, txtAudiobookSource, "Write a relevant eBook chapter about {0}.", 5);
                    }
                    break;
            }
        }
        private void txtAudiobookSource_TextChanged(object sender, EventArgs e)
        {
            lblCharCount.Text = "Current Char Count: " + txtAudiobookSource.Text.Length.ToString();
        }

        #endregion

        #region Synthesize Audio
        private async void btnGenerateMP3_Click(object sender, EventArgs e)
        {
            Application.ShowLoader = true;
            Application.Update(this);

            await Task.Run(async () =>
            {
                await SynthesizeAndPlayAudioAsync();
            });

            Application.ShowLoader = false;
            Application.Update(this);
        }
        private async Task SynthesizeAndPlayAudioAsync()
        {
            string textToSynthesize = txtAudiobookSource.Text;
            var audioData = await openAITTS.SynthesizeTextAsync(textToSynthesize);

            if (audioData != null)
            {
                // Save the audio data to a file (for example, output.mp3)
                audioFilename = clsRandomFilenameGenerator.Generate();
                Directory.CreateDirectory(clsGlobals.audioFolderPath);
                string outputPath = Path.Combine(clsGlobals.audioFolderPath, audioFilename + ".mp3");
                File.WriteAllBytes(outputPath, audioData);

                PlayAudio(outputPath);
            }
            else
            {
                MessageBox.Show("TTS Synthesis failed.");
            }
        }
        private void PlayAudio(string fileUrl)
        {
            byte[] audioBytes = File.ReadAllBytes(fileUrl);
            string base64 = Convert.ToBase64String(audioBytes);
            string dataUri = $"data:audio/mpeg;base64,{base64}";

            // https://wavesurfer.xyz/examples/?webaudio.js
            widget1.Call("initAudioPlayer", dataUri);
        }

        #endregion

        #region Save MP3
        private void btnSaveMP3_Click(object sender, EventArgs e)
        {
            string audioFileDuration = AudioHelper.AudioFileDuration(Path.Combine(clsGlobals.audioFolderPath, audioFilename + ".mp3"));
            AddListViewItem(ChapterDisplayOrder.ToString()
                            , this.txtAudiobookTitle.Text
                            , this.txtAudiobookChapterTitle.Text
                            , audioFileDuration
                            , audioFilename + ".mp3");
            ChapterDisplayOrder += 1;
        }
        private void initializeListView()
        {
            // Adding column headers
            this.lvAudioChapters.Columns.Add("Order", 50);
            this.lvAudioChapters.Columns.Add("Book Title", 200);
            this.lvAudioChapters.Columns.Add("Chapter", 100);
            this.lvAudioChapters.Columns.Add("Duration", 100);
            this.lvAudioChapters.Columns.Add("Filename", 200);
            this.lvAudioChapters.Columns.Add("", 100);
        }
        private void AddListViewItem(string order, string bookTitle, string chapter, string duration, string filename)
        {
            ListViewItem item = new ListViewItem(order);
            item.SubItems.Add(bookTitle);
            item.SubItems.Add(chapter);
            item.SubItems.Add(duration);
            item.SubItems.Add(filename);
            this.lvAudioChapters.Items.Add(item);

            // Create a container for the button
            var container = new ContainerControl
            {
                Dock = DockStyle.Fill
            };
            var downloadButton = new Button
            {
                Text = "Download",
                Dock = DockStyle.Fill,
                Tag = filename,
                ForeColor = Color.Blue
            };
            downloadButton.Click += DownloadButton_Click;
            container.Controls.Add(downloadButton);

            item.SubItems.Add(new ListViewItem.ListViewSubItem() { Control = container });

            // Add the item to the ListView
            this.lvAudioChapters.Items.Add(item);
        }
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            // Implement download functionality
            var button = sender as Button;
            string filename = button?.Tag as string;

            string filePath = Path.Combine(clsGlobals.audioFolderPath, filename);
            if (File.Exists(filePath))
            {
                Application.Download(filePath);
            }
            else
            {
                AlertBox.Show($"File {filename} not found.", MessageBoxIcon.Warning);
            }
        }

        #endregion

        private void toolBarButton1_Click(object sender, EventArgs e)
        {
            string audioFolderPath = clsGlobals.audioFolderPath;
            ////string audioFilename = "QybCm744zjpp.mp3";
            //string audioFileDuration = AudioHelper.AudioFileDuration(Path.Combine(audioFolderPath, audioFilename));
            //MessageBox.Show(audioFilename + ".mp3");
            //MessageBox.Show(audioFileDuration);

            string outputFilePath = Path.Combine(audioFolderPath,"concatenated.mp3");
            AudioHelper.ConcatenateMp3Files(this.lvAudioChapters.Items, outputFilePath);
            MessageBox.Show($"MP3 files concatenated into {outputFilePath}");
        }

        private async void txtAudiobookTitle_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            int clickedIndex = this.txtAudiobookTitle.Tools.IndexOf(e.Tool);

            switch (clickedIndex)
            {
                case 0:
                    {
                        await GenerateResponseForTextBoxesAsync(txtAudiobookTitle, txtAudiobookTitle, "Write a relevant eBook title about {0}.", 5);
                    }
                    break;
            }
        }

        private async void txtAudiobookDescription_ToolClick(object sender, ToolClickEventArgs e)
        {
            // Get the index of the clicked tool
            int clickedIndex = this.txtAudiobookDescription.Tools.IndexOf(e.Tool);

            switch (clickedIndex)
            {
                case 0:
                    {
                        await GenerateResponseForTextBoxesAsync(txtAudiobookTitle, txtAudiobookDescription, "Write a relevant and short eBook summary about {0}.", 5);
                    }
                    break;
            }
        }
    }
}
