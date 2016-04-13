using System.Runtime.Serialization;
using Demo.Business.Entities;

namespace Demo.Data.Contracts.Dto
{
    [DataContract]
    public class CartItemInfoDto
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
