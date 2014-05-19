using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Domain.Controller;
using Common.Interfaces;

namespace LonelyTreeExam.UserControls
{
    /// <summary>
    /// Interaction logic for CurrentPaymentUserControl.xaml
    /// </summary>
    public partial class IncomingPaymentsUserControl : UserControl
    {
        public IncomingPaymentsUserControl(PaymentController paymentController)
        {
            InitializeComponent();

            this.paymentController = paymentController;

            details = new DetailsUserControl(paymentController);
            details.commissionerTextBox.Text = "Lonely Tree";
            details.commissionerTextBox.IsEnabled = false;
            detailsUserControl.Content = details;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));

            RefreshPaymentDataGrid();
        }

        internal void RefreshPaymentDataGrid()
        {
            paymentsDataGrid.ItemsSource = null;

            incomingPayments = paymentController.ReadAllIncomingPayments();
            paymentsDataGrid.ItemsSource = incomingPayments;
            details.commissionerTextBox.Text = "Lonely Tree";
        }

        private PaymentController paymentController;
        private DetailsUserControl details;
        private IPayment selectedPayment;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private List<IPayment> incomingPayments;

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            details.attachmentsListView.ItemsSource = null;
            paymentsDataGrid.SelectedItem = null;
            details.commissionerTextBox.Text = "Lonely Tree";
            details.ClearAttachments();
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
            details.attachmentsListView.ItemsSource = null;
            details.commissionerTextBox.Text = "Lonely Tree";
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
                details.commissionerTextBox.Text = "Lonely Tree";
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
                details.commissionerTextBox.Text = "Lonely Tree";
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

                foreach (IPayment payment in incomingPayments)
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6}", payment.Responsible, 
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
                paymentsDataGrid.ItemsSource = incomingPayments;
            }

            paymentsDataGrid.Items.Refresh();
        }
    }
}
