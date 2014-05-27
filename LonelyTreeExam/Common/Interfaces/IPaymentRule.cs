using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Enums;

namespace Common.Interfaces
{
    public interface IPaymentRule
    {
        ISupplier Supplier { get; set; }
        ICustomer Customer { get; set; }
        BookingType BookingType { get; set; }
        decimal Percentage { get; set; }
        int DaysOffset { get; set; }
        BaseDate BaseDate { get; set; }
        PaymentType PaymentType { get; set; }
    }
}
