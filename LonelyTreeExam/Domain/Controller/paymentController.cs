using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;

namespace Domain.Controller
{
    public class paymentController
    {
        public paymentController()
        {
            dataAccessFacade = new DataAccessFacade();

            PaymentsCollection = new PaymentsCollection(dataAccessFacade);

        }

        public List<IPayment> ReadAllArtists()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in PaymentsCollection.ReadAll())
            {
                payments.Add(payment);
            }

            return payments;
        }
    }
}
