using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace DataAccess
{
   public interface IDataAccessFacade
   {
        IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner);
        List<IPayment> ReadAllPayments();
        void UpdateArtist(IPayment payment);
        void DeleteArtist(IPayment payment);
   }
}

