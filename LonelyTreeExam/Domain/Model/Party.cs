using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Party : IParty
    {
        public string Name { get; set; }
        public string Note { get; set; }
    }
}
