﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess;

namespace Domain.Model
{
    internal class Payment : Accountability, IPayment
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
            set { _paymentEntity.DueAmount = value; }
        }
        public DateTime PaidDate
        {
            get { return _paymentEntity.PaidDate; }
            set { _paymentEntity.PaidDate = value; }
        }
        public decimal PaidAmount
        {
            get { return _paymentEntity.PaidAmount; }
            set { _paymentEntity.PaidAmount = value; }
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
        public IReadOnlyList<string> Attachments
        {
            get { return _paymentEntity.Attachments; }
        }

        public void AddAttachment(string path)
        {
            _paymentEntity.AddAttachment(path);
        }
        #endregion

        //skal måske bruges i sprint 2!!
        //internal IPayment paymentEntity
        //{
        //    get { return _paymentEntity; }
        //    set { _paymentEntity = value; }
        //}

        internal Payment(DateTime dueDate, decimal dueAmount, string responsible,
            string commissioner, IDataAccessFacade dataAccessFacade) 
        {
            this.dataAccessFacade = dataAccessFacade;

            _paymentEntity = dataAccessFacade.CreatePayment(dueDate, dueAmount, responsible, commissioner);
            this._accountabilityEntity = _paymentEntity;
        }

        internal Payment(IPayment paymentEntity, IDataAccessFacade dataAccessFacade) 
        {
            _paymentEntity = paymentEntity;
            this._accountabilityEntity = _paymentEntity;
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Update()
        {
            dataAccessFacade.UpdatePayment(_paymentEntity);
        }

        internal void Delete()
        {

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

        private IPayment _paymentEntity;
        private IDataAccessFacade dataAccessFacade;
    }
}