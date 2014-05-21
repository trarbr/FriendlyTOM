using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess.Entities;

namespace DataAccess.Mappers
{
    internal class PartyMapper
    {
        internal SupplierMapper SupplierMapper { get; set; }
        internal CustomerMapper CustomerMapper { get; set; }

        internal PartyMapper()
        {

        }
        internal APartyEntity Read(int id)
        {
            // check if supplier, if not get customer and return
            APartyEntity party = SupplierMapper.Read(id);
            if (party == null)
            {
                party = CustomerMapper.Read(id);
            }

            return party;
        }
    }
}
