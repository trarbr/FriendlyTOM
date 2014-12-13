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
using FriendlyTOM.AutoComplete;
using System.Globalization;

namespace FriendlyTOM.UserControls
{
    /// <summary>
    /// Interaction logic for SuppliersUserControl.xaml
    /// </summary>
    public partial class SuppliersUserControl : UserControl
    {
        public SuppliersUserControl(SupplierController supplierController, CustomerController customerController)
        {
            InitializeComponent();
            this.customerController = customerController;
            this.supplierController = supplierController;
            culture = MainWindow.GetCulture();
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
            supplierTypeComboBox.ItemsSource = Enum.GetValues(typeof(SupplierType));
            supplierTypeComboBox.SelectedIndex = 0;
            accountTypeComboBox.ItemsSource = Enum.GetValues(typeof(AccountType));
            accountTypeComboBox.SelectedIndex = 0;
            bookingTypeComboBox.ItemsSource = Enum.GetValues(typeof (BookingType));
            bookingTypeComboBox.SelectedIndex = 0;
            baseDateComboBox.ItemsSource = Enum.GetValues(typeof (BaseDate));
            baseDateComboBox.SelectedIndex = 0;
            paymentTypeComboBox.ItemsSource = Enum.GetValues(typeof (PaymentType));
            paymentTypeComboBox.SelectedIndex = 0;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
            autoCompleteEntries = new HashSet<string>();
            addCustomersToAutoComplete();
        }

        private CustomerController customerController;
        private SupplierController supplierController;
        private ISupplier selectedSupplier;
        private IPaymentRule selectedPaymentRule;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private HashSet<string> autoCompleteEntries;
        private CultureInfo culture;

        private void refreshSupplierDataGrid()
        {
            suppliersDataGrid.ItemsSource = null;
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
        }

