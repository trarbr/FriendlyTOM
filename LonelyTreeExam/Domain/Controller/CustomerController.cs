using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            dataAccessFacade = new DataAccessFacade();
            customerCollection = new CustomerCollection(dataAccessFacade);
        }

        public List<ICustomer> ReadAllCustomers()
        {
            List<ICustomer> customers = new List<ICustomer>();
            foreach (Customer customer in customerCollection.ReadAll())
            {
                customers.Add(customer);
            }
            return customers;
        }

        public ICustomer CreateCustomer(CustomerType type, string name, string note)
        {
            return customerCollection.Create(type, name, note);
        }

        public void UpdateCustomer(ICustomer customer)
        {
            customerCollection.Update((Customer) customer);
        }

        public void DeleteCustomer(ICustomer customer)
        {
            customerCollection.Delete((Customer) customer);
        }

        #endregion

        #region Private Properties
        private IDataAccessFacade dataAccessFacade;
        private CustomerCollection customerCollection;
        #endregion
    }
}
