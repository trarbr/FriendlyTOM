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

namespace DataAccess.Helpers
{
    internal static class StoredProcedures
    {
        #region Payment
        internal const string READ_ALL_PAYMENTS = "ReadAllPayments";
        internal const string CREATE_PAYMENT = "InsertPayment";
        internal const string UPDATE_PAYMENT = "UpdatePayment";
        #endregion

        #region Customer
        internal const string READ_ALL_CUSTOMERS = "ReadAllCustomers";
        internal const string CREATE_CUSTOMER = "InsertCustomer";
        internal const string UPDATE_CUSTOMER = "UpdateCustomer";
        #endregion

        #region Supplier
        internal const string READ_ALL_SUPPLIERS = "ReadAllSuppliers";
        internal const string CREATE_SUPPLIER = "InsertSupplier";
        internal const string UPDATE_SUPPLIER = "UpdateSupplier";
        #endregion

        #region Booking
        internal const string READ_ALL_BOOKINGS = "ReadAllBookings";
        internal const string CREATE_BOOKING = "InsertBooking";
        internal const string UPDATE_BOOKING = "UpdateBooking";
        #endregion

        #region PaymentRules
        internal const string READ_ALL_PAYMENT_RULES = "ReadAllPaymentRules";
        internal const string CREATE_PAYMENT_RULE = "InsertPaymentRule";
        internal const string UPDATE_PAYMENT_RULE = "UpdatePaymentRule";
        #endregion
    }
}
