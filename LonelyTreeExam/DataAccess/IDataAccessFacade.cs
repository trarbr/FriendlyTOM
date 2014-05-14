using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;
using Common.Interfaces;
using Common.Enums;

namespace DataAccess
{
   public interface IDataAccessFacade
   {
       IPayment CreatePayment(DateTime dueDate, decimal dueAmount, string responsible, string commissioner);
       List<IPayment> ReadAllPayments();
       void UpdatePayment(IPayment payment);
       void DeletePayment(IPayment payment);

       ISupplier CreateSupplier(string name, string note, string paymentInfo, SupplierType type);
       List<ISupplier> ReadAllSuppliers();
       void UpdateSupplier(ISupplier supplier);
       void DeleteSupplier(ISupplier supplier);

       ICustomer CreateCustomer(CustomerType type);
       List<ICustomer> ReadAllCustomers();
       void UpdateCustomers(ICustomer customer);
       void DeleteCustomer(ICustomer customer);
   }
}

