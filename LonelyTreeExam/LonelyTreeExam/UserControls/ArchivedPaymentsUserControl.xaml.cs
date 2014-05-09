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

using Domain.Controller;
using Common.Interfaces;
namespace LonelyTreeExam.UserControls
{
    /// <summary>
    /// Interaction logic for ArchivedPaymentsUserControl.xaml
    /// </summary>
    public partial class ArchivedPaymentsUserControl : UserControl
    {
        PaymentController paymentController;

        public ArchivedPaymentsUserControl(PaymentController paymentController)
        {
            InitializeComponent();

            this.paymentController = paymentController;

            detailsUserControl.Content = new DetailsUserControl(paymentController);

            Refresh();
        }

        public void Refresh()
        {
            List<IPayment> payments = paymentController.ReadAllPayments();

            List<IPayment> archivedPayments = new List<IPayment>();
            foreach (IPayment payment in payments)
            {
                if (payment.Archived == true)
                {
                    archivedPayments.Add(payment);
                }
            }

            archivedPaymentsDataGrid.DataContext = archivedPayments;
        }
    }
}
