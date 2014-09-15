using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess;

namespace Domain.Controller
{
    public class SettingsController
    {
        public void BackupDatabase(string backupPath)
        {
            IDataAccessFacade dataAccessFacade = DataAccessFacade.GetInstance();

        }
    }
}
