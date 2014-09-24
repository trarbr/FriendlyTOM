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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;
using Common.Enums;

namespace DataAccess.Entities
{
    internal class PaymentEntity : Entity, IPayment
    {
        #region Public Properties
        public IParty Payee 
        {
            get { return _payee; }
            set { _payee = (APartyEntity)value; }
        }
        public IParty Payer 
        {
            get { return _payer; }
            set { _payer = (APartyEntity)value; }
        }
        public string Note { get; set; }
        public DateTime DueDate { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime PaidDate { get; set; }
        public decimal PaidAmount { get; set; }
        public bool Paid { get; set; }
        public bool Archived { get; set; }
        public PaymentType Type { get; set; }
        public string Sale { get; set; }
        public int Booking { get; set; }
        public string Invoice { get; set; }
        public IReadOnlyList<string> Attachments
        {
            get { return _attachments; }
        }
        #endregion

        #region Public Methods
        public PaymentEntity(DateTime dueDate, decimal dueAmount, IParty payer, 
            IParty payee, PaymentType type, string sale, int booking) 
        {
            _attachments = new List<string>();

            Payee = payee;
            Payer = payer;
            Note = "";
            DueDate = dueDate;
            DueAmount = dueAmount;
            PaidDate = new DateTime(1900,01,01);
            PaidAmount = 0m;
            Paid = false;
            Archived = false;
            Type = type;
            Sale = sale;
            Booking = booking;
            Invoice = "";
        }

        public void DeleteAttachment(string attachment)
        {
            _attachments.Remove(attachment);
        }

        public void AddAttachment(string attachment)
        {
            _attachments.Add(attachment);
        }
        #endregion

        private APartyEntity _payer;
        private APartyEntity _payee;
        private List<string> _attachments;
    }
}
