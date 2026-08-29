using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de CreateProductRequest
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class ProductListResponse
    {

        [System.Runtime.Serialization.DataMember]
        public bool Success { get; set; }

        [System.Runtime.Serialization.DataMember]
        public Product[] Products { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int Total { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Message { get; set; }
    }
}
