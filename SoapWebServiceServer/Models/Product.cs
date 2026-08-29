using System;
using System.Linq;

namespace SoapWebServiceServer.Models
{
    /// <summary>
    /// Modelo de Producto
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class Product
    {
        [System.Runtime.Serialization.DataMember]
        public int Id { get; set; }

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
