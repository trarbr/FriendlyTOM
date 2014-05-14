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
            customerMapper = new CustomerMapper(connectionString);
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

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner)
        {
            return paymentMapper.Create(dueDate, dueAmount, responsible, commissioner);
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
        
        #region Public Supplier Methods
        public ISupplier CreateSupplier(string name, string note, string paymentInfo, SupplierType type)
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

        public ICustomer CreateCustomer(CustomerType type)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Public Customer Methods
        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            return customerMapper.Create(note, name);
        }

       public List<ICustomer> ReadAllCustomers()
        {
            List<ICustomer> customers = new List<ICustomer>();
           List<CustomerEntity> customerEntities = customerMapper.ReadAll();

           foreach (CustomerEntity customerEntity in customerEntities)
           {
               customers.Add(customerEntity);
           }
           return customers;
        }

        public void UpdateCustomers(ICustomer customer)
        {
           customerMapper.Update((CustomerEntity) customer);
        }

        public void DeleteCustomer(ICustomer customer)
        {
            customerMapper.Delete((CustomerEntity) customer);
        }
        #endregion

        #region Private Properties
        private string connectionString;
        private PaymentMapper paymentMapper;
        private CustomerMapper customerMapper;
        #endregion
    }
}
