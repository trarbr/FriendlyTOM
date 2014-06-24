// MM
using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Helpers;
using System.Collections.Generic;

namespace Domain.Model
{
    internal class Supplier : AParty, ISupplier
    {
        #region Public Properties
        public SupplierType Type
        {
            get { return _supplierEntity.Type; }
            set { _supplierEntity.Type = value; }
        }

        public string AccountNo
        {
            get { return _supplierEntity.AccountNo; }
            set { _supplierEntity.AccountNo = value; }
        }

        public AccountType AccountType
        {
            get { return _supplierEntity.AccountType; }
            set { _supplierEntity.AccountType = value; }
        }

        public string AccountName
        {
            get { return _supplierEntity.AccountName; }
            set { _supplierEntity.AccountName = value; }
        }

        public string OwnerId
        {
            get { return _supplierEntity.OwnerId; }
            set { _supplierEntity.OwnerId = value; }
        }

        public string Bank
        {
            get { return _supplierEntity.Bank; }
            set { _supplierEntity.Bank = value; }
        }

        public IReadOnlyList<IPaymentRule> PaymentRules
        {
            get { return _paymentRules; }
        }

        #endregion

        internal ISupplier _supplierEntity;

        #region Internal Methods
        internal Supplier(string name, string note, SupplierType type, IDataAccessFacade dataAccessFacade)
        {
            //Calls validation on "name" for securing right input.
            validateName(name);
            this.dataAccessFacade = dataAccessFacade;
            _supplierEntity = dataAccessFacade.CreateSupplier(name, note, type);
            //Calls party class to put supplierentity as a party. 
            initializeParty(_supplierEntity);

            Register register = Register.GetInstance();
            register.RegisterSupplier(_supplierEntity, this);
        }

        internal Supplier(IDataAccessFacade dataAccessFacade, ISupplier supplierEntity)
        {
            this.dataAccessFacade = dataAccessFacade;
            _supplierEntity = supplierEntity;
            initializeParty(_supplierEntity);

            readPaymentRules();

            Register register = Register.GetInstance();
            register.RegisterSupplier(_supplierEntity, this);
        }

        internal static List<Supplier> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            //Takes all the objects that read and changes them to a supplier from an entity.
            List<ISupplier> supplierEntities = dataAccessFacade.ReadAllSuppliers();
            List<Supplier> suppliers = new List<Supplier>();

            foreach (ISupplier supplierEntity in supplierEntities)
            {
                Supplier supplier = new Supplier(dataAccessFacade, supplierEntity);
                suppliers.Add(supplier);
            }
            return suppliers;
        }

        internal void Update()
        {
            //Calls update in the dataAccessFacade
            dataAccessFacade.UpdateSupplier(_supplierEntity);
        }

        internal void Delete()
        {
            //Calls delete in the dataAccessFacade
            dataAccessFacade.DeleteSupplier(_supplierEntity);
        }

        internal void AddPaymentRule(Customer customer, BookingType bookingType, decimal percentage, int daysOffset, 
            BaseDate baseDate, PaymentType paymentType)
        {
            PaymentRule paymentRule = new PaymentRule(this, customer, bookingType, percentage, daysOffset, baseDate,
                paymentType, dataAccessFacade);

            readPaymentRules();
        }

        internal void DeletePaymentRule(PaymentRule paymentRule)
        {
            paymentRule.Delete();

            readPaymentRules();
        }
        #endregion

        #region Private Fields
        private IDataAccessFacade dataAccessFacade;
        private List<PaymentRule> _paymentRules;
        #endregion

        private void readPaymentRules()
        {
            _paymentRules = new List<PaymentRule>();

            foreach (IPaymentRule paymentRuleEntity in _supplierEntity.PaymentRules)
            {
                PaymentRule paymentRule = new PaymentRule(paymentRuleEntity, this, dataAccessFacade);
                _paymentRules.Add(paymentRule);
            }
        }
    }
}
