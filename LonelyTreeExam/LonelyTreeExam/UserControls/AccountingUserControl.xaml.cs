using Domain.Controller;
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
        public AccountingUserControl()
        {
            InitializeComponent();
            paymentController = new PaymentController();


            currentPaymentsUserControl.Content = new CurrentPaymentUserControl(paymentController);
            archiveUserControl.Content = new ArchivedPaymentsUserControl(paymentController);
        }

        private DetailsUserControl details;
        private PaymentsUserControl currentPayments;
        private PaymentsUserControl archivedPayments;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private PaymentController paymentController;


        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
