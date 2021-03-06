﻿/*
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

using Common.Interfaces;
using DataAccess;
using Domain.Collections;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Controller
{
    public class BookingController
    {
        #region Setup
        internal BookingController(IDataAccessFacade dataAccessFacade)
        {
            this.dataAccessFacade = dataAccessFacade;
        }

        internal void Initialize(CustomerController customerController, PaymentController paymentController)
        {
            this.customerController = customerController;
            this.paymentController = paymentController;
            bookingCollection = new BookingCollection(dataAccessFacade);
        }
        #endregion

        #region CRUD
        public IBooking CreateBooking(ISupplier supplier, ICustomer customer, string sale, int bookingNumber,
            DateTime StartDate, DateTime EndDate)
        {
            //Calls Bookingcollection class for create
            return bookingCollection.Create((Supplier)supplier, (Customer)customer, sale, bookingNumber, StartDate, 
                EndDate);
        }

        public List<IBooking> ReadAllBookings()
        {
            //Calls Bookingcollection class for readall
            List<IBooking> bookings = new List<IBooking>();
            foreach (Booking booking in bookingCollection.ReadAll())
            {
                bookings.Add(booking);
            }
            return bookings;
        }

        public void UpdateBooking(IBooking booking)
        {
            var bookingModel = (Booking)booking;
            bookingModel.CalculateAmounts();
            bookingCollection.Update((Booking) booking);
        }

        public void DeleteBooking(IBooking booking)
        {
            //Calls Bookingcollection class for delete
            bookingCollection.Delete((Booking) booking);
        }

        internal void DeleteBookingsForParty(AParty party)
        {
            var bookingsToDelete = ReadAllBookings()
                .Where<IBooking>(b => b.Supplier == party || b.Customer == party);
            foreach (var booking in bookingsToDelete)
            {
                DeleteBooking(booking);
            }
        }
        #endregion

        public void CreatePaymentsForBooking(IBooking booking)
        {
            Booking bookingModel = (Booking)booking;
            PaymentStrategy paymentStrategy = new PaymentStrategy(customerController);
            paymentStrategy.CreatePayments(bookingModel, paymentController);
        }

        private IDataAccessFacade dataAccessFacade;
        private BookingCollection bookingCollection;
        private PaymentController paymentController;
        private CustomerController customerController;

    }
}
