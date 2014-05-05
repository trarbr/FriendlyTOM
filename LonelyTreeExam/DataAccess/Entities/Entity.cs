using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    internal class Entity
    {
        internal int Id { get; set; }
        internal DateTime LastModified { get; set; }
        internal bool Deleted { get; set; }

        public Entity(int id, DateTime lastModified, bool deleted)
        {
            Id = id;
            LastModified = lastModified;
            Deleted = deleted;
        }
    }
}
