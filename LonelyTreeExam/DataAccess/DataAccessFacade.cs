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
        }

        #region Public Methods
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
            PaymentEntity pay = payment as PaymentEntity;
            paymentMapper.Delete(pay);
        }
        #endregion

        #region Private Properties
        private string connectionString;
        private PaymentMapper paymentMapper;
        #endregion




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
