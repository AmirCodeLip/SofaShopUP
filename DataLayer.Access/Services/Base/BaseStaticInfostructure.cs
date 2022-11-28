using DataLayer.Domin.Models.BaseModels.Interfaces;

namespace DataLayer.Access.Services.Base
{
    public static class BaseStaticInfostructure
    {
        public static void Delete(this IDeleteBase deleteModel)
        {
            deleteModel.IsDeleted = true;
        }
        public static IEnumerable<T> NotDeleted<T>(this ICollection<T> collection) where T : IDeleteBase
         => collection.Where(x => !x.IsDeleted);

        public static IQueryable<T> NotDeleted<T>(this IQueryable<T> collection) where T : IDeleteBase
         => collection.Where(x => !x.IsDeleted);
        public static IEnumerable<T> Deleted<T>(this ICollection<T> collection) where T : IDeleteBase
         => collection.Where(x => x.IsDeleted);

        public static IQueryable<T> Deleted<T>(this IQueryable<T> collection) where T : IDeleteBase
         => collection.Where(x => x.IsDeleted);
    }
}
