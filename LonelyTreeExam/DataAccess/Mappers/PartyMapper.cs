// PI
using DataAccess.Entities;

namespace DataAccess.Mappers
{
    internal class PartyMapper
    {
        #region Internal Properties
        internal SupplierMapper SupplierMapper { get; set; }
        internal CustomerMapper CustomerMapper { get; set; }
        #endregion

        #region internal Methods
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
        #endregion
    }
}
