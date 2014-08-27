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
