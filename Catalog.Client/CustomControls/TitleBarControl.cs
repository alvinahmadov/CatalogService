using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Catalog.Client.Properties;

namespace Catalog.Client
{
    public partial class TitleBarControl : UserControl
    {
        public TitleBarControl()
        {
            InitializeComponent();
            radTitleBar.ElementTree.EnableApplicationThemeName = false;
            radTitleBar.TitleBarElement.BorderPrimitive.Visibility = ElementVisibility.Visible;
            radTitleBar.ThemeName = "MaterialBlueGrey";
            radTitleBar.BackColor = Color.FromArgb(52, 53, 54);
            this.BackColor = Color.FromArgb(52, 53, 54); 

        }
    }
}
