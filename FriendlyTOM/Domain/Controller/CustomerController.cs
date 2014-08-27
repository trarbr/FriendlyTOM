// LB
using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;

namespace Domain.Controller
{
    public class CustomerController
    {
        #region Public Methods
        public CustomerController()
        {
            dataAccessFacade = DataAccessFacade.GetInstance();
            customerCollection = new CustomerCollection(dataAccessFacade);
        }

        /// <summary>
        /// For testing against a specified DataAccessFacade
        /// </summary>
        /// <param name="dataAccessFacade"></param>
        public CustomerController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            customerCollection = new CustomerCollection(dataAccessFacade);
        }

        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            //Calls custommercollection class for Create
            return customerCollection.Create(type, note, name);
        }

        public List<ICustomer> ReadAllCustomers()
        {
            //Calls custommercollection class for readAll
            List<ICustomer> customers = new List<ICustomer>();
            foreach (Customer customer in customerCollection.ReadAll())
            {
                customers.Add(customer);
            }
            return customers;
        }
        
        public void UpdateCustomer(ICustomer customer)
        {
            //Calls custommercollection class for update
            customerCollection.Update((Customer) customer);
        }

        public void DeleteCustomer(ICustomer customer)
        {
            //Calls custommercollection class for delete
            customerCollection.Delete((Customer) customer);
        }
        #endregion

        internal Customer findLonelyTree()
        {
            Customer lonelyTree = null;
            foreach (Customer customer in customerCollection.ReadAll())
            {
                if (customer.Name == "Lonely Tree")
                {
                    lonelyTree = customer;
                    break;
                }
            }

            return lonelyTree;
        }

        internal Customer findAnyCustomer()
        {
            Customer anyCustomer = null;
            foreach (Customer customer in customerCollection.ReadAll())
            {
                if (customer.Name == "Any")
                {
                    anyCustomer = customer;
                    break;
                }
            }

            return anyCustomer;
        }

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private CustomerCollection customerCollection;
        #endregion
    }
}
