using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Domain.Collections
{
    internal class PaymentsCollection
    {
        internal PaymentsCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;

            ReadAll();
        }

         internal List<Payment> ReadAll()
        {
            if (payments == null)
            {
                payments = Payment.ReadAll(dataAccessFacade);
            }

            return payments;
        }
    }
}
