using System;
using Wisej.Web;

namespace WJ_HustleForProfit_003.Forms
{
    public partial class frmHustleListing : Form
    {
        public frmHustleListing()
        {
            InitializeComponent();
        }

        private void frmHustles_Load(object sender, EventArgs e)
        {
            splitContainer3.Panel2Collapsed = true;
            splitContainer3.SplitterWidth = 5;
        }

        private void splitButtonWizard_Click(object sender, EventArgs e)
        {
            this.pnlHustle.Visible = true;
        }
    }
}
