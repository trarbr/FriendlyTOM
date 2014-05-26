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
using LonelyTreeExam.AutoComplete;

namespace LonelyTreeExam.UserControls
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
            AddCustomersToAutoComplete(customerController.ReadAllCustomers());
        }

        private CustomerController customerController;
        private SupplierController supplierController;
        private ISupplier selectedSupplier;
        private IPaymentRule selectedPaymentRule;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private HashSet<string> autoCompleteEntries;

        private void refreshDataGrid()
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

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (supplierTabControl.IsSelected)
                {
                    if (selectedSupplier == null)
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
                        refreshDataGrid();
                        suppliersDataGrid.SelectedItem = null;
                        setValuesInTextBoxes();
                    }
                    else
                    {
                        int currentIndex = suppliersDataGrid.SelectedIndex;

                        selectedSupplier.Name = nameTextBox.Text;
                        selectedSupplier.Type = (SupplierType)supplierTypeComboBox.SelectedItem;
                        selectedSupplier.Note = noteTextBox.Text;
                        selectedSupplier.AccountName = accountNameTextBox.Text;
                        selectedSupplier.AccountNo = accountNoTextBox.Text;
                        selectedSupplier.AccountType = (AccountType)accountTypeComboBox.SelectedItem;
                        selectedSupplier.Bank = bankTextBox.Text;
                        selectedSupplier.OwnerId = ownerIdTextBox.Text;

                        supplierController.UpdateSupplier(selectedSupplier);
                        refreshDataGrid();
                        suppliersDataGrid.SelectedIndex = currentIndex;
                    }
                }
                else if (paymentRuleTabControl.IsSelected)
                {
                    ICustomer customer = null;

                    foreach (ICustomer theCustomer in customerController.ReadAllCustomers())
                    {
                        if (theCustomer.Name == customerTextBox.Text)
                        {
                            customer = theCustomer;
                        }
                    }

                    ISupplier supplier = selectedSupplier;
                    BookingType bookingType = (BookingType) bookingTypeComboBox.SelectedItem;
                    decimal percentage;
                    decimal.TryParse(percentageTextBox.Text, out percentage);
                    int daysOffSet;
                    int.TryParse(daysOffsetTextBox.Text, out daysOffSet);
                    BaseDate baseDate = (BaseDate) baseDateComboBox.SelectedItem;
                    PaymentType paymentType = (PaymentType) paymentTypeComboBox.SelectedItem;


                    supplierController.AddPaymentRule(supplier, customer, bookingType, percentage, daysOffSet, baseDate,
                                                      paymentType);
                    refreshPaymentRuleDataGrid();
                    setPaymentRuleValuesInTextBoxes();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setValuesInTextBoxes()
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
                percentageTextBox.Text = selectedPaymentRule.Percentage.ToString();
                daysOffsetTextBox.Text = selectedPaymentRule.DaysOffset.ToString();
                baseDateComboBox.Text = selectedPaymentRule.BaseDate.ToString();
                paymentTypeComboBox.SelectedItem = selectedPaymentRule.PaymentType;
            }
            else
            {
                supplierTextBox.Text = "";
                customerTextBox.Text = "";
                bookingTypeComboBox.SelectedIndex = 0;
                percentageTextBox.Text = "";
                daysOffsetTextBox.Text = "";
                baseDateComboBox.SelectedIndex = 0;
                paymentTypeComboBox.SelectedIndex = 0;
            }
        }

        internal void AddCustomersToAutoComplete(List<ICustomer> customers)
        {
            foreach (ICustomer customer in customers)
            {
                if (!autoCompleteEntries.Contains(customer.Name))
                {
                    customerTextBox.AddItem(new AutoCompleteEntry(customer.Name, null));
                    autoCompleteEntries.Add(customer.Name);
                }
            }
        }

        private void suppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSupplier = (ISupplier)suppliersDataGrid.SelectedItem;
            setValuesInTextBoxes();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            suppliersDataGrid.SelectedItem = null;
            setValuesInTextBoxes();
            paymentRuleDataGrid.SelectedItem = null;
            setPaymentRuleValuesInTextBoxes();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (supplierTabControl.IsSelected)
            {
                if (selectedSupplier != null)
                {
                    foreach (ISupplier supplier in suppliersDataGrid.SelectedItems)
                    {
                        supplierController.DeleteSupplier(supplier);
                    }
                    suppliersDataGrid.SelectedItem = null;
                    refreshDataGrid();
                }
            }
            else if (paymentRuleTabControl.IsSelected)
            {
                if (selectedPaymentRule != null)
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

        private void paymentRuleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPaymentRule = (IPaymentRule) paymentRuleDataGrid.SelectedItem;
            setPaymentRuleValuesInTextBoxes();
        }
    }
}
