// LB
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
