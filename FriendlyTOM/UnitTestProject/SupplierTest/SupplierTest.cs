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

            Supplier supplier = new Supplier(entity, stub);

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
