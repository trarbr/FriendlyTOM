using System;
using System.Collections.Generic;
using System.IO;
using Common.Interfaces;
using Common.Enums;
using DataAccess;

namespace Domain.Model
{
    internal class Payment : AAccountability, IPayment
    {
        #region Public Properties/Methods
        public DateTime DueDate
        {
            get { return _paymentEntity.DueDate; }
            set { _paymentEntity.DueDate = value; }
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

        public void DeleteAttachment(string attachment)
        {
            _paymentEntity.DeleteAttachment(attachment);
        }

        public void AddAttachment(string attachment)
        {
            validateFilePathExists(attachment);
            _paymentEntity.AddAttachment(attachment);    
        }
        #endregion

        #region Internal Methods
        internal Payment(DateTime dueDate, decimal dueAmount, IParty responsible,
            IParty commissioner, PaymentType type, string sale, int booking,
            IDataAccessFacade dataAccessFacade) 
        {
            validateDueAmount(dueAmount);
            //validateResponsible(responsible);
            //validateCommissioner(commissioner);
            validateSale(sale);

            this.dataAccessFacade = dataAccessFacade;

            // This makes sense
            AParty aResponsible = (AParty)responsible;
            IParty responsibleEntity = aResponsible._partyEntity;
            AParty aCommissioner = (AParty)commissioner;
            IParty commissionerEntity = aCommissioner._partyEntity;

            _paymentEntity = dataAccessFacade.CreatePayment(dueDate, dueAmount, responsibleEntity,
                commissionerEntity, type, sale, booking);

            // This makes less sense - can't accountability set its own Responsibilty and Commissioner?
            this._accountabilityEntity = _paymentEntity;
            this.Responsible = responsible;
            this.Commissioner = commissioner;
        }

        internal Payment(IPayment paymentEntity, IDataAccessFacade dataAccessFacade) 
        {
            _paymentEntity = paymentEntity;
            this._accountabilityEntity = _paymentEntity;
            // Need to know if it is supplier or customer if AParty really is to be abstract
            AParty responsible = new AParty();
            responsible._partyEntity = _paymentEntity.Responsible;
            Responsible = responsible;
            AParty commissioner = new AParty();
            commissioner._partyEntity = _paymentEntity.Commissioner;
            Commissioner = commissioner;
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Update()
        {
            dataAccessFacade.UpdatePayment(_paymentEntity);
        }

        internal void Delete()
        {
            dataAccessFacade.DeletePayment(_paymentEntity);
        }

        internal static List<Payment> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            List<IPayment> paymentEntities = dataAccessFacade.ReadAllPayments();
            List<Payment> payments = new List<Payment>();

            foreach (IPayment paymentEntity in paymentEntities)
            {
                Payment payment = new Payment(paymentEntity, dataAccessFacade);
                payments.Add(payment);
            }
            return payments;
        }
        #endregion

        #region ValidationDecimalsAndDueDate

        private void validateDueAmount(decimal value)
        {
            validateDecimal(value, "DueAmount");
        }

        private void validatePaidAmount(decimal value)
        {
            validateDecimal(value, "PaidAmount");
        }

        private void validateDecimal(decimal number, string paramName)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be less than zero");
            }
        }

        private void validateFilePathExists(string pathName)
        {
            if (!File.Exists(pathName))
            {
                throw new ArgumentOutOfRangeException("attachment", "Filename doesn't exist");
            }
        }

        //Checks if the value of the "name" is not null or whitespace
        private void validateSale(string value)
        {
            validateNullOrWhiteSpace(value, "Sale");
        }
        #endregion

        #region Private Properties
        private IPayment _paymentEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion
    }
}
