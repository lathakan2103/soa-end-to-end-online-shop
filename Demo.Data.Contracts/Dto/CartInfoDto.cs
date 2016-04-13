using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Demo.Data.Contracts.Dto
{
    [DataContract]
    public class CartInfoDto
    {
        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public int CustomerId { get; set; }

        [DataMember]
        public List<CartItemInfoDto> CartItemList { get; set; }

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
