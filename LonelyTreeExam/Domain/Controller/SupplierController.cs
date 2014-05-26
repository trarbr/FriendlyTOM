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

        public ISupplier CreateSupplier(string name, string note,
            SupplierType type)
        {
            //Calls the suppliercollection class for create.
            return supplierCollection.Create(name, note, type);
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            //Calls the suppliercollection class for readall 
            List<ISupplier> suppliers = new List<ISupplier>();
            foreach (Supplier supplier in supplierCollection.ReadAll())
            {
                suppliers.Add(supplier);
            }

            return suppliers;
        }
        
        public void UpdateSupplier(ISupplier supplier)
        {
            //Calls the suppliercollection class for update
            supplierCollection.Update((Supplier) supplier);
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            //Calls the suppliercollection class for delete
            supplierCollection.Delete((Supplier) supplier);
        }
       #endregion

       #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private SupplierCollection supplierCollection;
        #endregion
    }
}
