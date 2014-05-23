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
        public BookingController()
        {
            dataAccessFacade = DataAccessFacade.GetInstance();
            bookingCollection = new BookingCollection(dataAccessFacade);
        }
        #endregion

        #region Public CRUD
        public List<IBooking> ReadAllBookings()
        {
            List<IBooking> bookings = new List<IBooking>();
            foreach (Booking booking in bookingCollection.ReadAll())
            {
                bookings.Add(booking);
            }
            return bookings;
        }

        public IBooking CreateBooking(IParty responsible, IParty commissioner, string sale, int bookingNumber,
            DateTime StartDate, DateTime EndDate)
        {
            return bookingCollection.Create(responsible, commissioner, sale, bookingNumber, StartDate, EndDate);
        }

        public void UpdateBooking(IBooking booking)
        {
            bookingCollection.Update((Booking) booking);
        }

        public void DeleteBooking(IBooking booking)
        {
            bookingCollection.Delete((Booking) booking);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private BookingCollection bookingCollection;
        #endregion
    }
}
