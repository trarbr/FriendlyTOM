using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Interfaces;
using Common.Enums;

namespace DataAccess.Entities
{
    internal class BookingEntity : AAccountabilityEntity, IBooking
    {
        public string Sale { get; set; }
        public int BookingNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BookingType Type { get; set; }
        public decimal IVAExempt { get; set; }
        public decimal IVASubject { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Service { get; set; }
        public decimal IVA { get; set; }
        public decimal ProductRetention { get; set; }
        public decimal SupplierRetention { get; set; }
        public decimal TransferAmount { get; set; }

        internal BookingEntity(IParty responsible, IParty commissioner, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate) : base(responsible, commissioner)
        {
            Responsible = responsible;
            Commissioner = commissioner;
            Sale = sale;
            BookingNumber = bookingNumber;
            StartDate = startDate;
            EndDate = endDate;

            IVAExempt = 0;
            IVASubject = 0;
            SubTotal = 0;
            Service = 0;
            IVA = 0;
            ProductRetention = 0;
            SupplierRetention = 0;
            TransferAmount = 0;
        }
    }
}
