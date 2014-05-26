using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace Domain.Controller
{
    public class BookingController
    {
        #region Public Constructor
        public BookingController(PaymentController paymentController)
        {
            dataAccessFacade = DataAccessFacade.GetInstance();
            bookingCollection = new BookingCollection(dataAccessFacade);
            this.paymentController = paymentController;
        }
        #endregion

        #region Public CRUD
        public IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber,
            DateTime StartDate, DateTime EndDate)
        {
            //Calls Bookingcollection class for create
            return bookingCollection.Create((Supplier)supplier, (Customer)customer, sale, bookingNumber, StartDate, 
                EndDate);
        }

        public List<IBooking> ReadAllBookings()
        {
            //Calls Bookingcollection class for readall
            List<IBooking> bookings = new List<IBooking>();
            foreach (Booking booking in bookingCollection.ReadAll())
            {
                bookings.Add(booking);
            }
            return bookings;
        }

        public void UpdateBooking(IBooking booking)
        {
            //Calls Bookingcollection class for update
            bookingCollection.Update((Booking) booking);
        }

        public void DeleteBooking(IBooking booking)
        {
            //Calls Bookingcollection class for delete
            bookingCollection.Delete((Booking) booking);
        }
        #endregion

        public void CalculatePaymentsForBooking(IBooking booking)
        {
            Booking bookingModel = (Booking)booking;
            bookingModel.CalculateAmounts();
            PaymentStrategy paymentStrategy = new PaymentStrategy();
            paymentStrategy.CreatePayments(bookingModel, paymentController);
        }

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private BookingCollection bookingCollection;
        private PaymentController paymentController;
        #endregion
    }
}
