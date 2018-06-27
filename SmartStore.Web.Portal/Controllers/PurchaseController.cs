using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartStore.Data.Entities;
using SmartStore.Data.Models;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Web.Portal.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly ILogger<PurchaseController> _logger;
        private readonly IMapper _mapper;
        private readonly IShoppingRepository _shoppingRepo;

        public PurchaseController(ILogger<PurchaseController> logger,
                                  IMapper mapper,
                                  IShoppingRepository shoppingRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _shoppingRepo = shoppingRepository;
        }

        [HttpGet("[controller]/[action]/{id}", Name = "PurchaseDetails")]
        [Authorize]
        public IActionResult Details(int id)
        {
            PurchaseOrder purchaseOrder = _shoppingRepo.GetPurchaseOrderById(id);

            OrderModel order = _mapper.Map<OrderModel>(purchaseOrder);

            return View(order);
        }

        public IActionResult History()
        {
            IEnumerable<OrderModel> orders = new List<OrderModel>();

            try
            {
                IEnumerable<PurchaseOrder> purchaseOrders = _shoppingRepo.GetPurchaseOrdersFromUser(User.Identity.Name);

                orders = _mapper.Map<IEnumerable<OrderModel>>(purchaseOrders);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message, e);
            }

            return View(orders);
        }
    }
}