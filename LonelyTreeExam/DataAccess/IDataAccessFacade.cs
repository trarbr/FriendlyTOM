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
        IPayment CreatePayment(string paymentName);
        List<IPayment> ReadAllPayments();
        void UpdateArtist(IPayment payment);
        void DeleteArtist(IPayment payment);
   }
}

