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

namespace DataAccess
{
    public class DataAccessFacade
    {
        public DataAccessFacade()
        {
            connectionString = File.ReadAllText("C:\\ConnectString.txt");
            paymentMapper = new PaymentMapper(connectionString);
        }
        
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

        private string connectionString;
        private PaymentMapper paymentMapper;
    }
}
