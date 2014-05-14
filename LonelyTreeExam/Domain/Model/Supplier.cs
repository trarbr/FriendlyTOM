﻿using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    internal class Supplier : ISupplier
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string PaymentInfo { get; set; }
        public SupplierType Type { get; set; }

        internal Supplier(string name, SupplierType type)
        {
            
        }
    }
}
