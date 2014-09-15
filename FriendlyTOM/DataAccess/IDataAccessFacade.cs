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
using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess
{
   public interface IDataAccessFacade
   {
       void BackupDatabase(string backupPath);

        #region Payment
        IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking);
        List<IPayment> ReadAllPayments();
        void UpdatePayment(IPayment payment);
        void DeletePayment(IPayment payment);
        #endregion
        
        #region Supplier
        ISupplier CreateSupplier(string name, string note, SupplierType type);
        List<ISupplier> ReadAllSuppliers();
        void UpdateSupplier(ISupplier supplier);
        void DeleteSupplier(ISupplier supplier);
        #endregion

        #region Customer
        ICustomer CreateCustomer(CustomerType type, string note, string name);
        List<ICustomer> ReadAllCustomers();
        void UpdateCustomers(ICustomer customer);
        void DeleteCustomer(ICustomer customer);
        #endregion

        #region Booking
        IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber,
            DateTime startDate, DateTime endDate);
        List<IBooking> ReadAllBookings();
        void UpdateBooking(IBooking booking);
        void DeleteBooking(IBooking booking);
        #endregion

        #region PaymentRule
        IPaymentRule CreatePaymentRule(ISupplier supplierEntity, ICustomer customerEntity, BookingType bookingType,
            decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType);
        void UpdatePaymentRule(IPaymentRule paymentRuleEntity);
        void DeletePaymentRule(IPaymentRule paymentRuleEntity);
        #endregion
   }
}

