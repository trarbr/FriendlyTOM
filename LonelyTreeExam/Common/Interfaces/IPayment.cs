using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPayment : IAccountability
    {
        IReadOnlyCollection<string> Attachments { get; }
        DateTime DueDate { get; set; }
        decimal DueAmount { get; set; }
        DateTime PaidDate { get; set; }
        decimal PaidAmount { get; set; }
        bool Paid { get; set; }

        void AddAttachment(string attachment);
    }
}
