using Common.Interfaces;
using System;

namespace Domain.Model
{
    internal class AParty : IParty
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

        #region Validation
        //Validates the name is not empty.
        protected void validateName(string value)
        {
            string paramName = "Name";
            validateNullOrWhiteSpace(value, paramName);
            validateTextLength(value, paramName);
        }

        //validation method for checking for whitespace or null. 
        protected void validateNullOrWhiteSpace(string text, string paramName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be empty");
            }
        }

        //Validation method for checking the length of a string.
        private void validateTextLength(string text, string paramName)
        {
            if (text.Length > 100)
            {
                throw new ArgumentOutOfRangeException(paramName, "text may not be over 100 caracters");
            }
        }
        #endregion

        internal IParty _partyEntity { get; set; }
    }
}
