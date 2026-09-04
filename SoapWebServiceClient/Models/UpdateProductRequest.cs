using System;
using System.Linq;

namespace SoapWebServiceClient.Models
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
    }
}
