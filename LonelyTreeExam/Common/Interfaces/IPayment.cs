using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPayment : IAccountability
    {
        DateTime DueDate { get; set; }
        decimal DueAmount { get; set; }
        DateTime PaidDate { get; set; }
        decimal PaidAmount { get; set; }
        IReadOnlyList<string> Attachments { get; }
        bool Paid { get; set; }
        bool Archived { get; set; }
        PaymentType Type { get; set; }

        void DeleteAttachment(string attachment);

        void AddAttachment(string attachment);
    }
}
