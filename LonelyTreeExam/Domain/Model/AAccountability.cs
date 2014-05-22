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
                _responsible = (Party)value;
                _accountabilityEntity.Responsible = _responsible._partyEntity;
            }
        }

        public IParty Commissioner 
        {
            get { return _commissioner; }
            set
            {
                validateCommissioner(value);
                _commissioner = (Party)value;
                _accountabilityEntity.Commissioner = _commissioner._partyEntity;
            }
        }

        #endregion

        internal AAccountability()
        { }

        internal IAccountability _accountabilityEntity;

        protected void initializeAccountability(IAccountability accountabilityEntity, IParty responsible, 
            IParty commissioner)
        {
            _accountabilityEntity = accountabilityEntity;
            Responsible = responsible;
            Commissioner = commissioner;
        }

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

        private Party _responsible;
        private Party _commissioner;
    }
}
