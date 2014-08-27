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
            //if customers are empty do a readall from the database. 
            //and returns them all to the customer list.
            if (customers == null)
            {
                customers = Customer.ReadAll(dataAccessFacade);
            }
            return customers;
        }

        internal Customer Create(CustomerType type, string note, string name)
        {
            //Adds a new customer object and adds it to the list. 
            Customer customer = new Customer(type, note, name, dataAccessFacade);
            customers.Add(customer);
            return customer;
        }

        internal void Update(Customer customer)
        {
            //Calls for an update on this specific customer.
            customer.Update();
        }

        internal void Delete(Customer customer)
        {
            //Calls delete and removes it from the list.
            //Do not remove if customer is Lonely Tree or Any
            if (!(customer.Name == "Lonely Tree" || customer.Name == "Any"))
            {
                customer.Delete();
                customers.Remove(customer);
            }
        }
        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private List<Customer> customers;
        #endregion
    }
}
