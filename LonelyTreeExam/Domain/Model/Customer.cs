using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;
using DataAccess;

namespace Domain.Model
{
    internal class Customer : Accountability, ICustomer
    {
        #region Public Properties

        public CustomerType Type
        {
            get { return _customerEntity.Type; }
            set { _customerEntity.Type = value; }
        }

        public string Name
        {
            get { return _customerEntity.Name; }
            set
            {
                validateName(value);
                _customerEntity.Name = value;
            }
        }
        #endregion

        #region Internal Methods
        internal Customer(CustomerType type, string note, string name, IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
            _customerEntity = dataAccessFacade.CreateCustomer(type, note, name);
            this._accountabilityEntity = _customerEntity;
        }

        internal Customer(ICustomer customerEntity, IDataAccessFacade dataAccessFacade)
        {
            _customerEntity = customerEntity;
            this._accountabilityEntity = (IAccountability) _customerEntity;
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Update()
        {
            dataAccessFacade.UpdateCustomers(_customerEntity);
        }

        internal void Delete()
        {
            dataAccessFacade.DeleteCustomer(_customerEntity);
        }

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

        #region Validation

        private void validateName(string value)
        {
            validateNullOrWhiteSpace(value, "Name");
        }

        private void validateNote(string value)
        {
            validateNullOrWhiteSpace(value, "Note");
        }

        #endregion

        #region Private Properties
        private ICustomer _customerEntity;
        private IDataAccessFacade dataAccessFacade;
        #endregion

        
    }
}