        private void refreshPaymentRuleDataGrid()
        {
            paymentRuleDataGrid.ItemsSource = null;
            paymentRuleDataGrid.ItemsSource = selectedSupplier.PaymentRules;
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<ISupplier> searchedSuppliers = new List<ISupplier>();
                suppliersDataGrid.ItemsSource = searchedSuppliers;

                foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                        supplier.Name, supplier.Note, supplier.Type.ToString(), supplier.AccountName, 
                        supplier.AccountNo, supplier.AccountType, supplier.Bank, supplier.OwnerId);
                    if (searchData.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedSuppliers.Add(supplier);
                    }
                }
            }
            else
            {
                suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
            }

            suppliersDataGrid.Items.Refresh();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (supplierTabControl.IsSelected)
            {
                suppliersDataGrid.SelectedItem = null;
                setSupplierValuesInTextBoxes();
            }
            else
            {
                paymentRuleDataGrid.SelectedItem = null;
                setPaymentRuleValuesInTextBoxes();
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (supplierTabControl.IsSelected)
            {
                if (selectedSupplier != null)
                {
                    MessageBoxResult doDelete = MessageBox.Show(
                        "Delete selected suppliers(s)? This will delete all bookings, payments and payment rules related to the supplier(s)!", 
                        "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (doDelete == MessageBoxResult.Yes)
                    {
                        foreach (ISupplier supplier in suppliersDataGrid.SelectedItems)
                        {
                            supplierController.DeleteSupplier(supplier);
                        }
                        suppliersDataGrid.SelectedItem = null;
                        refreshSupplierDataGrid();
                    }
                }
            }
            else if (paymentRuleTabControl.IsSelected)
            {
                if (selectedPaymentRule != null)
                {
                    MessageBoxResult doDelete = MessageBox.Show("Delete selected payment rules(s)?", 
                        "Confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (doDelete == MessageBoxResult.Yes)
                    {
                        foreach (IPaymentRule paymentRule in paymentRuleDataGrid.SelectedItems)
                        {
                            supplierController.DeletePaymentRule(paymentRule);
                        }
                        paymentRuleDataGrid.SelectedItem = null;
                        refreshPaymentRuleDataGrid();
                    }
                }
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (supplierTabControl.IsSelected)
                {
                    createOrUpdateSupplier();
                }
                else if (paymentRuleTabControl.IsSelected)
                {
                    createOrUpdatePaymentRule();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createOrUpdateSupplier()
        {
            if (selectedSupplier == null)
            {
                createNewSupplier();

                refreshSupplierDataGrid();
                setSupplierValuesInTextBoxes();
            }
            else
            {
                int currentIndex = suppliersDataGrid.SelectedIndex;

                updateExistingSupplier();

                refreshSupplierDataGrid();
                suppliersDataGrid.SelectedIndex = currentIndex;
            }
        }

        private void createNewSupplier()
        {
            string name = nameTextBox.Text;
            SupplierType type = (SupplierType)supplierTypeComboBox.SelectedItem;
            string note = noteTextBox.Text;

            ISupplier supplier = supplierController.CreateSupplier(name, note, type);
            supplier.AccountName = accountNameTextBox.Text;
            supplier.AccountNo = accountNoTextBox.Text;
            supplier.AccountType = (AccountType)accountTypeComboBox.SelectedItem;
            supplier.Bank = bankTextBox.Text;
            supplier.OwnerId = ownerIdTextBox.Text;

            supplierController.UpdateSupplier(supplier);
        }

        private void updateExistingSupplier()
        {
            selectedSupplier.Name = nameTextBox.Text;
            selectedSupplier.Type = (SupplierType)supplierTypeComboBox.SelectedItem;
            selectedSupplier.Note = noteTextBox.Text;
            selectedSupplier.AccountName = accountNameTextBox.Text;
            selectedSupplier.AccountNo = accountNoTextBox.Text;
            selectedSupplier.AccountType = (AccountType)accountTypeComboBox.SelectedItem;
            selectedSupplier.Bank = bankTextBox.Text;
            selectedSupplier.OwnerId = ownerIdTextBox.Text;

            supplierController.UpdateSupplier(selectedSupplier);
        }

        private void createOrUpdatePaymentRule()
        {
            if (selectedPaymentRule == null)
            {
                createNewPaymentRule();

                refreshPaymentRuleDataGrid();
                setPaymentRuleValuesInTextBoxes();
            }
            else
            {
                int currentIndex = paymentRuleDataGrid.SelectedIndex;

                updateExistingPaymentRule();

                refreshPaymentRuleDataGrid();
                paymentRuleDataGrid.SelectedIndex = currentIndex;
            }
        }

        private void createNewPaymentRule()
        {
            decimal percentage;
            int daysOffSet;
            decimal.TryParse(percentageTextBox.Text, NumberStyles.Any, culture, out percentage);
            int.TryParse(daysOffsetTextBox.Text, out daysOffSet);

            ICustomer customer = findCustomerByName();
            ISupplier supplier = selectedSupplier;
            BookingType bookingType = (BookingType)bookingTypeComboBox.SelectedItem;
            BaseDate baseDate = (BaseDate)baseDateComboBox.SelectedItem;
            PaymentType paymentType = (PaymentType)paymentTypeComboBox.SelectedItem;
            supplierController.AddPaymentRule(supplier, customer, bookingType, percentage, daysOffSet, baseDate,
                                              paymentType);
        }

        private void updateExistingPaymentRule()
        {
            decimal percentage;
            int daysOffSet;
            decimal.TryParse(percentageTextBox.Text, NumberStyles.Any, culture, out percentage);
            int.TryParse(daysOffsetTextBox.Text, out daysOffSet);

            ICustomer customer = findCustomerByName();
            BookingType bookingType = (BookingType)bookingTypeComboBox.SelectedItem;
            BaseDate baseDate = (BaseDate)baseDateComboBox.SelectedItem;
            PaymentType paymentType = (PaymentType)paymentTypeComboBox.SelectedItem;

            selectedPaymentRule.Percentage = percentage;
            selectedPaymentRule.DaysOffset = daysOffSet;
            selectedPaymentRule.Customer = customer;
            selectedPaymentRule.BookingType = bookingType;
            selectedPaymentRule.BaseDate = baseDate;
            selectedPaymentRule.PaymentType = paymentType;

            // PaymentRules are updated by updating the Supplier
            supplierController.UpdateSupplier(selectedSupplier);
        }

        private void setSupplierValuesInTextBoxes()
        {
            if (selectedSupplier != null)
            {
                nameTextBox.Text = selectedSupplier.Name;
                supplierTypeComboBox.SelectedItem = selectedSupplier.Type;
                noteTextBox.Text = selectedSupplier.Note;

                accountNoTextBox.Text = selectedSupplier.AccountNo;
                accountTypeComboBox.SelectedItem = selectedSupplier.AccountType;
                accountNameTextBox.Text = selectedSupplier.AccountName;
                ownerIdTextBox.Text = selectedSupplier.OwnerId;
                bankTextBox.Text = selectedSupplier.Bank;

                paymentRuleDataGrid.ItemsSource = selectedSupplier.PaymentRules;
                supplierTextBox.Text = selectedSupplier.Name;
            }
            else
            {
                nameTextBox.Text = "";
                supplierTypeComboBox.SelectedIndex = 0;
                noteTextBox.Text = "";
                accountNoTextBox.Text = "";
                accountTypeComboBox.SelectedIndex = 0;
                accountNameTextBox.Text = "";
                ownerIdTextBox.Text = "";
                bankTextBox.Text = "";

                paymentRuleDataGrid.ItemsSource = null;
                supplierTextBox.Text = "";
            }
        }

        private void setPaymentRuleValuesInTextBoxes()
        {
            if (selectedPaymentRule != null)
            {
                customerTextBox.Text = selectedPaymentRule.Customer.Name;
                bookingTypeComboBox.SelectedItem = selectedPaymentRule.BookingType;
                percentageTextBox.Text = selectedPaymentRule.Percentage.ToString("N2", culture.NumberFormat);
                daysOffsetTextBox.Text = selectedPaymentRule.DaysOffset.ToString();
                baseDateComboBox.Text = selectedPaymentRule.BaseDate.ToString();
                paymentTypeComboBox.SelectedItem = selectedPaymentRule.PaymentType;
            }
            else
            {
                customerTextBox.Text = "";
                bookingTypeComboBox.SelectedIndex = 0;
                percentageTextBox.Text = "";
                daysOffsetTextBox.Text = "";
                baseDateComboBox.SelectedIndex = 0;
                paymentTypeComboBox.SelectedIndex = 0;
            }
        }

        private void addCustomersToAutoComplete()
        {
            foreach (ICustomer customer in customerController.ReadAllCustomers())
            {
                if (!autoCompleteEntries.Contains(customer.Name))
                {
                    customerTextBox.AddItem(new AutoCompleteEntry(customer.Name, null));
                    autoCompleteEntries.Add(customer.Name);
                }
            }
        }

        private ICustomer findCustomerByName()
        {
            ICustomer customer = null;
            foreach (ICustomer theCustomer in customerController.ReadAllCustomers())
            {
                if (theCustomer.Name == customerTextBox.Text)
                {
                    customer = theCustomer;
                }
            }
            return customer;
        }

        private void suppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSupplier = (ISupplier)suppliersDataGrid.SelectedItem;
            setSupplierValuesInTextBoxes();
        }

        private void paymentRuleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPaymentRule = (IPaymentRule) paymentRuleDataGrid.SelectedItem;
            setPaymentRuleValuesInTextBoxes();
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

        internal void Refresh()
        {
            addCustomersToAutoComplete();
        }
    }
}
