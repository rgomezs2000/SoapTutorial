using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de CreateProductRequest
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class ProductResponse
    {

        [System.Runtime.Serialization.DataMember]
        public bool Success { get; set; }

        [System.Runtime.Serialization.DataMember]
        public Product Data { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Message { get; set; }
    }
}
