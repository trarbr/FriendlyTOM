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

        public SupplierEntity(SupplierType type, string note, string name) 
            :base(note, name)
        {
            Type = type;
            AccountNo = "";
            AccountType = AccountType.Undefined;
            AccountName = "";
            OwnerId = "";
            Bank = "";

        }
    }
}
