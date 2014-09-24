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

using Common.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    internal class Register
    {
        internal static Register GetInstance()
        {
            if (_register == null)
            {
                _register = new Register();
            }

            return _register;
        }

        internal void RegisterSupplier(ISupplier supplierEntity, Supplier supplier)
        {
            if (!suppliers.ContainsKey(supplierEntity))
            {
                suppliers.Add(supplierEntity, supplier);
            }
        }

        internal Supplier GetSupplier(ISupplier supplierEntity)
        {
            return suppliers[supplierEntity];  
        }

        internal void RegisterCustomer(ICustomer customerEntity, Customer customer)
        {
            if (!customers.ContainsKey(customerEntity))
            {
                customers.Add(customerEntity, customer);
            }
        }

        internal Customer GetCustomer(ICustomer customerEntity)
        {
            return customers[customerEntity];
        }

        private Dictionary<ISupplier, Supplier> suppliers;
        private Dictionary<ICustomer, Customer> customers;
        private static Register _register;

        private Register()
        {
            suppliers = new Dictionary<ISupplier, Supplier>();
            customers = new Dictionary<ICustomer, Customer>();
        }
    }
}
