using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using System.Collections.Generic;

namespace Domain.Controller
{
    public class SupplierController
    {
       #region Public Methods
        public SupplierController()
        {
            dataAccessFacade = DataAccessFacade.GetInstance();
            supplierCollection = new SupplierCollection(dataAccessFacade);
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            List<ISupplier> suppliers = new List<ISupplier>();
            foreach (Supplier supplier in supplierCollection.ReadAll())
            {
                suppliers.Add(supplier);
            }

            return suppliers;
        }

        public ISupplier CreateSupplier(string name, string note, 
            SupplierType type)
        {
            return supplierCollection.Create(name, note, type);
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            supplierCollection.Update((Supplier) supplier);
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            supplierCollection.Delete((Supplier) supplier);
        }
       #endregion

       #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private SupplierCollection supplierCollection;

        #endregion
    }
}
