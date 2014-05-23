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
