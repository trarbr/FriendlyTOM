using Common.Interfaces;
using Domain.Controller;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public DetailsUserControl(PaymentController controller)
        {
            InitializeComponent();
            paymentController = controller;
            culture = new CultureInfo("en-US");
        }

        internal void SetValuesInTextBoxes(IPayment payment)
        {
            if (payment != null)
            {
                dueDateDataPicker.SelectedDate = payment.DueDate;
                dueAmountTextBox.Text = payment.DueAmount.ToString("N2", culture.NumberFormat);
                responsibleTextBox.Text = payment.Responsible;
                commissionerTextBox.Text = payment.Commissioner;
                paidDateDatePicker.SelectedDate = payment.PaidDate;
                paidAmountTextBox.Text = payment.PaidAmount.ToString("N2", culture.NumberFormat);
                paidCheckBox.IsChecked = payment.Paid;
                noteTextBox.Text = payment.Note;
            }
            else
            {
                dueDateDataPicker.SelectedDate = null;
                dueAmountTextBox.Text = "";
                responsibleTextBox.Text = "";
                commissionerTextBox.Text = "";
                paidDateDatePicker.SelectedDate = null;
                paidAmountTextBox.Text = "";
                paidCheckBox.IsChecked = false;
                noteTextBox.Text = "";
            }
        }

        internal void CreatePayment()
        {
            decimal dueAmount;
            decimal.TryParse(dueAmountTextBox.Text, NumberStyles.Any, culture, out dueAmount);

            IPayment payment = paymentController.CreatePayment(dueDateDataPicker.SelectedDate.Value, dueAmount,
                                                               responsibleTextBox.Text, commissionerTextBox.Text);

            decimal paidAmount;
            decimal.TryParse(paidAmountTextBox.Text, NumberStyles.Any, culture, out paidAmount);

            payment.PaidAmount = paidAmount;
            payment.Paid = paidCheckBox.IsChecked.Value;
            payment.PaidDate = paidDateDatePicker.SelectedDate.Value;
            payment.Note = noteTextBox.Text;
            paymentController.UpdatePayment(payment);
        }

        internal void UpdatePayment(IPayment selectedPayment)
        {
            IPayment payment = selectedPayment;
            if (payment != null)
            {
                decimal dueAmount;
                decimal.TryParse(dueAmountTextBox.Text, NumberStyles.Any, culture, out dueAmount);

                decimal paidAmount;
                decimal.TryParse(paidAmountTextBox.Text, NumberStyles.Any, culture, out paidAmount);

                payment.DueDate = dueDateDataPicker.SelectedDate.Value;
                payment.DueAmount = dueAmount;
                payment.Responsible = responsibleTextBox.Text;
                payment.Commissioner = commissionerTextBox.Text;
                payment.PaidAmount = paidAmount;
                payment.Paid = paidCheckBox.IsChecked.Value;
                payment.PaidDate = paidDateDatePicker.SelectedDate.Value;
                payment.Note = noteTextBox.Text;

                paymentController.UpdatePayment(payment);
            }
        }

        private PaymentController paymentController;
        private CultureInfo culture;

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofg = new OpenFileDialog();
            ofg.ShowDialog();
        }
    }
}
