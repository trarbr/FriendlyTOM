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
using DataAccess.Entities;
using System.Collections.Generic;
using Common.Enums;

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
            APartyEntity payee = new SupplierEntity(SupplierType.Cruise, "", "Galasam");
            APartyEntity payer = new CustomerEntity(CustomerType.Bureau, "", "Lonely Tree");
            PaymentType type = PaymentType.Full;
            string sale = "VF March";
            int booking = 128;

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, payer, 
                payee, type, sale, booking);

            DateTime expectedPaidDate = new DateTime(1900, 01, 01);
            decimal expectedPaidAmount = 0m;
            bool expectedPaid = false;
            bool expectedArchived = false;
            int expectedAttachmentsCount = 0;

            Assert.AreEqual(dueDate, paymentEntity.DueDate);
            Assert.AreEqual(dueAmount, paymentEntity.DueAmount);
            Assert.AreEqual(payee, paymentEntity.Payee);
            Assert.AreEqual(payer, paymentEntity.Payer);
            Assert.AreEqual(expectedPaidDate, paymentEntity.PaidDate);
            Assert.AreEqual(expectedPaidAmount, paymentEntity.PaidAmount);
            Assert.AreEqual(expectedPaid, paymentEntity.Paid);
            Assert.AreEqual(expectedArchived, paymentEntity.Archived);
            Assert.AreEqual(expectedAttachmentsCount, paymentEntity.Attachments.Count);
        }

        // TODO: Test DeleteAttachment

        [TestMethod]
        public void TestAddAttachment()
        {
            DateTime dueDate = new DateTime(2010, 10, 10);
            decimal dueAmount = 100m;
            APartyEntity payee = new SupplierEntity(SupplierType.Cruise, "", "Galasam");
            APartyEntity payer = new CustomerEntity(CustomerType.Bureau, "", "Lonely Tree");
            PaymentType type = PaymentType.Full;
            string sale = "SR Josef";
            int booking = 59;

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, payer, 
                payee, type, sale, booking);

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
