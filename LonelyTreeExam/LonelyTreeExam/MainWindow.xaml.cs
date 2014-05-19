using Domain.Controller;
using LonelyTreeExam.UserControls;
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
            PaymentController paymentController = new PaymentController();
            SupplierController supplierController = new SupplierController();
            CustomerController customerController = new CustomerController();

            accountingUserControl.Content = new AccountingUserControl(paymentController, supplierController, 
                customerController);
            suppliersUserControl.Content = new SuppliersUserControl(supplierController);
            customersUserControl.Content = new CustomersUserControl(customerController);
        }
    }
}
