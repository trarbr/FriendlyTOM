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
    public class PaymentController
    {
        #region Public Methods
        public PaymentController()
        {
            dataAccessFacade = new DataAccessFacade();

            _paymentCollection = new PaymentCollection(dataAccessFacade);

        }

        public List<IPayment> ReadAllPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in _paymentCollection.ReadAll())
            {
                payments.Add(payment);
            }

            return payments;
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection _paymentCollection;
        #endregion
    }
}
