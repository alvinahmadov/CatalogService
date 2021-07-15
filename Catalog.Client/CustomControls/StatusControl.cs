using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    public partial class StatusControl : UserControl
    {
        public RadLabel ViewLabel => this.companyInfoLabel;

        public RadLabel UpdateStatus => this.updateStatusLabel;

        public StatusControl()
        {
            new MaterialBlueGreyTheme();
            InitializeComponent();
            this.companyMailLabel.Text = Common.MESSAGE.Gui.COMPANY_MAIL;
            UpdateStatus.Text = string.Format(Common.MESSAGE.Gui.UPDATE_STATUS, System.DateTime.Now);
            ViewLabel.Text = string.Format(Common.MESSAGE.Gui.COMPANY_INFO, System.DateTime.Now.Year);
            ThemeResolutionService.ApplicationThemeName = "MaterialBlueGrey";
        }

        private void companyMailLabel_LinkClicked(System.Object sender, LinkLabelLinkClickedEventArgs e)
        {
            var buttons = MessageBoxButtons.OKCancel;
            var confirmResult = RadMessageBox.Show("Открыть почту?", "", buttons);
            if (confirmResult == DialogResult.OK)
            {
                var url = ((LinkLabel)(sender)).Text;
                System.Diagnostics.Process.Start(string.Format("mailto:{0}", url));
            }
        }
    }
}
