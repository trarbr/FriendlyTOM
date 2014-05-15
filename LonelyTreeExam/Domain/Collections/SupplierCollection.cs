using Common.Enums;
using DataAccess;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Collections
{
    internal class SupplierCollection
    {
        #region Internal Constructors/Methods
        internal SupplierCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;

            ReadAll();
        }

        internal List<Supplier> ReadAll()
        {
            if (suppliers == null)
            {
                suppliers = Supplier.ReadAll(dataAccessFacade);
            }

            return suppliers;
        }

        internal Supplier Create(string name, string note, string paymentInfo, SupplierType type)
        {
            Supplier supplier = new Supplier(name, note, paymentInfo, type, dataAccessFacade);
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
