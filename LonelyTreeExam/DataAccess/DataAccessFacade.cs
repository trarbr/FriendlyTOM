using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using DataAccess.Entities;
using DataAccess.Mappers;
using Common.Enums;

namespace DataAccess
{
    public class DataAccessFacade : IDataAccessFacade
    {
        /// <summary>
        /// Initializes a DataAccessFacade for accessing a MS SQL database
        /// </summary>
        /// <param name="test">For integration tests, set test = true to use test database</param>
        public DataAccessFacade(bool test = false)
        {
            if (!test)
            {
                connectionString = File.ReadAllText("C:\\ConnectString.txt");
            }
            else
            {
                connectionString = 
                    @"Data Source=localhost\SQLEXPRESS;Initial Catalog=LTTEST;Integrated Security=True";
            }

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

            //paymentRuleMapper.ReadAll();
        }

        public static IDataAccessFacade GetInstance()
        {
            if (instance == null)
            {
                return instance = new DataAccessFacade();   
            }
                return instance;
        }

        #region Public Payment Methods
        /// <summary>
        /// Should initiate a connection and a readall procedure from the database.
        /// </summary>
        /// <returns>payments</returns>
        public List<IPayment> ReadAllPayments()
        {
            List<IPayment> payments = new List<IPayment>();
            List<PaymentEntity> paymentEntities = paymentMapper.ReadAll();
            foreach (PaymentEntity paymentEntity in paymentEntities)
            {
                payments.Add(paymentEntity);
            }

            return payments;
        }

        public IPayment CreatePayment(DateTime dueDate, decimal dueAmount, IParty payer,
            IParty payee, PaymentType type, string sale, int booking)
        {
            return paymentMapper.Create(dueDate, dueAmount, payer, payee, type, sale, booking);
        }
      
        public void UpdatePayment(IPayment payment)
        {
            paymentMapper.Update((PaymentEntity) payment);
        }

        public void DeletePayment(IPayment payment)
        {
            paymentMapper.Delete((PaymentEntity) payment);
        }
        #endregion

        
        #region Public Supplier Methods
        public ISupplier CreateSupplier(string name, string note, SupplierType type)
        {
            return supplierMapper.Create(name, note, type);
        }

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
            supplierMapper.Update((SupplierEntity)supplier);
            foreach (IPaymentRule paymentRule in supplier.PaymentRules)
            {
                paymentRuleMapper.Update((PaymentRuleEntity)paymentRule);
            }
        }

        public void DeleteSupplier(ISupplier supplier)
        {
            SupplierEntity sup = supplier as SupplierEntity;
            supplierMapper.Delete(sup);
        }
        #endregion

        #region Public Customer Methods
        public ICustomer CreateCustomer(CustomerType type, string note, string name)
        {
            return customerMapper.Create(type, note, name);
        }

        public List<ICustomer> ReadAllCustomers()
        {
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
           customerMapper.Update((CustomerEntity) customer);
        }

        public void DeleteCustomer(ICustomer customer)
        {
            customerMapper.Delete((CustomerEntity) customer);
        }
        #endregion

        #region Booking Methods
        public IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber, DateTime startDate, DateTime endDate)
        {
            return bookingMapper.Create(supplier, customer, sale, bookingNumber, startDate, endDate);
        }

        public List<IBooking> ReadAllBookings()
        {
            List<IBooking> bookings = new List<IBooking>();
            List<BookingEntity> bookingEntities = bookingMapper.ReadAll();

            foreach (BookingEntity bookingEntity in bookingEntities)
            {
                bookings.Add(bookingEntity);
            }

            return bookings;
        }

        public void UpdateBookings(IBooking booking)
        {
            bookingMapper.Update((BookingEntity)booking);
        }

        public void DeleteBooking(IBooking booking)
        {
            bookingMapper.Delete((BookingEntity)booking);
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


        public IPaymentRule CreatePaymentRule(ISupplier supplierEntity, ICustomer customerEntity, 
            BookingType bookingType, decimal percentage, int daysOffset, BaseDate baseDate, PaymentType paymentType)
        {
            SupplierEntity s = (SupplierEntity)supplierEntity;
            CustomerEntity c = (CustomerEntity)customerEntity;

            return paymentRuleMapper.Create(s, c, bookingType, percentage, daysOffset, baseDate, paymentType);
        }

        public List<IPaymentRule> ReadAllPaymentRules()
        {
            throw new NotImplementedException();
        }

        public void UpdatePaymentRule(IPaymentRule paymentRuleEntity)
        {
            paymentRuleMapper.Update((PaymentRuleEntity)paymentRuleEntity);
        }

        public void DeletePaymentRule(IPaymentRule paymentRuleEntity)
        {
            paymentRuleMapper.Delete((PaymentRuleEntity)paymentRuleEntity);
        }
    }
}
