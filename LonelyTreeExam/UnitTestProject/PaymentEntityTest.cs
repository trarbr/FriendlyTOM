using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataAccess.Entities;
using System.Collections.Generic;

namespace UnitTestProject
{
    [TestClass]
    public class PaymentEntityTest
    {
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

        public void TestAddAttachment()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 100m;
            string commissioner = "Henry";
            string responsible = "Peter";

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, responsible, 
                commissioner);

            List<string> expectedAttachments = new List<string>();
            expectedAttachments.Add("attachment1");
            expectedAttachments.Add("attachment2");
            expectedAttachments.Add("attachment3");

            paymentEntity.AddAttachment("attachment1");
            paymentEntity.AddAttachment("attachment2");
            paymentEntity.AddAttachment("attachment3");

            List<string> actualAttachments = new List<string>();

            foreach (string attachment in paymentEntity.Attachments)
            {
                actualAttachments.Add(attachment);
            }

            CollectionAssert.AreEqual(expectedAttachments, actualAttachments);
        }
    }
}
