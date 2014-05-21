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
            get { return _responsible; }
            set
            {
                validateResponsible(value);
                //_accountabilityEntity.Responsible = value;
                _responsible = (AParty)value;
                _accountabilityEntity.Responsible = _responsible._partyEntity;
            }
        }

        public IParty Commissioner 
        {
            get { return _commissioner; }
            set
            {
                validateCommissioner(value);
                _commissioner = (AParty)value;
                _accountabilityEntity.Commissioner = _commissioner._partyEntity;
            }
        }

        #endregion

        internal AAccountability()
        { }

        internal IAccountability _accountabilityEntity;

        #region ValidateAllProperties

        protected void validateResponsible(IParty value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Payer was not found");
            }
        }

        protected void validateCommissioner(IParty value)
        {
            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Payee was not found");
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

        internal AParty _responsible;
        internal AParty _commissioner;
    }
}
