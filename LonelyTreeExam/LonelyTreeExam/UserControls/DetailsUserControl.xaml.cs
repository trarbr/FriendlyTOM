﻿using System.Diagnostics;
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
            //AddAutoCompleteEntries();
            paymentTypeComboBox.ItemsSource = Enum.GetValues(typeof(PaymentType));
            paymentTypeComboBox.SelectedIndex = 0;
            autoCompleteEntries = new HashSet<string>();
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
                    int booking;
                    int.TryParse(bookingTextBox.Text, out booking);
                    IPayment payment = paymentController.CreatePayment(dueDate, dueAmount, 
                        responsibleTextBox.Text, commissionerTextBox.Text,
                        (PaymentType)paymentTypeComboBox.SelectedItem, saleTextBox.Text, booking);

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
                    payment.Invoice = invoiceTextBox.Text;
                    
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
                selectedPayment.Type = (PaymentType)paymentTypeComboBox.SelectedItem;
                selectedPayment.Sale = saleTextBox.Text;
                int booking;
                int.TryParse(bookingTextBox.Text, out booking);
                selectedPayment.Booking = booking;
                selectedPayment.Invoice = invoiceTextBox.Text;
                
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
        private HashSet<string> autoCompleteEntries;
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
                paymentTypeComboBox.SelectedItem = selectedPayment.Type;
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
                saleTextBox.Text = selectedPayment.Sale;
                bookingTextBox.Text = selectedPayment.Booking.ToString();
                invoiceTextBox.Text = selectedPayment.Invoice;
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
                paymentTypeComboBox.SelectedIndex = 0;
                saleTextBox.Text = "";
                bookingTextBox.Text = "";
                invoiceTextBox.Text = "";
                attachmentsListView.ItemsSource = null;
            }
        }

        internal void AddSuppliersToAutoComplete(List<ISupplier> suppliers)
        {
            foreach (ISupplier supplier in suppliers)
            {
                if (!autoCompleteEntries.Contains(supplier.Name))
                {
                    commissionerTextBox.AddItem(new AutoCompleteEntry(supplier.Name, null));
                    autoCompleteEntries.Add(supplier.Name);
                }
            }
        }

        internal void AddCustomersToAutoComplete(List<ICustomer> customers)
        {
            foreach (ICustomer customer in customers)
            {
                if (!autoCompleteEntries.Contains(customer.Name))
                {
                    responsibleTextBox.AddItem(new AutoCompleteEntry(customer.Name, null));
                    autoCompleteEntries.Add(customer.Name);
                }
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
