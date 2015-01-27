/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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

namespace FriendlyTOM.UserControls
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
            details.payerTextBox.Text = "Lonely Tree";
            details.payerTextBox.IsEnabled = false;
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
            details.payerTextBox.Text = "Lonely Tree";

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
            details.payerTextBox.Text = "Lonely Tree";
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
                MessageBoxResult doDelete = MessageBox.Show("Delete selected payment(s)?", "Confirm deletion",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (doDelete == MessageBoxResult.Yes)
                {
                    foreach (IPayment payment in paymentsDataGrid.SelectedItems)
                    {
                        paymentController.DeletePayment(payment);
                    }
                    paymentsDataGrid.SelectedItem = null;
                    details.payerTextBox.Text = "Lonely Tree";
                    RefreshPaymentDataGrid();
                }
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
                details.payerTextBox.Text = "Lonely Tree";
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
                // BindingList / ObservableCollection instead?
                List<IPayment> searchedPayments = new List<IPayment>();
                paymentsDataGrid.ItemsSource = searchedPayments;

                foreach (IPayment payment in outgoingPayments)
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}",
                        payment.Payee.Name, payment.DueDate.ToString("yyyy-MM-dd"), payment.DueAmount,
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
