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
    /// Interaction logic for AccountingUserControl.xaml
    /// </summary>
    public partial class AccountingUserControl : UserControl
    {
        int current_tab_index;

        public AccountingUserControl(PaymentController paymentController, SupplierController supplierController)
        {
            InitializeComponent();
            this.paymentController = paymentController;
            this.supplierController = supplierController;

            incomingPaymentsControl = new IncomingPaymentsUserControl(paymentController);
            archivedPaymentsControl = new ArchivedPaymentsUserControl(paymentController);
            outgoingPaymentsControl = new OutgoingPaymentsUserControl(paymentController, supplierController);

            incomingPaymentsUserControl.Content = incomingPaymentsControl;
            outgoingPaymentsUserControl.Content = outgoingPaymentsControl;
            archiveUserControl.Content = archivedPaymentsControl;

            current_tab_index = mainTabNavigation.SelectedIndex;


        }

        private PaymentController paymentController;
        private SupplierController supplierController;
        private IncomingPaymentsUserControl incomingPaymentsControl;
        private OutgoingPaymentsUserControl outgoingPaymentsControl;
        private ArchivedPaymentsUserControl archivedPaymentsControl;

        // ReadAllPayments and refresh DataGrid when changing tabs
        // current_tab_index is implemented to avoid xaml error before build is complete
        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != current_tab_index)
            {
                incomingPaymentsControl.RefreshPaymentDataGrid();
                outgoingPaymentsControl.RefreshPaymentDataGrid();
                archivedPaymentsControl.RefreshPaymentDataGrid();

                current_tab_index = mainTabNavigation.SelectedIndex;
            }
        }
    }
}
