using System.Collections.Generic;
using SmartStore.Domain.Models;

namespace SmartStore.Domain.Interfaces.Repositories
{
    public interface IProductsRepository
    {
        List<ProductModel> GetProducts();
    }
}