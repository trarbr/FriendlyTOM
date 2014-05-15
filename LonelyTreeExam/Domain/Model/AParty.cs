using Common.Interfaces;
using System;

namespace Domain.Model
{
    internal abstract class AParty : IParty
    {
        public string Name
        {
            get { return _partyEntity.Name; }
            set
            {
                validateName(value);
                _partyEntity.Name = value;
            }
        }

        public string Note
        {
            get { return _partyEntity.Note; }
            set
            {
                _partyEntity.Note = value;
            }
        }

        #region Validation
        protected void validateName(string paramName)
        {
            if (paramName == "")
            {
                throw new ArgumentOutOfRangeException(paramName, "Name may not be empty");
            }
        }
        #endregion

        internal IParty _partyEntity;
    }
}
