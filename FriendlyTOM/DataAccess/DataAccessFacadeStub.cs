﻿/*
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

using Common.Interfaces;
using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Common.Enums;

namespace DataAccess
{
    public class DataAccessFacadeStub : IDataAccessFacade
    {
        //Class is for test purpose.
        List<IPayment> payments;
        #region Payment Stuff

        public DataAccessFacadeStub()
        {
            payments = new List<IPayment>();
            customers = new List<ICustomer>();
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer, IParty payee, 
            PaymentType type, string sale, int booking)
        {
            PaymentEntity entity = new PaymentEntity(dueDate, dueAmount, payer, payee, type, sale, booking);
            payments.Add(entity);

            return entity;
        }

        public List<IPayment> ReadAllPayments()
        {
            return payments;
        }

        public void UpdatePayment(IPayment payment)
        {
            PaymentEntity entity = (PaymentEntity)payment;

            entity.LastModified = DateTime.Now;
        }

        public void DeletePayment(IPayment payment)
        {
            PaymentEntity entity = (PaymentEntity)payment;

            entity.Deleted = true;
        }

        #endregion

        #region Supplier Stuff
        List<ISupplier> supplierList = new List<ISupplier>();

        public ISupplier CreateSupplier(string name, string note, SupplierType type)
        {
            SupplierEntity entity = new SupplierEntity(type, note, name);
            supplierList.Add(entity);

            return entity;
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            return supplierList;
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            SupplierEntity entity = (SupplierEntity) supplier;
            entity.LastModified = DateTime.Now;
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            SupplierEntity entity = (SupplierEntity) supplier;
            entity.Deleted = true;
        }
        #endregion

        #region Customer Stuff

        private List<ICustomer> customers;

        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            CustomerEntity entity = new CustomerEntity(type, note, name);
            customers.Add(entity);
            return entity;
        }

        public List<ICustomer> ReadAllCustomers()
        {
            return customers;
        }

        public void UpdateCustomers(ICustomer customer)
        {
            CustomerEntity entity = (CustomerEntity) customer;
            entity.LastModified = DateTime.Now;
        }

        public void DeleteCustomer(ICustomer customer)
        {
            CustomerEntity entity = (CustomerEntity) customer;
            entity.Deleted = true;
        }
        #endregion

        #region Booking Stuff
        List<IBooking> bookings = new List<IBooking>();
        public IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate)
        {
            BookingEntity booking = new BookingEntity(supplier, customer, sale, bookingNumber, startDate, endDate);
            bookings.Add(booking);

            return booking;
        }

        public List<IBooking> ReadAllBookings()
        {
            return bookings;
        }

        public void UpdateBooking(IBooking booking)
        {
            BookingEntity entity = (BookingEntity)booking;
            entity.LastModified = DateTime.Now;
        }

        public void DeleteBooking(IBooking booking)
        {
            BookingEntity entity = (BookingEntity)booking;
            entity.Deleted = true;

            bookings.Remove(booking);
        }
        #endregion


        #region PaymentRule Stuff
        public IPaymentRule CreatePaymentRule(ISupplier supplierEntity, ICustomer customerEntity, 
            BookingType bookingType, decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            PaymentRuleEntity paymentRule = new PaymentRuleEntity(supplierEntity, customerEntity, bookingType,
                percentage, daysOffset, baseDate, paymentType);

            SupplierEntity supplier = (SupplierEntity)supplierEntity;
            supplier.AddPaymentRule(paymentRule);

            return paymentRule;
        }

        public void UpdatePaymentRule(IPaymentRule paymentRuleEntity)
        {
            PaymentRuleEntity paymentRule = (PaymentRuleEntity)paymentRuleEntity;
            paymentRule.LastModified = DateTime.Now;
        }

        public void DeletePaymentRule(IPaymentRule paymentRuleEntity)
        {
            PaymentRuleEntity paymentRule = (PaymentRuleEntity)paymentRuleEntity;
            paymentRule.Deleted = true;

            SupplierEntity supplier = (SupplierEntity)(paymentRule.Supplier);
            supplier.RemovePaymentRule(paymentRule);
        }
        #endregion

        public void BackupDatabase(string backupPath)
        {
            throw new NotImplementedException();
        }


        public void RestoreDatabase(string backupPath)
        {
            throw new NotImplementedException();
        }

        public void SetupDatabase(string serverString, string databaseName)
        {
            throw new NotImplementedException();
        }


        public void InitializeDatabase()
        {
            throw new NotImplementedException();
        }

        public void SetupDatabase(string serverString, string databaseName, string installPath)
        {
            throw new NotImplementedException();
        }

        public void SetupDatabase(string installPath)
        {
            throw new NotImplementedException();
        }

        public void SetupDatabase(Version version)
        {
            throw new NotImplementedException();
        }
    }
}
