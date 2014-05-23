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
        internal List<Booking> ReadAll()
        {
            if (bookings == null)
            {
                bookings = Booking.ReadAll(dataAccessFacade);
            }
            return bookings;
        }

        internal void Update(Booking booking)
        {
            booking.Update();
        }

        internal void Delete(Booking booking)
        {
            booking.Delete();
            bookings.Remove(booking);
        }

        internal IBooking Create(IParty responsible, IParty commissioner, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate)
        {
            Booking booking = new Booking(responsible, commissioner, sale, bookingNumber, startDate, endDate, 
                dataAccessFacade);
            bookings.Add(booking);
            return booking;
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Booking> bookings;
        #endregion
    }
}
