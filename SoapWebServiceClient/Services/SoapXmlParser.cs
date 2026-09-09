using System;
using System.Xml.Linq;
using System.Linq;

namespace SoapWebServiceClient.Services
{
    public class SoapXmlParser
    {
        private static readonly XNamespace TnsNamespace = "http://productservice.example.com/2025";
        private static readonly XNamespace SoapNamespace = "http://schemas.xmlsoap.org/soap/envelope/";

        public static object ParseGetAllProducts(string soapXml)
        {
            try
            {
                var doc = XDocument.Parse(soapXml);
                var body = doc.Descendants(SoapNamespace + "Body").FirstOrDefault();
                var response = body?.Descendants(TnsNamespace + "GetAllProductsResponse").FirstOrDefault();

                if (response == null)
                    return new { error = "Respuesta SOAP inválida" };

                var products = response.Descendants(TnsNamespace + "Product")
                    .Select(product => new
                    {
                        id = int.Parse(product.Element(TnsNamespace + "Id")?.Value ?? "0"),
                        nombre = product.Element(TnsNamespace + "Nombre")?.Value ?? "",
                        descripcion = product.Element(TnsNamespace + "Descripcion")?.Value ?? "",
                        precio = double.Parse(product.Element(TnsNamespace + "Precio")?.Value ?? "0"),
                        stock = int.Parse(product.Element(TnsNamespace + "Stock")?.Value ?? "0")
                    })
                    .ToList();

                return new
                {
                    success = response.Element(TnsNamespace + "Success")?.Value == "true",
                    total = int.Parse(response.Element(TnsNamespace + "Total")?.Value ?? "0"),
                    products = products
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        public static object ParseGetProduct(string soapXml)
        {
            try
            {
                var doc = XDocument.Parse(soapXml);
                var body = doc.Descendants(SoapNamespace + "Body").FirstOrDefault();
                var response = body?.Descendants(TnsNamespace + "GetProductResponse").FirstOrDefault();

                if (response == null)
                    return new { error = "Respuesta SOAP inválida" };

                return new
                {
                    success = response.Element(TnsNamespace + "Success")?.Value == "true",
                    id = int.Parse(response.Element(TnsNamespace + "ProductId")?.Value ?? "0"),
                    nombre = response.Element(TnsNamespace + "Nombre")?.Value,
                    descripcion = response.Element(TnsNamespace + "Descripcion")?.Value,
                    precio = double.Parse(response.Element(TnsNamespace + "Precio")?.Value ?? "0"),
                    stock = int.Parse(response.Element(TnsNamespace + "Stock")?.Value ?? "0"),
                    message = response.Element(TnsNamespace + "Message")?.Value
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        public static object ParseCreateProduct(string soapXml)
        {
            try
            {
                var doc = XDocument.Parse(soapXml);
                var body = doc.Descendants(SoapNamespace + "Body").FirstOrDefault();
                var response = body?.Descendants(TnsNamespace + "CreateProductResponse").FirstOrDefault();

                if (response == null)
                    return new { error = "Respuesta SOAP inválida" };

                return new
                {
                    success = response.Element(TnsNamespace + "Success")?.Value == "true",
                    productId = int.Parse(response.Element(TnsNamespace + "ProductId")?.Value ?? "0"),
                    message = response.Element(TnsNamespace + "Message")?.Value
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        public static object ParseUpdateProduct(string soapXml)
        {
            try
            {
                var doc = XDocument.Parse(soapXml);
                var body = doc.Descendants(SoapNamespace + "Body").FirstOrDefault();
                var response = body?.Descendants(TnsNamespace + "UpdateProductResponse").FirstOrDefault();

                if (response == null)
                    return new { error = "Respuesta SOAP inválida" };

                return new
                {
                    success = response.Element(TnsNamespace + "Success")?.Value == "true",
                    message = response.Element(TnsNamespace + "Message")?.Value
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }

        public static object ParseDeleteProduct(string soapXml)
        {
            try
            {
                var doc = XDocument.Parse(soapXml);
                var body = doc.Descendants(SoapNamespace + "Body").FirstOrDefault();
                var response = body?.Descendants(TnsNamespace + "DeleteProductResponse").FirstOrDefault();

                if (response == null)
                    return new { error = "Respuesta SOAP inválida" };

                return new
                {
                    success = response.Element(TnsNamespace + "Success")?.Value == "true",
                    message = response.Element(TnsNamespace + "Message")?.Value
                };
            }
            catch (Exception ex)
            {
                return new { error = ex.Message };
            }
        }
    }
}