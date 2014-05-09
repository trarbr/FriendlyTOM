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
    /// Interaction logic for DetailsUserControl.xaml
    /// </summary>
    public partial class DetailsUserControl : UserControl
    {
        public PaymentsUserControl PaymentsUserControl { get; set; }

        public DetailsUserControl(PaymentController controller)
        {
            paymentController = controller;
            InitializeComponent();
            attachmentsUserControl.Content = new AttachmentsUserControl();
        }

        public void SetValuesInTextBoxes(IPayment payment)
        {
            if (payment != null)
            {
                dueDateDataPicker.SelectedDate = payment.DueDate;
                dueAmountTextBox.Text = payment.DueAmount.ToString("N2");
                responsibleTextBox.Text = payment.Responsible;
                commissionerTextBox.Text = payment.Commissioner;
                paidDateDatePicker.SelectedDate = payment.PaidDate;
                paidAmountTextBox.Text = payment.PaidAmount.ToString("N2");
                paidCheckBox.IsChecked = payment.Paid;
                noteTextBox.Text = payment.Note;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            decimal dueAmount;
            decimal.TryParse(dueAmountTextBox.Text, out dueAmount);

            IPayment payment = paymentController.CreatePayment(dueDateDataPicker.SelectedDate.Value, dueAmount,
                                                               responsibleTextBox.Text, commissionerTextBox.Text);

            decimal paidAmount;
            decimal.TryParse(paidAmountTextBox.Text, out paidAmount);

            payment.PaidAmount = paidAmount;
            payment.Paid = paidCheckBox.IsChecked.Value;
            payment.PaidDate = paidDateDatePicker.SelectedDate.Value;
            payment.Note = noteTextBox.Text;
            paymentController.UpdatePayment(payment);

            updateGUI();

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            IPayment payment = (IPayment)PaymentsUserControl.mainDataGrid.SelectedItem;

            if (payment != null)
            {
                decimal dueAmount;
                decimal.TryParse(dueAmountTextBox.Text, out dueAmount);

                decimal paidAmount;
                decimal.TryParse(paidAmountTextBox.Text, out paidAmount);

                payment.DueDate = dueDateDataPicker.SelectedDate.Value;
                payment.DueAmount = dueAmount;
                payment.Responsible = responsibleTextBox.Text;
                payment.Commissioner = commissionerTextBox.Text;
                payment.PaidAmount = paidAmount;
                payment.Paid = paidCheckBox.IsChecked.Value;
                payment.PaidDate = paidDateDatePicker.SelectedDate.Value;
                payment.Note = noteTextBox.Text;

                paymentController.UpdatePayment(payment);

                updateGUI();
            }
        }

        private PaymentController paymentController;

        private void updateGUI()
        {
            PaymentsUserControl.mainDataGrid.ItemsSource = null;
            PaymentsUserControl.mainDataGrid.ItemsSource = paymentController.ReadAllPayments();
        }
    }
}
