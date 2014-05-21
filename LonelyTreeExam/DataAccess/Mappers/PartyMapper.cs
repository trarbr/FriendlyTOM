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
        internal PartyMapper()
        {

        }
        internal APartyEntity Read(int id)
        {
            // check if supplier, if not get customer and return
            APartyEntity party = supplierMapper.Read(id);
            if (party == null)
            {
                party = customerMapper.Read(id);
            }

            return party;
        }

        internal SupplierMapper supplierMapper { get; set; }
        internal CustomerMapper customerMapper { get; set; }
    }
}
