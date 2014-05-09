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
            set { _accountabilityEntity.Note = value; }
        }
        public string Responsible 
        {
            get { return _accountabilityEntity.Responsible; }
            set { _accountabilityEntity.Responsible = value; }
        }
        public string Commissioner 
        {
            get { return _accountabilityEntity.Commissioner; }
            set { _accountabilityEntity.Commissioner = value; }
        }

        #endregion

        internal Accountability()
        { }

        internal IAccountability _accountabilityEntity;


    }
}
