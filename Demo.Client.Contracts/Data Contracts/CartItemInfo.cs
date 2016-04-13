using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using Demo.Client.Entities;

namespace Demo.Client.Contracts
{
    [DataContract]
    public class CartItemInfo : DataContractBase
    {
        [DataMember]
        public int CartItemId { get; set; }

        [DataMember]
        public int CartId { get; set; }

        [DataMember]
        public Product Product { get; set; }

        [DataMember]
        public int Quantity { get; set; }
    }
}
