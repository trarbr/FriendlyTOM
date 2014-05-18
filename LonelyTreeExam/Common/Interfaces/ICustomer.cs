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
        string Name { get; set; }
        string Note { get; set; }
    }
}
