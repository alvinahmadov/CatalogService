using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Themes;
using Telerik.WinControls.UI;

namespace Catalog.Client
{
    public partial class TableHeader : UserControl
    {
        public RadLabel ViewLabel { get { return this.viewLabel; } }

        public TableHeader()
        {
            InitializeComponent();
            new MaterialBlueGreyTheme();
            ThemeResolutionService.ApplicationThemeName = "MaterialBlueGrey";
        }


        private void radLocationList_SelectedIndexChanged(System.Object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            //e.Position
        }
    }
}
