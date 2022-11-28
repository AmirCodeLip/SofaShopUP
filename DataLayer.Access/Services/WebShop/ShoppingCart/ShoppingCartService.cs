#if shop_project
using DataLayer.Access.Data;
using DataLayer.Access.Services.A_WebShop.Product;
using DataLayer.Access.Services.WebShop.ShopCartItem;
using DataLayer.Domin.Models.WebShop;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public class ShoppingCartService : IShoppingCartRepository
    {
        public HttpContext HttpContext { get; set; }
        private readonly IProductRepository _productRepository;
        private readonly IShopCartItemRepository _shopCartItemRepository;
        private readonly ApplicationDbContext _dbContext;
        public ShoppingCartService(
            IProductRepository productRepository,
            IShopCartItemRepository shopCartItemRepository,
            ApplicationDbContext dbContext
            )
        {
            _dbContext = dbContext;
            //_context = context;
            _productRepository = productRepository;
            _shopCartItemRepository = shopCartItemRepository;
        }
        public const string CartSessionKey = "CartId";
        public string _shoppingCartId = "";
        public string GetCartId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_shoppingCartId))
                {
                    if (HttpContext.Session.Keys.FirstOrDefault(x => x == CartSessionKey) == null)
                    {

                        HttpContext.Session.SetString(CartSessionKey, Guid.NewGuid().ToString());
                    }
                    _shoppingCartId = HttpContext.Session.GetString(CartSessionKey);
                }
                return _shoppingCartId;
            }

        }

        public async Task<bool> AddToCart(int id)
        {
            var product = await _productRepository.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (product == null)
                return false;
            var cartItem = await _shopCartItemRepository.
                FirstOrDefaultAsync(c => c.CartId == GetCartId && c.ProductId == id);
            if (cartItem == null)
            {
                cartItem = new ShopCartItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = id,
                    CartId = GetCartId,
                    Quantity = 1,
                    DateCreated = DateTime.Now,
                };
                await _dbContext.ShopCartItems.AddAsync(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ShopCartItem>> GetCartItems() => await _shopCartItemRepository.AsQueryable().
            Include(x => x.Product).Where(c => c.CartId == GetCartId).ToListAsync();



    }
}
#endif