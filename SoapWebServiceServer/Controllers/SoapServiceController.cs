using Microsoft.AspNetCore.Mvc;
using SoapWebServiceServer.Services;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

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
        public IActionResult GetWsdl(string wsdl = "")
        {
            if (string.IsNullOrEmpty(wsdl))
            {
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

            //Generar el WSDL
            var wsdlContent = GenerateWsdl();
            return Content(wsdlContent, "application/wsdl+xml; charset=utf-8");
        }

        /// <summary>
        /// Endpoint SOAP que procesa las solicitudes
        /// POST: /ProductService.asmx
        /// </summary>
        [HttpPost("ProductService.asmx")]
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
                        "GetProduct" => await HandleGetPr
                    };
                }
            }catch(Exception ex)
            {

            }
        }

        // ============ HANDLERS ============

        private async Task<string> HandleGetProducts(XElement operation)
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

            }
            catch (Exception ex)
            {

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
            string xml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
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
      <xsd:element name=""GetAllProducts"">
        <xsd:complexType/>
      </xsd:element>
      <xsd:element name=""CreateProduct"">
        <xsd:complexType>
          <xsd:sequence>
            <xsd:element name=""request"">
              <xsd:complexType>
                <xsd:sequence>
                  <xsd:element name=""Nombre"" type=""xsd:string""/>
                  <xsd:element name=""Descripcion"" type=""xsd:string""/>
                  <xsd:element name=""Precio"" type=""xsd:double""/>
                  <xsd:element name=""Stock"" type=""xsd:int""/>
                </xsd:sequence>
              </xsd:complexType>
            </xsd:element>
          </xsd:sequence>
        </xsd:complexType>
      </xsd:element>
    </xsd:schema>
  </types>

  <message name=""GetProductRequest"">
    <part name=""parameters"" element=""tns:GetProduct""/>
  </message>
  <message name=""GetProductResponse"">
    <part name=""parameters"" type=""xsd:string""/>
  </message>

  <portType name=""ProductServicePort"">
    <operation name=""GetProduct"">
      <input message=""tns:GetProductRequest""/>
      <output message=""tns:GetProductResponse""/>
    </operation>
  </portType>

  <binding name=""ProductServiceBinding"" type=""tns:ProductServicePort"">
    <soap:binding style=""document"" transport=""http://schemas.xmlsoap.org/soap/http""/>
    <operation name=""GetProduct"">
      <soap:operation soapAction=""GetProduct""/>
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
