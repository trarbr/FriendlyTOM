using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using DataAccess;

namespace Domain.Model
{
    internal class Booking : AAccountability, IBooking
    {
        #region Public Properties
        public string Sale
        {
            get { return _bookingEntity.Sale; }
            set { _bookingEntity.Sale = value; }
        }
        public int BookingNumber
        {
            get { return _bookingEntity.BookingNumber; }
            set { _bookingEntity.BookingNumber = value; }
        }
        public DateTime StartDate
        {
            get { return _bookingEntity.StartDate; }
            set { _bookingEntity.StartDate = value; }
        }
        public DateTime EndDate
        {
            get { return _bookingEntity.EndDate; }
            set { _bookingEntity.EndDate = value; }
        }
        public BookingType Type
        {
            get { return _bookingEntity.Type; }
            set { _bookingEntity.Type = value; }
        }
        public decimal IVAExempt
        {
            get { return _bookingEntity.IVAExempt; }
            set { _bookingEntity.IVAExempt = value; }
        }
        public decimal IVASubject
        {
            get { return _bookingEntity.IVASubject; }
            set { _bookingEntity.IVASubject = value; }
        }
        public decimal SubTotal
        {
            get { return _bookingEntity.SubTotal; }
            set { _bookingEntity.SubTotal = value; }
        }
        public decimal Service
        {
            get { return _bookingEntity.Service; }
            set { _bookingEntity.Service = value; }
        }
        public decimal IVA
        {
            get { return _bookingEntity.IVA; }
            set { _bookingEntity.IVA = value; }
        }
        public decimal ProductRetention
        {
            get { return _bookingEntity.ProductRetention; }
            set { _bookingEntity.ProductRetention = value; }
        }
        public decimal SupplierRetention
        {
            get { return _bookingEntity.SupplierRetention; }
            set { _bookingEntity.SupplierRetention = value; }
        }
        public decimal TransferAmount
        {
            get { return _bookingEntity.TransferAmount; }
            set { _bookingEntity.TransferAmount = value; }
        }
#endregion

        #region Internal Constructor
        internal Booking(IBooking bookingEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = bookingEntity;
            // Create Models of responsible and commissioner
            Party responsible = new Party(_bookingEntity.Responsible);
            Party commissioner = new Party(_bookingEntity.Commissioner);
            initializeAccountability(_bookingEntity, responsible, commissioner);
        }

        internal Booking(IParty responsible, IParty commissioner, string sale, int bookingNumber, DateTime startDate, 
            DateTime endDate, IDataAccessFacade dataAccessFacade)
        {
            // Get entities for DataAccess
            IParty responsibleEntity = ((Party)responsible)._partyEntity;
            IParty commissionerEntity = ((Party)commissioner)._partyEntity;
            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = dataAccessFacade.CreateBooking(responsibleEntity, commissionerEntity, sale,
                bookingNumber,startDate, endDate);
            initializeAccountability(_bookingEntity, responsible, commissioner);
        }
        #endregion

        #region Internal Methods
        internal static List<Booking> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            //Calls readall bookings and adds them to a list
            List<IBooking> bookingEntities = dataAccessFacade.ReadAllBookings();
            List<Booking> bookings = new List<Booking>();

            foreach (IBooking bookingEntity in bookingEntities)
            {
                Booking booking = new Booking(bookingEntity, dataAccessFacade);
                bookings.Add(booking);
            }
            return bookings;
        }

        internal void Update()
        {
            //Calls dataAccessFacade update method for updating a booking
            dataAccessFacade.UpdateBooking(_bookingEntity);
        }

        internal void Delete()
        {
            //Calls dataAccessFacade delete method for removing a booking from the list
            dataAccessFacade.DeleteBooking(_bookingEntity);
        }
        #endregion

        #region Private fields
        private IBooking _bookingEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion
    }
}
