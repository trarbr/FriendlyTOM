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
    /// Interaction logic for CustomersUserControl.xaml
    /// </summary>
    public partial class CustomersUserControl : UserControl
    {
        public CustomersUserControl()
        {
            InitializeComponent();
            
            customerController = new CustomerController();
            customersDataGrid.ItemsSource = customerController.ReadAllCustomers();
            customerTypeComboBox.ItemsSource = Enum.GetValues(typeof(CustomerType));
            customerTypeComboBox.SelectedIndex = 0;
        }

        private CustomerController customerController;
        private ICustomer selectedCustomer;

        private void setValuesInTextBoxes()
        {
            if (selectedCustomer != null)
            {
                nameTextBox.Text = selectedCustomer.Name;
                customerTypeComboBox.SelectedItem = selectedCustomer.Type;
                noteTextBox.Text = selectedCustomer.Note;
            }
            else
            {
                nameTextBox.Text = "";
                customerTypeComboBox.SelectedIndex = 0;
                noteTextBox.Text = "";
            }
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void suppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCustomer = (ICustomer)customersDataGrid.SelectedItem;
            setValuesInTextBoxes();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer == null)
            {
                string name = nameTextBox.Text;
                CustomerType type = (CustomerType)customerTypeComboBox.SelectedItem;
                string note = noteTextBox.Text;

                customerController.CreateCustomer(type, note, name);
            }
            else
            {
                selectedCustomer.Name = nameTextBox.Text;
                selectedCustomer.Type = (CustomerType)customerTypeComboBox.SelectedItem;
                selectedCustomer.Note = noteTextBox.Text;

                customerController.UpdateCustomer(selectedCustomer);
            }
            customersDataGrid.ItemsSource = null;
            customersDataGrid.ItemsSource = customerController.ReadAllCustomers();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            customersDataGrid.SelectedItem = null;
        }
    }
}
