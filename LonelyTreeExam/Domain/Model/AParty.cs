using Common.Interfaces;
using System;

namespace Domain.Model
{
    internal abstract class AParty : IParty
    {
        #region Public Properties
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
        #endregion

        #region Protected Methods
        protected void initializeParty(IParty partyEntity)
        {
            _partyEntity = partyEntity;
        }
        #endregion

        #region Validation
        //Validates the name is not empty.
        protected void validateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException("Name", "may not be empty");
            }
        }
        #endregion

        #region Internal Fields
        internal IParty _partyEntity;
        #endregion
    }
}
