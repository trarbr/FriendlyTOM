using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    internal static class StoredProcedures
    {
        internal const string READ_ALL_PAYMENTS = "ReadAllPayments";
        internal const string CREATE_PAYMENT = "InsertPayment";
        internal const string UPDATE_PAYMENT = "UpdatePayment";

        internal const string READ_ALL_CUSTOMERS = "ReadAllCustomers";
        internal const string CREATE_CUSTOMER = "InsertCustomer";
        internal const string UPDATE_CUSTOMER = "UpdateCustomer";

        internal const string READ_ALL_SUPPLIERS = "ReadAllSuppliers";
        internal const string CREATE_SUPPLIER = "InsertSupplier";
        internal const string UPDATE_SUPPLIER = "UpdateSupplier";
    }
}
