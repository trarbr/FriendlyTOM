/*
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess;
using Domain.Model;
using Common.Enums;
using System.Collections.Generic;
using Domain.Collections;
using Common.Interfaces;

namespace UnitTestProject
{
    [TestClass]
    public class PaymentCollectionTest
    {
        IDataAccessFacade dataAccessFacadeStub;
        PaymentCollection paymentCollection;
        DateTime validDueDate;
        decimal validDueAmount;
        IParty validPayer;
        IParty validPayee;
        PaymentType validType;
        string validSale;
        int validBooking;

        [TestInitialize]
        public void Initialize()
        {
            dataAccessFacadeStub = new DataAccessFacadeStub();
            paymentCollection = new PaymentCollection(dataAccessFacadeStub);
            validDueDate = new DateTime(2010, 10, 10);
            validDueAmount = 1m;
            validPayer = new Customer("Lonely Tree", "", CustomerType.Bureau, dataAccessFacadeStub);
            validPayee = new Supplier("Galasam", "", SupplierType.Cruise, dataAccessFacadeStub);
            validType = PaymentType.Balance;
            validSale = "VF Jan";
            validBooking = 2;
        }

        [TestMethod]
        public void TestReadAll()
        {
            Payment p1 = createValidPayment();
            p1.Note = "C1";
            Payment p2 = createValidPayment();
            p2.Note = "C2";
            Payment p3 = createValidPayment();
            p3.Note = "C3";

            List<Payment> expectedPayments = new List<Payment>() { p1, p2, p3 };
            List<Payment> actualPayments = paymentCollection.ReadAll();

            CollectionAssert.AreEqual(expectedPayments, actualPayments);
        }

        [TestMethod]
        public void TestReadAllArchived()
        {
            createValidPayment();
            createValidPayment();
            createValidPayment();

            Payment p1 = createValidPayment();
            p1.Note = "Archived1";
            p1.Archived = true;
            p1.Update();

            Payment p2 = createValidPayment();
            p2.Note = "Archived2";
            p2.Archived = true;
            p2.Update();

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacadeStub);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2 };
            List<Payment> actualPayments = paymentCollection.ReadAllArchived();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
            }
        }

        [TestMethod]
        public void TestReadAllIncoming()
        {
            createValidPayment();
            createValidPayment();
            createValidPayment();

            IParty commissioner = new Supplier("Lonely Tree", "", SupplierType.Cruise, dataAccessFacadeStub);

            Payment p1 = createValidPayment();
            p1.Note = "Moved to Lonely Tree1";
            p1.Payee = validPayer;
            p1.Payer = validPayee;
            p1.Update();

            Payment p2 = createValidPayment();
            p2.Note = "Moved to Lonely Tree2";
            p2.Payee = validPayer;
            p2.Payer = validPayee;
            p2.Update();

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacadeStub);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2 };
            List<Payment> actualPayments = paymentCollection.ReadAllIncoming();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
                Assert.AreEqual(expectedPayments[i].Payee.Name, actualPayments[i].Payee.Name);
                Assert.AreEqual(expectedPayments[i].Payer.Name, actualPayments[i].Payer.Name);
            }
        }

        [TestMethod]
        public void TestReadAllOutgoing()
        {
            Payment p1 = createValidPayment();
            p1.Note = "Outgoing1";
            p1.Update();
            Payment p2 = createValidPayment();
            p2.Note = "Outgoing2";
            p2.Update();
            Payment p3 = createValidPayment();
            p3.Note = "Outgoing3";
            p3.Update();

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacadeStub);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2, p3 };
            List<Payment> actualPayments = paymentCollection.ReadAllOutgoing();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
                Assert.AreEqual(expectedPayments[i].Payee.Name, actualPayments[i].Payee.Name);
                Assert.AreEqual(expectedPayments[i].Payer.Name, actualPayments[i].Payer.Name);
            }
        }

        [TestMethod]
        public void TestUpdateArchived()
        {
            Payment p1 = createValidPayment();

            p1.Archived = true;
            p1.Note = "Moved to archive";
            paymentCollection.Update(p1);

            List<Payment> expectedArchivedPayments = new List<Payment>() { p1 };
            List<Payment> actualArchivedPayments = paymentCollection.ReadAllArchived();

            CollectionAssert.AreEqual(expectedArchivedPayments, actualArchivedPayments);

            List<Payment> expectedOutgoingPayments = new List<Payment>();
            List<Payment> actualOutgoingPayments = paymentCollection.ReadAllOutgoing();

            CollectionAssert.AreEqual(expectedOutgoingPayments, actualOutgoingPayments);

            List<Payment> expectedIncomingPayments = new List<Payment>();
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        [TestMethod]
        public void TestUpdateRestored()
        {
            Payment p1 = createValidPayment();
            p1.Archived = true;

            p1.Archived = false;
            p1.Note = "Restored";
            paymentCollection.Update(p1);

            List<Payment> expectedArchivedPayments = new List<Payment>();
            List<Payment> actualArchivedPayments = paymentCollection.ReadAllArchived();

            CollectionAssert.AreEqual(expectedArchivedPayments, actualArchivedPayments);

            List<Payment> expectedOutgoingPayments = new List<Payment>() { p1 };
            List<Payment> actualOutgoingPayments = paymentCollection.ReadAllOutgoing();

            CollectionAssert.AreEqual(expectedOutgoingPayments, actualOutgoingPayments);

            List<Payment> expectedIncomingPayments = new List<Payment>();
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        [TestMethod]
        public void TestUpdateOutgoingToIncoming()
        {
            Payment p1 = createValidPayment();

            p1.Payee = validPayer;
            p1.Payer = validPayee;
            p1.Note = "Moved from outgoing to incoming";
            paymentCollection.Update(p1);

            List<Payment> expectedArchivedPayments = new List<Payment>();
            List<Payment> actualArchivedPayments = paymentCollection.ReadAllArchived();

            CollectionAssert.AreEqual(expectedArchivedPayments, actualArchivedPayments);

            List<Payment> expectedOutgoingPayments = new List<Payment>();
            List<Payment> actualOutgoingPayments = paymentCollection.ReadAllOutgoing();

            CollectionAssert.AreEqual(expectedOutgoingPayments, actualOutgoingPayments);

            List<Payment> expectedIncomingPayments = new List<Payment>() { p1 };
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        [TestMethod]
        public void TestUpdateIncomingToOutgoing()
        {
            Payment p1 = createValidPayment();

            // switch payer and payee to make incoming payment
            p1.Payee = validPayer;
            p1.Payer = validPayee; 
            p1.Note = "Moved from outgoing to incoming";
            paymentCollection.Update(p1);

            // now make it an outgoing payment again
            p1.Payee = validPayee;
            p1.Payer = validPayer;
            p1.Note = "Moved from incoming to outgoing";
            paymentCollection.Update(p1);

            List<Payment> expectedArchivedPayments = new List<Payment>();
            List<Payment> actualArchivedPayments = paymentCollection.ReadAllArchived();

            CollectionAssert.AreEqual(expectedArchivedPayments, actualArchivedPayments);

            List<Payment> expectedOutgoingPayments = new List<Payment>() { p1 };
            List<Payment> actualOutgoingPayments = paymentCollection.ReadAllOutgoing();

            CollectionAssert.AreEqual(expectedOutgoingPayments, actualOutgoingPayments);

            List<Payment> expectedIncomingPayments = new List<Payment>();
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        private Payment createValidPayment()
        {

            return paymentCollection.Create(validDueDate, validDueAmount, validPayer, validPayee, validType,
                validSale, validBooking);
        }
    }
}
