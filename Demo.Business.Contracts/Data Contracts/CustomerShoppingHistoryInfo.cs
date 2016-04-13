using System.Collections.Generic;
using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using Demo.Business.Entities;

namespace Demo.Business.Contracts
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
