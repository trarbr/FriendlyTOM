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

namespace FriendlyTOM.UserControls
{
    /// <summary>
    /// Interaction logic for AccountingUserControl.xaml
    /// </summary>
    public partial class AccountingUserControl : UserControl
    {
        int currentTabIndex;

        public AccountingUserControl(PaymentController paymentController, SupplierController supplierController,
            CustomerController customerController)
        {
            InitializeComponent();
            this.paymentController = paymentController;
            this.supplierController = supplierController;
            this.customerController = customerController;

            incomingPaymentsControl = new IncomingPaymentsUserControl(paymentController,
                customerController, supplierController);
            outgoingPaymentsControl = new OutgoingPaymentsUserControl(paymentController,
                customerController, supplierController);
            archivedPaymentsControl = new ArchivedPaymentsUserControl(paymentController);

            incomingPaymentsUserControl.Content = incomingPaymentsControl;
            outgoingPaymentsUserControl.Content = outgoingPaymentsControl;
            archiveUserControl.Content = archivedPaymentsControl;

            currentTabIndex = mainTabNavigation.SelectedIndex;


        }

        internal void RefreshAll()
        {
            incomingPaymentsControl.RefreshPaymentDataGrid();
            outgoingPaymentsControl.RefreshPaymentDataGrid();
            archivedPaymentsControl.RefreshPaymentDataGrid();
        }

        private PaymentController paymentController;
        private SupplierController supplierController;
        private CustomerController customerController;
        private IncomingPaymentsUserControl incomingPaymentsControl;
        private OutgoingPaymentsUserControl outgoingPaymentsControl;
        private ArchivedPaymentsUserControl archivedPaymentsControl;

        // ReadAllPayments and refresh DataGrid when changing tabs
        // current_tab_index is implemented to avoid xaml error before build is complete
        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != currentTabIndex)
            {
                RefreshAll();

                currentTabIndex = mainTabNavigation.SelectedIndex;
            }
        }
    }
}
