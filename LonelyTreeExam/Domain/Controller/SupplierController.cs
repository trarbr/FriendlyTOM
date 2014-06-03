// MM
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

        /// <summary>
        /// For testing against a specified DataAccessFacade
        /// </summary>
        /// <param name="dataAccessFacade"></param>
        public SupplierController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
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

        public void AddPaymentRule(ISupplier supplier, ICustomer customer, BookingType bookingType, decimal percentage,
            int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            Supplier supp = (Supplier)supplier;
            Customer cust = (Customer)customer;

            supp.AddPaymentRule(cust, bookingType, percentage, daysOffset, baseDate, paymentType);
        }

        public void DeletePaymentRule(IPaymentRule paymentRule)
        {
            PaymentRule payRule = (PaymentRule)paymentRule;
            Supplier supp = (Supplier)payRule.Supplier;

            supp.DeletePaymentRule(payRule);
        }
        #endregion

       #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private SupplierCollection supplierCollection;
        #endregion
    }
}
