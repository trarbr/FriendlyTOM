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
        public CustomersUserControl(CustomerController customerController)
        {
            InitializeComponent();
            this.customerController = customerController;
            customersDataGrid.ItemsSource = customerController.ReadAllCustomers();
            customerTypeComboBox.ItemsSource = Enum.GetValues(typeof(CustomerType));
            customerTypeComboBox.SelectedIndex = 0;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
        }

        private CustomerController customerController;
        private ICustomer selectedCustomer;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;

        private void refreshDataGrid()
        {
            customersDataGrid.ItemsSource = null;
            customersDataGrid.ItemsSource = customerController.ReadAllCustomers();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<ICustomer> searchedCustomers = new List<ICustomer>();
                customersDataGrid.ItemsSource = searchedCustomers;

                foreach (ICustomer customer in customerController.ReadAllCustomers())
                {
                    string searchData = string.Format("{0} {1} {2}",
                        customer.Name, customer.Note, customer.Type.ToString());
                    if (searchData.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedCustomers.Add(customer);
                    }
                }
            }
            else
            {
                customersDataGrid.ItemsSource = customerController.ReadAllCustomers();
            }

            customersDataGrid.Items.Refresh();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            refreshDataGrid();
        }

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

        private void customersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCustomer = (ICustomer)customersDataGrid.SelectedItem;
            setValuesInTextBoxes();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            customersDataGrid.SelectedItem = null;
            setValuesInTextBoxes();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer != null)
            {
                foreach (ICustomer customer in customersDataGrid.SelectedItems)
                {
                    customerController.DeleteCustomer(customer);
                }
                customersDataGrid.SelectedItem = null;
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
