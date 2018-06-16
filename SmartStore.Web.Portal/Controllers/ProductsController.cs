using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;

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

        [HttpGet, AllowAnonymous]
        public IActionResult Index()
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                var prodList = _productsRepo.GetProducts();
                products = _mapper.Map<List<ProductModel>>(prodList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return View(products);
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Details(int id)
        {
            var product = _productsRepo.GetProductById(id);
            ProductModel productModel = _mapper.Map<ProductModel>(product);
            
            return View(productModel);
        }
    }
}
