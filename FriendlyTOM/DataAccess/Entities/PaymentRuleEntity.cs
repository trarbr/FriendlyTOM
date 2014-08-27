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
