#if shop_project
using DataLayer.Domin.Models.WebShop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services.WebShop
{
    public interface IShoppingCartRepository
    {
        string GetCartId { get; }

        Task<bool> AddToCart(int id);

        Task<List<ShopCartItem>> GetCartItems();

    }
}
#endif