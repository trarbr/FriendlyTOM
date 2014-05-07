using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IAccountability
    {
        string Commisioner { get; set; }
        string Responsible { get; set; }
        string Note { get; set; }
        string Status { get; set; }
    }
}
