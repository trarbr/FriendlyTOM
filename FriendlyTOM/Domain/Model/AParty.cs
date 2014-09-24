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

using Common.Interfaces;
using System;

namespace Domain.Model
{
    internal abstract class AParty : IParty
    {
        #region Public Properties
        public string Name
        {
            get { return _partyEntity.Name; }
            set
            {
                validateName(value);
                _partyEntity.Name = value;
            }
        }

        public string Note
        {
            get { return _partyEntity.Note; }
            set
            {
                _partyEntity.Note = value;
            }
        }
        #endregion

        #region Protected Methods
        protected void initializeParty(IParty partyEntity)
        {
            _partyEntity = partyEntity;
        }
        #endregion

        #region Validation
        //Validates the name is not empty.
        protected void validateName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException("Name", "may not be empty");
            }
        }
        #endregion

        #region Internal Fields
        internal IParty _partyEntity;
        #endregion
    }
}
