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
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }

        public CustomerEntity(CustomerType type, string note, string name)
            : base(note, name)
        {
            Type = type;
            ContactPerson = "";
            Email = "";
            Address = "";
            PhoneNo = "";
            FaxNo = "";
        }
    }
}
