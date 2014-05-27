using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DataAccess.Entities;
using DataAccess.Helpers;
using Common.Enums;
using Common.Interfaces;

namespace DataAccess.Mappers
{
    internal class PaymentMapper : ASQLMapper<PaymentEntity>
    {
        internal PartyMapper PartyMapper { get; set; }

        #region Internal Methods
        internal PaymentMapper(string connectionString)
        {
            this.connectionString = connectionString;
            //Creates new instance of the entity dictionary with payments.
            this.entityMap = new Dictionary<int, PaymentEntity>();
        }

        internal PaymentEntity Create(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking)
        {
            //Creates a new payment with different parameters.
            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, payer, payee,
                type, sale, booking);
            insert(paymentEntity);

            return paymentEntity;
        }

        internal List<PaymentEntity> ReadAll()
        {
            //Read all payments in the database
            List<PaymentEntity> payments = selectAll();
            return payments;
        }

        internal void Update(PaymentEntity payment)
        {
            //calls update method to update a single payment in the database
            update(payment);
        }

        internal void Delete(PaymentEntity payment)
        {
            //Sets it to be deletede then calls the update, so its being removed from the database. 
            payment.Deleted = true;
            Update(payment);
        }
        #endregion

        #region Protected Methods
        protected override string insertProcedureName
        {
            get { return StoredProcedures.CREATE_PAYMENT; }
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_PAYMENTS; }
        }
        
        protected override string updateProcedureName
        {
            get { return StoredProcedures.UPDATE_PAYMENT; }
        }

        protected override PaymentEntity entityFromReader(SqlDataReader reader)
        {
            //Reads the different columns from the database and set them to corresponding data type in the program. 
            DateTime dueDate = (DateTime)reader["DueDate"];
            decimal dueAmount = (decimal)reader["DueAmount"];
            DateTime paidDate = (DateTime)reader["PaidDate"];
            decimal paidAmount = (decimal)reader["PaidAmount"];
            bool paid = (bool)reader["Paid"];
            bool archived = (bool)reader["Archived"];
            string attachments = (string)reader["Attachments"];

            int payerId = (int)reader["Payer"];
            int payeeId = (int)reader["Payee"];
            string note = (string)reader["Note"];
            PaymentType type = (PaymentType)Enum.Parse(typeof(PaymentType), reader["Type"].ToString());
            string sale = (string)reader["Sale"];
            int booking = (int)reader["Booking"];
            string invoice = (string)reader["Invoice"];

            int id = (int)reader["PaymentId"];
            DateTime lastModified = (DateTime)reader["LastModified"];
            bool deleted = (bool)reader["Deleted"];

            APartyEntity payer = PartyMapper.Read(payerId);
            APartyEntity payee = PartyMapper.Read(payeeId);

            PaymentEntity paymentEntity = new PaymentEntity(dueDate, dueAmount, payer,
                payee, type, sale, booking);
            paymentEntity.PaidDate = paidDate;
            paymentEntity.PaidAmount = paidAmount;
            paymentEntity.Paid = paid;
            paymentEntity.Archived = archived;
            if (attachments != "")
            {
                foreach (string attachment in attachments.Split(';'))
                {
                    paymentEntity.AddAttachment(attachment);
                }
            }

            paymentEntity.Note = note;
            paymentEntity.Invoice = invoice;

            paymentEntity.Id = id;
            paymentEntity.LastModified = lastModified;
            paymentEntity.Deleted = deleted;

            return paymentEntity; 
        }

        protected override void addInsertParameters(PaymentEntity entity, 
            SqlParameterCollection parameters)
        {
            //Adds data to a new row in the database.
            addPaymentParameters(entity, parameters);
        }

        protected override void addUpdateParameters(PaymentEntity entity, 
            SqlParameterCollection parameters)
        {
            //Adds new data to a allready exsisting row.
            addPaymentParameters(entity, parameters);
        }
        #endregion

        #region Private Methods
        private void addPaymentParameters(PaymentEntity entity,
            SqlParameterCollection parameters)
        {
            //Adds different parameters from the database. 
            SqlParameter parameter = new SqlParameter("@DueDate", entity.DueDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@DueAmount", entity.DueAmount);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Paid", entity.Paid);
            parameters.Add(parameter);
            parameter = new SqlParameter("@PaidDate", entity.PaidDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@PaidAmount", entity.PaidAmount);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Archived", entity.Archived);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Payer", ((APartyEntity)entity.Payer).Id);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Payee", ((APartyEntity)entity.Payee).Id);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Attachments", string.Join(";", entity.Attachments));
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
            parameter = new SqlParameter("@Sale", entity.Sale);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Booking", entity.Booking);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Invoice", entity.Invoice);
            parameters.Add(parameter);
        }
        #endregion
    }
}
