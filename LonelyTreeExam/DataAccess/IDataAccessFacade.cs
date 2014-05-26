using System;
using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess
{
   public interface IDataAccessFacade
   {
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

