using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Model;

namespace Domain.Collections
{
    internal class CustomerCollection
    {
        internal CustomerCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }

        internal List<Customer> ReadAll()
        {
            if (customers == null)
            {
                customers = Customer.ReadAll(dataAccessFacade);
            }
            return customers;
        }

        internal Customer Create(CustomerType type, string note, string name)
        {
            Customer customer = new Customer(type, note, name, dataAccessFacade);
            customers.Add(customer);
            return customer;
        }

        internal void Update(Customer customer)
        {
            customer.Update();
        }

        internal void Delete(Customer customer)
        {
            customer.Delete();
            customers.Remove(customer);
        }

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Customer> customers;
        #endregion
    }
}
