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
    internal class Supplier : Party, ISupplier
    {
        #region Public Properties/Methods
        public SupplierType Type
        {
            get { return _supplierEntity.Type; }
            set { _supplierEntity.Type = value; }
        }

        public string AccountNo
        {
            get { return _supplierEntity.AccountNo; }
            set { _supplierEntity.AccountNo = value; }
        }

        public AccountType AccountType
        {
            get { return _supplierEntity.AccountType; }
            set { _supplierEntity.AccountType = value; }
        }

        public string AccountName
        {
            get { return _supplierEntity.AccountName; }
            set { _supplierEntity.AccountName = value; }
        }

        public string OwnerId
        {
            get { return _supplierEntity.OwnerId; }
            set { _supplierEntity.OwnerId = value; }
        }

        public string Bank
        {
            get { return _supplierEntity.Bank; }
            set { _supplierEntity.Bank = value; }
        }

        #endregion

        #region Internal Methods
        internal Supplier(string name, string note, SupplierType type, IDataAccessFacade dataAccessFacade)
        {
            validateName(name);

            this.dataAccessFacade = dataAccessFacade;
            _supplierEntity = dataAccessFacade.CreateSupplier(name, note, type);
            initializeParty(_supplierEntity);
        }

        internal Supplier(IDataAccessFacade dataAccessFacade, ISupplier supplierEntity)
        {
            this.dataAccessFacade = dataAccessFacade;
            _supplierEntity = supplierEntity;
            initializeParty(_supplierEntity);
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
