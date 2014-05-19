﻿using System.Collections.Generic;
﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;
using DataAccess;

namespace Domain.Model
{
    internal class Customer : AParty, ICustomer
    {
        #region Public Properties
        public CustomerType Type
        {
            get { return _customerEntity.Type; }
            set { _customerEntity.Type = value; }
        }
        #endregion

        #region Internal Methods
        internal Customer(CustomerType type, string note, string name, IDataAccessFacade dataAccessFacade)
        {
            validateName(name);

            this.dataAccessFacade = dataAccessFacade;

            _customerEntity = dataAccessFacade.CreateCustomer(type, note, name);
            this._partyEntity = _customerEntity;
        }

        internal Customer(ICustomer customerEntity, IDataAccessFacade dataAccessFacade)
        {
            _customerEntity = customerEntity;
            this._partyEntity = _customerEntity;
            this.dataAccessFacade = dataAccessFacade;
        }

        //Calls for updating an object of a customer. 
        //Gets a new LastModified date to now. 
        internal void Update()
        {
            dataAccessFacade.UpdateCustomers(_customerEntity);
        }

        //Calls for removing an object in a list of customers.
        internal void Delete()
        {
            dataAccessFacade.DeleteCustomer(_customerEntity);
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
        #endregion

        #region Private fields
        private ICustomer _customerEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion

        #region Validation
        //Checks if the value of the "name" is not null or whitespace
        /*
        private void validateName(string value)
        {
            validateNullOrWhiteSpace(value, "Name");
        }
        */
        #endregion
    }
}