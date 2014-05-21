using Common.Interfaces;
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
    /// Interaction logic for BookingsUserControl.xaml
    /// </summary>
    public partial class BookingsUserControl : UserControl
    {
        public BookingsUserControl()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<IBooking> searchedCustomers = new List<IBooking>();
                bookingTypeComboBox.ItemsSource = searchedCustomers;

                foreach (ICustomer customer in customerController.ReadAllCustomers())
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}",
                        customer.Name, customer.Note, customer.Type.ToString(), customer.ContactPerson,
                        customer.Email, customer.Address, customer.PhoneNo, customer.FaxNo);
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
    }
}
