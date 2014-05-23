using System.Collections.Generic;
using Common.Enums;
using DataAccess;
using Domain.Model;

namespace Domain.Collections
{
    internal class CustomerCollection
    {
        #region Internal Constructor
        internal CustomerCollection(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            ReadAll();
        }
        #endregion

        #region Internal CRUD
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
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Customer> customers;
        #endregion
    }
}
