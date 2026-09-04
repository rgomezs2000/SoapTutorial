using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoapWebServiceClient.Services;
using System.Threading.Tasks;
using SoapWebServiceClient.Models;
using System.Reflection.Metadata.Ecma335;

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
            var response = await _soapClient.GetAllProductsAsync();

            return Ok(new
            {
                xmlResponse = response
            });
        }

        /// <summary>
        /// Obtiene un producto por ID (consumiendo SOAP con Flurl)
        /// GET: /api/products/1
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var response = await _soapClient.GetProductAsync(id);

            return Ok(new
            {
                xmlResponse = response
            });
        }

        /// <summary>
        /// Crea un producto (consumiendo SOAP con Flurl)
        /// POST: /api/products
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            var response = await _soapClient.CreateProductAsync(request.Nombre, request.Descripcion, request.Precio, request.Stock);

            return Ok(new
            {
                xmlResponse = response
            });
        }

        /// <summary>
        /// Actualiza un producto (consumiendo SOAP con Flurl)
        /// PUT: /api/products
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            var response = await _soapClient.UpdateProductAsync(request.Id, request.Nombre, request.Descripcion, request.Precio, request.Stock);

            return Ok(new
            {
                xmlResponse = response
            });
        }

        /// <summary>
        /// Elimina un producto (consumiendo SOAP con Flurl)
        /// DELETE: /api/products/1
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _soapClient.DeleteProductAsync(id);

            return Ok(new
            {
                xmlResponse = response
            });
        }
    }
}
