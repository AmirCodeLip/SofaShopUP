using DataLayer.Domin.Models.Interfaces;
using System;

namespace DataLayer.Domin.Models
{
    public class ShopCartItem:IDeleteBase
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string CartId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ShopProduct Product { get; set; }
        public bool IsDeleted { get; set; }
    }
}
