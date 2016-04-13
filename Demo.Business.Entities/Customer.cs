using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Demo.Business.Entities
{
    [DataContract]
    public class Customer : EntityBase, IIdentifiableEntity, ICustomerOwnedEntity
    {
        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public string LoginEmail { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Hausnumber { get; set; }

        [DataMember]
        public string ZipCode { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string CreditCard { get; set; }

        [DataMember]
        public string ExpirationDate { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        #region IIdentifiableEntity

        public int EntityId
        {
            get { return this.CustomerId; }
            set { this.CustomerId = value; }
        }

        #endregion

        #region ICustomerOwnedEntity

        public int OwnerCustomerId => this.CustomerId;

        #endregion
    }
}
