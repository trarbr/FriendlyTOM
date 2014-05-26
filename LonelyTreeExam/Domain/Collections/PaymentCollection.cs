using System;
using System.Collections.Generic;
using DataAccess;
using Domain.Model;
using Common.Enums;
using Common.Interfaces;

namespace Domain.Collections
{
    internal class PaymentCollection
    {
        #region Internal Methods
        //Construtor calls readall on for the new instance of dataaccessfacade. 
        internal PaymentCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }

        /// <summary>
        /// Reads all Payments currently in the database.
        /// </summary>
        /// <returns>payments</returns>
        internal List<Payment> ReadAll()
        {
            //if the list is empty do a readall in the database. 
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
                        //if its a payment that is being paid to Lonely Tree, 
                        //set the Commissioner name to Lonely Tree
                    else if (payment.Commissioner.Name == "Lonely Tree")
                    {
                        incomingPayments.Add(payment);
                    }
                        //if its a payment that is being paid by Lonely Tree,
                        //set the Responsible name to Lonely Tree.
                    else if (payment.Responsible.Name == "Lonely Tree")
                    {
                        outgoingPayments.Add(payment);
                    }
                }
            }

            return payments;
        }
        //does a readAll and all where Archived is true, is being added to this list. 
        internal List<Payment> ReadAllArchived()
        {
            if (payments == null)
            {
                ReadAll();
            }

            return archivedPayments;
        }

        //does a ReadAll and if lonely tree is Commissioner they are being added to this list. 
        internal List<Payment> ReadAllIncoming()
        {
            if (payments == null)
            {
                ReadAll();
            }

            return incomingPayments;
        }

        //does a ReadAll and if lonely tree is Responsible they are being added to this list. 
        internal List<Payment> ReadAllOutgoing()
        {
            if (payments == null)
            {
                ReadAll();
            }
            return outgoingPayments;
        }

        //adds the new payment to the correct list after being sorted
        internal Payment Create(DateTime dueDate, decimal dueAmount, IParty responsible,
             IParty commissioner, PaymentType type, string sale, int booking)
        {
            Payment payment = new Payment(dueDate, dueAmount, responsible, commissioner, type,
                sale, booking, dataAccessFacade);
            payments.Add(payment);
            return payment;
        }

        //Checks  if the payment is in the correct list and only in 1 list. 
         public void Update(Payment payment)
         {
             if (payment.Archived == true)
             {
                 if (!archivedPayments.Contains(payment))
                 {
                     archivedPayments.Add(payment);
                     incomingPayments.Remove(payment);
                     outgoingPayments.Remove(payment);
                 }
             }
             else if (payment.Archived == false)
             {
                 archivedPayments.Remove(payment);

                 if (payment.Commissioner.Name == "Lonely Tree")
                 {
                     if (!incomingPayments.Contains(payment))
                     {
                         incomingPayments.Add(payment);
                         outgoingPayments.Remove(payment);
                     }
                 }
                 if (payment.Responsible.Name == "Lonely Tree")
                 {
                     if (!outgoingPayments.Contains(payment))
                     {
                         outgoingPayments.Add(payment);
                         incomingPayments.Remove(payment);
                     }
                 }
             }
             payment.Update();
         }

        //Deletes the instance of the object in all lists.
         internal void Delete(Payment payment)
         {
             payment.Delete();
             archivedPayments.Remove(payment);
             incomingPayments.Remove(payment);
             outgoingPayments.Remove(payment);
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
