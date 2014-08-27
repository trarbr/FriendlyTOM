// MM
using Common.Interfaces;

namespace DataAccess.Entities
{
    internal abstract class APartyEntity : Entity, IParty
    {
        #region Public Properties
        public string Name { get; set; }
        public string Note { get; set; }
        #endregion

        #region Constructor
        internal APartyEntity(string note, string name)
        {
            Note = note;
            Name = name;
        }
        #endregion
    }
}
