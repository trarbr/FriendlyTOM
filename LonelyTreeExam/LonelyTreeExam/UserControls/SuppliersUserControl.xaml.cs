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
        }

        private SupplierController supplierController;

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Skift IPayment til IParty!
            if (searchTextBox.Text != "")
            {
                List<IPayment> searchedSuppliers = new List<IPayment>();
                suppliersDataGrid.ItemsSource = searchedSuppliers;

                foreach (IPayment supplier in supplierController.ReadAllSuppliers())
                {
                    // Kun find resultater hvor navnet starter med søgeteksten:
                    // if (supplier.Responsible.StartsWith(searchTextBox.Text, StringComparison.OrdinalIgnoreCase)
                    // {
                    //    searchedSuppliers.Add(supplier);
                    // }

                    // Find resultater hvor søgeteksten indegår:
                    // Fordel: nemt at tilføje flere søgekritier til searchData (feks fakturaId, datoer etc)
                    string searchData = string.Format("{0}",supplier.Responsible);
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
    }
}
