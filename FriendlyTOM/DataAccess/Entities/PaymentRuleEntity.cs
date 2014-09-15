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

using Common.Interfaces;
using Common.Enums;

namespace DataAccess.Entities
{
    internal class PaymentRuleEntity : Entity, IPaymentRule
    {
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
        public BookingType BookingType { get; set; }
        public decimal Percentage { get; set; }
        public int DaysOffset { get; set; }
        public BaseDate BaseDate { get; set; }
        public PaymentType PaymentType { get; set; }

        internal PaymentRuleEntity(ISupplier supplierEntity, ICustomer customerEntity, BookingType bookingType,
            decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            Supplier = supplierEntity;
            Customer = customerEntity;
            BookingType = bookingType;
            Percentage = percentage;
            DaysOffset = daysOffset;
            BaseDate = baseDate;
            PaymentType = paymentType;
        }

        private SupplierEntity _supplier;
        private CustomerEntity _customer;
    }
}
