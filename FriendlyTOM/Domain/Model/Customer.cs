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
﻿using Common.Enums;
using Common.Interfaces;
using DataAccess;
using Domain.Helpers;

namespace Domain.Model
{
    internal class Customer : AParty, ICustomer
    {
        #region Public Properties
        public CustomerType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string ContactPerson
        {
            get { return _contactPerson; }
            set { _contactPerson = value; }
        }
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public string PhoneNo
        {
            get { return _phoneNo; }
            set { _phoneNo = value; }
        }
        public string FaxNo
        {
            get { return _faxNo; }
            set { _faxNo = value; }
        }
        #endregion

        #region Internal Methods
        internal Customer(ICustomer customerEntity, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _customerEntity = customerEntity;
            _type = customerEntity.Type;
            _contactPerson = customerEntity.ContactPerson;
            _email = customerEntity.Email;
            _address = customerEntity.Address;
            _phoneNo = customerEntity.PhoneNo;
            _faxNo = customerEntity.FaxNo;

            initializeParty(_customerEntity);

            Register register = Register.GetInstance();
            register.RegisterCustomer(_customerEntity, this);
        }

        internal Customer(string name, string note, CustomerType type, IDataAccessFacade dataAccessFacade)
        {
            validateName(name);
            this.dataAccessFacade = dataAccessFacade;
            _customerEntity = dataAccessFacade.CreateCustomer(type, note, name);
            _type = type;

            initializeParty(_customerEntity);

            Register register = Register.GetInstance();
            register.RegisterCustomer(_customerEntity, this);
        }

        //Reads all entities of customer there is in the DataAccessFacade.
        //Returns them to a list here.
        internal static List<Customer> ReadAll(IDataAccessFacade dataAccessFacade)
        {
            List<ICustomer> customerEntities = dataAccessFacade.ReadAllCustomers();
            List<Customer> customers = new List<Customer>();

            foreach (ICustomer customerEntity in customerEntities)
            {
                Customer customer = new Customer(customerEntity, dataAccessFacade);
                customers.Add(customer);
            }
            return customers;
        }

        //Calls for updating an object of a customer. 
        //Gets a new LastModified date to now. 
        internal void Update()
        {
            _customerEntity.Name = _name;
            _customerEntity.Note = _note;
            _customerEntity.Type = _type;
            _customerEntity.ContactPerson = _contactPerson;
            _customerEntity.Email = _email;
            _customerEntity.Address = _address;
            _customerEntity.PhoneNo = _phoneNo;
            _customerEntity.FaxNo = _faxNo;

            dataAccessFacade.UpdateCustomer(_customerEntity);
        }

        //Calls for removing an object in a list of customers.
        internal void Delete()
        {
            dataAccessFacade.DeleteCustomer(_customerEntity);
        }
        #endregion

        #region Private fields
        private CustomerType _type;
        private string _contactPerson;
        private string _email;
        private string _address;
        private string _phoneNo;
        private string _faxNo;
        internal ICustomer _customerEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion
    }
}
