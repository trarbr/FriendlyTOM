// LB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using DataAccess.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class CustomerEntityTest
    {
        [TestMethod]
        public void TestConstructorSetsAllProperties()
        {
            CustomerType type = CustomerType.Bureau;
            string note = "bla";
            string name = "Peter";

            CustomerEntity customerEntity = new CustomerEntity(type, note,
                name);
            Assert.AreEqual(customerEntity.Name, name);
            Assert.AreEqual(customerEntity.Note, note);
            Assert.AreEqual(type, customerEntity.Type);
        }
    }
}
