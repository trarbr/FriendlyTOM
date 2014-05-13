using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataAccessFacadeStub : IDataAccessFacade
    {

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
