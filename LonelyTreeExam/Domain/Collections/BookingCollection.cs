using Common.Interfaces;
using DataAccess;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Collections
{
    internal class BookingCollection
    {
        public BookingCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }

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

        internal IBooking Create(Supplier supplier, Customer customer, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate)
        {
            Booking booking = new Booking(supplier, customer, sale, bookingNumber, startDate, endDate, 
                dataAccessFacade);
            bookings.Add(booking);
            return booking;
        }

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Booking> bookings;
        #endregion
    }
}
