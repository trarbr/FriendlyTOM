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

namespace LonelyTreeExam.UserControls
{
    /// <summary>
    /// Interaction logic for SuppliersUserControl.xaml
    /// </summary>
    public partial class SuppliersUserControl : UserControl
    {
        public SuppliersUserControl(SupplierController supplierController)
        {
            InitializeComponent();
            this.supplierController = supplierController;
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
            supplierTypeComboBox.ItemsSource = Enum.GetValues(typeof(SupplierType));
            supplierTypeComboBox.SelectedIndex = 0;
            accountTypeComboBox.ItemsSource = Enum.GetValues(typeof(AccountType));
            accountTypeComboBox.SelectedIndex = 0;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
        }

        private SupplierController supplierController;
        private ISupplier selectedSupplier;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;

        private void refreshDataGrid()
        {
            suppliersDataGrid.ItemsSource = null;
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
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
                    selectedSupplier.AccountType = (AccountType) accountTypeComboBox.SelectedItem;
                    selectedSupplier.Bank = bankTextBox.Text;
                    selectedSupplier.OwnerId = ownerIdTextBox.Text;

                    supplierController.UpdateSupplier(selectedSupplier);
                    refreshDataGrid();
                    suppliersDataGrid.SelectedIndex = currentIndex;
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
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
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
    }
}
