using Common.Enums;
using DataAccess;
using DataAccess.Entities;
using Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestClass]
    public class SupplierTest
    {
        [TestMethod]
        public void TestSupplierConstructorValidData()
        {
            string name = "gert";
            string note = "total fed note";
            SupplierType type = SupplierType.Cruise;

            DataAccessFacadeStub stub = new DataAccessFacadeStub();
            SupplierEntity entity = new SupplierEntity(type, note, name);

            Supplier supplier = new Supplier(stub, entity);

            string expectedName = "gert";
            string expectedNote = "total fed note";
            SupplierType expectedType = SupplierType.Cruise;

            Assert.AreEqual(expectedNote, supplier.Note);
            Assert.AreEqual(expectedName, supplier.Name);
            Assert.AreEqual(expectedType, supplier.Type);
        }

        [TestMethod]
        public void TestNameEmptyString()
        {
            string name = "";
            string note = "total fed note";
            SupplierType type = SupplierType.Cruise;

            DataAccessFacadeStub stub = new DataAccessFacadeStub();

            bool caughtException = false;

            try
            {
                Supplier supplier = new Supplier(name, note, type, stub);

            }
            catch (ArgumentOutOfRangeException)
            {
                caughtException = true;
            }

            Assert.AreEqual(true, caughtException);
        }
    }
}
