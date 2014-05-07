using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domain.Model;

namespace Domain.Collections
{
    internal class PaymentsCollection
    {
        #region Internal Methods

        /// <summary>
        /// make a collection of information from readAll.
        /// </summary>
        /// <param name="dataAccessFacade"></param>
        internal PaymentsCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;

            ReadAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>payments</returns>
         internal List<Payment> ReadAll()
        {
            if (payments == null)
            {
                payments = Payment.ReadAll((DataAccessFacade) dataAccessFacade);
            }

            return payments;
        }
        #endregion

        #region Private Properties
         private IDataAccessFacade dataAccessFacade;
        private List<Payment> payments;
#endregion
    }
}
