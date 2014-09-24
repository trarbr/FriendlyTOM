/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using Common.Interfaces;
using Common.Enums;
using DataAccess;
using Domain.Helpers;

namespace Domain.Model
{
    internal class Payment : IPayment
    {
        #region Public Properties
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
                Register register = Register.GetInstance();
                _payer = register.GetSupplier((ISupplier)_paymentEntity.Payer);
                _payee = register.GetCustomer((ICustomer)_paymentEntity.Payee);
            }
            else if (_paymentEntity.Payer is ICustomer)
            {
                Register register = Register.GetInstance();
                _payer = register.GetCustomer((ICustomer)_paymentEntity.Payer);
                _payee = register.GetSupplier((ISupplier)_paymentEntity.Payee);
            }

        }
#endregion

        #region Internal CRUD
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

        #region Validation
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
                throw new ArgumentOutOfRangeException("Party was not found");
            }
        }
        #endregion

        #region Private Fields
        private IPayment _paymentEntity;
        private IDataAccessFacade dataAccessFacade;
        private AParty _payee;
        private AParty _payer;
        #endregion
    }
}
