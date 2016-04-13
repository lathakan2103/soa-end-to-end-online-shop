using System.Collections.Generic;
using System.Runtime.Serialization;
using Demo.Business.Entities;

namespace Demo.Data.Contracts.Dto
{
    [DataContract]
    public class CustomerShoppingHistoryInfoDto
    {
        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public IEnumerable<CartInfoDto> CartList { get; set; } 
    }
}
