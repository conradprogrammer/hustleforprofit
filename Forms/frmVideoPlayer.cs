using System;
using Wisej.Web;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmVideoPlayer : Form
    {
        private string youtubeVideoId = "dQw4w9WgXcQ";

        public frmVideoPlayer()
        {
            InitializeComponent();
        }

        private void frmVideoPlayer_Load(object sender, EventArgs e)
        {

        }

        private void widget1_WidgetEvent(object sender, WidgetEventArgs e)
        {
            switch (e.Type)
            {
                case "youtubeApiReady":
                    widget1.Call("loadVideo", youtubeVideoId);
                    break;
                case "videoLoaded":
                    MessageBox.Show("Video loaded");
                    break;
                case "videoStarted":
                    MessageBox.Show("Video started playing");
                    break;
                case "videoPaused":
                    MessageBox.Show("Video paused");
                    break;
                case "videoEnded":
                    MessageBox.Show("Video ended");
                    break;
            }
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            widget1.Call("togglePlayPause");
        }
    }
}
