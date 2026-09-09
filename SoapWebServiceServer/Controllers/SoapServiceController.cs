using Microsoft.AspNetCore.Mvc;
using SoapWebServiceServer.Services;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using SoapWebServiceServer.Models;
using System.Reflection.Metadata.Ecma335;

namespace SoapWebServiceServer.Controllers
{
    [ApiController]
    [Route("")]
    public class SoapServiceController : ControllerBase
    {
        private readonly ProductService _productService;

        public SoapServiceController()
        {
           _productService = new ProductService();
        }

        /// <summary>
        /// Endpoint SOAP que expone el WSDL
        /// GET: /ProductService.asmx?wsdl
        /// </summary>
        [HttpGet("ProductService.asmx")]
public IActionResult GetWsdl([FromQuery] string wsdl = "")
{
    // ✅ VERIFICAR SI EXISTE EL PARÁMETRO EN LA QUERY STRING
    if (Request.Query.ContainsKey("wsdl"))
    {
        var wsdlContent = GenerateWsdl();
        return Content(wsdlContent, "application/wsdl+xml; charset=utf-8");
    }

    // Si no, retornar el HTML
    var html = @"
<!DOCTYPE html>
<html>
<head>
    <title>ProductService Web Service</title>
</head>
<body>
    <h1>ProductService Web Service</h1>
    <p>Este es un Web Service SOAP de Productos</p>
    <p><a href='?wsdl'>Ver WSDL</a></p>
    <h2>Operaciones disponibles:</h2>
    <ul>
        <li>GetProduct</li>
        <li>GetAllProducts</li>
        <li>CreateProduct</li>
        <li>UpdateProduct</li>
        <li>DeleteProduct</li>
    </ul>
</body>
</html>";
    return Content(html, "text/html; charset=utf-8");
}

        /// <summary>
        /// Endpoint SOAP que procesa las solicitudes
        /// POST: /ProductService.asmx
        /// </summary>
        [HttpPost("ProductService.asmx")]
        [Consumes("application/soap+xml", "text/xml")]
        public async Task<IActionResult> ProcessSoapRequest()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var soapRequest = await reader.ReadToEndAsync();

                    //Parsear el XML
                    var xmlDoc = XDocument.Parse(soapRequest);
                    var ns = XNamespace.Get("http://schemas.xmlsoap.org/soap/envelope/");
                    var body = xmlDoc.Root?.Element(ns + "Body");

                    if (body == null)
                    {
                        return BadRequest("SOAP No encontrado");
                    }

                    //Obtener elemento de operacion
                    var operation = body.FirstNode as XElement;

                    if(operation == null)
                    {
                        return BadRequest("Operación no encontrada");
                    }

                    var operationName = operation.Name.LocalName;

                    //procesar segun la operacion
                    string soapResponse = operationName switch
                    {
                        "GetProduct" => await HandleGetProduct(operation),
                        "GetAllProducts" => await HandleGetAllProducts(operation),
                        "CreateProduct" => await HandleCreateProduct(operation),
                        "UpdateProduct" => await HandleUpdateProduct(operation),
                        "DeleteProduct" => await HandleDeleteProduct(operation),
                        _ => GenerateSoapFault("operacion no reconocida")
                    };

