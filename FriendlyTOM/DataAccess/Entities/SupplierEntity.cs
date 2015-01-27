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
using System.Collections.Generic;

namespace DataAccess.Entities
{
    internal class SupplierEntity : APartyEntity, ISupplier
    {
        #region Public Properties
        public SupplierType Type { get; set; }
        public string AccountNo { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
        public string OwnerId { get; set; }
        public string Bank { get; set; }
        public IReadOnlyList<IPaymentRule> PaymentRules
        {
            get { return _paymentRules; }
        }
        #endregion

        #region Constructor
        public SupplierEntity(SupplierType type, string note, string name) 
            :base(note, name)
        {
            Type = type;
            AccountNo = "";
            AccountType = AccountType.CheckingAccount;
            AccountName = "";
            OwnerId = "";
            Bank = "";

            _paymentRules = new List<PaymentRuleEntity>();
        }
        #endregion

        internal void AddPaymentRule(PaymentRuleEntity paymentRule)
        {
            _paymentRules.Add(paymentRule);
        }

        internal void RemovePaymentRule(PaymentRuleEntity paymentRule)
        {
            _paymentRules.Remove(paymentRule);
        }

        private List<PaymentRuleEntity> _paymentRules;
    }
}
