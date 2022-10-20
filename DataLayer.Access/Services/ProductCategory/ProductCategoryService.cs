using DataLayer.Access.Data;
using DataLayer.Access.ViewModel;
using DataLayer.Domin;
using DataLayer.Domin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class ProductCategoryService : BaseService<ShopProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryService(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<BaseServiceResult<ShopProductCategory>> GetEdit(int? id)
        {
            var model = new BaseServiceResult<ShopProductCategory>();
            if (id == null)
            {
                model.Status = ActionStatus.NotFound;
            }
            else
            {
                model.Result = await FindAsync(id);
                if (model.Result == null)
                {
                    model.Status = ActionStatus.Success;
                }
            }
            return model;
        }

        public async Task SetEdit(ShopProductCategory productCategory)
        {
            if (productCategory.Id == 0)
            {
                _context.Add(productCategory);
            }
            else
            {
                _context.Update(productCategory);
            }
            await SaveChangesAsync();
        }

        public ShopProductCategory RequestCategory(string id)
        {
            if (!id.Contains("Cate"))
                return null;
            return FirstOrDefault(x => x.Id == int.Parse(id.Replace("Cate", "")));
        }
    }
}
