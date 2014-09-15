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

using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPayment
    {
        IParty Payee { get; set; }
        IParty Payer { get; set; }
        string Note { get; set; }
        DateTime DueDate { get; set; }
        decimal DueAmount { get; set; }
        DateTime PaidDate { get; set; }
        decimal PaidAmount { get; set; }
        IReadOnlyList<string> Attachments { get; }
        bool Paid { get; set; }
        bool Archived { get; set; }
        PaymentType Type { get; set; }
        string Sale { get; set; }
        int Booking { get; set; }
        string Invoice { get; set; }

        void DeleteAttachment(string attachment);
        void AddAttachment(string attachment);
    }
}
