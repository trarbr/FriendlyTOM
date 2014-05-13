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

        public AccountingUserControl()
        {
            InitializeComponent();
            paymentController = new PaymentController();

            incomingPaymentsControl = new IncomingPaymentsUserControl(paymentController);
            archivedPaymentsControl = new ArchivedPaymentsUserControl(paymentController);
            outgoingPaymentsControl = new OutgoingPaymentsUserControl(paymentController);

            incomingPaymentsUserControl.Content = incomingPaymentsControl;
            outgoingPaymentsUserControl.Content = outgoingPaymentsControl;
            archiveUserControl.Content = archivedPaymentsControl;

            current_tab_index = mainTabNavigation.SelectedIndex;


        }

        private PaymentController paymentController;
        private IncomingPaymentsUserControl incomingPaymentsControl;
        private OutgoingPaymentsUserControl outgoingPaymentsControl;
        private ArchivedPaymentsUserControl archivedPaymentsControl;

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
