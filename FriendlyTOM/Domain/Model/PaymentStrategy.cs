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

using Domain.Controller;
using Common.Interfaces;
using Common.Enums;

namespace Domain.Model
{
    // creates payments by applying a set of PaymentRules to a booking!
    internal class PaymentStrategy
    {
        internal PaymentStrategy(CustomerController customerController)
        {
            this.customerController = customerController;
        }

        internal void CreatePayments(Booking booking, PaymentController paymentController)
        {
            Supplier supplier = (Supplier)booking.Supplier;
            Customer customer = (Customer)booking.Customer;
            BookingType bookingType = booking.Type;

            List<IPaymentRule> paymentRules = findMatchingPaymentRules(supplier, customer, bookingType);

            foreach (IPaymentRule paymentRule in paymentRules)
            {
                DateTime dueDate;

                if (paymentRule.BaseDate == BaseDate.StartDate)
                {
                    dueDate = booking.StartDate.AddDays(paymentRule.DaysOffset);
                }
                else
                {
                    dueDate = booking.EndDate.AddDays(paymentRule.DaysOffset);
                }

                decimal dueAmount = booking.TransferAmount * paymentRule.Percentage / 100;

                Customer lonelyTree = customerController.findLonelyTree();

                IPayment payment = paymentController.CreatePayment(dueDate, dueAmount, lonelyTree, booking.Supplier, 
                    paymentRule.PaymentType, booking.Sale, booking.BookingNumber);

                paymentController.UpdatePayment(payment);
            }
        }

        private CustomerController customerController;

        private List<IPaymentRule> findMatchingPaymentRules(Supplier supplier, Customer customer, BookingType bookingType)
        {
            IReadOnlyList<IPaymentRule> suppliersPaymentRules = supplier.PaymentRules;
            List<IPaymentRule> paymentRulesForCustomer = findPaymentRulesForCustomer(suppliersPaymentRules, customer);
            if (paymentRulesForCustomer.Count == 0)
            {
                Customer anyCustomer = customerController.findAnyCustomer();
                paymentRulesForCustomer = findPaymentRulesForCustomer(suppliersPaymentRules, anyCustomer);
            }
            // TODO: This is undocumented / unwanted behaviour!
            List<IPaymentRule> paymentRulesForBookingType = findPaymentRulesForBookingType(paymentRulesForCustomer, bookingType);
            if (paymentRulesForBookingType.Count == 0)
            {
                paymentRulesForBookingType = findPaymentRulesForBookingType(paymentRulesForCustomer, BookingType.Standard);
            }

            return paymentRulesForBookingType;
        }

        private List<IPaymentRule> findPaymentRulesForCustomer(IReadOnlyList<IPaymentRule> suppliersPaymentRules, Customer customer)
        {
            List<IPaymentRule> paymentRulesForCustomer = new List<IPaymentRule>();
            foreach (IPaymentRule paymentRule in suppliersPaymentRules)
            {
                if (((Customer)paymentRule.Customer)._customerEntity == customer._customerEntity)
                {
                    paymentRulesForCustomer.Add(paymentRule);
                }
            }

            return paymentRulesForCustomer;
        }

        private List<IPaymentRule> findPaymentRulesForBookingType(List<IPaymentRule> paymentRulesForCustomer, BookingType bookingType)
        {
            List<IPaymentRule> paymentRulesForBookingType = new List<IPaymentRule>();
            foreach (IPaymentRule paymentRule in paymentRulesForCustomer)
            {
                if (paymentRule.BookingType == bookingType)
                {
                    paymentRulesForBookingType.Add(paymentRule);
                }
            }

            return paymentRulesForBookingType;
        }
    }
}
