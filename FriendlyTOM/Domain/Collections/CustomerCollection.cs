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
