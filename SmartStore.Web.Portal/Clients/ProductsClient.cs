using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartStore.Domain.Models;

namespace SmartStore.Web.Portal.Clients
{
    public class ProductsClient
    {
        private HttpClient Client { get; set; }

        public ProductsClient()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri($"http://localhost:5000/api/products");
        }

        public List<ProductModel> GetProducts()
        {
            List<ProductModel> productsList = new List<ProductModel>();

            var responseTask = Client.GetAsync("");

            responseTask.Wait();

            var response = responseTask.Result;

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                productsList = string.IsNullOrEmpty(data) ?
                                default(List<ProductModel>) :
                                JsonConvert.DeserializeObject<List<ProductModel>>(data);
            }

            return productsList;
        }
    }
}
