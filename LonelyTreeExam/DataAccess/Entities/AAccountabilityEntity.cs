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
        public IParty Responsible { get; set; }
        public IParty Commissioner { get; set; }

        internal AAccountabilityEntity(IParty responsible, IParty commissioner) 
        {
            Responsible = responsible;
            Commissioner = commissioner;
            Note = "";
        }
    }
}
