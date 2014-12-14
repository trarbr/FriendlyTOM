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
                AppController app = AppController.Instance;
                app.Initialize();

                settingsUserControl.Content = new SettingsUserControl(app.SettingsController);
                paymentsControl = new AccountingUserControl(app.PaymentController, 
                    app.SupplierController, app.CustomerController);
                accountingUserControl.Content = paymentsControl;
                suppliersControl = new SuppliersUserControl(app.SupplierController, 
                    app.CustomerController);
                suppliersUserControl.Content = suppliersControl;
                customersUserControl.Content = new CustomersUserControl(app.CustomerController);
                bookingsControl = new BookingsUserControl(app.BookingController, 
                    app.SupplierController, app.CustomerController);
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

        private AccountingUserControl paymentsControl;
        private BookingsUserControl bookingsControl;
        private SuppliersUserControl suppliersControl;
        private int currentTabIndex;

        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != currentTabIndex)
            {
                paymentsControl.RefreshAll();
                bookingsControl.Refresh();
                suppliersControl.Refresh();

                currentTabIndex = mainTabNavigation.SelectedIndex;
            }
        }
    }
}
