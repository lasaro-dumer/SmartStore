using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Data;
using SmartStore.Data.Entities;
using SmartStore.Domain.Models;

namespace SmartStore.Service.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private ISmartStoreRepository _repos;
        private IMapper _mapper;

        public ProductsController(ISmartStoreRepository repository,
            IMapper mapper)
        {
            _repos = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Product> products = _repos.GetProducts();
            
            return Ok(_mapper.Map<List<ProductModel>>(products));
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync([FromBody]Product product)
        {
            _repos.Add(product);

            if (await _repos.SaveAllAsync())
            {
                return Created($"{product.Id}", product);
            }

            return BadRequest();
        }
    }
}
