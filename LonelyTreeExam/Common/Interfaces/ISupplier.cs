using Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ISupplier : IParty
    {
        string PaymentInfo { get; set; }
        SupplierType Type { get; set; }
    }
}
