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

        internal void Update(Model.Supplier supplier)
        {
            supplier.Update();
        }

        internal void Delete(Model.Supplier supplier)
        {
            supplier.Delete();
            suppliers.Remove(supplier);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Supplier> suppliers;
        #endregion
    }
}
