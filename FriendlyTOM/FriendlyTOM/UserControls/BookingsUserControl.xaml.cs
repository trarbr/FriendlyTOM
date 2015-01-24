﻿/*
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

using Common.Enums;
using Common.Interfaces;
using Domain.Controller;
using FriendlyTOM.AutoComplete;
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

namespace FriendlyTOM.UserControls
{
    /// <summary>
    /// Interaction logic for BookingsUserControl.xaml
    /// </summary>
    public partial class BookingsUserControl : UserControl
    {
        public BookingsUserControl(BookingController bookingController, SupplierController supplierController,
            CustomerController customerController)
        {
            InitializeComponent();
            this.bookingController = bookingController;
            this.supplierController = supplierController;
            this.customerController = customerController;
            bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
            bookingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BookingType));
            bookingTypeComboBox.SelectedIndex = 0;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
            autoCompleteEntries = new HashSet<IParty>();
            addAutoCompleteEntries();
            culture = MainWindow.GetCulture();
        }

        internal void Refresh()
        {
            refreshDataGrid();
            addAutoCompleteEntries();
        }

        #region Private Fields
        private BookingController bookingController;
        private SupplierController supplierController;
        private CustomerController customerController;
        private IBooking selectedBooking;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private HashSet<IParty> autoCompleteEntries;
        private CultureInfo culture;
        #endregion

        #region Private Methods
        private void addAutoCompleteEntries()
        {
            foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
            {
                if (!autoCompleteEntries.Contains(supplier))
                {
                    supplierTextBox.AddItem(new AutoCompleteEntry(supplier.Name, null));
                    autoCompleteEntries.Add(supplier);
                }
            }

            foreach (ICustomer customer in customerController.ReadAllCustomers())
            {
                if (!autoCompleteEntries.Contains(customer))
                {
                    customerTextBox.AddItem(new AutoCompleteEntry(customer.Name, null));
                    autoCompleteEntries.Add(customer);
                }
            }
        }

        private void refreshDataGrid()
        {
            bookingsDataGrid.ItemsSource = null;
            bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            bookingsDataGrid.SelectedItem = null;
            setValuesInTextBoxes();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking != null)
            {
                MessageBoxResult doDelete = MessageBox.Show("Delete selected booking(s)?", "Confirm deletion",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (doDelete == MessageBoxResult.Yes)
                {
                    foreach (IBooking booking in bookingsDataGrid.SelectedItems)
                    {
                        bookingController.DeleteBooking(booking);
                    }
                    bookingsDataGrid.SelectedItem = null;
                    refreshDataGrid();
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedBooking == null)
                {
                    createNewBooking();

                    refreshDataGrid();
                    setValuesInTextBoxes();
                }
                else
                {
                    int currentIndex = bookingsDataGrid.SelectedIndex;

                    updateExistingBooking();
                    
                    refreshDataGrid();
                    bookingsDataGrid.SelectedIndex = currentIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createNewBooking()
        {
            int bookingNumber;
            int.TryParse(bookingNumberTextBox.Text, out bookingNumber);

            string sale = saleTextBox.Text;
            DateTime startDate = startDateDatePicker.SelectedDate.Value;
            DateTime endDate = endDateDatePicker.SelectedDate.Value;

            ISupplier selectedSupplier = findSupplierByName();
            ICustomer selectedCustomer = findCustomerByName();

            IBooking booking = bookingController.CreateBooking(selectedSupplier, selectedCustomer, sale, bookingNumber,
                startDate, endDate);

            decimal iVAExempt;
            decimal iVASubject;
            decimal service;
            decimal productRetention;
            decimal supplierRetention;

            decimal.TryParse(IVAExemptTextBox.Text, NumberStyles.Any, culture, out iVAExempt);
            decimal.TryParse(IVASubjectTextBox.Text, NumberStyles.Any, culture, out iVASubject);
            decimal.TryParse(serviceTextBox.Text, NumberStyles.Any, culture, out service);
            decimal.TryParse(productRetentionTextBox.Text, NumberStyles.Any, culture, out productRetention);
            decimal.TryParse(supplierRetentionTextBox.Text, NumberStyles.Any, culture, out supplierRetention);

            booking.Type = (BookingType)bookingTypeComboBox.SelectedItem;
            booking.IVAExempt = iVAExempt;
            booking.IVASubject = iVASubject;
            booking.Service = service;
            booking.ProductRetention = productRetention;
            booking.SupplierRetention = supplierRetention;
            booking.Note = noteTextBox.Text;

            bookingController.UpdateBooking(booking);
        }

        private void updateExistingBooking()
        {
            decimal iVAExempt;
            decimal iVASubject;
            decimal service;
            decimal productRetention;
            decimal supplierRetention;
            int bookingNumber;

            decimal.TryParse(IVAExemptTextBox.Text, NumberStyles.Any, culture, out iVAExempt);
            decimal.TryParse(IVASubjectTextBox.Text, NumberStyles.Any, culture, out iVASubject);
            decimal.TryParse(serviceTextBox.Text, NumberStyles.Any, culture, out service);
            decimal.TryParse(productRetentionTextBox.Text, NumberStyles.Any, culture, out productRetention);
            decimal.TryParse(supplierRetentionTextBox.Text, NumberStyles.Any, culture, out supplierRetention);
            int.TryParse(bookingNumberTextBox.Text, out bookingNumber);
            ISupplier selectedSupplier = findSupplierByName();
            ICustomer selectedCustomer = findCustomerByName();

            selectedBooking.IVAExempt = iVAExempt;
            selectedBooking.IVASubject = iVASubject;
            selectedBooking.Service = service;
            selectedBooking.ProductRetention = productRetention;
            selectedBooking.SupplierRetention = supplierRetention;
            selectedBooking.BookingNumber = bookingNumber;
            selectedBooking.Customer = selectedCustomer;
            selectedBooking.Supplier = selectedSupplier;
            selectedBooking.EndDate = endDateDatePicker.SelectedDate.Value;
            selectedBooking.StartDate = startDateDatePicker.SelectedDate.Value;
            selectedBooking.Note = noteTextBox.Text;
            selectedBooking.Sale = saleTextBox.Text;
            selectedBooking.Type = (BookingType)bookingTypeComboBox.SelectedItem;

            bookingController.UpdateBooking(selectedBooking);
        }

        private ICustomer findCustomerByName()
        {
            ICustomer selectedCustomer = null;
            foreach (ICustomer customer in customerController.ReadAllCustomers())
            {
                if (customer.Name == customerTextBox.Text)
                {
                    selectedCustomer = customer;
                    break;
                }
            }
            return selectedCustomer;
        }

        private ISupplier findSupplierByName()
        {
            ISupplier selectedSupplier = null;
            foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
            {
                if (supplier.Name == supplierTextBox.Text)
                {
                    selectedSupplier = supplier;
                    break;
                }
            }
            return selectedSupplier;
        }


        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (detailsGrid.Visibility != Visibility.Collapsed)
            {
                detailsGrid.Visibility = Visibility.Collapsed;
                bottomButtonsGrid.Visibility = Visibility.Collapsed;
                collapseImage.Source = collapsePlusImage;
                collapseButton.ToolTip = "Show details";
            }
            else
            {
                detailsGrid.Visibility = Visibility.Visible;
                bottomButtonsGrid.Visibility = Visibility.Visible;
                collapseImage.Source = collapseMinImage;
                collapseButton.ToolTip = "Hide details";
            }
        }

        private void bookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBooking = (IBooking)bookingsDataGrid.SelectedItem;
            setValuesInTextBoxes();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<IBooking> searchedBookings = new List<IBooking>();
                bookingsDataGrid.ItemsSource = searchedBookings;

                foreach (IBooking booking in bookingController.ReadAllBookings())
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                        booking.Service, booking.Sale, booking.Type.ToString(), booking.StartDate,
                        booking.SupplierRetention, booking.Supplier.Name, booking.ProductRetention, 
                        booking.Note, booking.IVASubject, booking.IVAExempt, booking.EndDate, booking.Customer.Name, 
                        booking.BookingNumber);
                    if (searchData.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedBookings.Add(booking);
                    }
                }
            }
            else
            {
                bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
            }

            bookingsDataGrid.Items.Refresh();
        }

        private void setValuesInTextBoxes()
        {
            if (selectedBooking != null)
            {
                saleTextBox.Text = selectedBooking.Sale;
                bookingNumberTextBox.Text = selectedBooking.BookingNumber.ToString();
                startDateDatePicker.SelectedDate = selectedBooking.StartDate;
                endDateDatePicker.SelectedDate = selectedBooking.EndDate;
                bookingTypeComboBox.SelectedItem = selectedBooking.Type;
                IVAExemptTextBox.Text = selectedBooking.IVAExempt.ToString("N2", culture.NumberFormat);
                IVASubjectTextBox.Text = selectedBooking.IVASubject.ToString("N2", culture.NumberFormat);
                serviceTextBox.Text = selectedBooking.Service.ToString("N2", culture.NumberFormat);
                productRetentionTextBox.Text = selectedBooking.ProductRetention.ToString("N2", culture.NumberFormat);
                supplierRetentionTextBox.Text = selectedBooking.SupplierRetention.ToString("N2", culture.NumberFormat);
                supplierTextBox.Text = selectedBooking.Supplier.Name;
                customerTextBox.Text = selectedBooking.Customer.Name;
                noteTextBox.Text = selectedBooking.Note;
            }
            else
            {
                saleTextBox.Text = "";
                bookingNumberTextBox.Text = "";
                startDateDatePicker.SelectedDate = null;
                endDateDatePicker.SelectedDate = null;
                bookingTypeComboBox.SelectedIndex = 0;
                IVAExemptTextBox.Text = "";
                IVASubjectTextBox.Text = "";
                serviceTextBox.Text = "";
                productRetentionTextBox.Text = "";
                supplierRetentionTextBox.Text = "";
                supplierTextBox.Text = "";
                customerTextBox.Text = "";
                noteTextBox.Text = "";
            }
        }

        private void createPayments_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking == null)
            {
                MessageBox.Show("Please select a booking first!");
            }
            else
            {
                updateExistingBooking();
                bookingController.CreatePaymentsForBooking(selectedBooking);
                refreshDataGrid();
            }
        }
        #endregion
    }
}
