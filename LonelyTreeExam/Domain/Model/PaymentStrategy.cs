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
        private Booking booking;
        private PaymentController paymentController;

        public PaymentStrategy(Booking booking, PaymentController paymentController)
        {
            this.booking = booking;
            this.paymentController = paymentController;
        }


        internal void CreatePayments()
        {
            Supplier supplier = (Supplier)booking.Supplier;
            Customer customer = (Customer)booking.Customer;

            // finding has to do more, but this will work in some cases
            IReadOnlyList<IPaymentRule> suppliersPaymentRules = supplier.PaymentRules;
            List<IPaymentRule> paymentRulesForCustomer = findPaymentRulesForCustomer(suppliersPaymentRules, customer);
            List<IPaymentRule> paymentRulesForBookingType = findPaymentRulesForBookingType(paymentRulesForCustomer, booking.Type);

            foreach (IPaymentRule paymentRule in paymentRulesForBookingType)
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

                booking.CalculateTransferAmount();
                decimal dueAmount = booking.TransferAmount * paymentRule.Percentage / 100;

                Customer payingCustomer = paymentController.CustomerLonelyTree;

                IPayment payment = paymentController.CreatePayment(dueDate, dueAmount, payingCustomer, booking.Supplier, 
                    paymentRule.PaymentType, booking.Sale, booking.BookingNumber);

                paymentController.UpdatePayment(payment);
            }
        }

        private List<IPaymentRule> findPaymentRulesForCustomer(IReadOnlyList<IPaymentRule> suppliersPaymentRules, Customer customer)
        {
            List<IPaymentRule> paymentRules = new List<IPaymentRule>();
            foreach (IPaymentRule paymentRule in suppliersPaymentRules)
            {
                if (((Customer)paymentRule.Customer)._customerEntity == customer._customerEntity)
                {
                    paymentRules.Add(paymentRule);
                }
            }

            return paymentRules;
        }

        private List<IPaymentRule> findPaymentRulesForBookingType(List<IPaymentRule> paymentRules, BookingType bookingType)
        {
            List<IPaymentRule> filteredPaymentRules = new List<IPaymentRule>();
            foreach (IPaymentRule paymentRule in paymentRules)
            {
                if (paymentRule.BookingType == bookingType)
                {
                    filteredPaymentRules.Add(paymentRule);
                }
            }

            return filteredPaymentRules;
        }
    }
}
