using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmartStore.Data.Entities;
using SmartStore.Data.Repositories.Interfaces;

namespace SmartStore.Data.Repositories
{
    public class ShoppingRepository : BaseRepository, IShoppingRepository
    {
        public ShoppingRepository(SmartStoreDbContext context)
            : base(context)
        {
        }

        public ShoppingCart SaveCart(ShoppingCart shoppingCart)
        {
            for (int i = 0; i < shoppingCart.CartItems.Count; i++)
            {
                int prodId = shoppingCart.CartItems[i].Product.Id;
                shoppingCart.CartItems[i].Product = _context.Products.FirstOrDefault(p => p.Id == prodId);
            }

            bool addCart = true;

            if (shoppingCart.Id != 0)
            {
                ShoppingCart storedCart = _context.ShoppingCarts
                                                  .Include(s => s.CartItems)
                                                  .FirstOrDefault(s => s.Id == shoppingCart.Id);

                if (storedCart != null)
                {
                    addCart = false;

                    var cartItemsToRemove = storedCart.CartItems
                                                 .Where(ci => !shoppingCart.CartItems
                                                                           .Any(ci2 => ci.ProductId == ci2.ProductId))
                                                 .ToList();
                    foreach (var cartItem in cartItemsToRemove)
                    {
                        storedCart.CartItems.Remove(cartItem);
                    }
                    foreach (var cartItem in shoppingCart.CartItems)
                    {
                        var ci = storedCart.CartItems
                                           .FirstOrDefault(c => c.ProductId == cartItem.ProductId);
                        if (ci == null)
                        {
                            storedCart.CartItems.Add(cartItem);
                        }
                        else
                        {
                            ci.Quantity = cartItem.Quantity;
                        }
                    }

                    storedCart.LastUpdated = shoppingCart.LastUpdated;
                    storedCart.UserId = shoppingCart.UserId;
                    storedCart.UnauthenticatedUserId = shoppingCart.UnauthenticatedUserId;
                }
                else
                {
                    shoppingCart.Id = 0;
                }
            }

            if (addCart)
            {
                _context.Add(shoppingCart);
            }

            _context.SaveChanges();

            return shoppingCart;
        }

        public ShoppingCart GetShoppingCartFromUser(string userId, string unauthenticatedUserId)
        {
            ShoppingCart shoppingCart = null;
            if (!string.IsNullOrEmpty(userId))
            {
                shoppingCart = _context.ShoppingCarts
                                        .Include(s => s.CartItems)
                                            .ThenInclude(c => c.Product)
                                        .OrderByDescending(s => s.LastUpdated)
                                        .FirstOrDefault(s => s.UserId == userId);
            }
            else if (!string.IsNullOrEmpty(unauthenticatedUserId))
            {
                shoppingCart = _context.ShoppingCarts
                                        .Include(s => s.CartItems)
                                            .ThenInclude(c => c.Product)
                                        .OrderByDescending(s => s.LastUpdated)
                                        .FirstOrDefault(s => s.UnauthenticatedUserId == unauthenticatedUserId);
            }
            return shoppingCart;
        }

        public void DeleteCart(ShoppingCart shoppingCart)
        {
            Delete(shoppingCart);

            _context.SaveChanges();
        }

        public PurchaseOrder CreateClientPurchase(ShoppingCart shoppingCart)
        {
            UserEntity user = _context.Users.SingleOrDefault(u => u.Id == shoppingCart.UserId);

            OrderStatus orderWaitingStock = _context.OrderStatus.SingleOrDefault(s => s.Name == OrderStatus._WaitingStock);
            OrderItemStatus waitingStock = _context.OrderItemStatus.SingleOrDefault(s => s.Name == OrderItemStatus._WaitingStock);

            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {
                User = user,
                CreditCarCompany = user.CreditCardCompany,
                CreditCarNumber = user.CreditCardNumber,
                Creation = DateTime.Now,
                Updated = DateTime.Now,
                Status = orderWaitingStock
            };

            _context.Add(purchaseOrder);

            bool savedOrder = _context.SaveChanges() > 0;

            if (savedOrder)
            {
                List<PurchaseOrderItem> orderItems = new List<PurchaseOrderItem>();

                foreach (CartItem item in shoppingCart.CartItems)
                {
                    Product product = _context.Products.SingleOrDefault(p => p.Id == item.Product.Id);
                    PurchaseOrderItem purchaseOrderItem = new PurchaseOrderItem()
                    {
                        Order = purchaseOrder,
                        Product = product,
                        Quantity = item.Quantity,
                        UnitPrice = product.SellingPrice,
                        Status = waitingStock
                    };

                    _context.Add(purchaseOrderItem);
                    orderItems.Add(purchaseOrderItem);
                }

                bool savedOrderItems = _context.SaveChanges() > 0;

                if (savedOrderItems)
                {
                    Delete(shoppingCart);

                    int changes = _context.SaveChanges();

                    return _context.PurchaseOrders
                                    .Include(p => p.OrderItems)
                                        .ThenInclude(p => p.Product)
                                    .SingleOrDefault(p => p.Id == purchaseOrder.Id);
                }
                else
                {
                    throw new Exception($"Purchase order items not saved in the database, but order saved with Id = {purchaseOrder.Id}");
                }
            }
            else
            {
                throw new Exception("Purchase order not saved in the database");
            }
        }

        public IEnumerable<OrderItemStatus> GetOrderItemStatuses()
        {
            return _context.OrderItemStatus.ToList();
        }

        public IEnumerable<OrderStatus> GetOrderStatuses()
        {
            return _context.OrderStatus.ToList();
        }

        public PurchaseOrder GetPurchaseOrderById(int id)
        {
            return _context.PurchaseOrders
                            .Include(p => p.OrderItems)
                                .ThenInclude(i => i.Product)
                            .Include(p => p.OrderItems)
                                .ThenInclude(i => i.Status)
                            .Include(p => p.Status)
                            .Include(p => p.User)
                            .SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<PurchaseOrder> GetPurchaseOrdersFromUser(string userName)
        {
            return _context.PurchaseOrders
                            .Include(p => p.User)
                            .Include(p => p.OrderItems)
                                .ThenInclude(i => i.Product)
                            .Include(p => p.Status)
                            .Where(p => p.User.UserName == userName)
                            .ToList();
        }
    }
}
