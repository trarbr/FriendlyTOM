using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal class Entity
    {
        #region Internal Properties
        internal int Id { get; set; }
        internal DateTime LastModified { get; set; }
        internal bool Deleted { get; set; }
        #endregion

        #region Constructors
        internal Entity(int id, DateTime lastModified, bool deleted)
        {
            Id = id;
            LastModified = lastModified;
            Deleted = deleted;
        }

        internal Entity() : this(0, DateTime.MinValue, false){}
        #endregion
    }
}
