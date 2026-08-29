using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de CreateProductRequest
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class DeleteProductResponse
    {

        [System.Runtime.Serialization.DataMember]
        public bool Success { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Message { get; set; }
    }
}
