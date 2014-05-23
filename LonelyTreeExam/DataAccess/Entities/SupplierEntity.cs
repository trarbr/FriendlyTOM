using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal class SupplierEntity : APartyEntity, ISupplier
    {
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

        public SupplierEntity(SupplierType type, string note, string name) 
            :base(note, name)
        {
            Type = type;
            AccountNo = "";
            AccountType = AccountType.Undefined;
            AccountName = "";
            OwnerId = "";
            Bank = "";

            _paymentRules = new List<PaymentRuleEntity>();
        }

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
