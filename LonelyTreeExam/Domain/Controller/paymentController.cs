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

            paymentCollection = new PaymentCollection(dataAccessFacade);

        }

        public List<IPayment> ReadAllPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAll())
            {
                payments.Add(payment);
            }

            return payments;
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner)
        {
            return paymentCollection.Create(dueDate, dueAmount, responsible, commissioner);
        }

        public void UpdatePayment(IPayment payment)
        {
            paymentCollection.Update((Payment) payment);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection paymentCollection;
        #endregion
    }
}
