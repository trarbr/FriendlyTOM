﻿using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal class SupplierEntity : APartyEntity, ISupplier
    {
        public string PaymentInfo { get; set; }
        public SupplierType Type { get; set; }

        public SupplierEntity(string paymentInfo, SupplierType type, string note, string name) 
            :base(note, name)
        {
            PaymentInfo = paymentInfo;
            Type = type;
        }
    }
}
