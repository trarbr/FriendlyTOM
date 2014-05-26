using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Validation
        protected void validateName(string paramName)
        {
            if (paramName == "")
            {
                throw new ArgumentOutOfRangeException(paramName, "Name may not be empty");
            }
        }
        #endregion

        #region Internal Fields
        internal IParty _partyEntity;
        #endregion
    }
}
