// TB
using Common.Interfaces;
using DataAccess;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace Domain.Collections
{
    internal class BookingCollection
    {
        #region Internal Constructor
        public BookingCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }
        #endregion

        #region Internal CRUD
        internal IBooking Create(Supplier supplier, Customer customer, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate)
        {
            //Creates a new object and adds it to a list of Bookings.
            Booking booking = new Booking(supplier, customer, sale, bookingNumber, startDate, endDate, 
                dataAccessFacade);
            bookings.Add(booking);
            return booking;
        }

        internal List<Booking> ReadAll()
        {
            //if the list is empty do a readall from the database. 
            if (bookings == null)
            {
                bookings = Booking.ReadAll(dataAccessFacade);
            }
            return bookings;
        }

        internal void Update(Booking booking)
        {
            //Call update for a specific booking.
            booking.Update();
        }

        internal void Delete(Booking booking)
        {
            //Calls delete for an object and removes it from the list. 
            booking.Delete();
            bookings.Remove(booking);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Booking> bookings;
        #endregion
    }
}
