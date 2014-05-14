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
        List<IPayment> payments = new List<IPayment>();

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner, PaymentType type, string sale, int booking)
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
            PaymentEntity entity = (PaymentEntity) payment;

            entity.Deleted = true;
        }

        public ISupplier CreateSupplier()
        {
            throw new NotImplementedException();
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            throw new NotImplementedException();
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            throw new NotImplementedException();
        }

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
    }
}
