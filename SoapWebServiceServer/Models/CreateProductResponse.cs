using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de CreateProductResponse
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class CreateProductResponse
    {
        [System.Runtime.Serialization.DataMember]
        public bool Success { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int ProductId { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Message { get; set; }
    }
}