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
        public SuppliersUserControl()
        {
            InitializeComponent();
            supplierController = new SupplierController();
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
            supplierTypeComboBox.ItemsSource = Enum.GetValues(typeof(SupplierType));
        }

        private SupplierController supplierController;

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<ISupplier> searchedSuppliers = new List<ISupplier>();
                suppliersDataGrid.ItemsSource = searchedSuppliers;

                foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
                {
                    string searchData = string.Format("{0} {1} {2} {3}",
                        supplier.Name, supplier.Note, supplier.PaymentInfo, supplier.Type.ToString());
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
            string name = nameTextBox.Text;
            SupplierType type = (SupplierType)supplierTypeComboBox.SelectedItem;
            string paymentInfo = paymentInfoTextBox.Text;
            string note = noteTextBox.Text;

            supplierController.CreateSupplier(name, note, paymentInfo, type);
            suppliersDataGrid.ItemsSource = null;
            suppliersDataGrid.ItemsSource = supplierController.ReadAllSuppliers();
        }
    }
}
