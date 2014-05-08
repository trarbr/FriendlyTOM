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
    }
}
