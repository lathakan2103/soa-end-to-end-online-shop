using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using Demo.Business.Entities;

namespace Demo.Business.Contracts
{
    [DataContract]
    public class CartItemInfo : DataContractBase
    {
        [DataMember]
        public int CartItemId { get; set; }

        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public bool StilOpen { get; set; }

        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
