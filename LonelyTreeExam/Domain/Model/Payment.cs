using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    internal class Payment : IPayment
    {
        #region Public Properties/Methods
        public DateTime DueDate { get; set; }
        public DateTime PaidDate { get; set; }
        public Decimal Amount { get; set; }
        public bool Archived { get; set; }
        public IReadOnlyList<string> Attachments
        {
            get { return _paymentEntity.Invoice; }
        }

        public void AddAttachment(string path)
        {

        }
        #endregion

        internal IPayment paymentEntity
        {
            get { return _paymentEntity; }
            set { _paymentEntity = value; }
        }

        internal Payment(DateTime dueDate, Decimal amount)
        {
            DueDate = dueDate;
            Amount = amount;
            PaidDate = DateTime.Today;
        }

        internal void Update()
        {

        }

        internal void Delete()
        {

        }

        internal void ReadAll()
        {

        }

        private IPayment _paymentEntity;
    }
}
