using Common.Enums;
using DataAccess;
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
            string paymentinfo = "dankort super vigtig data";
            SupplierType type = SupplierType.Cruise;

            DataAccessFacadeStub stub = new DataAccessFacadeStub();

            Supplier supplier = new Supplier(name, note, paymentinfo, type, stub);

            string expectedName = "gert";
            string expectedNote = "total fed note";
            string expectedPaymentInfo = "dankort super vigtig data";
            SupplierType expectedType = SupplierType.Cruise;

            Assert.AreEqual(expectedPaymentInfo, supplier.PaymentInfo);
            Assert.AreEqual(expectedNote, supplier.Note);
            Assert.AreEqual(expectedName, supplier.Name);
            Assert.AreEqual(expectedType, supplier.Type);
        }

        [TestMethod]
        public void TestNameAsNull()
        {
            string name = "";
            string note = "total fed note";
            string paymentinfo = "dankort super vigtig data";
            SupplierType type = SupplierType.Cruise;

            DataAccessFacadeStub stub = new DataAccessFacadeStub();

            bool callException = false;

            try
            {
                Supplier supplier = new Supplier(name, note, paymentinfo, type, stub);

            }
            catch (ArgumentOutOfRangeException)
            {
                callException = true;
            }

            Assert.AreEqual(false, callException);
        }
    }
}
