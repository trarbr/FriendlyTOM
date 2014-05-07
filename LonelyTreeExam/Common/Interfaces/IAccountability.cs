using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAccountability
    {
        string Status { get; set; }
        string Note { get; set; }
        string Responsible { get; set; }
        string Commissioner { get; set; }
    }
}
