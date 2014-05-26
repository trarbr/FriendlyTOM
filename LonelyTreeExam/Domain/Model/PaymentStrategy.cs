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

                booking.CalculateAmounts();
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
            List<IPaymentRule> paymentRulesForBookingType = findPaymentRulesForBookingType(paymentRulesForCustomer, bookingType);

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
