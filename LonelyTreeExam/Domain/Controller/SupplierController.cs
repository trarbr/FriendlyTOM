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

        public void AddPaymentRule(ISupplier supplier, ICustomer customer, BookingType bookingType, decimal percentage,
            int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            Supplier s = (Supplier)supplier;
            Customer c = (Customer)customer;

            s.AddPaymentRule(c, bookingType, percentage, daysOffset, baseDate, paymentType);
        }

        public void DeletePaymentRule(IPaymentRule paymentRule)
        {
            PaymentRule p = (PaymentRule)paymentRule;
            Supplier s = (Supplier)p.Supplier;

            s.DeletePaymentRule(p);
        }
        #endregion

       #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private SupplierCollection supplierCollection;
        #endregion
    }
}
