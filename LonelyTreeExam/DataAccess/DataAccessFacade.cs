﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess.Entities;
using DataAccess.Mappers;

namespace DataAccess
{
    public class DataAccessFacade : IDataAccessFacade
    {
        public DataAccessFacade()
        {
            connectionString = File.ReadAllText("C:\\ConnectString.txt");
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

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner)
        {
            return paymentMapper.Create(dueDate, dueAmount, responsible, commissioner);
        }
      
        public void UpdatePayment(IPayment payment)
        {
            paymentMapper.UpdatePayment((PaymentEntity) payment);
        }

        public void DeletePayment(IPayment payment)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Private Properties
        private string connectionString;
        private PaymentMapper paymentMapper;
        #endregion

      
    }
}