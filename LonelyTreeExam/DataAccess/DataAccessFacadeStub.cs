using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Common.Enums;

namespace DataAccess
{
    public class DataAccessFacadeStub : IDataAccessFacade
    {
        List<IPayment> payments;
        #region Payment Stuff

        public DataAccessFacadeStub()
        {
            payments = new List<IPayment>();
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner, 
            PaymentType type, string sale, int booking)
        {
            PaymentEntity entity = new PaymentEntity(dueDate, dueAmount, responsible, commissioner, type, sale, booking);
            payments.Add(entity);

            return entity;
        }

        public List<IPayment> ReadAllPayments()
        {
            return payments;
        }

        public void UpdatePayment(IPayment payment)
        {
            PaymentEntity entity = (PaymentEntity)payment;

            entity.LastModified = DateTime.Now;
        }

        public void DeletePayment(IPayment payment)
        {
            PaymentEntity entity = (PaymentEntity)payment;

            entity.Deleted = true;
        }
        #endregion

        #region Supplier Stuff
        List<ISupplier> supplierList = new List<ISupplier>();

        public ISupplier CreateSupplier(string paymentinfo, string name, string note, SupplierType type)
        {
            SupplierEntity entity = new SupplierEntity(paymentinfo, type, note, name);
            supplierList.Add(entity);

            return entity;
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            return supplierList;
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            SupplierEntity entity = (SupplierEntity) supplier;
            entity.LastModified = DateTime.Now;
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            SupplierEntity entity = (SupplierEntity) supplier;
            entity.Deleted = true;
        }
        #endregion

        #region Customer Stuff
        public ICustomer CreateCustomer()
        {
            throw new NotImplementedException();
        }

        public List<ICustomer> ReadAllCustomers()
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomers(ICustomer customer)
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer(ICustomer customer)
        {
            throw new NotImplementedException();
        }
        #endregion
        
    }
}
