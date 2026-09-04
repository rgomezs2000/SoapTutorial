using Flurl;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoapWebServiceClient.ProductServices;

namespace SoapWebServiceClient.Services
{
    /// <summary>
    /// Cliente SOAP para consumir el Web Service de Productos usando Flurl
    /// </summary>
    public class ProductSoapClient
    {
        private readonly string _baseUrl;

        public ProductSoapClient(string baseUrl = "https://localhost:7141")
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        public async Task<string> GetAllProductsAsync()
        {
            try
            {
                // Crear XML SOAP para GetAllProducts
                var soapRequest = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    <tns:GetAllProducts>
    </tns:GetAllProducts>
  </soap:Body>
</soap:Envelope>";

                // Usar Flurl para POST
                var response = await _baseUrl.AppendPathSegment("/ProductService.asmx")
                    .WithHeader("Content-Type", "text/xml; charset=utf-8")
                    .WithHeader("SOAPAction", "http://productservice.example.com/2025/GetAllProducts")
                    .PostStringAsync(soapRequest);

                // Obtener respuesta como string
                var result = await response.GetStringAsync();

                return result;

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public async Task<string> GetProductAsync(int productId)
        {
            try
            {
                // Crear XML SOAP para GetProductAsync
                var soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    <tns:GetProduct>
      <tns:productId>{productId}</tns:productId>
    </tns:GetProduct>
  </soap:Body>
</soap:Envelope>";

                // Usar Flurl para POST
                var response = await _baseUrl.AppendPathSegment("/ProductService.asmx")
                    .WithHeader("Content-Type", "text/xml; charset=utf-8")
                    .WithHeader("SOAPAction", "http://productservice.example.com/2025/GetProduct")
                    .PostStringAsync(soapRequest);

                var result = await response.GetStringAsync();

                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        public async Task<string> CreateProductAsync(string nombre, string descripcion, double precio, int stock)
        {
            try
            {
                // Crear XML SOAP para CreateProductAsync
                var soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    <tns:CreateProduct>
      <tns:request>
        <tns:Nombre>{nombre}</tns:Nombre>
        <tns:Descripcion>{descripcion}</tns:Descripcion>
        <tns:Precio>{precio}</tns:Precio>
        <tns:Stock>{stock}</tns:Stock>
      </tns:request>
    </tns:CreateProduct>
  </soap:Body>
</soap:Envelope>";

                // Usar Flurl para POST
                var response = await _baseUrl.AppendPathSegment("/ProductService.asmx")
                    .WithHeader("Content-Type", "text/xml; charset=utf-8")
                    .WithHeader("SOAPAction", "http://productservice.example.com/2025/CreateProduct")
                    .PostStringAsync(soapRequest);

                var result = await response.GetStringAsync();

                return result;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Actualiza un producto
        /// </summary>
        public async Task<string> UpdateProductAsync(int id, string nombre, string descripcion, double precio, int stock)
        {
            try
            {
                // Crear XML SOAP para UpdateProductAsync
                var soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    <tns:UpdateProduct>
      <tns:request>
        <tns:Id>{id}</tns:Id>
        <tns:Nombre>{nombre}</tns:Nombre>
        <tns:Descripcion>{descripcion}</tns:Descripcion>
        <tns:Precio>{precio}</tns:Precio>
        <tns:Stock>{stock}</tns:Stock>
      </tns:request>
    </tns:UpdateProduct>
  </soap:Body>
</soap:Envelope>";

                // Usar Flurl para POST
                var response = await _baseUrl.AppendPathSegment("/ProductService.asmx")
                    .WithHeader("Content-Type", "text/xml; charset=utf-8")
                    .WithHeader("SOAPAction", "http://productservice.example.com/2025/UpdateProduct")
                    .PostStringAsync(soapRequest);

                var result = await response.GetStringAsync();

                return result;

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        public async Task<string> DeleteProductAsync(int productId)
        {
            try
            {
                // Crear XML SOAP para DeleteProductAsync
                var soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    <tns:DeleteProduct>
      <tns:productId>{productId}</tns:productId>
    </tns:DeleteProduct>
  </soap:Body>
</soap:Envelope>";

                // Usar Flurl para POST
                var response = await _baseUrl.AppendPathSegment("/ProductService.asmx")
                    .WithHeader("Content-Type", "text/xml; charset=utf-8")
                    .WithHeader("SOAPAction", "http://productservice.example.com/2025/DeleteProduct")
                    .PostStringAsync(soapRequest);

                var result = await response.GetStringAsync();

                return result;

            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
