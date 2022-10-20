using DataLayer.Access.Data;
using DataLayer.Access.Services.Base;
using DataLayer.Access.ViewModel;
using DataLayer.Domin;
using DataLayer.Domin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Access.Services
{
    public class ProductService : BaseService<ShopProduct>, IProductRepository
    {
        public IProductCategoryRepository _productCategoryRepository;
        public ProductService(ApplicationDbContext context,
            IProductCategoryRepository productCategoryRepository) : base(context)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<List<ShopProduct>> ListProductAsync()
        {
            return await AsQueryable().Include(p => p.Category).ToListAsync();
        }

        public SelectList GetProductCategorySelectList()
        {
            return new SelectList(_productCategoryRepository.GetList(), "Id", "FaName");
        }

        public async Task<BaseServiceResult<ShopProduct>> GetEdit(int? id)
        {
            var model = new BaseServiceResult<ShopProduct>();
            if (id == null)
            {
                model.Status = ActionStatus.NotFound;
            }
            else
            {
                model.Result = await FindAsync(false, id);
                if (model.Result == null)
                {
                    model.Status = ActionStatus.Success;
                }
            }
            return model;
        } 
 
        public BaseServiceResult<ProductUserIndex> UserIndex(string id)
        {
            var model = new BaseServiceResult<ProductUserIndex>();
            ShopProductCategory productCategory = null;
            if (string.IsNullOrWhiteSpace(id) ||
                (productCategory = _productCategoryRepository.RequestCategory(id)) == null)
            {
                model.Status = ActionStatus.NotFound;
            }
            else
            {
                var specificationRelations = _context.ShopProductCategorySpecifications
                    .Include(x => x.CategorySpecificationRelation)
                    .Include(x => x.CategorySpecificationRelation.CategorySpecification);
                model.Result.Products = AsQueryable().Where(x => x.CategoryId == productCategory.Id).ToList();
                model.Result.ProductCategories = productCategory;
            }
            return model;
        }

        public async Task SetEdit(ShopProduct product)
        {
            if (product.Id == 0)
            {
                _context.Add(product);
            }
            else
            {
                _context.Update(product);
            }
            await SaveChangesAsync();
        }
    }
}
