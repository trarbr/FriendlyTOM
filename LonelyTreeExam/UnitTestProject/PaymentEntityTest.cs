using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Entities;

namespace UnitTestProject
{
    [TestClass]
    public class PaymentEntityTest
    {
        // test efter constructor ingen værdier null
        [TestMethod]
        public void TestConstructorSetsAllProperties()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 100m;
            string commissioner = "Henry";
            string responsible = "Peter";

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, responsible, 
                commissioner);

            DateTime expectedPaidDate = new DateTime(1900, 01, 01);
            decimal expectedPaidAmount = 0m;
            bool expectedPaid = false;
            bool expectedArchived = false;
            int expectedAttachmentsCount = 0;

            Assert.AreEqual(dueDate, paymentEntity.DueDate);
            Assert.AreEqual(dueAmount, paymentEntity.DueAmount);
            Assert.AreEqual(commissioner, paymentEntity.Commissioner);
            Assert.AreEqual(responsible, paymentEntity.Responsible);
            Assert.AreEqual(expectedPaidDate, paymentEntity.PaidDate);
            Assert.AreEqual(expectedPaidAmount, paymentEntity.PaidAmount);
            Assert.AreEqual(expectedPaid, paymentEntity.Paid);
            Assert.AreEqual(expectedArchived, paymentEntity.Archived);
            Assert.AreEqual(expectedAttachmentsCount, paymentEntity.Attachments.Count);
            
        }

        // test add attachment
    }
}
