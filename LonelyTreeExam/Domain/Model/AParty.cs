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

        protected void validateNullOrWhiteSpace(string text, string paramName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be empty");
            }
            validateTextLength(text, paramName);
        }

        private void validateTextLength(string text, string paramName)
        {
            if (text.Length > 100)
            {
                throw new ArgumentOutOfRangeException(paramName, "text may not be over 100 caracters");
            }
        }
        #endregion

        internal IParty _partyEntity;
    }
}
