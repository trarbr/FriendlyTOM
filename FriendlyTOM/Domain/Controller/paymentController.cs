﻿/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
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
        public PaymentController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            paymentCollection = new PaymentCollection(dataAccessFacade);
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

        internal void DeletePaymentForParty(AParty party)
        {
            var paymentsToDelete = ReadAllPayments()
                .Where<IPayment>(p => p.Payee == party || p.Payer == party);
            foreach (var payment in paymentsToDelete)
            {
                DeletePayment(payment);
            }
        }

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection paymentCollection;
        #endregion
    }
}
