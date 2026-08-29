using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoapWebServiceServer.Models;
using System.Reflection.Metadata.Ecma335;

namespace SoapWebServiceServer.Services
{
    /// <summary>
    /// Implementación del servicio SOAP de productos
    /// </summary>
    public class ProductService : IProductService
    {
        //Base de datos ficticia en memoria
        private static List<Product> _products = new List<Product>
        {
            new Product{ Id = 1, Nombre = "Laptop", Descripcion = "Laptop HP 15 pulgadas", Precio = 800.00, Stock = 5 },
            new Product{ Id = 2, Nombre = "Mouse", Descripcion = "Mouse inalámbrico Logitech", Precio = 25.00, Stock = 50 },
            new Product{ Id = 3, Nombre = "Teclado", Descripcion = "Teclado mecánico RGB", Precio = 120.00, Stock = 15 },
        };

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public async Task<ProductResponse> GetProductAsync(int productId)
        {
            return await Task.Run(() =>
            {
                var product = _products.FirstOrDefault(p => p.Id == productId);

                if(product == null)
                {
                    return new ProductResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {productId} no encontrado"
                    };
                }

                return new ProductResponse
                {
                    Success = true,
                    Data = product,
                    Message = "Producto obtenido exitosamente"
                };
            });
        }

        /// <summary>
        /// Obtiene todos los productos
        /// </summary>
        public async Task<ProductListResponse> GetAllProductAsync()
        {
            return await Task.Run(() =>
            {
                return new ProductListResponse
                {
                    Success = true,
                    Products = _products.ToArray(),
                    Total = _products.Count,
                    Message = "Productos obtenidos exitosamente"
                };
            });
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        public async Task<CreateProductResponse> CreateProductAsync(CreateProductRequest request)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(request.Nombre))
                {
                    return new CreateProductResponse
                    {
                        Success = false,
                        Message = "El nombre del producto es requerido"
                    };
                }

                int newId = _products.Max(p => p.Id) + 1;

                var newProduct = new Product
                {
                    Id = newId,
                    Nombre = request.Nombre,
                    Descripcion = request.Descripcion,
                    Precio = request.Precio,
                    Stock = request.Stock,
                };

                _products.Add(newProduct);

                return new CreateProductResponse
                {
                    Success = true,
                    ProductId = newId,
                    Message = $"Producto '{newProduct.Nombre}' creado exitosamente con ID {newId}",
                };
            });
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        public async Task<UpdateProductResponse> UpdateProductAsync(UpdateProductRequest request)
        {
            return await Task.Run(() =>
            {
                var product = _products.FirstOrDefault(p => p.Id == request.Id);

                if(product == null)
                {
                    return new UpdateProductResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {request.Id} no encontrado"
                    };
                }

                product.Nombre = request.Nombre;
                product.Descripcion = request.Descripcion;
                product.Precio = request.Precio;
                product.Stock = request.Stock;

                return new UpdateProductResponse
                {
                    Success = true,
                    Message = $"Producto '{product.Nombre}' actualizado exitosamente"
                };
            });
        }

        /// <summary>
        /// Elimina un producto
        /// </summary>
        public async Task<DeleteProductResponse> DeleteProductAsync(int productId)
        {
            return await Task.Run(() =>
            {
                var product = _products.FirstOrDefault(p => p.Id == productId);

                if(product == null)
                {
                    return new DeleteProductResponse
                    {
                        Success = false,
                        Message = $"Producto con ID {productId} no encontrado"
                    };
                }

                _products.Remove(product);

                return new DeleteProductResponse
                {
                    Success = true,
                    Message = $"Producto '{product.Nombre}' eliminado exitosamente"
                };
            });
        }
    }
}
