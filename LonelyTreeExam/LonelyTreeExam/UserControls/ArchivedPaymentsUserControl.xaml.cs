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
    /// Interaction logic for ArchivedPaymentsUserControl.xaml
    /// </summary>
    public partial class ArchivedPaymentsUserControl : UserControl
    {
        private PaymentController paymentController;
        private IPayment selectedPayment;

        public ArchivedPaymentsUserControl(PaymentController paymentController)
        {
            InitializeComponent();

            this.paymentController = paymentController;

            RefreshPaymentDataGrid();
        }

        internal void RefreshPaymentDataGrid()
        {
            paymentsDataGrid.ItemsSource = null;

            List<IPayment> payments = paymentController.ReadAllPayments();

            List<IPayment> archivedPayments = new List<IPayment>();
            foreach (IPayment payment in payments)
            {
                if (payment.Archived == true)
                {
                    archivedPayments.Add(payment);
                }
            }

            paymentsDataGrid.ItemsSource = archivedPayments;
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                selectedPayment.Archived = false;
                paymentsDataGrid.SelectedItem = null;
                RefreshPaymentDataGrid();
            }
        }

        private void paymentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPayment = (IPayment)paymentsDataGrid.SelectedItem;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                paymentController.DeletePayment(selectedPayment);
                paymentsDataGrid.SelectedItem = null;
                RefreshPaymentDataGrid();
            }
        }
    }
}
