using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Enums;
using Common.Interfaces;
using DataAccess;

namespace Domain.Model
{
    internal class PaymentRule : IPaymentRule
    {
        public ISupplier Supplier
        {
            get { return _supplier; }
            set
            {
                // validate not null
                _supplier = (Supplier)value;
                _paymentRuleEntity.Supplier = _supplier._supplierEntity;
            }
        }

        public ICustomer Customer
        {
            get { return _customer; }
            set
            {
                //validate not null
                _customer = (Customer)value;
                _paymentRuleEntity.Customer = _customer._customerEntity;
            }
        }

        public BookingType BookingType
        {
            get { return _paymentRuleEntity.BookingType; }
            set { _paymentRuleEntity.BookingType = value; }
        }

        public decimal Percentage
        {
            // validate - no more than 100
            get { return _paymentRuleEntity.Percentage; }
            set { _paymentRuleEntity.Percentage = value; }
        }

        public int DaysOffset
        {
            get { return _paymentRuleEntity.DaysOffset; }
            set { _paymentRuleEntity.DaysOffset = value; }
        }

        public BaseDate BaseDate
        {
            get { return _paymentRuleEntity.BaseDate; }
            set { _paymentRuleEntity.BaseDate = value; }
        }

        public PaymentType PaymentType
        {
            get { return _paymentRuleEntity.PaymentType; }
            set { _paymentRuleEntity.PaymentType = value; }
        }

        internal IPaymentRule _paymentRuleEntity;

        internal PaymentRule(Supplier supplier, Customer customer, BookingType bookingType, decimal percentage, 
            int daysOffset, BaseDate baseDate, PaymentType paymentType, IDataAccessFacade dataAccessFacade)
        {
            // validate

            // Get entities for DataAccess
            ISupplier supplierEntity = supplier._supplierEntity;
            ICustomer customerEntity = customer._customerEntity;

            this.dataAccessFacade = dataAccessFacade;
            _paymentRuleEntity = dataAccessFacade.CreatePaymentRule(supplierEntity, customerEntity, bookingType, 
                percentage, daysOffset, baseDate, paymentType);
        }
        
        internal PaymentRule(IPaymentRule paymentRuleEntity, Supplier supplier, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            this._paymentRuleEntity = paymentRuleEntity;

            // Get/Create Models of supplier and customer
            Supplier = supplier;
            Customer = new Customer(_paymentRuleEntity.Customer, dataAccessFacade);
        } 

        internal void Delete()
        {
            dataAccessFacade.DeletePaymentRule(_paymentRuleEntity);
        }

        private IDataAccessFacade dataAccessFacade;
        private Supplier _supplier;
        private Customer _customer;
    }
}
