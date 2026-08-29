using CoreWCF;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using SoapWebServiceServer.Models;

namespace SoapWebServiceServer.Services
{
    /// <summary>
    /// Contrato SOAP para el servicio de productos
    /// Esto define qué métodos estarán disponibles en el Web Service
    /// </summary>
    [ServiceContract(Namespace = "http://productservice.example.com/2025")]
    public interface IProductService
    {
        [OperationContract]
        Task<ProductResponse> GetProductAsync(int productId);

        [OperationContract]
        Task<ProductListResponse> GetAllProductAsync();

        [OperationContract]
        Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request);

        [OperationContract]
        Task<UpdateProductResponse> UpdateProductAsync(UpdateProductRequest request);

        [OperationContract]
        Task<DeleteProductResponse> DeleteProductAsync(int productId);
    }
}
