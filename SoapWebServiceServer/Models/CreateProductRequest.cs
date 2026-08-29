using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de CreateProductRequest
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class CreateProductRequest
    {

        [System.Runtime.Serialization.DataMember]
        public string Nombre { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string Descripcion { get; set; }

        [System.Runtime.Serialization.DataMember]
        public double Precio { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int Stock { get; set; }
    }
}
