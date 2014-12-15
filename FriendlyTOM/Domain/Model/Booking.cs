/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using DataAccess;
using Domain.Helpers;

namespace Domain.Model
{
    internal class Booking : IBooking
    {
        #region Public Properties
        public ISupplier Supplier
        {
            get { return _supplier; }
            set 
            {
                validateSupplier(value);
                _supplier = (Supplier)value;
            }
        }
        public ICustomer Customer
        {
            get { return _customer; }
            set 
            {
                validateCustomer(value);
                _customer = (Customer)value;
            }
        }
        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }
        public string Sale
        {
            get { return _sale; }
            set 
            {
                validateSale(value);
                _sale = value; 
            }
        }
        public int BookingNumber
        {
            get { return _bookingNumber; }
            set { _bookingNumber = value; }
        }
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set 
            {
                validateEndDate(StartDate, value);
                _endDate = value; 
            }
        }
        public BookingType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public decimal IVAExempt
        {
            get { return _iVAExempt; }
            set 
            {
                validateNotNegative("IVAExempt", value);
                _iVAExempt = value; 
            }
        }
        public decimal IVASubject
        {
            get { return _iVASubject; }
            set 
            { 
                validateNotNegative("IVASubject", value);
                _iVASubject = value; 
            }
        }
        public decimal Subtotal
        {
            get { return _subtotal; }
            set 
            { 
                validateNotNegative("Subtotal", value);
                _subtotal = value; 
            }
        }
        public decimal Service
        {
            get { return _service; }
            set 
            { 
                validateNotNegative("Service", value);
                _service = value;
            }
        }
        public decimal IVA
        {
            get { return _iVA; }
            set 
            { 
                validateNotNegative("IVA", value);
                _iVA = value; 
            }
        }
        public decimal ProductRetention
        {
            get { return _productRetention; }
            set 
            {
                validatePercentage("ProductRetention", value);
                _productRetention = value; 
            }
        }
        public decimal SupplierRetention
        {
            get { return _supplierRetention; }
            set
            {
                validatePercentage("SupplierRetention", value);
                _supplierRetention = value;
            }
        }
        public decimal TransferAmount
        {
            get { return _transferAmount; }
            set 
            { 
                validateNotNegative("TransferAmount", value);
                _transferAmount = value; 
            }
        }
        #endregion

        #region Internal Constructor
        internal Booking(IBooking bookingEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = bookingEntity;
            _note = bookingEntity.Note;
            _sale = bookingEntity.Sale;
            _bookingNumber = bookingEntity.BookingNumber;
            _startDate = bookingEntity.StartDate;
            _endDate = bookingEntity.EndDate;
            _type = bookingEntity.Type;
            _iVAExempt = bookingEntity.IVAExempt;
            _iVASubject = bookingEntity.IVASubject;
            _subtotal = bookingEntity.Subtotal;
            _service = bookingEntity.Service;
            _iVA = bookingEntity.IVA;
            _productRetention = bookingEntity.ProductRetention;
            _supplierRetention = bookingEntity.SupplierRetention;
            _transferAmount = bookingEntity.TransferAmount;

            // Create Models of supplier and customer
            Register register = Register.GetInstance();
            _supplier = register.GetSupplier(bookingEntity.Supplier);
            _customer = register.GetCustomer(bookingEntity.Customer);
        }

        internal Booking(Supplier supplier, Customer customer, string sale, int bookingNumber, DateTime startDate, 
            DateTime endDate, IDataAccessFacade dataAccessFacade)
        {
            validateSale(sale);
            validateEndDate(startDate, endDate);

            _supplier = supplier;
            _customer = customer;
            _sale = sale;
            _bookingNumber = bookingNumber;
            _startDate = startDate;
            _endDate = endDate;

            // Get entities for DataAccess
            ISupplier supplierEntity = supplier._supplierEntity;
            ICustomer customerEntity = customer._customerEntity;

            this.dataAccessFacade = dataAccessFacade;
            _bookingEntity = dataAccessFacade.CreateBooking(supplierEntity, customerEntity, sale, bookingNumber, 
                startDate, endDate);
        }
        #endregion

        #region Internal CRUD
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
            _bookingEntity.Supplier = _supplier._supplierEntity;
            _bookingEntity.Customer = _customer._customerEntity;
            _bookingEntity.Sale = _sale;
            _bookingEntity.BookingNumber = _bookingNumber;
            _bookingEntity.StartDate = _startDate;
            _bookingEntity.EndDate = _endDate;
            _bookingEntity.Type = _type;
            _bookingEntity.IVAExempt = _iVAExempt;
            _bookingEntity.IVASubject = _iVASubject;
            _bookingEntity.Subtotal = _subtotal;
            _bookingEntity.Service = _service;
            _bookingEntity.IVA = _iVA;
            _bookingEntity.ProductRetention = _productRetention;
            _bookingEntity.SupplierRetention = _supplierRetention;
            _bookingEntity.TransferAmount = _transferAmount;

            //Calls dataAccessFacade update method for updating a booking
            dataAccessFacade.UpdateBooking(_bookingEntity);
        }

        internal void Delete()
        {
            //Calls dataAccessFacade delete method for removing a booking from the list
            dataAccessFacade.DeleteBooking(_bookingEntity);
        }
        #endregion

        internal void CalculateAmounts()
        {
            Subtotal = IVAExempt + IVASubject;
            IVA = IVASubject * 0.12m;
            TransferAmount = Subtotal - (Subtotal * ProductRetention/100) + Service + IVA - (IVA * SupplierRetention/100);

            Update();
        }

        #region Private fields
        private IBooking _bookingEntity;
        private IDataAccessFacade dataAccessFacade;
        private Customer _customer;
        private Supplier _supplier;
        private string _note;
        private string _sale;
        private int _bookingNumber;
        private DateTime _startDate;
        private DateTime _endDate;
        private BookingType _type;
        private decimal _iVAExempt;
        private decimal _iVASubject;
        private decimal _subtotal;
        private decimal _service;
        private decimal _iVA;
        private decimal _productRetention;
        private decimal _supplierRetention;
        private decimal _transferAmount;
        #endregion

        #region Validation
        private void validateSupplier(ISupplier value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Supplier", "Supplier was not found");
            }
        }

        private void validateCustomer(ICustomer value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Customer", "Customer was not found");
            }
        }
        private void validateSale(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException("Sale", "may not be empty");
            }
        }
        private void validateEndDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new ArgumentOutOfRangeException("EndDate", "must be later than StartDate");
            }
        }
        private void validatePercentage(string paramName, decimal value)
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(paramName, "must be between 0 and 100");
            }
        }
        private void validateNotNegative(string paramName, decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "must be greater than 0");
            }
        }
        #endregion
    }
}
