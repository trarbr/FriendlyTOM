using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Model;
using Common.Interfaces;
using DataAccess.Entities;

namespace UnitTestProject
{
    [TestClass]
    public class PaymentTest
    {
        //lav ny Payment med parametre hvor al data er iorden.
        //test hvor decimal er større mindre og lig med nul

        public IPayment ValidPaymentInput()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 100m;
            string commissioner = "Henry";
            string responsible = "Peter";

            DataAccessFacadeStub stub = new DataAccessFacadeStub();

            Payment payment = new Payment(dueDate, dueAmount, responsible, commissioner, stub);

            return payment;
        }

        [TestMethod]
        public void TestConstructorValidData()
        {
            IPayment validPayment = ValidPaymentInput();

            DateTime expectedDueDate = new DateTime(2010, 10, 10);
            decimal expectedDueAmount = 100m;
            string expectedCommisioner = "Henry";
            string expectedResponsible = "Peter";

            Assert.AreEqual(expectedCommisioner, validPayment.Commissioner);
            Assert.AreEqual(expectedDueAmount, validPayment.DueAmount);
            Assert.AreEqual(expectedDueDate, validPayment.DueDate);
            Assert.AreEqual(expectedResponsible, validPayment.Responsible);
        }

        [TestMethod]
        public void TestDueAmountLessThanZero()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = -100m;
            string commissioner = "Henry";
            string responsible = "Peter";

            DataAccessFacadeStub stub = new DataAccessFacadeStub();

            bool callException = false;

            try
            {
                Payment paymentLessThanZero = new Payment(dueDate, dueAmount, responsible, commissioner, stub);
            }
            catch (ArgumentOutOfRangeException)
            {
                callException = true;
            }

            Assert.AreEqual(true, callException);
        }

        [TestMethod]
        public void TestPaidAmountLessThanZero()
        {
            IPayment payment = ValidPaymentInput();
            bool callException = false;

            try
            {
                payment.PaidAmount = -100m;
            }
            catch (ArgumentOutOfRangeException)
            {
                callException = true;
            }

            Assert.AreEqual(true, callException);
        }

        [TestMethod]
        public void TestEntityConstructorValidData()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 100m;
            string commissioner = "Henry";
            string responsible = "Peter";
            DataAccessFacadeStub stub = new DataAccessFacadeStub();
            PaymentEntity entity = new PaymentEntity(dueDate, dueAmount, responsible, commissioner);

            Payment payment = new Payment(entity, stub);

            DateTime expecteddueDate = new DateTime(2010, 10, 10);
            decimal expecteddueAmount = 100m;
            string expectedcommissioner = "Henry";
            string expectedresponsible = "Peter";
            PaymentEntity expectedPaymentEntity = new PaymentEntity(expecteddueDate, expecteddueAmount, expectedresponsible, expectedcommissioner);
            DataAccessFacadeStub expectedStub = new DataAccessFacadeStub();

            Payment expectedPayment = new Payment(expectedPaymentEntity,expectedStub);

            Assert.AreEqual(expectedPaymentEntity, entity);
            Assert.AreEqual(expectedStub, stub);
        }
    }
}
