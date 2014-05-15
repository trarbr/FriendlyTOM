using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess.Entities;
using DataAccess.Mappers;
using Common.Enums;

namespace DataAccess
{
    public class DataAccessFacade : IDataAccessFacade
    {
        /// <summary>
        /// Initializes a DataAccessFacade for accessing a MS SQL database
        /// </summary>
        /// <param name="test">For integration tests, set test = true to use test database</param>
        public DataAccessFacade(bool test = false)
        {
            if (!test)
            {
                connectionString = File.ReadAllText("C:\\ConnectString.txt");
            }
            else
            {
                connectionString = 
                    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=LTTEST;Integrated Security=True";
            }

            paymentMapper = new PaymentMapper(connectionString);
            supplierMapper = new SupplierMapper(connectionString);
        }

        #region Public Payment Methods
        /// <summary>
        /// Should initiate a connection and a readall procedure from the database.
        /// </summary>
        /// <returns>payments</returns>
        public List<IPayment> ReadAllPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            List<PaymentEntity> paymentEntities = paymentMapper.ReadAll();
            foreach (PaymentEntity paymentEntity in paymentEntities)
            {
                payments.Add(paymentEntity);
            }

            return payments;
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible,
            string commissioner, PaymentType type, string sale, int booking)
        {
            return paymentMapper.Create(dueDate, dueAmount, responsible, commissioner, type, sale, booking);
        }
      
        public void UpdatePayment(IPayment payment)
        {
            paymentMapper.Update((PaymentEntity) payment);
        }

        public void DeletePayment(IPayment payment)
        {
            paymentMapper.Delete((PaymentEntity) payment);
        }
        #endregion

        #region Private Properties
        private string connectionString;
        private PaymentMapper paymentMapper;
        private SupplierMapper supplierMapper;
        #endregion

        #region SupplierMethods
        public ISupplier CreateSupplier(string name, string note, string paymentInfo, SupplierType type)
        {
            return supplierMapper.Create(name, note, paymentInfo, type);
        }

        public List<ISupplier> ReadAllSuppliers()
        {
            List<ISupplier> suppliers = new List<ISupplier>();
            List<SupplierEntity> supplierEntities = supplierMapper.ReadAll();
            foreach (SupplierEntity supplierEntity in supplierEntities)
            {
                suppliers.Add(supplierEntity);
            }

            return suppliers;
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            supplierMapper.Update((SupplierEntity)supplier);
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            SupplierEntity sup = supplier as SupplierEntity;
            supplierMapper.Delete(sup);
        }
        #endregion

        #region Customer Methods
        private List<ICustomer> customers;
        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            CustomerEntity entity = new CustomerEntity(type, note, name);
            customers.Add(entity);
            return entity;
        }

        public List<ICustomer> ReadAllCustomers()
        {
            return customers;
        }

        public void UpdateCustomers(ICustomer customer)
        {
            CustomerEntity entity = (CustomerEntity)customer;
            entity.LastModified = DateTime.Now;
        }

        public void DeleteCustomer(ICustomer customer)
        {
            CustomerEntity entity = (CustomerEntity)customer;
            entity.Deleted = true;
        }
        #endregion
    }
}