                    return Content(soapResponse, "application/soap+xml; charset=utf-8");
                }
            }catch(Exception ex)
            {
                return Content(GenerateSoapFault($"Error: {ex.Message}"), "application/soap+xml; charset=utf-8");
            }
        }

        // ============ HANDLERS ============

        private async Task<string> HandleGetProduct(XElement operation)
        {
            try
            {
                string xml = string.Empty;
                var productIdElem = operation.Element(XName.Get("productId", "http://productservice.example.com/2025"));
                int productId = int.Parse(productIdElem?.Value ?? "0");

                var response = await _productService.GetProductAsync(productId);

                if (!response.Success)
                {
                    return GenerateSoapFault(response.Message);
                }

                xml = $@"
    <tns:GetProductResponse xmlns:tns=""http://productservice.example.com/2025"">
        <tns:Success>true</tns:Success>
        <tns:ProductId>{response.Data.Id}</tns:ProductId>
        <tns:Nombre>{response.Data.Nombre}</tns:Nombre>
        <tns:Descripcion>{response.Data.Descripcion}</tns:Descripcion>
        <tns:Precio>{response.Data.Precio}</tns:Precio>
        <tns:Stock>{response.Data.Stock}</tns:Stock>
        <tns:Message>{response.Message}</tns:Message>
    </tns:GetProductResponse>";

                return GenerateSoapResponse(xml);

            }
            catch (Exception ex)
            {
                return GenerateSoapFault(ex.Message);
            }
        }

        private async Task<string> HandleGetAllProducts(XElement operation)
        {
            try
            {
                var response = await _productService.GetAllProductAsync();
                string productsXml = string.Empty;

                foreach (var product in response.Products)
                {
                    productsXml += $@"
        <tns:Product>
            <tns:Id>{product.Id}</tns:Id>
            <tns:Nombre>{product.Nombre}</tns:Nombre>
            <tns:Descripcion>{product.Descripcion}</tns:Descripcion>
            <tns:Precio>{product.Precio}</tns:Precio>
            <tns:Stock>{product.Stock}</tns:Stock>
        </tns:Product>";
                }

                string xml = $@"
    <tns:GetAllProductsResponse xmlns:tns=""http://productservice.example.com/2025"">
        <tns:Success>true</tns:Success>
        <tns:Total>{response.Total}</tns:Total>
        {productsXml}
        <tns:Message>{response.Message}</tns:Message>
    </tns:GetAllProductsResponse>";

                return GenerateSoapResponse(xml);
            }
            catch (Exception ex)
            {
                return GenerateSoapFault(ex.Message);
            }
        }


        public async Task<string> HandleCreateProduct(XElement operation)
        {
            try
            {
                var ns = XNamespace.Get("http://productservice.example.com/2025");
                var request = operation.Element(ns + "request");
                string xml = string.Empty;

                var createRequest = new CreateProductRequest
                {
                    Nombre = request.Element(ns + "Nombre")?.Value ?? "",
                    Descripcion = request.Element(ns + "Descripcion")?.Value ?? "",
                    Precio = double.Parse(request.Element(ns + "Precio")?.Value ?? "0"),
                    Stock = int.Parse(request.Element(ns + "Stock")?.Value ?? "0")
                };

                var response = await _productService.CreateProductAsync(createRequest);

                xml = $@"
    <tns:CreateProductResponse xmlns:tns=""http://productservice.example.com/2025"">
        <tns:Success>{response.Success.ToString().ToLower()}</tns:Success>
        <tns:ProductId>{response.ProductId}</tns:ProductId>
        <tns:Message>{response.Message}</tns:Message>
    </tns:CreateProductResponse>";

                return GenerateSoapResponse(xml);
            }
            catch(Exception ex)
            {
                return GenerateSoapFault(ex.Message);
            }
        }

        private async Task<string> HandleUpdateProduct(XElement operation)
        {
            try
            {
                var ns = XNamespace.Get("http://productservice.example.com/2025");
                var request = operation.Element(ns + "request");
                string xml = string.Empty;

                var updateRequest = new UpdateProductRequest
                {
                    Id = int.Parse(request?.Element(ns + "Id")?.Value ?? "0"),
                    Nombre = request?.Element(ns + "Nombre")?.Value ?? "",
                    Descripcion = request?.Element(ns + "Descipcion")?.Value ?? "",
                    Precio = double.Parse(request?.Element(ns + "Precio")?.Value ?? "0"),
                    Stock = int.Parse(request?.Element(ns + "Stock")?.Value ?? "0"),

                };

                var response = await _productService.UpdateProductAsync(updateRequest);

                xml = $@"
    <tns:UpdateProductResponse xmlns:tns=""http://productservice.example.com/2025"">
        <tns:Success>{response.Success.ToString().ToLower()}</tns:Success>
        <tns:Message>{response.Message}</tns:Message>
    </tns:UpdateProductResponse>";

                return GenerateSoapResponse(xml);

            }
            catch(Exception ex)
            {
                return GenerateSoapFault(ex.Message);
            }
        }

        private async Task<string> HandleDeleteProduct(XElement operation)
        {
            try
            {
                var productIdElem = operation.Element(XName.Get("productId", "http://productservice.example.com/2025"));
                int productid = int.Parse(productIdElem?.Value ?? "0");

                var response = await _productService.DeleteProductAsync(productid);

                string xml = $@"
    <tns:DeleteProductResponse xmlns:tns=""http://productservice.example.com/2025"">
        <tns:Success>{response.Success.ToString().ToLower()}</tns:Success>
        <tns:Message>{response.Message}</tns:Message>
    </tns:DeleteProductResponse>";

                return GenerateSoapResponse(xml);

            }
            catch (Exception ex)
            {
                return GenerateSoapFault(ex.Message);
            }
        }

        // ============ UTILIDADES ============

        private string GenerateSoapResponse(string body)
        {
            string xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" 
               xmlns:tns=""http://productservice.example.com/2025"">
  <soap:Body>
    {body}
  </soap:Body>
</soap:Envelope>";
            return xml;
        }

        private string GenerateSoapFault(string message)
        {
            string xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <soap:Fault>
      <faultcode>soap:Server</faultcode>
      <faultstring>{message}</faultstring>
    </soap:Fault>
  </soap:Body>
</soap:Envelope>";
            return xml;
        }

        private string GenerateWsdl()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<definitions xmlns=""http://schemas.xmlsoap.org/wsdl/"" 
             xmlns:soap=""http://schemas.xmlsoap.org/wsdl/soap/"" 
             xmlns:tns=""http://productservice.example.com/2025"" 
             xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
             targetNamespace=""http://productservice.example.com/2025"" 
             name=""ProductService"">

  <types>
    <xsd:schema targetNamespace=""http://productservice.example.com/2025"">
      <xsd:element name=""GetProduct"">
        <xsd:complexType>
          <xsd:sequence>
            <xsd:element name=""productId"" type=""xsd:int""/>
          </xsd:sequence>
        </xsd:complexType>
      </xsd:element>
      <xsd:element name=""GetProductResponse"">
        <xsd:complexType>
          <xsd:sequence>
            <xsd:element name=""Success"" type=""xsd:boolean""/>
            <xsd:element name=""ProductId"" type=""xsd:int"" minOccurs=""0""/>
            <xsd:element name=""Nombre"" type=""xsd:string"" minOccurs=""0""/>
            <xsd:element name=""Message"" type=""xsd:string"" minOccurs=""0""/>
          </xsd:sequence>
        </xsd:complexType>
      </xsd:element>
      <xsd:element name=""GetAllProducts"">
        <xsd:complexType/>
      </xsd:element>
      <xsd:element name=""GetAllProductsResponse"">
        <xsd:complexType>
          <xsd:sequence>
            <xsd:element name=""Success"" type=""xsd:boolean""/>
            <xsd:element name=""Total"" type=""xsd:int""/>
            <xsd:element name=""Message"" type=""xsd:string"" minOccurs=""0""/>
          </xsd:sequence>
        </xsd:complexType>
      </xsd:element>
    </xsd:schema>
  </types>

  <message name=""GetProductRequest"">
    <part name=""parameters"" element=""tns:GetProduct""/>
  </message>
  <message name=""GetProductResponse"">
    <part name=""parameters"" element=""tns:GetProductResponse""/>
  </message>
  <message name=""GetAllProductsRequest"">
    <part name=""parameters"" element=""tns:GetAllProducts""/>
  </message>
  <message name=""GetAllProductsResponse"">
    <part name=""parameters"" element=""tns:GetAllProductsResponse""/>
  </message>

  <portType name=""ProductServicePort"">
    <operation name=""GetProduct"">
      <input message=""tns:GetProductRequest""/>
      <output message=""tns:GetProductResponse""/>
    </operation>
    <operation name=""GetAllProducts"">
      <input message=""tns:GetAllProductsRequest""/>
      <output message=""tns:GetAllProductsResponse""/>
    </operation>
  </portType>

  <binding name=""ProductServiceBinding"" type=""tns:ProductServicePort"">
    <soap:binding style=""document"" transport=""http://schemas.xmlsoap.org/soap/http""/>
    <operation name=""GetProduct"">
      <soap:operation soapAction=""http://productservice.example.com/2025/GetProduct""/>
      <input><soap:body use=""literal""/></input>
      <output><soap:body use=""literal""/></output>
    </operation>
    <operation name=""GetAllProducts"">
      <soap:operation soapAction=""http://productservice.example.com/2025/GetAllProducts""/>
      <input><soap:body use=""literal""/></input>
      <output><soap:body use=""literal""/></output>
    </operation>
  </binding>

  <service name=""ProductService"">
    <port name=""ProductServicePort"" binding=""tns:ProductServiceBinding"">
      <soap:address location=""https://localhost:7141/ProductService.asmx""/>
    </port>
  </service>
</definitions>";
            return xml;
        }
    }
}
