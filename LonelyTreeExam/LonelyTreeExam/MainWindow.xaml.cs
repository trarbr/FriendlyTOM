using Domain.Controller;
using LonelyTreeExam.UserControls;
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

namespace LonelyTreeExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                SupplierController supplierController = new SupplierController();
                CustomerController customerController = new CustomerController();
                PaymentController paymentController = new PaymentController();
                BookingController bookingController = new BookingController(paymentController, customerController);


                accountingControl = new AccountingUserControl(paymentController, supplierController, customerController);
                accountingUserControl.Content = accountingControl;
                suppliersUserControl.Content = new SuppliersUserControl(supplierController, customerController);
                customersUserControl.Content = new CustomersUserControl(customerController);
                bookingsControl = new BookingsUserControl(bookingController, supplierController,
                    customerController);
                bookingsUserControl.Content = bookingsControl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            currentTabIndex = 0;  
        }

        public static CultureInfo GetCulture()
        {
            return new CultureInfo("en-US");
        }

        private AccountingUserControl accountingControl;
        private BookingsUserControl bookingsControl;
        private int currentTabIndex;

        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != currentTabIndex)
            {
                accountingControl.RefreshAll();
                bookingsControl.Refresh();
            }
        }
    }
}
