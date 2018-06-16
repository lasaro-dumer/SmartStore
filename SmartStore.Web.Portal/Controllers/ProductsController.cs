using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;

namespace SmartStore.Web.Portal.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsRepository _productsRepo;
        private IMapper _mapper;
        private ILogger<ProductsController> _logger;

        public ProductsController(IProductsRepository productsRepo,
                                  IMapper mapper,
                                  ILogger<ProductsController> logger)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        protected void LoadExistingTags()
        {
            IEnumerable<Tag> existingTags = _productsRepo.GetExistingTags();

            TempData["ExistingTags"] = string.Join(',', existingTags.Select(t => $"'{t.Name}'").ToArray());
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            LoadExistingTags();
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _productsRepo.GetProductById(id);
            ProductModel productModel = _mapper.Map<ProductModel>(product);

            return View(productModel);
        }

        [HttpPost, AllowAnonymous]
        public IActionResult Search([FromBody]ProductSearchFilter filter)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                IEnumerable<Product> prodList = null;

                prodList = _productsRepo.GetProducts(filter.Name,
                                                     filter.Description,
                                                     filter.MinSellingPrice,
                                                     filter.MaxSellingPrice,
                                                     filter.ProductsToList,
                                                     filter.Tags);

                products = _mapper.Map<List<ProductModel>>(prodList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return PartialView("_ProductsResults", products);
        }
    }
}
