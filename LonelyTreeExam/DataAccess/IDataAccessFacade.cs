using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess
{
   public interface IDataAccessFacade
   {
        IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking);
        List<IPayment> ReadAllPayments();
        void UpdatePayment(IPayment payment);
        void DeletePayment(IPayment payment);

       ISupplier CreateSupplier(string name, string note, SupplierType type);
       List<ISupplier> ReadAllSuppliers();
       void UpdateSupplier(ISupplier supplier);
       void DeleteSupplier(ISupplier supplier);

       ICustomer CreateCustomer(CustomerType type, string note, string name);
       List<ICustomer> ReadAllCustomers();
       void UpdateCustomers(ICustomer customer);
       void DeleteCustomer(ICustomer customer);

       IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber,
           DateTime startDate, DateTime endDate);
       List<IBooking> ReadAllBookings();
       void UpdateBooking(IBooking booking);
       void DeleteBooking(IBooking booking);

       IPaymentRule CreatePaymentRule(ISupplier supplierEntity, ICustomer customerEntity, BookingType bookingType,
            decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType);
       List<IPaymentRule> ReadAllPaymentRules();
       void UpdatePaymentRule(IPaymentRule paymentRuleEntity);
       void DeletePaymentRule(IPaymentRule paymentRuleEntity);
   }
}

