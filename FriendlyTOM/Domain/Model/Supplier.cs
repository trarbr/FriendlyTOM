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
            get { return _type; }
            set { _type = value; }
        }

        public string AccountNo
        {
            get { return _accountNo; }
            set { _accountNo = value; }
        }

        public AccountType AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }

        public string AccountName
        {
            get { return _accountName; }
            set { _accountName = value; }
        }

        public string OwnerId
        {
            get { return _ownerId; }
            set { _ownerId = value; }
        }

        public string Bank
        {
            get { return _bank; }
            set { _bank = value; }
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
            _paymentRules = new List<PaymentRule>();
            //Calls party class to put supplierentity as a party. 
            initializeParty(_supplierEntity);

            Register register = Register.GetInstance();
            register.RegisterSupplier(_supplierEntity, this);
        }

        internal Supplier(ISupplier supplierEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _supplierEntity = supplierEntity;
            _type = supplierEntity.Type;
            _accountNo = supplierEntity.AccountNo;
            _accountType = supplierEntity.AccountType;
            _accountName = supplierEntity.AccountName;
            _ownerId = supplierEntity.OwnerId;
            _bank = supplierEntity.Bank;
            _paymentRules = new List<PaymentRule>();

            initializeParty(_supplierEntity);

            foreach (var paymentRuleEntity in _supplierEntity.PaymentRules)
            {
                var paymentRule = new PaymentRule(paymentRuleEntity, dataAccessFacade);
                _paymentRules.Add(paymentRule);
            }

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
                Supplier supplier = new Supplier(supplierEntity, dataAccessFacade);
                suppliers.Add(supplier);
            }
            return suppliers;
        }

        internal void Update()
        {
            //Calls update in the dataAccessFacade
            _supplierEntity.Name = _name;
            _supplierEntity.Note = _note;
            _supplierEntity.Type = _type;
            _supplierEntity.AccountNo = _accountNo;
            _supplierEntity.AccountType = _accountType;
            _supplierEntity.AccountName = _accountName;
            _supplierEntity.OwnerId = _ownerId;
            _supplierEntity.Bank = _bank;

            dataAccessFacade.UpdateSupplier(_supplierEntity);

            foreach (var paymentRule in _paymentRules)
            {
                paymentRule.Update();
            }
        }

        internal void Delete()
        {
            //Calls delete in the dataAccessFacade
            dataAccessFacade.DeleteSupplier(_supplierEntity);
        }

        // supplier.AddPaymentRule
        // p = new PaymentRule
        //     daf.CreatePaymentRule
        //         _supplierE.AddPaymentRule(p._paymentRuleEntity)
        //             _paymentRules.Add
        // _paymentRules.Add(p)

        // supplier.DeletePaymentRule()
        // p.Delete()
        //     daf.DeletePaymentRule
        //         _supplierE.DeletePaymentRule(p._paymentRuleEntity)
        //             _paymentRules.Add
        // _paymentRules.Remove

        // supplier.Update()
        //     daf.UpdateSupplier()
        //         foreach paymentRule, do update

        // supplier.ReadAll()
        //     foreach paymentRule that the supplierEntity has:
        //         make new paymentRule, add it to _paymentRules

        internal void AddPaymentRule(Customer customer, BookingType bookingType, decimal percentage, 
            int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            PaymentRule paymentRule = new PaymentRule(this, customer, bookingType, percentage, 
                daysOffset, baseDate, paymentType, dataAccessFacade);

            _paymentRules.Add(paymentRule);
        }

        internal void DeletePaymentRule(PaymentRule paymentRule)
        {
            paymentRule.Delete();

            _paymentRules.Remove(paymentRule);
        }
        #endregion

        #region Private Fields
        private IDataAccessFacade dataAccessFacade;
        private List<PaymentRule> _paymentRules;
        private SupplierType _type;
        private string _accountNo;
        private AccountType _accountType;
        private string _accountName;
        private string _ownerId;
        private string _bank;
        #endregion
    }
}
