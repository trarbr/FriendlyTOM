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
    /// Interaction logic for PaymentsUserControl.xaml
    /// </summary>
    public partial class PaymentsUserControl : UserControl
    {
        public DetailsUserControl DetailsUserControl { get; set; }

        public PaymentsUserControl(string submitButtonText, BitmapImage submitButtonImage, 
                                   string submitButtonToolTip, PaymentController controller, DetailsUserControl details)
        {
            paymentController = controller;
            DetailsUserControl = details;
            InitializeComponent();
            this.submitButtonTextBlock.Text = submitButtonText;
            this.submitButtonImage.Source = submitButtonImage;
            this.submitButton.ToolTip = submitButtonToolTip;
            mainDataGrid.ItemsSource = paymentController.ReadAllPayments();
            DetailsUserControl.PaymentsUserControl = this;
        }

        private PaymentController paymentController;

        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DetailsUserControl.PaymentsUserControl = this;
        }
    }
}
