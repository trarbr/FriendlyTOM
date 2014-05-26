using System;
using System.Collections.Generic;
using System.IO;
using Common.Interfaces;
using DataAccess.Entities;
using DataAccess.Mappers;
using Common.Enums;

namespace DataAccess
{
    public class DataAccessFacade : IDataAccessFacade
    {
        #region Public Constructor
        /// <summary>
        /// Initializes a DataAccessFacade for accessing a MS SQL database
        /// </summary>
        /// <param name="test">For integration tests, set test = true to use test database</param>
        public DataAccessFacade(bool test = false)
        {
            if (!test)
            {
                //take the database information from a textfile.
                connectionString = File.ReadAllText("C:\\ConnectString.txt");
            }
            else
            {
                connectionString = 
                    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=LTTEST;Integrated Security=True";
            }

            //Creates a new instance of the mappers with the connection information
            paymentMapper = new PaymentMapper(connectionString);
            customerMapper = new CustomerMapper(connectionString);
            supplierMapper = new SupplierMapper(connectionString);
            bookingMapper = new BookingMapper(connectionString);
            paymentRuleMapper = new PaymentRuleMapper(connectionString);

            PartyMapper partyMapper = new PartyMapper();

            partyMapper.CustomerMapper = customerMapper;
            partyMapper.SupplierMapper = supplierMapper;

            paymentMapper.PartyMapper = partyMapper;
            bookingMapper.CustomerMapper = customerMapper;
            bookingMapper.SupplierMapper = supplierMapper;
            paymentRuleMapper.CustomerMapper = customerMapper;
            paymentRuleMapper.SupplierMapper = supplierMapper;

            customerMapper.ReadAll();
            supplierMapper.ReadAll();
        }

        public static IDataAccessFacade GetInstance()
        {
            if (instance == null)
            {
                return instance = new DataAccessFacade();   
            }
                return instance;
        }
        #endregion

        #region Public Payment Methods
        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking)
        {
            //Calls create in the mapper and gives it the neccessary parameters.
            return paymentMapper.Create(dueDate, dueAmount, payer, payee, type, sale, booking);
        }

        /// <summary>
        /// Should initiate a connection and a readall procedure from the database.
        /// </summary>
        /// <returns>payments</returns>
        public List<IPayment> ReadAllPayments()
        {
            //Calls readall in the mapper and add those objects of payment to a list with payments and returns it. 
            List<IPayment> payments = new List<IPayment>();
            List<PaymentEntity> paymentEntities = paymentMapper.ReadAll();
            foreach (PaymentEntity paymentEntity in paymentEntities)
            {
                payments.Add(paymentEntity);
            }

            return payments;
        }

        public void UpdatePayment(IPayment payment)
        {
            //Calls update for a specific payment.
            paymentMapper.Update((PaymentEntity) payment);
        }

        public void DeletePayment(IPayment payment)
        {
            //Calls the delete for a specific payment.
            paymentMapper.Delete((PaymentEntity) payment);
        }
        #endregion
        
        #region Public Supplier Methods
        public ISupplier CreateSupplier(string name, string note, SupplierType type)
        {
            //Calls Create and gives it the neccessary parameters. 
            return supplierMapper.Create(name, note, type);
        }

        //Calls readall in the mapper and add those objects of supplier to a list with payments and returns it. 
        public List<ISupplier> ReadAllSuppliers()
        {
            List<ISupplier> suppliers = new List<ISupplier>();
            List<SupplierEntity> supplierEntities = supplierMapper.ReadAll();
            paymentRuleMapper.ReadAll(); // populate SupplierEntities with PaymentRuleEntities
            foreach (SupplierEntity supplierEntity in supplierEntities)
            {
                suppliers.Add(supplierEntity);
            }


            return suppliers;
        }

        public void UpdateSupplier(ISupplier supplier)
        {
            //Calls update for a specific supplier.
            supplierMapper.Update((SupplierEntity)supplier);
            foreach (IPaymentRule paymentRule in supplier.PaymentRules)
            {
                paymentRuleMapper.Update((PaymentRuleEntity)paymentRule);
            }
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            //Calls delete on a specific supplier. 
            SupplierEntity sup = supplier as SupplierEntity;
            supplierMapper.Delete(sup);
        }
        #endregion

        #region Public Customer Methods
        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            //Calls create in the mapper and gives it the neccessary parameters.
            return customerMapper.Create(type, note, name);
        }

        public List<ICustomer> ReadAllCustomers()
        {
            //Calls readall in the mapper and add those objects of customer to a list with Customers and returns it.
            List<ICustomer> customers = new List<ICustomer>();
           List<CustomerEntity> customerEntities = customerMapper.ReadAll();

           foreach (CustomerEntity customerEntity in customerEntities)
           {
               customers.Add(customerEntity);
           }

           return customers;
        }

        public void UpdateCustomers(ICustomer customer)
        {
            //Calls update for a specific customer.
           customerMapper.Update((CustomerEntity) customer);
        }

        public void DeleteCustomer(ICustomer customer)
        {
            //Calls the delete for a specific customer.
            customerMapper.Delete((CustomerEntity) customer);
        }
        #endregion

        #region Public Booking Methods
        public IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber, 
            DateTime startDate, DateTime endDate)
        {
            //Calls create in the mapper and gives it the neccessary parameters.
            return bookingMapper.Create(supplier, customer, sale, bookingNumber, startDate, endDate);
        }

        public List<IBooking> ReadAllBookings()
        {
            //Calls readall in the mapper and add those objects of Bookings to a list with bookings and returns it.
            List<IBooking> bookings = new List<IBooking>();
            List<BookingEntity> bookingEntities = bookingMapper.ReadAll();

            foreach (BookingEntity bookingEntity in bookingEntities)
            {
                bookings.Add(bookingEntity);
            }

            return bookings;
        }

        public void UpdateBooking(IBooking booking)
        {
            //Calls update for a specific Booking.
            bookingMapper.Update((BookingEntity)booking);
        }

        public void DeleteBooking(IBooking booking)
        {
            //Calls the delete for a specific Booking.
            bookingMapper.Delete((BookingEntity)booking);
        }
        #endregion

        #region Public PaymentRule Methods
        public IPaymentRule CreatePaymentRule(ISupplier supplierEntity, ICustomer customerEntity, 
            BookingType bookingType, decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            SupplierEntity s = (SupplierEntity)supplierEntity;
            CustomerEntity c = (CustomerEntity)customerEntity;

            return paymentRuleMapper.Create(s, c, bookingType, percentage, daysOffset, baseDate, paymentType);
        }

        public void UpdatePaymentRule(IPaymentRule paymentRuleEntity)
        {
            paymentRuleMapper.Update((PaymentRuleEntity)paymentRuleEntity);
        }

        public void DeletePaymentRule(IPaymentRule paymentRuleEntity)
        {
            paymentRuleMapper.Delete((PaymentRuleEntity)paymentRuleEntity);
        }
        #endregion

        #region Private Properties
        private string connectionString;
        private PaymentMapper paymentMapper;
        private CustomerMapper customerMapper;
        private SupplierMapper supplierMapper;
        private BookingMapper bookingMapper;
        private PaymentRuleMapper paymentRuleMapper;
        private static DataAccessFacade instance;
        #endregion
    }
}
