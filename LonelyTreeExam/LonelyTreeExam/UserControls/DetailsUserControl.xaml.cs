using System.Diagnostics;
using Common.Enums;
using Common.Interfaces;
using Domain.Controller;
using LonelyTreeExam.AutoComplete;
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
            attachmentList =  new List<string>();
            culture = new CultureInfo("en-US");
            AddAutoCompleteEntries();
            paymentTypeComboBox.ItemsSource = Enum.GetValues(typeof(PaymentType));
        }

        #region Internal Methods

        
        internal void CreatePayment()
        {
            if (dueDateDatePicker.SelectedDate != null)
            {
                try
                {
                    decimal dueAmount;
                    decimal.TryParse(dueAmountTextBox.Text, NumberStyles.Any, culture, out dueAmount);
                    DateTime dueDate = dueDateDatePicker.SelectedDate.Value;
                    IPayment payment = paymentController.CreatePayment(dueDate, dueAmount, 
                        responsibleTextBox.Text, commissionerTextBox.Text);

                    decimal paidAmount;
                    decimal.TryParse(paidAmountTextBox.Text, NumberStyles.Any, culture, out paidAmount);

                    payment.PaidAmount = paidAmount;
                    payment.Paid = paidCheckBox.IsChecked.Value;
                    if (paidDateDatePicker.SelectedDate != null)
                    {
                        payment.PaidDate = paidDateDatePicker.SelectedDate.Value;
                    }
                    payment.Note = noteTextBox.Text;

                    foreach (string attachments in attachmentList)
                    {
                        payment.AddAttachment(attachments);
                    }
                    
                    paymentController.UpdatePayment(payment);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Must select a due date");
            }
        }

        internal void UpdatePayment()
        {
            if (selectedPayment != null)
            {
                decimal dueAmount;
                decimal.TryParse(dueAmountTextBox.Text, NumberStyles.Any, culture, out dueAmount);

                decimal paidAmount;
                decimal.TryParse(paidAmountTextBox.Text, NumberStyles.Any, culture, out paidAmount);

                if (dueDateDatePicker.SelectedDate != null)
                {
                    selectedPayment.DueDate = dueDateDatePicker.SelectedDate.Value;
                }
                selectedPayment.DueAmount = dueAmount;
                selectedPayment.Responsible = responsibleTextBox.Text;
                selectedPayment.Commissioner = commissionerTextBox.Text;
                selectedPayment.PaidAmount = paidAmount;
                selectedPayment.Paid = paidCheckBox.IsChecked.Value;
                if (paidDateDatePicker.SelectedDate != null)
                {
                    selectedPayment.PaidDate = paidDateDatePicker.SelectedDate.Value;
                }
                selectedPayment.Note = noteTextBox.Text;
                
                paymentController.UpdatePayment(selectedPayment);
            }
        }

        internal void SetSelectedPayment(IPayment selectedPayment)
        {
            this.selectedPayment = selectedPayment;
            setValuesInTextBoxes();

        }
        #endregion

        #region Private Fields
        private List<string> attachmentList;
        private PaymentController paymentController;
        private CultureInfo culture;
        private IPayment selectedPayment;
        #endregion
        
        #region Private Method
        private void setValuesInTextBoxes()
        {
            if (selectedPayment != null)
            {
                dueDateDatePicker.SelectedDate = selectedPayment.DueDate;
                dueAmountTextBox.Text = selectedPayment.DueAmount.ToString("N2", culture.NumberFormat);
                responsibleTextBox.Text = selectedPayment.Responsible;
                commissionerTextBox.Text = selectedPayment.Commissioner;
                if (selectedPayment.PaidDate == new DateTime(1900, 1, 1))
                {
                    paidDateDatePicker.SelectedDate = null;
                }
                else
                {
                    paidDateDatePicker.SelectedDate = selectedPayment.PaidDate;
                }
                paidAmountTextBox.Text = selectedPayment.PaidAmount.ToString("N2", culture.NumberFormat);
                paidCheckBox.IsChecked = selectedPayment.Paid;
                noteTextBox.Text = selectedPayment.Note;
                attachmentsListView.ItemsSource = selectedPayment.Attachments;
            }
            else
            {
                dueDateDatePicker.SelectedDate = null;
                dueAmountTextBox.Text = "";
                responsibleTextBox.Text = "";
                commissionerTextBox.Text = "";
                paidDateDatePicker.SelectedDate = null;
                paidAmountTextBox.Text = "";
                paidCheckBox.IsChecked = false;
                noteTextBox.Text = "";
                attachmentsListView.ItemsSource = null;
            }
        }

        private void AddAutoCompleteEntries()
        {
            foreach (IPayment payment in paymentController.ReadAllPayments())
            {
                responsibleTextBox.AddItem(new AutoCompleteEntry(payment.Responsible, null));
            }
        }

        private void responsibleTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                responsibleTextBox.FocusComboBox();
            }
            else if (e.Key == Key.Enter)
            {
                responsibleTextBox.SelectItem();
            }
        }

        private void paidCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (paidCheckBox.IsChecked == true)
            {
                paidDateDatePicker.IsEnabled = true;
                paidAmountTextBox.IsEnabled = true;
                paymentTypeComboBox.IsEnabled = true;
            }
            else
            {
                paidDateDatePicker.IsEnabled = false;
                paidAmountTextBox.IsEnabled = false;
                paymentTypeComboBox.IsEnabled = false;
            }
        }

        private void addAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            string pathName;
            pathName = openFileDialog.FileName;

            if (pathName != "")
            {
                if (selectedPayment == null)
                {
                    attachmentList = new List<string>();
                    attachmentList.Add(pathName);
                    attachmentsListView.ItemsSource = null;
                    attachmentsListView.ItemsSource = attachmentList;
                }
                else if (selectedPayment != null)
                {
                    selectedPayment.AddAttachment(pathName);
                    UpdatePayment();
                    attachmentsListView.ItemsSource = null;
                    attachmentsListView.ItemsSource = selectedPayment.Attachments;
                }
            }
        }

        private void deleteAttachmentButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedAttachment = "";
            selectedAttachment = attachmentsListView.SelectedItem.ToString();
            if(selectedAttachment != "")
            {
                if (selectedPayment == null)
                {
                    attachmentList.Remove(selectedAttachment);
                    attachmentsListView.ItemsSource = null;
                    attachmentsListView.ItemsSource = attachmentList;
                }
                else
                {
                    selectedPayment.DeleteAttachment(selectedAttachment);
                    attachmentsListView.ItemsSource = null;
                    attachmentsListView.ItemsSource = selectedPayment.Attachments;
                }

                
            }
        }

        private void attachmentsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (attachmentsListView.SelectedItem != null)
                {
                    Process.Start(attachmentsListView.SelectedItem.ToString());    
                }
                else if (attachmentsListView.SelectedItem == null)
                {
                    MessageBox.Show("Need to select file");
                }
            }
            catch (Exception x)
            {

                x = new Exception(MessageBox.Show("File was not found").ToString());
            }



        }
        #endregion

    }
}
