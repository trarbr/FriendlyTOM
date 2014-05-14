using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Domain.Model;
using Common.Enums;

namespace Domain.Collections
{
    internal class PaymentCollection
    {
        #region Internal Methods

        /// <summary>
        /// make a collection of information from readAll.
        /// </summary>
        /// <param name="dataAccessFacade"></param>
        internal PaymentCollection(IDataAccessFacade dataAccessFacade)
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
                payments = Payment.ReadAll(dataAccessFacade);

                archivedPayments = new List<Payment>();
                incomingPayments = new List<Payment>();
                outgoingPayments = new List<Payment>();

                foreach (Payment payment in payments)
                {
                    if (payment.Archived == true)
                    {
                        archivedPayments.Add(payment);
                    }
                    else if (payment.Commissioner == "Lonely Tree")
                    {
                        incomingPayments.Add(payment);
                    }
                    else if (payment.Responsible == "Lonely Tree")
                    {
                        outgoingPayments.Add(payment);
                    }
                }
            }

            return payments;
        }

        internal List<Payment> ReadAllArchived()
        {
            if (payments == null)
            {
                ReadAll();
            }

            return archivedPayments;
        }

        internal List<Payment> ReadAllIncoming()
        {
            if (payments == null)
            {
                ReadAll();
            }

            return incomingPayments;
        }

        internal List<Payment> ReadAllOutgoing()
        {
            if (payments == null)
            {
                ReadAll();
            }

            return outgoingPayments;
        }

        internal Payment Create(DateTime dueDate, decimal dueAmount, string responsible,
             string commissioner, PaymentType type, string sale, int booking)
        {
            Payment payment = new Payment(dueDate, dueAmount, responsible, commissioner, type,
                sale, booking, dataAccessFacade);
            payments.Add(payment);

            return payment;
        }

         public void Update(Payment payment)
         {
             if (payment.Archived == true)
             {
                 if (!archivedPayments.Contains(payment))
                 {
                     archivedPayments.Add(payment);
                     if (incomingPayments.Contains(payment))
                     {
                         incomingPayments.Remove(payment);
                     }
                     else if (outgoingPayments.Contains(payment))
                     {
                         outgoingPayments.Remove(payment);
                     }
                 }
             }
             else if (payment.Archived == false)
             {
                 if (archivedPayments.Contains(payment))
                 {
                     archivedPayments.Remove(payment);
                 }

                 if (payment.Commissioner == "Lonely Tree")
                 {
                     if (!incomingPayments.Contains(payment))
                     {
                         incomingPayments.Add(payment);
                         if (outgoingPayments.Contains(payment))
                         {
                             outgoingPayments.Remove(payment);
                         }
                     }
                 }
                 if (payment.Responsible == "Lonely Tree")
                 {
                     if (!outgoingPayments.Contains(payment))
                     {
                         outgoingPayments.Add(payment);

                         if (incomingPayments.Contains(payment))
                         {
                             incomingPayments.Remove(payment);
                         }
                     }
                 }
             }

             payment.Update();
         }

         internal void Delete(Payment payment)
         {
             payment.Delete();
             payments.Remove(payment);
         }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Payment> payments;
        private List<Payment> archivedPayments;
        private List<Payment> incomingPayments;
        private List<Payment> outgoingPayments;
        #endregion
    }
}
