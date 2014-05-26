using System;
using System.Collections.Generic;
using System.IO;
using Common.Interfaces;
using Common.Enums;
using DataAccess;

namespace Domain.Model
{
    internal class Payment : IPayment
    {
        #region Public Properties/Methods
        public IParty Payee
        {
            get { return _payee; }
            set
            {
                validateParty(value);
                _payee = (AParty)value;
                _paymentEntity.Payee = _payee._partyEntity;
            }
        }
        public IParty Payer
        {
            get { return _payer; }
            set
            {
                validateParty(value);
                _payer = (AParty)value;
                _paymentEntity.Payer = _payer._partyEntity; 
            }
        }
        public string Note
        {
            get { return _paymentEntity.Note; }
            set { _paymentEntity.Note = value; }
        }
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
        internal Payment(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking,
            IDataAccessFacade dataAccessFacade) 
        {
            validateDueAmount(dueAmount);
            validateParty(payer);
            validateParty(payee);
            validateSale(sale);

            // Get entities for DataAccess
            IParty payerEntity = ((AParty)payer)._partyEntity;
            IParty payeeEntity = ((AParty)payee)._partyEntity;

            this.dataAccessFacade = dataAccessFacade;
            _paymentEntity = dataAccessFacade.CreatePayment(dueDate, dueAmount, payerEntity,
                payeeEntity, type, sale, booking);

            Payer = payer;
            Payee = payee;
        }

        internal Payment(IPayment paymentEntity, IDataAccessFacade dataAccessFacade) 
        {
            this.dataAccessFacade = dataAccessFacade;
            this._paymentEntity = paymentEntity;

            // Create Models of payer and payee
            if (_paymentEntity.Payer is ISupplier)
            {
                _payer = new Supplier(dataAccessFacade, (ISupplier)_paymentEntity.Payer);
                _payee = new Customer((ICustomer)_paymentEntity.Payee, dataAccessFacade);
            }
            else if (_paymentEntity.Payer is ICustomer)
            {
                _payer = new Customer((ICustomer)_paymentEntity.Payer, dataAccessFacade);
                _payee = new Supplier(dataAccessFacade, (ISupplier)_paymentEntity.Payee);
            }

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

        #region Validation
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

        //Checks if the value of the Sale is not null or whitespace
        private void validateSale(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException("Sale", "may not be empty");
            }
        }

        protected void validateParty(IParty value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Payer was not found");
            }
        }
        #endregion

        #region Private Properties
        private IPayment _paymentEntity;
        private IDataAccessFacade dataAccessFacade;
        private AParty _payee;
        private AParty _payer;
        #endregion
    }
}
