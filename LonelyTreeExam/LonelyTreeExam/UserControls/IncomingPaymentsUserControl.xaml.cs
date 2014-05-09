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
    public partial class IncomingPaymentsUserControl : UserControl
    {
        private PaymentController paymentController;
        private DetailsUserControl details;
        private IPayment selectedPayment;

        public IncomingPaymentsUserControl(PaymentController paymentController)
        {
            InitializeComponent();

            this.paymentController = paymentController;

            details = new DetailsUserControl(paymentController);
            detailsUserControl.Content = details;

            RefreshPaymentDataGrid();
        }

        internal void RefreshPaymentDataGrid()
        {
            // denne logik skal muligvis ned i Controller laget
            paymentsDataGrid.ItemsSource = null;

            List<IPayment> allPayments = paymentController.ReadAllPayments();
            List<IPayment> incomingPayments = new List<IPayment>();

            foreach (IPayment payment in allPayments)
            {
                if (payment.Archived == false && payment.Responsible != "Lonely Tree")
                {
                    incomingPayments.Add(payment);
                }
            }

            paymentsDataGrid.ItemsSource = incomingPayments;
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            paymentsDataGrid.SelectedItem = null;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment == null)
            {
                details.CreatePayment();
            }
            else
            {
                details.UpdatePayment(selectedPayment);
            }

            RefreshPaymentDataGrid();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            paymentsDataGrid.SelectedItem = null;
        }

        private void paymentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPayment = (IPayment)paymentsDataGrid.SelectedItem;
            details.SetValuesInTextBoxes(selectedPayment);
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

        private void archiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                selectedPayment.Archived = true;
                paymentsDataGrid.SelectedItem = null;
                RefreshPaymentDataGrid();
            }
        }
    }
}
