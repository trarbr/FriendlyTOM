using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess.Entities
{
    internal class CustomerEntity : APartyEntity, ICustomer
    {
        public CustomerType Type { get; set; }

        public CustomerEntity(CustomerType type, string note, string name)
            : base(note, name)
        {
            Type = type;
        }

        public string Responsible { get; set; }
        public string Commissioner { get; set; }
    }
}
