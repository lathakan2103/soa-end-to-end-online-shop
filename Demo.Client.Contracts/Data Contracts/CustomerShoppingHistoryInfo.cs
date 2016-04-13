using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using Demo.Client.Entities;

namespace Demo.Client.Contracts
{
    [DataContract]
    public class CustomerShoppingHistoryInfo : DataContractBase
    {
        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public CartInfo[] CartList { get; set; } 
    }
}
