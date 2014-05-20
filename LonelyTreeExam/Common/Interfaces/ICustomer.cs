using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;

namespace Common.Interfaces
{
    public interface ICustomer : IParty
    {
        CustomerType Type { get; set; }
        string ContactPerson { get; set; }
        string Email { get; set; }
        string Address { get; set; }
        string PhoneNo { get; set; }
        string FaxNo { get; set; }
    }
}
