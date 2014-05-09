using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Interfaces;

namespace Domain.Model
{
    internal abstract class Accountability : IAccountability
    {
        #region public properties
        public string Note 
        {
            get { return _accountabilityEntity.Note; }
            set
            {
                _accountabilityEntity.Note = value;
            }
        }
        public string Responsible 
        {
            get { return _accountabilityEntity.Responsible; }
            set
            {
                chooseResponsible(value);
                _accountabilityEntity.Responsible = value;
            }
        }

        public string Commissioner 
        {
            get { return _accountabilityEntity.Commissioner; }
            set
            {
                chooseCommissioner(value);
                _accountabilityEntity.Commissioner = value;
            }
        }

        #endregion

        internal Accountability()
        { }

        internal IAccountability _accountabilityEntity;

        #region ValidateResponsibleAndCommissioner

        private void chooseResponsible(string value)
        {
            validateNullOrWhiteSpace(value, "Responsible");
        }

        private void chooseCommissioner(string value)
        {
            validateNullOrWhiteSpace(value, "Commissioner");
        }

        private void validateNullOrWhiteSpace(string text, string paramName)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentOutOfRangeException(paramName, "may not be empty");
            }
        }
        #endregion
    }
}
