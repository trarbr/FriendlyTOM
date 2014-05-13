using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using Common.Enums;

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

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, 
            string commissioner, PaymentType type)
        {
            return paymentCollection.Create(dueDate, dueAmount, responsible, commissioner, type);
        }

        public void UpdatePayment(IPayment payment)
        {
            paymentCollection.Update((Payment)payment);
        }

        public void DeletePayment(IPayment payment)
        {
            paymentCollection.Delete((Payment)payment);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection paymentCollection;
        #endregion
    }
}
