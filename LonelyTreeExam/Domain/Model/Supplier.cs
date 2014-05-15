using Common.Enums;
using Common.Interfaces;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    internal class Supplier : AParty, ISupplier
    {
        #region Public Properties/Methods
        public string PaymentInfo
        {
            get { return _supplierEntity.PaymentInfo; }
            set { _supplierEntity.PaymentInfo = value; }
        }
        public SupplierType Type
        {
            get { return _supplierEntity.Type; }
            set { _supplierEntity.Type = value; }
        }
        #endregion

        #region Internal Methods
        internal Supplier(string name, string note, string paymentInfo, SupplierType type, IDataAccessFacade dataAccessFacade)
        {
            validateName(name);

            this.dataAccessFacade = dataAccessFacade;

            _supplierEntity = dataAccessFacade.CreateSupplier(name, note, paymentInfo, type);
            this._partyEntity = _supplierEntity;
        }

        internal Supplier(IDataAccessFacade dataAccessFacade, ISupplier supplierEntity)
        {
            _supplierEntity = supplierEntity;
            this.dataAccessFacade = dataAccessFacade;
            this._partyEntity = _supplierEntity;
        }

        internal void Update()
        {
            dataAccessFacade.UpdateSupplier(_supplierEntity);
        }

        internal void Delete()
        {
            dataAccessFacade.DeleteSupplier(_supplierEntity);
        }

        internal static List<Supplier> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            List<ISupplier> supplierEntities = dataAccessFacade.ReadAllSuppliers();
            List<Supplier> suppliers = new List<Supplier>();

            foreach (ISupplier supplierEntity in supplierEntities)
            {
                Supplier supplier = new Supplier(dataAccessFacade, supplierEntity);
                suppliers.Add(supplier);
            }
            return suppliers;
        }
        #endregion

        private IDataAccessFacade dataAccessFacade;
        private ISupplier _supplierEntity;
    }
}
