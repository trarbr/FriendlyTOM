using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FriendlyTOM.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsUserControl.xaml
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            InitializeComponent();

            aboutLabel.Text = readAbout();
        }

        private string readAbout()
        {
            // read from README or hardcode?
            StringBuilder aboutBuilder = new StringBuilder();

            // Version should **not** be hardcoded
            aboutBuilder.AppendLine("Friendly TOM version 0.1."); 
            aboutBuilder.AppendLine("Your Friendly Tour Operations Manager.\n");
            aboutBuilder.Append("Created by Troels Brødsgaard, Peter Ipsen, Mikkel Møller Jensen and Lasse Bredgaard, ");
            aboutBuilder.AppendLine("in collaboration with Lonely Tree, Ecuador.");
            // also collaboration with Lonely Tree, ecuadorian tour operator.
            aboutBuilder.AppendLine("This software is released under the Apache 2.0 license. For full terms, see LICENSE file or web-url.");
            aboutBuilder.AppendLine("Currently maintained and developed by Troels Brødsgaard.");
            aboutBuilder.AppendLine("Please send comments, support requests etc to ");
            aboutBuilder.AppendLine("To see the manual, look at: \n");
            aboutBuilder.AppendLine("Uses icons from Silk and Ballicons iconsets.");


            return aboutBuilder.ToString();
        }
    }
}
