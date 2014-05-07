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
        public string Note { get; set; }
        public string Responsible { get; set; }
        public string Commissioner { get; set; }

        #endregion

        public Accountability(string responsible, string commissioner)
        {
            Responsible = responsible;
            Commissioner = commissioner;
        }


    }
}
