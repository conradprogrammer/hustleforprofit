using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using iText.Commons.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Wisej.Web;
using System.Drawing;
using WJ_HustleForProfit_003.Shared;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmAudiobooks : Form
    {
        private clsOpenAITTS openAITTS;


        public frmAudiobooks()
        {
            InitializeComponent();
            openAITTS = new clsOpenAITTS("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");
        }
        private void frmAudiobook_Load(object sender, EventArgs e)
        {

        }
        private async void buttonSynthesize_Click(object sender, EventArgs e)
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
            string textToSynthesize = txtInput.Text;
            var audioData = await openAITTS.SynthesizeTextAsync(textToSynthesize);

            if (audioData != null)
            {
                Directory.CreateDirectory(clsGlobals.audioFolderPath);
                string outputPath = Path.Combine(clsGlobals.audioFolderPath, "output.mp3");
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

        
    }
}
