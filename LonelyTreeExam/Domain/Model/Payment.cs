using System;
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

        internal IPayment paymentEntity
        {
            get { return _paymentEntity; }
            set { _paymentEntity = value; }
        }

        internal Payment(DateTime dueDate, Decimal dueAmount, string responsible,
            string commissioner, DataAccessFacade dataAccessFacade) 
            :base (responsible, commissioner)
        {
            DueDate = dueDate;
            DueAmount = dueAmount;
            this.dataAccessFacade = dataAccessFacade;
            //dataAccessFacade.CreatePayment();
        }

        internal Payment(IPayment paymentEntity, DataAccessFacade dataAccessFacade) 
            :base(paymentEntity.Responsible, paymentEntity.Commissioner)
        {
            _paymentEntity = paymentEntity;
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Update()
        {

        }

        internal void Delete()
        {

        }

        internal static List<Payment> ReadAll(DataAccessFacade dataAccessFacade)
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
        private DataAccessFacade dataAccessFacade;
    }
}
