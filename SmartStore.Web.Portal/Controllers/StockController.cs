using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;
using SmartStore.Web.Portal.Models;
using SmartStore.Web.Portal.Utility;

namespace SmartStore.Web.Portal.Controllers
{
    public class StockController : Controller
    {
        private IProductsRepository _productsRepo;
        private IMapper _mapper;
        private ILogger<StockController> _logger;

        public StockController(IProductsRepository productsRepo, ILogger<StockController> logger, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet, Authorize]
        public IActionResult NewProduct()
        {
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> NewProduct(ProductModel productModel)
        {
            bool saved = false;
            try
            {
                Product pe = _mapper.Map<Product>(productModel);
                _productsRepo.Add(pe);
                saved = await _productsRepo.SaveAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            if (saved)
                this.AddInformationMessage($"Product {productModel.Name} saved successfully");
            else
                this.AddErrorMessage($"Unable to save product {productModel.Name}");

            return View();
        }

        [HttpGet, Authorize]
        public IActionResult Status()
        {
            return View();
        }

        [HttpPost, Authorize]
        public IActionResult Search([FromBody]StockSearchFilter filter)
        {
            IEnumerable<StockMovement> prodList = null;

            prodList = _productsRepo.GetProductsWithStock(filter.Name,
                                                          filter.Description,
                                                          filter.MinSellingPrice,
                                                          filter.MaxSellingPrice,
                                                          filter.MinStockBalance,
                                                          filter.MaxStockBalance);

            List<ProductModel> products = _mapper.Map<List<ProductModel>>(prodList);

            return PartialView("_ProductsStock", products);
        }
    }
}