using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal abstract class APartyEntity : Entity, IParty
    {
        public string Name { get; set; }
        public string Note { get; set; }

        internal APartyEntity(string note, string name)
        {
            Note = note;
            Name = name;
        }

     }
}
