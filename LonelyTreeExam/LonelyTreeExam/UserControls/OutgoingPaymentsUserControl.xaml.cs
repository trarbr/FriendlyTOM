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
        public OutgoingPaymentsUserControl(PaymentController paymentController,
            CustomerController customerController, SupplierController supplierController)
        {
            InitializeComponent();

            this.paymentController = paymentController;
            this.supplierController = supplierController;
            this.customerController = customerController;

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
        private CustomerController customerController;
        private DetailsUserControl details;
        private IPayment selectedPayment;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private List<IPayment> outgoingPayments;

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            details.attachmentsListView.ItemsSource = null;
            paymentsDataGrid.SelectedItem = null;
            details.responsibleTextBox.Text = "Lonely Tree";
            details.ClearAttachments();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment == null)
            {
                details.CreatePayment(supplierController.ReadAllSuppliers(), customerController.ReadAllCustomers());
                RefreshPaymentDataGrid();
            }
            else
            {
                int currentIndex = paymentsDataGrid.SelectedIndex;
                details.UpdatePayment(supplierController.ReadAllSuppliers(), customerController.ReadAllCustomers());
                RefreshPaymentDataGrid();
                paymentsDataGrid.SelectedIndex = currentIndex;
            }
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
                foreach (IPayment payment in paymentsDataGrid.SelectedItems)
                {
                    paymentController.DeletePayment(payment);
                }
                paymentsDataGrid.SelectedItem = null;
                details.responsibleTextBox.Text = "Lonely Tree";
                RefreshPaymentDataGrid();
            }
        }

        private void archiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPayment != null)
            {
                foreach (IPayment payment in paymentsDataGrid.SelectedItems)
                {
                    payment.Archived = true;
                    paymentController.UpdatePayment(payment);
                }
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
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                        payment.Commissioner, payment.DueDate.ToString("yyyy-MM-dd"), payment.DueAmount,
                        payment.PaidDate.ToString("yyyy-MM-dd"), payment.PaidAmount, payment.Note, payment.Sale,
                        payment.Booking, payment.Invoice, payment.Type);

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
