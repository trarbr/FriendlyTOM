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

using System;
using Common.Interfaces;
using Common.Enums;

namespace DataAccess.Entities
{
    internal class BookingEntity : Entity, IBooking
    {
        #region Public Properties
        public ISupplier Supplier 
        {
            get { return _supplier; }
            set { _supplier = (SupplierEntity)value; }
        }
        public ICustomer Customer 
        {
            get { return _customer; }
            set { _customer = (CustomerEntity)value; }
        }
        public string Note { get; set; }
        public string Sale { get; set; }
        public int BookingNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingType Type { get; set; }
        public decimal IVAExempt { get; set; }
        public decimal IVASubject { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Service { get; set; }
        public decimal IVA { get; set; }
        public decimal ProductRetention { get; set; }
        public decimal SupplierRetention { get; set; }
        public decimal TransferAmount { get; set; }
        #endregion

        internal BookingEntity(ISupplier supplier, ICustomer customer, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate) 
        {
            Supplier = supplier;
            Customer = customer;
            Sale = sale;
            BookingNumber = bookingNumber;
            StartDate = startDate;
            EndDate = endDate;

            Note = "";
            IVAExempt = 0;
            IVASubject = 0;
            Subtotal = 0;
            Service = 0;
            IVA = 0;
            ProductRetention = 0;
            SupplierRetention = 0;
            TransferAmount = 0;
        }

        private SupplierEntity _supplier;
        private CustomerEntity _customer;
    }
}
