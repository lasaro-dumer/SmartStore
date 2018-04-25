using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Web.Portal.Controllers
{
    public class ProductsController : Controller
    {
        private IProductsRepository _productsRepo;
        private IMapper _mapper;

        public ProductsController(IProductsRepository productsRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<ProductModel> products = new List<ProductModel>();

            products = _mapper.Map<List<ProductModel>>(_productsRepo.GetProducts());

            return View(products);
        }
    }
}
