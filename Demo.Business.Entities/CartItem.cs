using System.Runtime.Serialization;
using Core.Common.Contracts;
using Core.Common.Core;

namespace Demo.Business.Entities
{
    [DataContract]
    public class CartItem : EntityBase, IIdentifiableEntity
    {
        [DataMember]
        public int CartItemId { get; set; }

        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        #region IIdentifiableEntity

        public int EntityId
        {
            get { return this.CartItemId; }
            set { this.CartItemId = value; }
        }

        #endregion
    }
}
