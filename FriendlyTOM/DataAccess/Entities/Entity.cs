/*
Copyright 2014 The Friendly TOM Team (see AUTHORS.rst)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

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
