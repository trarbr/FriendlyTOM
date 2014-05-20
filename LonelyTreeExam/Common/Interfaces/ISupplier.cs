using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ISupplier : IParty
    {
        SupplierType Type { get; set; }
        string AccountNo { get; set; }
        AccountType AccountType { get; set; }
        string AccountName { get; set; }
        string OwnerId { get; set; }
        string Bank { get; set; }
    }
}
