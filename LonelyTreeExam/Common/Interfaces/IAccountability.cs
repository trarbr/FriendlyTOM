using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAccountability
    {
        string Note { get; set; }
        IParty Responsible { get; set; }
        IParty Commissioner { get; set; }
    }
}
