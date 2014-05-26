using System.Data.SqlClient;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using DataAccess.Helpers;
using Common.Interfaces;
using Common.Enums;

namespace DataAccess.Mappers
{
    internal class BookingMapper : ASQLMapper<BookingEntity>
    {
        internal CustomerMapper CustomerMapper;
        internal SupplierMapper SupplierMapper;

        #region Internal Methods
        internal BookingMapper(string connectionString)
        {
            this.connectionString = connectionString;
            this.entityMap = new Dictionary<int, BookingEntity>();
        }

        internal BookingEntity Create(IParty responsible, IParty commissioner, string sale, int bookingNumber,
            DateTime StartDate, DateTime EndDate)
        {
            //Uses the information set in the GUI to push the objet to the database by calling the insert method.
            BookingEntity bookingEntity = new BookingEntity(responsible, commissioner, sale, bookingNumber,
                StartDate, EndDate);

            insert(bookingEntity);

            return bookingEntity;
        }

        internal List<BookingEntity> ReadAll()
        {
            //Reads all bookings from the database. 
            List<BookingEntity> bookings = selectAll();
            return bookings;
        } 

        internal void Update(BookingEntity booking)
        {
            //Calls the update method
            update(booking);
        }

        internal void Delete(BookingEntity booking)
        {
            //Sets the row to deletede and calls update. 
            booking.Deleted = true;
            update(booking);
        }
        #endregion

        #region Protected Methods
        protected override string insertProcedureName
        {
            get { return StoredProcedures.CREATE_BOOKING; }
        }

        protected override string selectAllProcedureName
        {
            get { return StoredProcedures.READ_ALL_BOOKINGS; }
        }

        protected override string updateProcedureName
        {
            get { return StoredProcedures.UPDATE_BOOKING; }
        }

        protected override BookingEntity entityFromReader(SqlDataReader reader)
        {
            //Sets data from the database to corresponding data type for usage in the program.
            int responsibleId = (int) reader["Responsible"];
            int commissionerId = (int) reader["Commissioner"];
            string note = (string)reader["Note"];
            string sale = (string) reader["Sale"];
            int bookingNumber = (int) reader["BookingNumber"];
            DateTime startDate = (DateTime) reader["StartDate"];
            DateTime endDate = (DateTime) reader["EndDate"];
            BookingType type = (BookingType) Enum.Parse(typeof (BookingType), reader["Type"].ToString());
            decimal iVAExempt = (decimal) reader["IVAExempt"];
            decimal iVASubject = (decimal) reader["IVASubject"];
            decimal service = (decimal) reader["Service"];
            decimal iVA = (decimal) reader["IVA"];
            decimal productRetention = (decimal) reader["ProductRetention"];
            decimal supplierRetention = (decimal) reader["supplierRetention"];
            decimal transferAmount = (decimal) reader["TransferAmount"];

            int id = (int) reader["BookingId"];
            DateTime lastModified = (DateTime) reader["LastModified"];
            bool deleted = (bool) reader["Deleted"];

            IParty responsible = SupplierMapper.Read(responsibleId);
            IParty commissioner = CustomerMapper.Read(commissionerId);

            BookingEntity bookingEntity = new BookingEntity(responsible, commissioner, sale, bookingNumber,
                startDate, endDate);
            //Uses the data to make it into an booking object.
            bookingEntity.Note = note;
            bookingEntity.Id = id;
            bookingEntity.LastModified = lastModified;
            bookingEntity.Deleted = deleted;
            bookingEntity.Type = type;
            bookingEntity.IVAExempt = iVAExempt;
            bookingEntity.IVA = iVA;
            bookingEntity.IVASubject = iVASubject;
            bookingEntity.Service = service;
            bookingEntity.ProductRetention = productRetention;
            bookingEntity.SupplierRetention = supplierRetention;
            bookingEntity.TransferAmount = transferAmount;

            return bookingEntity;
        }

        protected override void addInsertParameters(BookingEntity entity, 
            SqlParameterCollection parameters)
        {
            addBookingParameters(entity, parameters);
        }

        protected override void addUpdateParameters(BookingEntity entity, 
            SqlParameterCollection parameters)
        {
            addBookingParameters(entity, parameters);
        }

        private void addBookingParameters(BookingEntity entity, SqlParameterCollection parameters)
        {
            //Sets the parameters that a used by booking.
            SqlParameter parameter = new SqlParameter("@Sale", entity.Sale);
            parameters.Add(parameter);
            parameter = new SqlParameter("@BookingNumber", entity.BookingNumber);
            parameters.Add(parameter);
            parameter = new SqlParameter("@StartDate", entity.StartDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@EndDate", entity.EndDate);
            parameters.Add(parameter);
            parameter = new SqlParameter("@IVAExempt", entity.IVAExempt);
            parameters.Add(parameter);
            parameter = new SqlParameter("@IVASubject", entity.IVASubject);
            parameters.Add(parameter);
            parameter = new SqlParameter("@SubTotal", entity.SubTotal);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Service", entity.Service);
            parameters.Add(parameter);
            parameter = new SqlParameter("@IVA", entity.IVA);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Type", entity.Type.ToString());
            parameters.Add(parameter);
            parameter = new SqlParameter("@ProductRetention", entity.ProductRetention);
            parameters.Add(parameter);
            parameter = new SqlParameter("@SupplierRetention", entity.SupplierRetention);
            parameters.Add(parameter);
            parameter = new SqlParameter("@TransferAmount", entity.TransferAmount);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Responsible", ((APartyEntity)entity.Responsible).Id);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Commissioner", ((APartyEntity)entity.Commissioner).Id);
            parameters.Add(parameter);
            parameter = new SqlParameter("@Note", entity.Note);
            parameters.Add(parameter);
        }
        #endregion
    }
}
