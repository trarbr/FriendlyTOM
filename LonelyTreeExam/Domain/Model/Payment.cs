using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Enums;
using DataAccess;

namespace Domain.Model
{
    internal class Payment : Accountability, IPayment
    {
        #region Public Properties/Methods
        public DateTime DueDate
        {
            get { return _paymentEntity.DueDate; }
            set
            {
                validateDueDateNotNull(value);
                _paymentEntity.DueDate = value;
            }
        }
        public decimal DueAmount
        {
            get { return _paymentEntity.DueAmount; }
            set
            {
                validateDueAmount(value);
                _paymentEntity.DueAmount = value;
            }
        }
        public DateTime PaidDate
        {
            get { return _paymentEntity.PaidDate; }
            set { _paymentEntity.PaidDate = value; }
        }
        public decimal PaidAmount
        {
            get { return _paymentEntity.PaidAmount; }
            set
            {
                validatePaidAmount(value);
                _paymentEntity.PaidAmount = value;
            }
        }
        public bool Archived
        {
            get { return _paymentEntity.Archived; }
            set { _paymentEntity.Archived = value; }
        }
        public bool Paid
        {
            get { return _paymentEntity.Paid; }
            set { _paymentEntity.Paid = value; }
        }
        public PaymentType Type
        {
            get { return _paymentEntity.Type; }
            set { _paymentEntity.Type = value; }
        }
        public string Sale
        {
            get { return _paymentEntity.Sale; }
            set
            {
                validateSale(value);
                _paymentEntity.Sale = value;
            }
        }
        public int Booking
        {
            get { return _paymentEntity.Booking; }
            set { _paymentEntity.Booking = value; }
        }
        public string Invoice
        {
            get { return _paymentEntity.Invoice; }
            set { _paymentEntity.Invoice = value; }
        }
        public IReadOnlyList<string> Attachments
        {
            get { return _paymentEntity.Attachments; }
        }

        //Deletes one attachment with the filepath as a parameter. accesses DeleteAttachment through
        //the specific paymentEntity, saved in the specific payment.
        public void DeleteAttachment(string attachment)
        {
            _paymentEntity.DeleteAttachment(attachment);
        }

        //Adds one attachment with the filepath as a parameter. accesses AddAttachment through
        //the specific paymentEntity, saved in the specific payment.
        public void AddAttachment(string attachment)
        {
            //validate if the filepath exists.
            validateFilePathExists(attachment);
            _paymentEntity.AddAttachment(attachment);    
        }
        #endregion

        #region Internal Constructors/Methods
        internal Payment(DateTime dueDate, decimal dueAmount, string responsible,
            string commissioner, PaymentType type, string sale, int booking,
            IDataAccessFacade dataAccessFacade)
        {
            validateDueAmount(dueAmount);
            validateDueDateNotNull(dueDate);
            validateResponsible(responsible);
            validateCommissioner(commissioner);
            validateSale(sale);

            this.dataAccessFacade = dataAccessFacade;

            _paymentEntity = dataAccessFacade.CreatePayment(dueDate, dueAmount, responsible,
                commissioner, type, sale, booking);
            this._accountabilityEntity = _paymentEntity;
        }

        internal Payment(IPayment paymentEntity, IDataAccessFacade dataAccessFacade)
        {
            _paymentEntity = paymentEntity;
            this._accountabilityEntity = _paymentEntity;
            this.dataAccessFacade = dataAccessFacade;
        }

        //updates _paymentEntity through dataAccesFacade
        internal void Update()
        {
            dataAccessFacade.UpdatePayment(_paymentEntity);
        }

        //Deletes _paymentEntity through dataAccessFacade
        internal void Delete()
        {
            dataAccessFacade.DeletePayment(_paymentEntity);
        }

        //Returns a list of all payments created, with dataAccessFacade as parameter to access ReadAllPayments
        //through dataAccessFacade, to get a list of paymentEntities.
        internal static List<Payment> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            List<IPayment> paymentEntities = dataAccessFacade.ReadAllPayments();
            List<Payment> payments = new List<Payment>();

            //foreach converts each paymentEntity from paymentEntities to a payment, and adds it to a list of payments.
            foreach (IPayment paymentEntity in paymentEntities)
            {
                Payment payment = new Payment(paymentEntity, dataAccessFacade);
                payments.Add(payment);
            }
            return payments;
        }
        #endregion
        
        #region ValidationDecimalsAndDueDate

        //sends dueAmount with decimal parameter through validateDecimal.
        private void validateDueAmount(decimal value)
        {
            validateDecimal(value, "DueAmount");
        }

        //sends paidAmount with decimal parameter through validateDecimal.
        private void validatePaidAmount(decimal value)
        {
            
            validateDecimal(value, "PaidAmount");
        }

        //validates if a decimal amount is under zero, parameter decimal number and string paramName as the name of the property. 
        //if it is it will throw a argumentOutOfRangeException.
        private void validateDecimal(decimal number, string paramName)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be less than zero");
            }
        }

        
        private void validateDueDateNotNull(DateTime date)
        {
            string paramName = "DueDate";
            if (date == null)
            {
                throw new ArgumentNullException(paramName, "may not be null");
            }
        }

        private void validateFilePathExists(string pathName)
        {
            if (!File.Exists(pathName))
            {
                throw new Exception("File name does not exists");
            }
        }

        private void validateSale(string value)
        {
            validateNullOrWhiteSpace(value, "Sale");
        }

        #endregion

        #region Private Fields
        private IPayment _paymentEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion
    }
}
