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
        int old_tab_selection;
        public AccountingUserControl()
        {
            paymentController = new PaymentController();
            details = new DetailsUserControl(paymentController);
            InitializeComponent();
            initializeDataGrids();
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
            collapseDetailedView();


            old_tab_selection = 0;


            currentPaymentsUserControl.Content = currentPayments;
            archiveUserControl.Content = archivedPayments;
        }

        private DetailsUserControl details;
        private PaymentsUserControl currentPayments;
        private PaymentsUserControl archivedPayments;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;
        private PaymentController paymentController;

        private void initializeDataGrids()
        {
            currentPayments = new PaymentsUserControl("Archive",
                new BitmapImage(new Uri("/Images/book_add2.png", UriKind.Relative)),
                "Move selected payment to archive", paymentController, details);
            archivedPayments = new PaymentsUserControl("Restore", 
                new BitmapImage(new Uri("/Images/book_next2.png", UriKind.Relative)),
                "Move selected payment to current payments", paymentController, details);

            currentPaymentsUserControl.Content = currentPayments;
            archiveUserControl.Content = archivedPayments;
        }

        private void collapseDetailedView()
        {
            if (detailsUserControl.Content != null)
            {
                detailsUserControl.Content = null;
                collapseImage.Source = collapsePlusImage;
                collapseButton.ToolTip = "Show details";
            }
            else
            {
                detailsUserControl.Content = details;
                collapseImage.Source = collapseMinImage;
                collapseButton.ToolTip = "Hide details";
            }
        }

        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {
            collapseDetailedView();
        }

        private void mainTabNavigation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mainTabNavigation.SelectedIndex != old_tab_selection)
            {
                archivedPayments.refreshDataGrid();
                currentPayments.refreshDataGrid();

                old_tab_selection = mainTabNavigation.SelectedIndex;
            }
        }

    }
}
