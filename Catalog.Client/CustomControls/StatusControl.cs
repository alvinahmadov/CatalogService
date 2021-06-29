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
            this.companyMailLabel.Text = "info@sbm-nn.ru";
            UpdateStatus.Text = $"<html>Последнее обновление: <b>{System.DateTime.Now}</b>";
            ViewLabel.Text = "© 2021 Центр оптовой торговли ТД СБМ-Волга, Россия, г. Нижний Новгород";
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
