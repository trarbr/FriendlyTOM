/*
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

using Domain.Controller;
using FriendlyTOM.UserControls;
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

namespace FriendlyTOM
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
                CustomerController customerController = new CustomerController();
                SupplierController supplierController = new SupplierController();
                PaymentController paymentController = new PaymentController();
                BookingController bookingController = new BookingController(paymentController, customerController);


                accountingControl = new AccountingUserControl(paymentController, supplierController, customerController);
                accountingUserControl.Content = accountingControl;
                suppliersControl = new SuppliersUserControl(supplierController, customerController);
                suppliersUserControl.Content = suppliersControl;
                customersUserControl.Content = new CustomersUserControl(customerController);
                bookingsControl = new BookingsUserControl(bookingController, supplierController,
                    customerController);
                bookingsUserControl.Content = bookingsControl;
                settingsUserControl.Content = new SettingsUserControl();
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
        private SuppliersUserControl suppliersControl;
        private int currentTabIndex;

        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != currentTabIndex)
            {
                accountingControl.RefreshAll();
                bookingsControl.Refresh();
                suppliersControl.Refresh();

                currentTabIndex = mainTabNavigation.SelectedIndex;
            }
        }
    }
}
