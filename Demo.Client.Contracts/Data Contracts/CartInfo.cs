using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.ServiceModel;

namespace Demo.Client.Contracts
{
    [DataContract]
    public class CartInfo : DataContractBase
    {
        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public List<CartItemInfo> CartItemList { get; set; }

        [DataMember]
        public decimal Total { get; set; }

        [DataMember]
        public decimal ShippingCost { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime? Canceled { get; set; }

        [DataMember]
        public DateTime? Approved { get; set; }

        [DataMember]
        public DateTime? Shipped { get; set; }
    }
}
