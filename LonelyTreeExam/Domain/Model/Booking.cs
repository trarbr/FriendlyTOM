using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Domain.Model
{
    internal class Booking : IBooking
    {
        public ISupplier Supplier
        {
            get { return _supplier; }
            set 
            { 
                _supplier = (Supplier)value;
                _bookingEntity.Supplier = _supplier._supplierEntity;
            }
        }
        public ICustomer Customer
        {
            get { return _customer; }
            set 
            { 
                _customer = (Customer)value;
                _bookingEntity.Customer = _customer._customerEntity;
            }
        }
        public string Note
        {
            get { return _bookingEntity.Note; }
            set { _bookingEntity.Note = value; }
        }
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

        internal Booking(IBooking bookingEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = bookingEntity;

            // Create Models of supplier and customer
            _supplier = new Supplier(dataAccessFacade, _bookingEntity.Supplier);
            _customer = new Customer(_bookingEntity.Customer, dataAccessFacade);
        }

        internal Booking(Supplier supplier, Customer customer, string sale, int bookingNumber, DateTime startDate, 
            DateTime endDate, IDataAccessFacade dataAccessFacade)
        {
            // Get entities for DataAccess
            ISupplier supplierEntity = supplier._supplierEntity;
            ICustomer customerEntity = customer._customerEntity;

            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = dataAccessFacade.CreateBooking(supplierEntity, customerEntity, sale, bookingNumber, 
                startDate, endDate);

            _supplier = supplier;
            _customer = customer;
        }

        internal static List<Booking> ReadAll(IDataAccessFacade dataAccessFacade)
        {
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
            dataAccessFacade.UpdateBookings(_bookingEntity);
        }

        internal void Delete()
        {
            dataAccessFacade.DeleteBooking(_bookingEntity);
        }

        internal void CalculateTransferAmount()
        {
            SubTotal = IVAExempt + IVASubject;
            IVA = IVASubject * 0.12m;
            TransferAmount = SubTotal - (SubTotal * ProductRetention/100) + Service + IVA - (IVA * SupplierRetention/100);
        }

        #region Private fields
        private IBooking _bookingEntity;
        private IDataAccessFacade dataAccessFacade;
        private Customer _customer;
        private Supplier _supplier;
        #endregion
    }
}
