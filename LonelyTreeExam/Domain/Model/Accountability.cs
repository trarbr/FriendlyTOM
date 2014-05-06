using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    internal class Accountability
    {
        #region public properties
        public string Status { get; set; }
        public string Note { get; set; }
        public string Responsible { get; set; }
        public string Commisioner { get; set; }

        #endregion
    }
}
