using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Model;
using Common.Interfaces;
using DataAccess.Entities;
using Common.Enums;

namespace UnitTestProject
{
    [TestClass]
    public class PaymentTest
    {
        //lav ny Payment med parametre hvor al data er iorden.
        //test hvor decimal er større mindre og lig med nul
        [TestMethod]
        public void TestConstructorValidData()
        {
            Payment validPayment = createValidPayment();

            DateTime expectedDueDate = new DateTime(2010, 10, 10);
            decimal expectedDueAmount = 1000m;
            string expectedCommisioner = "Galasam";
            string expectedResponsible = "Lonely Tree";
            PaymentType expectedType = PaymentType.Balance;
            string expectedSale = "VF Jan";
            int expectedBooking = 2;

            Assert.AreEqual(expectedCommisioner, validPayment.Commissioner);
            Assert.AreEqual(expectedDueAmount, validPayment.DueAmount);
            Assert.AreEqual(expectedDueDate, validPayment.DueDate);
            Assert.AreEqual(expectedResponsible, validPayment.Responsible);
            Assert.AreEqual(expectedType, validPayment.Type);
            Assert.AreEqual(expectedSale, validPayment.Sale);
            Assert.AreEqual(expectedBooking, validPayment.Booking);
        }

        [TestMethod]
        public void TestEntityConstructorValidData()
        {
            DateTime expectedDueDate = new DateTime(2010, 10, 10);
            decimal expectedDueAmount = 1000m;
            string expectedCommissioner = "Galasam";
            string expectedResponsible = "Lonely Tree";
            PaymentType expectedType = PaymentType.Balance;
            string expectedSale = "VF Jan";
            int expectedBooking = 2;
            IDataAccessFacade dataAccessFacadeStub = new DataAccessFacadeStub();

            PaymentEntity entity = new PaymentEntity(expectedDueDate, expectedDueAmount, expectedResponsible, 
                expectedCommissioner, expectedType, expectedSale, expectedBooking);
            Payment payment = new Payment(entity, dataAccessFacadeStub);

            Assert.AreEqual(expectedDueDate, payment.DueDate);
            Assert.AreEqual(expectedDueAmount, payment.DueAmount);
            Assert.AreEqual(expectedCommissioner, payment.Commissioner);
            Assert.AreEqual(expectedResponsible, payment.Responsible);
            Assert.AreEqual(expectedType, payment.Type);
            Assert.AreEqual(expectedSale, payment.Sale);
            Assert.AreEqual(expectedBooking, payment.Booking);
        }

        [TestMethod]
        public void TestConstructorValidatesDueAmount()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = -1m;
            string commissioner = "Galasam";
            string responsible = "Lonely Tree";
            PaymentType type = PaymentType.Balance;
            string sale = "VF Jan";
            int booking = 2;

            IDataAccessFacade dataAccessFacadeStub = new DataAccessFacadeStub();

            bool callException = false;

            try
            {
                Payment paymentLessThanZero = new Payment(dueDate, dueAmount, responsible, commissioner, type, sale, 
                    booking, dataAccessFacadeStub);
            }
            catch (ArgumentOutOfRangeException)
            {
                callException = true;
            }

            Assert.AreEqual(true, callException);
        }

        [TestMethod]
        public void TestConstructorValidatesCommissioner()
        {
            Assert.Fail("Write me!");
        }

        [TestMethod]
        public void TestConstructorValidatesResponsible()
        {
            Assert.Fail("Write me!");
        }

        [TestMethod]
        public void TestConstructorValidatesSale()
        {
            Assert.Fail("Write me!");
        }

        [TestMethod]
        public void TestPaidAmountLessThanZero()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.PaidAmount = -1m;
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestPaidAmountEqualZero()
        {
            Payment payment = createValidPayment();
            decimal expectedPaidAmount = 0m;
            payment.PaidAmount = expectedPaidAmount;

            Assert.AreEqual(expectedPaidAmount, payment.PaidAmount);
        }

        [TestMethod]
        public void TestPaidAmountMoreThanZero()
        {
            Payment payment = createValidPayment();
            decimal expectedPaidAmount = 1m;
            payment.PaidAmount = expectedPaidAmount;

            Assert.AreEqual(expectedPaidAmount, payment.PaidAmount);
        }

        [TestMethod]
        public void TestDueAmountLessThanZero()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.DueAmount = -1m;
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestDueAmountEqualZero()
        {
            Payment payment = createValidPayment();
            decimal expectedDueAmount = 0m;
            payment.DueAmount = expectedDueAmount;

            Assert.AreEqual(expectedDueAmount, payment.DueAmount);
        }

        [TestMethod]
        public void TestDueAmountMoreThanZero()
        {
            Payment payment = createValidPayment();
            decimal expectedDueAmount = 1m;
            payment.DueAmount = expectedDueAmount;

            Assert.AreEqual(expectedDueAmount, payment.DueAmount);
        }

        [TestMethod]
        public void TestSaleValidString()
        {
            Payment payment = createValidPayment();

            string expectedSale = "VF Chris";
            payment.Sale = expectedSale;

            Assert.AreEqual(expectedSale, payment.Sale);
        }

        [TestMethod]
        public void TestSaleNull()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.Sale = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestSaleEmptyString()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.Sale = "";
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestSaleWhitespace()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.Sale = " ";
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        //test attachments exceptions
        [TestMethod]
        public void TestAttachmentOneValidString()
        {
            Payment payment = createValidPayment();
            string expectedAttachment = @"C:\ConnectString.txt";        

            payment.AddAttachment(expectedAttachment);

            string actualAttachment = payment.Attachments[0];

            Assert.AreEqual(expectedAttachment, actualAttachment);
        }

        [TestMethod]
        public void TestAttachmentManyValidStrings()
        {
            Payment payment = createValidPayment();

            List<string> expectedAttachments = new List<string>();
            expectedAttachments.Add(@"C:\ConnectString.txt");
            expectedAttachments.Add(@"C:\Windows\notepad.exe");
            expectedAttachments.Add(@"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe");

            foreach (string attachment in expectedAttachments)
            {
                payment.AddAttachment(attachment);
            }

            List<string> actualAttachments = new List<string>();

            foreach (string attachment in payment.Attachments)
            {
                actualAttachments.Add(attachment);
            }

            CollectionAssert.AreEqual(expectedAttachments, actualAttachments);
        }

        [TestMethod]
        public void TestAttachmentNonexistingFile()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.AddAttachment("asdfasdfagdfgdfg");
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestAttachmentNull()
        {
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.AddAttachment(null);
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestInvoiceNull()
        {
            // Since this is a test related to DB, maybe it should be in PaymentEntity?
            // Wow, maybe we need validators in DataAccess as well! (mindblown)
            Payment payment = createValidPayment();
            bool caughtException = false;

            try
            {
                payment.Invoice = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        private Payment createValidPayment()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 1000m;
            string commissioner = "Galasam";
            string responsible = "Lonely Tree";
            PaymentType type = PaymentType.Balance;
            string sale = "VF Jan";
            int booking = 2;

            IDataAccessFacade dataAccessFacadeStub = new DataAccessFacadeStub();

            Payment payment = new Payment(dueDate, dueAmount, responsible, commissioner, type, sale, booking, 
                dataAccessFacadeStub);

            return payment;
        }
    }
}
