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
        private Dictionary<ISupplier, Supplier> suppliers;
        private Dictionary<ICustomer, Customer> customers;
        private static Register _register;

        private Register()
        {
            suppliers = new Dictionary<ISupplier, Supplier>();
            customers = new Dictionary<ICustomer, Customer>();
        }

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
    }
}
