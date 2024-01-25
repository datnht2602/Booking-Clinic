﻿using Clinic.DTO.Models;

namespace Clinic.Product.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListViewModel>> GetProductsAsync(string filterCriteria = null);
        Task<ProductDetailsViewModel> GetProductByIdASync(string productId,string productName);
        Task<ProductDetailsViewModel> AddProductAsync(ProductDetailsViewModel product);
        Task<HttpResponseMessage> UpdateProductAsync(ProductDetailsViewModel product);
        Task<HttpResponseMessage> DeleteProductAsync(string productId,string productName);
    }
}