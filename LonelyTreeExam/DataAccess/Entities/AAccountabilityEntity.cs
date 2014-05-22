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
        public IParty Responsible 
        {
            get { return _responsible; }
            set { _responsible = (APartyEntity)value; }
        }
        public IParty Commissioner
        {
            get { return _commissioner; }
            set { _commissioner = (APartyEntity)value; }
        }
        internal AAccountabilityEntity(IParty responsible, IParty commissioner) 
        {
            Responsible = responsible;
            Commissioner = commissioner;
            Note = "";
        }

        private APartyEntity _responsible;
        private APartyEntity _commissioner;
    }
}
