using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IStockRepository _stockRepo;
        private IMapper _mapper;
        private ILogger<StockController> _logger;

        public StockController(IProductsRepository productsRepo,
                                IStockRepository stockRepo,
                                ILogger<StockController> logger,
                                IMapper mapper)
        {
            _productsRepo = productsRepo;
            _stockRepo = stockRepo;
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
            {
                this.AddInformationMessage($"Product {productModel.Name} saved successfully");
                return RedirectToAction("Status");
            }
            else
            {
                this.AddErrorMessage($"Unable to save product {productModel.Name}");
                return View();
            }
        }

        [HttpGet, Authorize]
        public IActionResult Status()
        {
            return View();
        }

        [HttpPost, Authorize]
        public IActionResult Search([FromBody]StockSearchFilter filter)
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                IEnumerable<StockMovement> prodList = null;

                prodList = _productsRepo.GetProductsWithStock(filter.Name,
                                                              filter.Description,
                                                              filter.MinSellingPrice,
                                                              filter.MaxSellingPrice,
                                                              filter.MinStockBalance,
                                                              filter.MaxStockBalance,
                                                              filter.RecordsToReturn);

                products = _mapper.Map<List<ProductModel>>(prodList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return PartialView("_ProductsStock", products);
        }

        [HttpGet, Authorize]
        public IActionResult NewStockMovement(int productId)
        {
            try
            {
                List<StockMovementType> stockMovementTypes = _stockRepo.GetMovementTypes().ToList();
                Product product = _productsRepo.GetProductById(productId);
                NewStockMovementModel newStockMovement = new NewStockMovementModel()
                {
                    StockMovementTypes = _mapper.Map<List<SelectListItem>>(stockMovementTypes),
                    Product = _mapper.Map<ProductModel>(product)
                };

                return View(newStockMovement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return View();
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> NewStockMovement(NewStockMovementModel newStockMovement)
        {
            bool saved = false;

            try
            {
                saved = await _stockRepo.AddStockMovement(newStockMovement.Product.Id,
                                                    newStockMovement.StockMovementTypeId,
                                                    newStockMovement.Amount);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            if (saved)
                this.AddInformationMessage($"Stock movement to product {newStockMovement.Product.Name} saved successfully");
            else
                this.AddErrorMessage($"Unable to save new stock movement to product {newStockMovement.Product.Name}");

            return RedirectToAction("Status");
        }
    }
}