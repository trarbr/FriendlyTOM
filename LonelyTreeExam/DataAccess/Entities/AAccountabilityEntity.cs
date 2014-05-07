using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal abstract class AAccountabilityEntity : Entity, IAccountability 
    {
        public string Note { get; set; }
        public string Responsible { get; set; }
        public string Commissioner { get; set; }

        internal AAccountabilityEntity(string responsible, string commissioner) 
        {
            Responsible = responsible;
            Commissioner = commissioner;
            Note = "";
        }
    }
}
