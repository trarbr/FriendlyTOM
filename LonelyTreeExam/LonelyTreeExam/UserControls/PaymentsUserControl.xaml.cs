using Common.Interfaces;
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
            DetailsUserControl.PaymentsUserControl = this;
            refreshDataGrid();
            dataGrid.DataContext = findArchivedPayments();
        }

        public List<IPayment> findArchivedPayments()
        {
            List<IPayment> archivedPayments = new List<IPayment>();
            foreach (IPayment payment in paymentController.ReadAllPayments())
            {
                if (payment.Archived == true)
                {
                    archivedPayments.Add(payment);
                }
            }

            return archivedPayments;
        }
        
        public void refreshDataGrid()
        {
            /*
            mainDataGrid.ItemsSource = null;

            if (submitButtonTextBlock.Text == "Archive")
            {
                currentPayments = new List<IPayment>();
                foreach (IPayment payment in paymentController.ReadAllPayments())
                {
                    if (payment.Archived == false)
                    {
                        currentPayments.Add(payment);
                    }
                }
                mainDataGrid.ItemsSource = currentPayments;
            }
            else if (submitButtonTextBlock.Text == "Restore")
            {
                archivedPayments = new List<IPayment>();
                foreach (IPayment payment in paymentController.ReadAllPayments())
                {
                    if (payment.Archived == true)
                    {
                        archivedPayments.Add(payment);
                    }
                }
                mainDataGrid.ItemsSource = archivedPayments;
            }
            */
        }

        private PaymentController paymentController;
        private List<IPayment> archivedPayments;
        private List<IPayment> currentPayments;

        private void mainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            DetailsUserControl.PaymentsUserControl = this;
            IPayment payment = (IPayment) mainDataGrid.SelectedItem;
            DetailsUserControl.SetValuesInTextBoxes(payment);
            */
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            IPayment payment = (IPayment)mainDataGrid.SelectedItem;
            if (payment != null)
            {
                paymentController.DeletePayment(payment);
                refreshDataGrid();
            }
            */
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (mainDataGrid.SelectedItem != null)
            {
                if (submitButtonTextBlock.Text == "Archive")
                {
                    IPayment payment = (IPayment)mainDataGrid.SelectedItem;
                    payment.Archived = true;
                    paymentController.UpdatePayment(payment);
                    refreshDataGrid();
                }
                else if (submitButtonTextBlock.Text == "Restore")
                {
                    IPayment payment = (IPayment)mainDataGrid.SelectedItem;
                    payment.Archived = false;
                    paymentController.UpdatePayment(payment);
                    refreshDataGrid();
                }
            }
            */
        }
    }
}
