using Common.Enums;
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
    /// Interaction logic for BookingsUserControl.xaml
    /// </summary>
    public partial class BookingsUserControl : UserControl
    {
        public BookingsUserControl(BookingController bookingController, SupplierController supplierController,
            CustomerController customerController)
        {
            InitializeComponent();
            this.bookingController = bookingController;
            this.supplierController = supplierController;
            this.customerController = customerController;
            bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
            bookingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BookingType));
            bookingTypeComboBox.SelectedIndex = 0;
            collapsePlusImage = new BitmapImage(new Uri("/Images/collapse-plus.png", UriKind.Relative));
            collapseMinImage = new BitmapImage(new Uri("/Images/collapse-min.png", UriKind.Relative));
        }

        private BookingController bookingController;
        private SupplierController supplierController;
        private CustomerController customerController;
        private IBooking selectedBooking;
        private BitmapImage collapsePlusImage;
        private BitmapImage collapseMinImage;

        private void refreshDataGrid()
        {
            bookingsDataGrid.ItemsSource = null;
            bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            bookingsDataGrid.SelectedItem = null;
            setValuesInTextBoxes();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooking != null)
            {
                foreach (IBooking booking in bookingsDataGrid.SelectedItems)
                {
                    bookingController.DeleteBooking(booking);
                }
                bookingsDataGrid.SelectedItem = null;
                refreshDataGrid();
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedBooking == null)
                {
                    string sale = saleTextBox.Text;
                    int bookingNumber;
                    int.TryParse(saleTextBox.Text, out bookingNumber);
                    DateTime startDate = startDateDatePicker.SelectedDate.Value;
                    DateTime endDate = endDateDatePicker.SelectedDate.Value;

                    //TODO: FIX RESPONSIBLE OG COMMISSIONER!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    IParty responsible = null;
                    foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
                    {
                        if (supplier.Name == responsibleTextBox.Text)
                        {
                            responsible = supplier;
                            break;
                        }
                    }

                    IParty commissioner = null;
                    foreach (ICustomer customer in customerController.ReadAllCustomers())
                    {
                        if (customer.Name == commissionerTextBox.Text)
                        {
                            commissioner = customer;
                            break;
                        }
                    }

                    IBooking booking = bookingController.CreateBooking(responsible, commissioner, sale, bookingNumber,
                                                                       startDate, endDate);

                    booking.Type = (BookingType)bookingTypeComboBox.SelectedItem;
                    
                    decimal iVAExempt;
                    decimal iVASubject;
                    decimal service;
                    decimal productRetention;
                    decimal supplierRetention;

                    decimal.TryParse(IVAExemptTextBox.Text, out iVAExempt);
                    decimal.TryParse(IVASubjectTextBox.Text, out iVASubject);
                    decimal.TryParse(serviceTextBox.Text, out service);
                    decimal.TryParse(productRetentionTextBox.Text, out productRetention);
                    decimal.TryParse(supplierRetentionTextBox.Text, out supplierRetention);

                    booking.IVAExempt = iVAExempt;
                    booking.IVASubject = iVASubject;
                    booking.Service = service;
                    booking.ProductRetention = productRetention;
                    booking.SupplierRetention = supplierRetention;
                    bookingController.UpdateBooking(booking);

                    refreshDataGrid();
                    bookingsDataGrid.SelectedItem = null;
                    setValuesInTextBoxes();
                }
                else
                {
                    int currentIndex = bookingsDataGrid.SelectedIndex;

                    IParty responsible = null;
                    IParty commissioner = null;

                    decimal iVAExempt;
                    decimal iVASubject;
                    decimal service;
                    decimal productRetention;
                    decimal supplierRetention;
                    int bookingNumber;

                    decimal.TryParse(IVAExemptTextBox.Text, out iVAExempt);
                    decimal.TryParse(IVASubjectTextBox.Text, out iVASubject);
                    decimal.TryParse(serviceTextBox.Text, out service);
                    decimal.TryParse(productRetentionTextBox.Text, out productRetention);
                    decimal.TryParse(supplierRetentionTextBox.Text, out supplierRetention);
                    int.TryParse(bookingNumberTextBox.Text, out bookingNumber);

                    selectedBooking.IVAExempt = iVAExempt;
                    selectedBooking.IVASubject = iVASubject;
                    selectedBooking.Service = service;
                    selectedBooking.ProductRetention = productRetention;
                    selectedBooking.SupplierRetention = supplierRetention;
                    selectedBooking.BookingNumber = bookingNumber;
                    selectedBooking.Commissioner = commissioner;
                    selectedBooking.Responsible = responsible;
                    selectedBooking.EndDate = endDateDatePicker.SelectedDate.Value;
                    selectedBooking.StartDate = startDateDatePicker.SelectedDate.Value;
                    selectedBooking.Note = noteTextBox.Text;
                    selectedBooking.Sale = saleTextBox.Text;
                    selectedBooking.Type = (BookingType) bookingTypeComboBox.SelectedItem;

                    bookingController.UpdateBooking(selectedBooking);
                    refreshDataGrid();
                    bookingsDataGrid.SelectedIndex = currentIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void collapseButton_Click(object sender, RoutedEventArgs e)
        {
            if (detailsGrid.Visibility != Visibility.Collapsed)
            {
                detailsGrid.Visibility = Visibility.Collapsed;
                bottomButtonsGrid.Visibility = Visibility.Collapsed;
                collapseImage.Source = collapsePlusImage;
                collapseButton.ToolTip = "Show details";
            }
            else
            {
                detailsGrid.Visibility = Visibility.Visible;
                bottomButtonsGrid.Visibility = Visibility.Visible;
                collapseImage.Source = collapseMinImage;
                collapseButton.ToolTip = "Hide details";
            }
        }

        private void bookingsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBooking = (IBooking)bookingsDataGrid.SelectedItem;
            setValuesInTextBoxes();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchTextBox.Text != "")
            {
                List<IBooking> searchedBookings = new List<IBooking>();
                bookingsDataGrid.ItemsSource = searchedBookings;

                foreach (IBooking booking in bookingController.ReadAllBookings())
                {
                    string searchData = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}",
                        booking.Service, booking.Sale, booking.Type.ToString(), booking.StartDate,
                        booking.SupplierRetention, booking.Responsible, booking.ProductRetention, 
                        booking.Note, booking.IVASubject, booking.IVAExempt, booking.EndDate, booking.Commissioner, booking.BookingNumber);
                    if (searchData.IndexOf(searchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        searchedBookings.Add(booking);
                    }
                }
            }
            else
            {
                bookingsDataGrid.ItemsSource = bookingController.ReadAllBookings();
            }

            bookingsDataGrid.Items.Refresh();
        }

        private void setValuesInTextBoxes()
        {
            if (selectedBooking != null)
            {
                saleTextBox.Text = selectedBooking.Sale;
                bookingNumberTextBox.Text = selectedBooking.BookingNumber.ToString();
                startDateDatePicker.SelectedDate = selectedBooking.StartDate;
                endDateDatePicker.SelectedDate = selectedBooking.EndDate;
                bookingTypeComboBox.SelectedItem = selectedBooking.Type;
                IVAExemptTextBox.Text = selectedBooking.IVAExempt.ToString();
                IVASubjectTextBox.Text = selectedBooking.IVASubject.ToString();
                serviceTextBox.Text = selectedBooking.Service.ToString();
                productRetentionTextBox.Text = selectedBooking.ProductRetention.ToString();
                supplierRetentionTextBox.Text = selectedBooking.SupplierRetention.ToString();
                responsibleTextBox.Text = selectedBooking.Responsible.Name;
                commissionerTextBox.Text = selectedBooking.Commissioner.Name;
            }
            else
            {
                saleTextBox.Text = "";
                bookingNumberTextBox.Text = "";
                startDateDatePicker.SelectedDate = null;
                endDateDatePicker.SelectedDate = null;
                bookingTypeComboBox.SelectedIndex = 0;
                IVAExemptTextBox.Text = "";
                IVASubjectTextBox.Text = "";
                serviceTextBox.Text = "";
                productRetentionTextBox.Text = "";
                supplierRetentionTextBox.Text = "";
                responsibleTextBox.Text = "";
                commissionerTextBox.Text = "";
            }
        }
    }
}
