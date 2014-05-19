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

            dataAccessFacade = DataAccessFacade.GetInstance();
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

        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            return customerCollection.Create(type, note, name);
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
