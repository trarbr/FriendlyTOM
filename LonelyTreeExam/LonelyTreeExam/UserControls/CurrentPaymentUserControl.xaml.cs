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

using Domain.Controller;
using Common.Interfaces;

namespace LonelyTreeExam.UserControls
{
    /// <summary>
    /// Interaction logic for CurrentPaymentUserControl.xaml
    /// </summary>
    public partial class CurrentPaymentUserControl : UserControl
    {
        private PaymentController paymentController; 
        public CurrentPaymentUserControl(PaymentController paymentController)
        {
            InitializeComponent();

            this.paymentController = paymentController;

            detailsUserControl.Content = new DetailsUserControl(paymentController);

            Refresh();
        }

        public void Refresh()
        {
            List<IPayment> payments = paymentController.ReadAllPayments();

            List<IPayment> currentPayments = new List<IPayment>();
            foreach (IPayment payment in payments)
            {
                if (payment.Archived == false)
                {
                    currentPayments.Add(payment);
                }
            }

            currentPaymentsDataGrid.DataContext = currentPayments;
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            // clear details and selection in datagrid

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
