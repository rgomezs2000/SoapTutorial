using Microsoft.AspNetCore.Mvc;
using SoapWebServiceClient.Services;
using SoapWebServiceClient.Models;
using System.Threading.Tasks;

namespace SoapWebServiceClient.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductSoapClient _soapClient;

        public ProductsController()
        {
            _soapClient = new ProductSoapClient("https://localhost:7141");
        }

        /// <summary>
        /// Obtiene todos los productos (consumiendo SOAP con Flurl)
        /// GET: /api/products
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var xmlResponse = await _soapClient.GetAllProductsAsync();
            var jsonResponse = SoapXmlParser.ParseGetAllProducts(xmlResponse);
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Obtiene un producto por ID (consumiendo SOAP con Flurl)
        /// GET: /api/products/1
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var xmlResponse = await _soapClient.GetProductAsync(id);
            var jsonResponse = SoapXmlParser.ParseGetProduct(xmlResponse);
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Crea un nuevo producto (consumiendo SOAP con Flurl)
        /// POST: /api/products
        /// Body: { "nombre": "...", "descripcion": "...", "precio": 0, "stock": 0 }
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request == null)
                return BadRequest("Solicitud inválida");

            var xmlResponse = await _soapClient.CreateProductAsync(
                request.Nombre,
                request.Descripcion,
                request.Precio,
                request.Stock
            );
            var jsonResponse = SoapXmlParser.ParseCreateProduct(xmlResponse);
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Actualiza un producto existente (consumiendo SOAP con Flurl)
        /// PUT: /api/products
        /// Body: { "id": 1, "nombre": "...", "descripcion": "...", "precio": 0, "stock": 0 }
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            if (request == null || request.Id <= 0)
                return BadRequest("ID de producto inválido");

            var xmlResponse = await _soapClient.UpdateProductAsync(
                request.Id,
                request.Nombre,
                request.Descripcion,
                request.Precio,
                request.Stock
            );
            var jsonResponse = SoapXmlParser.ParseUpdateProduct(xmlResponse);
            return Ok(jsonResponse);
        }

        /// <summary>
        /// Elimina un producto (consumiendo SOAP con Flurl)
        /// DELETE: /api/products/1
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest("ID de producto inválido");

            var xmlResponse = await _soapClient.DeleteProductAsync(id);
            var jsonResponse = SoapXmlParser.ParseDeleteProduct(xmlResponse);
            return Ok(jsonResponse);
        }
    }
}