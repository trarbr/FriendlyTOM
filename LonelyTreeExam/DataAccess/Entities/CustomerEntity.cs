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
        
        public CustomerEntity(CustomerType type, string note, string name) : base(note, name)
        {
            _customers = new List<string>();
            Name = name;
            Note = note;
        }

        public void DeleteCustomer(string customer)
        {
            _customers.Remove(customer);
        }

        public void AddCustomer(string customer)
        {
            _customers.Add(customer);
        }


        private List<string> _customers;
    }
}
