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
using System.Linq;
using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;

namespace Domain.Controller
{
    public class CustomerController
    {
        #region Setup
        internal CustomerController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Initialize(BookingController bookingController, PaymentController paymentController)
        {
            this.bookingController = bookingController;
            this.paymentController = paymentController;
            customerCollection = new CustomerCollection(dataAccessFacade);
        }
        #endregion

        #region CRUD
        public ICustomer CreateCustomer(string name, string note, CustomerType type)
        {
            return customerCollection.Create(name, note, type);
        }

        public List<ICustomer> ReadAllCustomers()
        {
            var customers = customerCollection.ReadAll().Cast<ICustomer>().ToList();
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
            // Also deletes all related payments and bookings
            // TODO: Delete related payment rules
            customerCollection.Delete((Customer) customer);
            bookingController.DeleteBookingsForParty((AParty)customer);
            paymentController.DeletePaymentForParty((AParty)customer);
            supplierController.DeletePaymentRulesForCustomer(customer);
        }
        #endregion

        internal Customer findLonelyTree()
        {
            var lonelyTree = customerCollection.ReadAll().FirstOrDefault(c => c.Name == "Lonely Tree");
            return lonelyTree;
        }

        internal Customer findAnyCustomer()
        {
            var any = customerCollection.ReadAll().FirstOrDefault(c => c.Name == "Any");
            return any;
        }

        private IDataAccessFacade dataAccessFacade;
        private CustomerCollection customerCollection;
        private BookingController bookingController;
        private PaymentController paymentController;
        private SupplierController supplierController;
    }
}
