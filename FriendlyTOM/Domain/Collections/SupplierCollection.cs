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
using Domain.Model;
using System.Collections.Generic;

namespace Domain.Collections
{
    internal class SupplierCollection
    {
        #region Internal Constructor
        internal SupplierCollection(IDataAccessFacade dataAccessFacade)
        {
            //uses a new instance of dataAccessFacade and calls readAll.
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }
        #endregion

        #region Internal CRUD
        internal List<Supplier> ReadAll()
        {
            if (suppliers == null)
            {
                suppliers = Supplier.ReadAll(dataAccessFacade);
            }

            return suppliers;
        }

        internal Supplier Create(string name, string note, SupplierType type)
        {
            Supplier supplier = new Supplier(name, note, type, dataAccessFacade);
            suppliers.Add(supplier);

            return supplier;
        }

        internal void Update(Supplier supplier)
        {
            supplier.Update();
        }

        internal void Delete(Supplier supplier)
        {
            //Do not delete if supplier is Lonely Tree
            if (supplier.Name != "Lonely Tree")
            {
                supplier.Delete();
                suppliers.Remove(supplier);
            }
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Supplier> suppliers;
        #endregion
    }
}
