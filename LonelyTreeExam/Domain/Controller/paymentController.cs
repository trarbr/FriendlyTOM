using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;

namespace Domain.Controller
{
    public class paymentController
    {
        #region Public Methods
        public paymentController()
        {
            dataAccessFacade = new DataAccessFacade();

            paymentsCollection = new PaymentsCollection(dataAccessFacade);

        }

        public List<IPayment> ReadAllArtists()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentsCollection.ReadAll())
            {
                payments.Add(payment);
            }

            return payments;
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentsCollection paymentsCollection;
        #endregion
    }
}
