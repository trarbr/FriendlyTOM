using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;


namespace DataAccess.Entities
{
    internal class PaymentEntity : Entity, IPayment
    {
        public PaymentEntity(string paymentName, int id, DateTime lastModified, bool deleted) : base(id, lastModified, deleted)
        {
            Name = paymentName;
            _invoice = new List<int>();
        }

        public string Name { get; set; }
        public IReadOnlyCollection<int> Invoice
        {
            get { return _invoice; }
        }

        public void Addinvoice(int invoice)
        {
            _invoice.Add(invoice);
        }

        private List<int> _invoice;
    }
}
