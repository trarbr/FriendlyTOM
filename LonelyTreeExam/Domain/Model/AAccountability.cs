using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Domain.Model
{
    internal abstract class AAccountability : IAccountability
    {
        #region public properties
        public string Note 
        {
            get { return _accountabilityEntity.Note; }
            set
            {
                validateNoteLength(value);
                _accountabilityEntity.Note = value;
            }
        }
        public IParty Responsible 
        {
            get { return _accountabilityEntity.Responsible; }
            set
            {
                //validateResponsible(value);
                _accountabilityEntity.Responsible = value;
            }
        }

        public IParty Commissioner 
        {
            get { return _accountabilityEntity.Commissioner; }
            set
            {
                //validateCommissioner(value);
                _accountabilityEntity.Commissioner = value;
            }
        }

        #endregion

        internal AAccountability()
        { }

        internal IAccountability _accountabilityEntity;

        #region ValidateAllProperties

        protected void validateResponsible(string value)
        {
            validateNullOrWhiteSpace(value, "Responsible");
        }

        protected void validateCommissioner(string value)
        {
            validateNullOrWhiteSpace(value, "Commissioner");
        }

        protected void validateNullOrWhiteSpace(string text, string paramName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be empty");
            }
            validateTextLength(text, paramName);
        }

        protected void validateNoteLength(string value)
        {
            string paramName = "Note";
            if (value.Length > 2000)
            {
                throw new ArgumentOutOfRangeException(paramName, "text may not be over 2000 caracters");
            }
        }

        private void validateTextLength(string text, string paramName)
        {
            if (text.Length > 100)
            {
                throw new ArgumentOutOfRangeException(paramName, "text may not be over 100 caracters");
            }
        }
        #endregion
    }
}
