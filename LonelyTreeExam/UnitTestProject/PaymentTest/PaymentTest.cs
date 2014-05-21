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
        IDataAccessFacade dataAccessFacadeStub;
        DateTime validDueDate;
        decimal validDueAmount;
        IParty validResponsible;
        IParty validCommissioner;
        PaymentType validType;
        string validSale;
        int validBooking;

        [TestInitialize]
        public void SetValidData()
        {
            dataAccessFacadeStub = new DataAccessFacadeStub();
            validDueDate = new DateTime(2010, 10, 10);
            validDueAmount = 1m;
            validResponsible = new Customer(CustomerType.Bureau, "", "Lonely Tree", dataAccessFacadeStub);
            validCommissioner = new Supplier("Galasam", "", SupplierType.Cruise, dataAccessFacadeStub);
            validType = PaymentType.Balance;
            validSale = "VF Jan";
            validBooking = 2;
        }

        [TestMethod]
        public void TestPaymentConstructorValidData()
        {
            Payment validPayment = createValidPayment();

            Assert.AreEqual(validDueDate, validPayment.DueDate);
            Assert.AreEqual(validDueAmount, validPayment.DueAmount);
            Assert.AreEqual(validResponsible, validPayment.Responsible);
            Assert.AreEqual(validCommissioner, validPayment.Commissioner);
            Assert.AreEqual(validType, validPayment.Type);
            Assert.AreEqual(validSale, validPayment.Sale);
            Assert.AreEqual(validBooking, validPayment.Booking);
        }

        // Should EntityConstructor test for invalid data?
        [TestMethod]
        public void TestEntityConstructorValidData()
        {
            PaymentEntity entity = new PaymentEntity(validDueDate, validDueAmount, validResponsible, 
                validCommissioner, validType, validSale, validBooking);
            Payment payment = new Payment(entity, dataAccessFacadeStub);

            Assert.AreEqual(validDueDate, payment.DueDate);
            Assert.AreEqual(validDueAmount, payment.DueAmount);
            Assert.AreEqual(validResponsible, payment.Responsible);
            Assert.AreEqual(validCommissioner, payment.Commissioner);
            Assert.AreEqual(validType, payment.Type);
            Assert.AreEqual(validSale, payment.Sale);
            Assert.AreEqual(validBooking, payment.Booking);
        }

        [TestMethod]
        public void TestConstructorValidatesDueAmount()
        {
            decimal invalidDueAmount = -1m;
            bool callException = false;

            try
            {
                Payment payment = new Payment(validDueDate, invalidDueAmount, validResponsible, 
                    validCommissioner, validType, validSale, validBooking, dataAccessFacadeStub);
            }
            catch (ArgumentOutOfRangeException)
            {
                callException = true;
            }

            Assert.AreEqual(true, callException);
        }

        [TestMethod]
        public void TestConstructorValidatesResponsible()
        {
            // TODO: no validation to test here anymore
            /*
            string invalidResponsible = "";
            bool caughtException = false;

            try
            {
                Payment payment = new Payment(validDueDate, validDueAmount, invalidResponsible, validCommissioner,
                    validType, validSale, validBooking, dataAccessFacadeStub);
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
            */
        }

        [TestMethod]
        public void TestConstructorValidatesCommissioner()
        {
            // TODO: no validation to test here anymore
            /*
            string invalidCommissioner = "";
            bool caughtException = false;

            try
            {
                Payment payment = new Payment(validDueDate, validDueAmount, validResponsible, invalidCommissioner,
                    validType, validSale, validBooking, dataAccessFacadeStub);
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
            */
        }

        [TestMethod]
        public void TestConstructorValidatesSale()
        {
            string invalidSale = "";
            bool caughtException = false;

            try
            {
                Payment payment = new Payment(validDueDate, validDueAmount, validResponsible, validCommissioner,
                    validType, invalidSale, validBooking, dataAccessFacadeStub);
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }

        //test hvor decimal er større mindre og lig med nul
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

        [TestMethod]
        public void TestReadAllPayments()
        {
            ISupplier commissioner1 = new Supplier("", "Galasam", SupplierType.Cruise, dataAccessFacadeStub);
            ISupplier commissioner2 = new Supplier("", "Hansen Is", SupplierType.Cruise, dataAccessFacadeStub);
            ISupplier commissioner3 = new Supplier("", "Bondegaarden", SupplierType.Cruise, dataAccessFacadeStub);
            ISupplier commissioner4 = new Supplier("", "Timoto", SupplierType.Cruise, dataAccessFacadeStub);
            Payment payment1 = createValidPayment();
            payment1.Commissioner = commissioner1;
            Payment payment2 = createValidPayment();
            payment2.Commissioner = commissioner2;
            Payment payment3 = createValidPayment();
            payment3.Commissioner = commissioner3;
            Payment payment4 = createValidPayment();
            payment4.Commissioner = commissioner4;

            List<Payment> actualPayments = Payment.ReadAll(dataAccessFacadeStub);

            List<Payment> expectedPayments = new List<Payment>();

            expectedPayments.Add(payment1);
            expectedPayments.Add(payment2);
            expectedPayments.Add(payment3);
            expectedPayments.Add(payment4);

            for (int i = 0; i < actualPayments.Count; i++)
            {
                Assert.AreEqual(expectedPayments[i].Commissioner, actualPayments[i].Commissioner);
            }
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
        public void TestDeleteOneAttachment()
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

            payment.DeleteAttachment(@"C:\ConnectString.txt");
            expectedAttachments.Remove(@"C:\ConnectString.txt");

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
                payment.AddAttachment("nonexcistingfile");
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

        //[TestMethod]
        //public void TestInvoiceNull()
        //{
        //    // Since this is a test related to DB, maybe it should be in PaymentEntity?
        //    // Wow, maybe we need validators in DataAccess as well! (mindblown)
        //    //both Sale and Invoice.
        //    Payment payment = createValidPayment();
        //    bool caughtException = false;

        //    try
        //    {
        //        payment.Invoice = null;
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        caughtException = true;
        //    }

        //    Assert.AreEqual(true, caughtException);
        //}
        
        [TestMethod]
        public void TestInvoiceValidData()
        {
            Payment payment = createValidPayment();

            payment.Invoice = "factura 685467584";

            string expectedInvoice = "factura 685467584";

            Assert.AreEqual(expectedInvoice, payment.Invoice);
        }

        private Payment createValidPayment()
        {
            Payment payment = new Payment(validDueDate, validDueAmount, validResponsible, validCommissioner, validType, 
                validSale, validBooking, dataAccessFacadeStub);

            return payment;
        }
    }
}
