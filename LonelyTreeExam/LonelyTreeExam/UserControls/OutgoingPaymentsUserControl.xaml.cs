using Common.Interfaces;
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
    /// Interaction logic for OutgoingPaymentsUserControl.xaml
    /// </summary>
    public partial class OutgoingPaymentsUserControl : UserControl
    {
        public OutgoingPaymentsUserControl(PaymentController paymentController, SupplierController supplierController)
        {
            InitializeComponent();

            this.paymentController = paymentController;
            this.supplierController = supplierController;

            details = new DetailsUserControl(paymentController);
            details.responsibleTextBox.Text = "Lonely Tree";
            details.responsibleTextBox.IsEnabled = false;
            detailsUserControl.Content = details;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));

            RefreshPaymentDataGrid();
        }

        internal void RefreshPaymentDataGrid()
        {
            paymentsDataGrid.ItemsSource = null;

            outgoingPayments = paymentController.ReadAllOutgoingPayments();
            paymentsDataGrid.ItemsSource = outgoingPayments;
            details.responsibleTextBox.Text = "Lonely Tree";

            details.AddSuppliersToAutoComplete(supplierController.ReadAllSuppliers());
        }

        private PaymentController paymentController;
        private SupplierController supplierController;
        private DetailsUserControl details;
        private IPayment selectedPayment;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private List<IPayment> outgoingPayments;

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            paymentsDataGrid.SelectedItem = null;
            details.responsibleTextBox.Text = "Lonely Tree";
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment == null)
            {
                details.CreatePayment();
            }
            else
            {
                details.UpdatePayment();
            }

            RefreshPaymentDataGrid();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            paymentsDataGrid.SelectedItem = null;
            details.responsibleTextBox.Text = "Lonely Tree";
        }

        private void paymentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPayment = (IPayment)paymentsDataGrid.SelectedItem;
            details.SetSelectedPayment(selectedPayment);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                paymentController.DeletePayment(selectedPayment);
                paymentsDataGrid.SelectedItem = null;
                details.responsibleTextBox.Text = "Lonely Tree";
                RefreshPaymentDataGrid();
            }
        }

        private void archiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                selectedPayment.Archived = true;
                paymentController.UpdatePayment(selectedPayment);
                paymentsDataGrid.SelectedItem = null;
                details.responsibleTextBox.Text = "Lonely Tree";
                RefreshPaymentDataGrid();
            }
        }

        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (detailsUserControl.Content != null)
            {
                detailsUserControl.Content = null;
                collapseImage.Source = collapsePlusImage;
                collapseButton.ToolTip = "Show details";
                bottomStackPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                detailsUserControl.Content = details;
                collapseImage.Source = collapseMinImage;
                collapseButton.ToolTip = "Hide details";
                bottomStackPanel.Visibility = Visibility.Visible;
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<IPayment> searchedPayments = new List<IPayment>();
                paymentsDataGrid.ItemsSource = searchedPayments;

                foreach (IPayment payment in outgoingPayments)
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6}", payment.Commissioner,
                        payment.DueDate.ToString("yyyy-MM-dd"), payment.DueAmount,
                        payment.PaidDate.ToString("yyyy-MM-dd"), payment.PaidAmount, payment.Note, payment.Sale);

                    if (searchData.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedPayments.Add(payment);
                    }
                }
            }
            else
            {
                paymentsDataGrid.ItemsSource = outgoingPayments;
            }

            paymentsDataGrid.Items.Refresh();
        }
    }
}
