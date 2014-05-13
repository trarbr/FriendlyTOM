using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess
{
    public class DataAccessFacadeStub : IDataAccessFacade
    {

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner)
        {
            return new PaymentEntity(dueDate, dueAmount, responsible, commissioner);
        }

        public List<IPayment> ReadAllPayments()
        {
            throw new NotImplementedException();
        }

        public void UpdatePayment(IPayment payment)
        {
            throw new NotImplementedException();
        }

        public void DeletePayment(IPayment payment)
        {
            PaymentEntity entity = (PaymentEntity) payment;

            entity.Deleted = 1;
        }
    }
}
