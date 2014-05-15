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
        IDataAccessFacade dataAccessFacade;
        PaymentCollection paymentCollection;
        DateTime validDueDate;
        decimal validDueAmount;
        string validResponsible;
        string validCommissioner;
        PaymentType validType;
        string validSale;
        int validBooking;

        [TestInitialize]
        public void Initialize()
        {
            dataAccessFacade = new DataAccessFacadeStub();
            paymentCollection = new PaymentCollection(dataAccessFacade);
            validDueDate = new DateTime(2010, 10, 10);
            validDueAmount = 1m;
            validResponsible = "Lonely Tree";
            validCommissioner = "Galasam";
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

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacade);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2, p3 };
            List<Payment> actualPayments = paymentCollection.ReadAll();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
            }
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

            Payment p2 = createValidPayment();
            p2.Note = "Archived2";
            p2.Archived = true;

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacade);

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

            Payment p1 = createValidPayment();
            p1.Note = "Moved to Lonely Tree1";
            p1.Commissioner = "Lonely Tree";

            Payment p2 = createValidPayment();
            p2.Note = "Moved to Lonely Tree2";
            p2.Commissioner = "Lonely Tree";

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacade);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2 };
            List<Payment> actualPayments = paymentCollection.ReadAllIncoming();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
            }
        }

        [TestMethod]
        public void TestReadAllOutgoing()
        {
            Payment p1 = createValidPayment();
            p1.Note = "Outgoing1";
            Payment p2 = createValidPayment();
            p1.Note = "Outgoing2";
            Payment p3 = createValidPayment();
            p1.Note = "Outgoing3";

            PaymentCollection paymentCollection = new PaymentCollection(dataAccessFacade);

            List<Payment> expectedPayments = new List<Payment>() { p1, p2, p3 };
            List<Payment> actualPayments = paymentCollection.ReadAllOutgoing();

            for (int i = 0; i < expectedPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Note, actualPayments[i].Note);
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

            for (int i = 0; i < expectedArchivedPayments.Count; i++)
            {
                Assert.AreEqual(expectedArchivedPayments[i].Note, actualArchivedPayments[i].Note);
            }

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

            for (int i = 0; i < expectedOutgoingPayments.Count; i++)
            {
                Assert.AreEqual(expectedOutgoingPayments[i].Note, actualOutgoingPayments[i].Note);
            }

            List<Payment> expectedIncomingPayments = new List<Payment>();
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        [TestMethod]
        public void TestUpdateOutgoingToIncoming()
        {
            Payment p1 = createValidPayment();

            p1.Commissioner = "Lonely Tree";
            p1.Responsible = "Galasam";
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

            for (int i = 0; i < expectedOutgoingPayments.Count; i++)
            {
                Assert.AreEqual(expectedIncomingPayments[i].Note, actualIncomingPayments[i].Note);
            }
        }

        [TestMethod]
        public void TestUpdateIncomingToOutgoing()
        {
            Payment p1 = createValidPayment();

            p1.Commissioner = "Lonely Tree";
            p1.Responsible = "Galasam";
            p1.Note = "Moved from outgoing to incoming";
            paymentCollection.Update(p1);

            p1.Commissioner = "Galasam";
            p1.Responsible = "Lonely Tree";
            p1.Note = "Moved from incoming to outgoing";
            paymentCollection.Update(p1);

            List<Payment> expectedArchivedPayments = new List<Payment>();
            List<Payment> actualArchivedPayments = paymentCollection.ReadAllArchived();

            CollectionAssert.AreEqual(expectedArchivedPayments, actualArchivedPayments);

            List<Payment> expectedOutgoingPayments = new List<Payment>() { p1 };
            List<Payment> actualOutgoingPayments = paymentCollection.ReadAllOutgoing();

            for (int i = 0; i < expectedOutgoingPayments.Count; i++)
            {
                Assert.AreEqual(expectedOutgoingPayments[i].Note, actualOutgoingPayments[i].Note);
            }

            List<Payment> expectedIncomingPayments = new List<Payment>();
            List<Payment> actualIncomingPayments = paymentCollection.ReadAllIncoming();

            CollectionAssert.AreEqual(expectedIncomingPayments, actualIncomingPayments);
        }

        private Payment createValidPayment()
        {
            return paymentCollection.Create(validDueDate, validDueAmount, validResponsible, validCommissioner, validType,
                validSale, validBooking);
        }
    }
}
