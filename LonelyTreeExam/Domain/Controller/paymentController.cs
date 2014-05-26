﻿using System;
using System.Collections.Generic;
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
        public PaymentController(SupplierController supplierController, CustomerController customerController)
        {
            dataAccessFacade = DataAccessFacade.GetInstance();

            paymentCollection = new PaymentCollection(dataAccessFacade);

            foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
            {
                if (supplier.Name == "Lonely Tree")
                {
                    SupplierLonelyTree = (Supplier)supplier;
                    break;
                }
            }

            foreach (ICustomer customer in customerController.ReadAllCustomers())
            {
                if (customer.Name == "Lonely Tree")
                {
                    CustomerLonelyTree = (Customer)customer;
                    break;
                }
            }
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer, 
            IParty payee, PaymentType type, string sale, int booking)
        {
            //Calls the paymentCollection class for create
            return paymentCollection.Create(dueDate, dueAmount, payer, payee, type,
                sale, booking);
        }

        public List<IPayment> ReadAllPayments()
        {
            //Calls the paymentCollection class for readall
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAll())
            {
                payments.Add(payment);
            }
            return payments;
        }

        public List<IPayment> ReadAllArchivedPayments()
        {
            //Calls the paymentCollection class for Read all thats archived
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllArchived())
            {
                payments.Add(payment);
            }
            return payments;
        }

        public List<IPayment> ReadAllIncomingPayments()
        {
            //Calls the paymentCollection class for read all incoming
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllIncoming())
            {
                payments.Add(payment);
            }
            return payments;
        }

        public List<IPayment> ReadAllOutgoingPayments()
        {
            //Calls the paymentCollection class for read all outgoing
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllOutgoing())
            {
                payments.Add(payment);
            }
            return payments;
        }

        public void UpdatePayment(IPayment payment)
        {
            //Calls the paymentCollection class for update
            paymentCollection.Update((Payment)payment);
        }

        public void DeletePayment(IPayment payment)
        {
            //Calls the paymentCollection class for delete
            paymentCollection.Delete((Payment)payment);
        }
        #endregion

        internal Supplier SupplierLonelyTree;
        internal Customer CustomerLonelyTree;

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection paymentCollection;
        #endregion
    }
}
