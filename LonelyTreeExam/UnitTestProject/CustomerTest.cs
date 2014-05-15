using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;
using DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public ICustomer ValidCustomerInput()
        {
            string name = "Christine";
            string note = "So sweet";
            CustomerType type = CustomerType.PrivateCustomer;
            CustomerEntity customerEntity = new CustomerEntity(type, name, note);

            Assert.AreEqual(name, customerEntity.Name);

            return ;
        }
    }
}
