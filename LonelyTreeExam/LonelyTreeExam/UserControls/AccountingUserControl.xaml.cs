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

namespace LonelyTreeExam.UserControls
{
    /// <summary>
    /// Interaction logic for AccountingUserControl.xaml
    /// </summary>
    public partial class AccountingUserControl : UserControl
    {
        DetailsUserControl detailsWin;

        public AccountingUserControl()
        {
            InitializeComponent();
            detailsWin = new DetailsUserControl();
            detailsUserControl.Content = detailsWin;
        }

        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (detailsUserControl.Content != null)
            {
                detailsUserControl.Content = null;
                collapseImage.Source = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            }
            else
            {
                detailsUserControl.Content = detailsWin;
                collapseImage.Source = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
            }
        }
    }
}
