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

using Common.Enums;

namespace Common.Interfaces
{
    public interface IBooking 
    {
        ISupplier Supplier { get; set; }
        ICustomer Customer { get; set; }
        string Note { get; set; }
        string Sale { get; set; }
        int BookingNumber { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        BookingType Type { get; set; }
        decimal IVAExempt { get; set; }
        decimal IVASubject { get; set; }
        decimal Subtotal { get; set; }
        decimal Service { get; set; }
        decimal IVA { get; set; }
        decimal ProductRetention { get; set; }
        decimal SupplierRetention { get; set; }
        decimal TransferAmount { get; set; }
    }
}
