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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Helpers;

namespace Domain.Model
{
    internal class PaymentRule : IPaymentRule
    {
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

        public BookingType BookingType
        {
            get { return _bookingType; }
            set { _bookingType = value; }
        }

        public decimal Percentage
        {
            // validate - no more than 100
            get { return _percentage; }
            set { _percentage = value; }
        }

        public int DaysOffset
        {
            get { return _daysOffset; }
            set { _daysOffset = value; }
        }

        public BaseDate BaseDate
        {
            get { return _baseDate; }
            set { _baseDate = value; }
        }

        public PaymentType PaymentType
        {
            get { return _paymentType; }
            set { _paymentType = value; }
        }

        internal IPaymentRule _paymentRuleEntity;

        internal PaymentRule(Supplier supplier, Customer customer, BookingType bookingType, decimal percentage, 
            int daysOffset, BaseDate baseDate, PaymentType paymentType, IDataAccessFacade dataAccessFacade)
        {
            // validate
            validateCustomer(customer);
            validateSupplier(supplier);

            _supplier = supplier;
            _customer = customer;
            _bookingType = bookingType;
            _percentage = percentage;
            _daysOffset = daysOffset;
            _baseDate = baseDate;
            _paymentType = paymentType;

            // Get entities for DataAccess
            ISupplier supplierEntity = supplier._supplierEntity;
            ICustomer customerEntity = customer._customerEntity;

            this.dataAccessFacade = dataAccessFacade;
            _paymentRuleEntity = dataAccessFacade.CreatePaymentRule(supplierEntity, customerEntity, bookingType, 
                percentage, daysOffset, baseDate, paymentType);

        }

        internal PaymentRule(IPaymentRule paymentRuleEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _paymentRuleEntity = paymentRuleEntity;
            _bookingType = paymentRuleEntity.BookingType;
            _percentage = paymentRuleEntity.Percentage;
            _daysOffset = paymentRuleEntity.DaysOffset;
            _baseDate = paymentRuleEntity.BaseDate;
            _paymentType = paymentRuleEntity.PaymentType;

            // Get/Create Models of supplier and customer
            Register register = Register.GetInstance();
            _supplier = register.GetSupplier(paymentRuleEntity.Supplier);
            _customer = register.GetCustomer(paymentRuleEntity.Customer);
        } 

        internal void Update()
        {
            _paymentRuleEntity.Supplier = _supplier._supplierEntity;
            _paymentRuleEntity.Customer = _customer._customerEntity;
            _paymentRuleEntity.BookingType = _bookingType;
            _paymentRuleEntity.Percentage = _percentage;
            _paymentRuleEntity.DaysOffset = _daysOffset;
            _paymentRuleEntity.BaseDate = _baseDate;
            _paymentRuleEntity.PaymentType = _paymentType;

            dataAccessFacade.UpdatePaymentRule(_paymentRuleEntity);
        }

        internal void Delete()
        {
            dataAccessFacade.DeletePaymentRule(_paymentRuleEntity);
        }

        private IDataAccessFacade dataAccessFacade;
        private Supplier _supplier;
        private Customer _customer;
        private BookingType _bookingType;
        private decimal _percentage;
        private int _daysOffset;
        private BaseDate _baseDate;
        private PaymentType _paymentType;

        private void validateCustomer(ICustomer value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Supplier", "Supplier was not found");
            }
        }

        private void validateSupplier(ISupplier value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Supplier", "Supplier was not found");
            }
        }
    }
}
