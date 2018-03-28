using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using SmartStore.Domain.Interfaces.Repositories;
using SmartStore.Domain.Models;

namespace SmartStore.Api.Client
{
    public class ProductsRepository : IProductsRepository
    {
        public Uri BaseUri { get; private set; }

        public ProductsRepository()
        {
            BaseUri = new Uri($"http://localhost:5000/api/products");
        }

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> productsList = new List<ProductModel>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = BaseUri;

                var responseTask = client.GetAsync("");

                responseTask.Wait();

                var response = responseTask.Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    productsList = string.IsNullOrEmpty(data) ?
                                    default(List<ProductModel>) :
                                    JsonConvert.DeserializeObject<List<ProductModel>>(data);
                }
            }

            return productsList;
        }
    }
}
