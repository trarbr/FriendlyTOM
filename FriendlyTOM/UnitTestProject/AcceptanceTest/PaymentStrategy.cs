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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common.Enums;
using Common.Interfaces;
using Domain.Controller;
using DataAccess;
using System.Collections.Generic;
using System.Globalization;

namespace UnitTestProject.AcceptanceTest
{
    [TestClass]
    public class PaymentStrategy
    {
        CultureInfo culture;
        IDataAccessFacade dataAccessFacade;
        CustomerController customerController;
        SupplierController supplierController;
        PaymentController paymentController;
        BookingController bookingController;

        [TestInitialize]
        public void Initialize()
        {
            culture = new CultureInfo("en-US"); // Used for comparing decimals with strings

            dataAccessFacade = new DataAccessFacadeStub();
            customerController = new CustomerController(dataAccessFacade);
            supplierController = new SupplierController(dataAccessFacade);
            paymentController = new PaymentController(dataAccessFacade);
            bookingController = new BookingController(dataAccessFacade);

            customerController.Initialize(bookingController, paymentController, supplierController);
            supplierController.Initialize(bookingController, paymentController);
            bookingController.Initialize(customerController, paymentController);
            paymentController.Initialize();


            ICustomer lonelyTree = customerController.CreateCustomer("Lonely Tree", "", CustomerType.Bureau);
        }

        [TestMethod]
        public void TestVFJan13()
        {
            // Test scenario: Viktors Farmor booking 13 for sale VF Jan with Estrella Chimborazo
            ICustomer viktorsFarmor = customerController.CreateCustomer("Viktors Farmor", "", CustomerType.Bureau);
            ISupplier estrellaChimborazo = supplierController.CreateSupplier("Estrella Chimborazo", "", SupplierType.Restaurant);

            // PaymentRule to use for the booking
            supplierController.AddPaymentRule(estrellaChimborazo, viktorsFarmor, BookingType.Group, 100, -1,
                BaseDate.StartDate, PaymentType.Full); 

            // Dummy PaymentRule (must not be applied)
            supplierController.AddPaymentRule(estrellaChimborazo, viktorsFarmor, BookingType.FIT, 100, 14,
                BaseDate.EndDate, PaymentType.Full);

            IBooking booking = bookingController.CreateBooking(estrellaChimborazo, viktorsFarmor, "VF Jan", 13,
                new DateTime(2014, 01, 14), new DateTime(2014, 01, 14));
            booking.Type = BookingType.Group;
            booking.IVAExempt = 0m;
            booking.IVASubject = 378m;
            booking.ProductRetention = 2m;
            booking.SupplierRetention = 70m;

            bookingController.CalculatePaymentsForBooking(booking);

            // Check that TransferAmount and IVA match real world data
            string expectedTransferAmount = "384.05";
            string expectedIVA = "45.36";

            Assert.AreEqual(expectedTransferAmount, booking.TransferAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedIVA, booking.IVA.ToString("N2", culture.NumberFormat));

            // Check that correct number of payments was created
            int expectedNumberOfPayments = 1;
            List<IPayment> createdPayments = paymentController.ReadAllPayments();
            Assert.AreEqual(expectedNumberOfPayments, createdPayments.Count);

            // Check that Payment stats set by PaymentRules match real world data
            IPayment payment = createdPayments[0];
            string expectedDueAmount = "384.05";
            DateTime expectedDueDate = new DateTime(2014, 01, 13);

            Assert.AreEqual(expectedDueAmount, payment.DueAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedDueDate, payment.DueDate);
        }

        [TestMethod]
        public void TestSvaneRejserJosefsen3()
        {
            // Test scenario: Svane Rejser booking 3 for sale Svane Rejser Josefsen with NatureGalapagos
            ICustomer svaneRejser = customerController.CreateCustomer("Svane Rejser", "", CustomerType.Bureau);
            ISupplier natureGalapagos = supplierController.CreateSupplier("NatureGalapagos", "", SupplierType.Cruise);

            // PaymentRules to use for the booking
            supplierController.AddPaymentRule(natureGalapagos, svaneRejser, BookingType.Group, 30, -272,
                BaseDate.StartDate, PaymentType.Deposit);
            supplierController.AddPaymentRule(natureGalapagos, svaneRejser, BookingType.Group, 30, -90,
                BaseDate.StartDate, PaymentType.Deposit);
            supplierController.AddPaymentRule(natureGalapagos, svaneRejser, BookingType.Group, 40, -60,
                BaseDate.StartDate, PaymentType.Balance);

            // Dummy PaymentRule (must not be applied)
            supplierController.AddPaymentRule(natureGalapagos, svaneRejser, BookingType.Undefined, 100, 0,
                BaseDate.StartDate, PaymentType.Full);

            IBooking booking = bookingController.CreateBooking(natureGalapagos, svaneRejser, "Svane Rejser Josefsen",
                3, new DateTime(2014, 6, 5), new DateTime(2014, 6, 12));
            booking.Type = BookingType.Group;
            booking.IVAExempt = 1625m;
            booking.IVASubject = 0m;
            booking.ProductRetention = 0m;
            booking.SupplierRetention = 0m;

            bookingController.CalculatePaymentsForBooking(booking);

            // Check that TransferAmount and IVA match real world data
            string expectedTransferAmount = "1,625.00";
            string expectedIVA = "0.00";

            Assert.AreEqual(expectedTransferAmount, booking.TransferAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedIVA, booking.IVA.ToString("N2", culture.NumberFormat));

            // Check that correct number of payments was created
            int expectedNumberOfPayments = 3;
            List<IPayment> createdPayments = paymentController.ReadAllPayments();
            Assert.AreEqual(expectedNumberOfPayments, createdPayments.Count);

            // Check that Payment stats set by PaymentRules match real world data
            IPayment payment1 = createdPayments[0];
            string expectedDueAmount = "487.50";
            DateTime expectedDueDate = new DateTime(2013, 9, 6);

            Assert.AreEqual(expectedDueAmount, payment1.DueAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedDueDate, payment1.DueDate);

            IPayment payment2 = createdPayments[1];
            expectedDueAmount = "487.50";
            expectedDueDate = new DateTime(2014, 3, 7);

            Assert.AreEqual(expectedDueAmount, payment2.DueAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedDueDate, payment2.DueDate);

            IPayment payment3 = createdPayments[2];
            expectedDueAmount = "650.00";
            expectedDueDate = new DateTime(2014, 4, 6);

            Assert.AreEqual(expectedDueAmount, payment3.DueAmount.ToString("N2", culture.NumberFormat));
            Assert.AreEqual(expectedDueDate, payment3.DueDate);

        }
    }
}
