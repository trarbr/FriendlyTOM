using Common.Enums;
using Common.Interfaces;

namespace DataAccess.Entities
{
    internal class SupplierEntity : APartyEntity, ISupplier
    {
        #region Public Properties
        public SupplierType Type { get; set; }
        public string AccountNo { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountName { get; set; }
        public string OwnerId { get; set; }
        public string Bank { get; set; }
        #endregion

        #region Constructor
        public SupplierEntity(SupplierType type, string note, string name) 
            :base(note, name)
        {
            Type = type;
            AccountNo = "";
            AccountType = AccountType.Undefined;
            AccountName = "";
            OwnerId = "";
            Bank = "";
        }
        #endregion
    }
}
