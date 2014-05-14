using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using DataAccess;

namespace Domain.Model
{
    internal class Customer : AParty, ICustomer
    {
        public CustomerType Type
        {
            get { return _customerEntity.Type; }
            set { _customerEntity.Type = value; }
        }

        internal Customer(CustomerType type)
        {
            _customerEntity = dataAccessFacade.CreateCustomer(Type);
            this._partyEntity = _customerEntity;
        }

        internal Customer(ICustomer customerEntity, IDataAccessFacade dataAccessFacade)
        {
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

        private ICustomer _customerEntity;
        private IDataAccessFacade dataAccessFacade;

    }
}
