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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using DataAccess;
using DataAccess.Entities;
using Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class CustomerTest
    {
        IDataAccessFacade dataAccessFacadeStub;
        string validName;
        string validNote;
        CustomerType validType;

        [TestInitialize]
        public void SetValidData()
        {
            dataAccessFacadeStub = new DataAccessFacadeStub();
            validName = "VF Jan";
            validNote = "8 Persons";
            validType = CustomerType.Bureau;
        }

        [TestMethod]
        public void TestConstructorValidData()
        {
            Customer validCustomer = createValidCustomer();

            Assert.AreEqual(validName, validCustomer.Name);
            Assert.AreEqual(validNote, validCustomer.Note);
            Assert.AreEqual(validType, validCustomer.Type);
        }

        [TestMethod]
        public void TestEntityConstructorValidData()
        {
            CustomerEntity entity = new CustomerEntity(validType, validNote,
                validName);
            Customer customer = new Customer(entity, dataAccessFacadeStub);

            Assert.AreEqual(validName, customer.Name);
            Assert.AreEqual(validNote, customer.Note);
            Assert.AreEqual(validType, customer.Type);
        }

        [TestMethod]
        public void TestNameValidString()
        {
            Customer customer = createValidCustomer();
            string expectedName = "Lonely Tree";
            customer.Name = expectedName;
            Assert.AreEqual(expectedName, customer.Name);
        }

        [TestMethod]
        public void TestNameEmptyString()
        {
            Customer customer = createValidCustomer();
            bool caughtException = false;
            try
            {
                customer.Name = "";
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }
            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestNameWhitespace()
        {
            Customer customer = createValidCustomer();
            bool caughtException = false;
            try
            {
                customer.Name = " ";
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }
            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestNameNull()
        {
            Customer customer = createValidCustomer();
            bool caughtException = false;
            try
            {
                customer.Name = null;
            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }
            Assert.AreEqual(true, caughtException);
        }

        [TestMethod]
        public void TestReadAllCustomers()
        {
            Customer customer1 = createValidCustomer();
            customer1.Name = "1";
            Customer customer2 = createValidCustomer();
            customer2.Name = "2";
            Customer customer3 = createValidCustomer();
            customer3.Name = "3";
            Customer customer4 = createValidCustomer();
            customer4.Name = "4";

            List<Customer> actualCustomers = Customer.ReadAll(dataAccessFacadeStub);
            List<Customer> expectedCustomers = new List<Customer>();
            expectedCustomers.Add(customer1);
            expectedCustomers.Add(customer2);
            expectedCustomers.Add(customer3);
            expectedCustomers.Add(customer4);

            for (int i = 0; i < actualCustomers.Count; i++)
            {
                Assert.AreEqual(expectedCustomers[i].Name, actualCustomers[i].Name);
            }
        }

        private Customer createValidCustomer()
        {
            Customer customer = new Customer(validType, validNote, validName, dataAccessFacadeStub);
            return customer;
        }
    }
}
