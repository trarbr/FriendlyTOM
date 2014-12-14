/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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
        public BookingController bookingController { get; set; }
        public PaymentController paymentController { get; set; }

        #region Public Methods
        public CustomerController()
        {
            dataAccessFacade = DataAccessFacade.Instance;
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
            bookingController.DeleteBookingsForParty((AParty)customer);
            paymentController.DeletePaymentForParty((AParty)customer);
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
