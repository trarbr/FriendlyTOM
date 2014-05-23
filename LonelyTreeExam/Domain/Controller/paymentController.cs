using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using Common.Enums;

namespace Domain.Controller
{
    public class PaymentController
    {
        #region Public Methods
        public PaymentController(SupplierController supplierController, CustomerController customerController)
        {
            dataAccessFacade = DataAccessFacade.GetInstance();

            paymentCollection = new PaymentCollection(dataAccessFacade);

            foreach (ISupplier supplier in supplierController.ReadAllSuppliers())
            {
                if (supplier.Name == "Lonely Tree")
                {
                    SupplierLonelyTree = (Supplier)supplier;
                    break;
                }
            }

            foreach (ICustomer customer in customerController.ReadAllCustomers())
            {
                if (customer.Name == "Lonely Tree")
                {
                    CustomerLonelyTree = (Customer)customer;
                    break;
                }
            }
        }

        internal Supplier SupplierLonelyTree;
        internal Customer CustomerLonelyTree;

        public List<IPayment> ReadAllPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAll())
            {
                payments.Add(payment);
            }

            return payments;
        }

        public List<IPayment> ReadAllArchivedPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllArchived())
            {
                payments.Add(payment);
            }

            return payments;
        }

        public List<IPayment> ReadAllIncomingPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllIncoming())
            {
                payments.Add(payment);
            }

            return payments;
        }

        public List<IPayment> ReadAllOutgoingPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            foreach (Payment payment in paymentCollection.ReadAllOutgoing())
            {
                payments.Add(payment);
            }

            return payments;
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty responsible, 
            IParty commissioner, PaymentType type, string sale, int booking)
        {
            return paymentCollection.Create(dueDate, dueAmount, responsible, commissioner, type,
                sale, booking);
        }

        public void UpdatePayment(IPayment payment)
        {
            paymentCollection.Update((Payment)payment);
        }

        public void DeletePayment(IPayment payment)
        {
            paymentCollection.Delete((Payment)payment);
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private PaymentCollection paymentCollection;
        #endregion
    }
}
