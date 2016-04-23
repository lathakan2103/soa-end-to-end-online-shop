using System;
using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Demo.Business.Entities
{
    [DataContract]
    public class Cart : EntityBase, IIdentifiableEntity, ICustomerOwnedEntity
    {
        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public int[] CartItemId { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public decimal ShippingCost { get; set; }

        [DataMember]
        public bool StilOpen { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime? Canceled { get; set; }

        [DataMember]
        public DateTime? Approved { get; set; }

        [DataMember]
        public DateTime? Shipped { get; set; }

        #region IIdentifiableEntity

        public int EntityId
        {
            get { return this.CartId; }
            set { this.CartId = value; }
        }

        #endregion

        #region ICustomerOwnedEntity

        public int OwnerCustomerId => this.CustomerId;

        #endregion
    }
}
