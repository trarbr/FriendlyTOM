using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Controller
{
    public class SupplierController
    {
        #region Public Methods
        public SupplierController()
        {
            dataAccessFacade = new DataAccessFacade();
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

        public ISupplier CreateSupplier(string name, string note, string paymentInfo, 
            SupplierType type)
        {
            return supplierCollection.Create(name, note, paymentInfo, type);
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
