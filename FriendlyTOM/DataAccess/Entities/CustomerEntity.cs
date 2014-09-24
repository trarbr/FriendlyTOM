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

using Common.Enums;
using Common.Interfaces;

namespace DataAccess.Entities
{
    internal class CustomerEntity : APartyEntity, ICustomer
    {
        #region Public Properties
        public CustomerType Type { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string FaxNo { get; set; }
        #endregion

        #region Constructor
        public CustomerEntity(CustomerType type, string note, string name)
            : base(note, name)
        {
            Type = type;
            ContactPerson = "";
            Email = "";
            Address = "";
            PhoneNo = "";
            FaxNo = "";
        }
        #endregion
    }
}
