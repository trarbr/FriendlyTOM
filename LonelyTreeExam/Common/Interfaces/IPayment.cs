using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPayment
    {
        string Name { get; set; }
        IReadOnlyCollection<int> Invoice { get; }

        void Addinvoice(int invoice);

    }
}
