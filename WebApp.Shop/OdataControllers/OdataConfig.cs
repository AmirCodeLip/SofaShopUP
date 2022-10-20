using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using DataLayer.Infrastructure.WebModels.FileManager;

namespace WebApp.Shop.OdataControllers
{
    public static class OdataConfig
    {
        private static IEdmModel GetRootModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            var t = nameof(FObjectKindController).Substring(0, 11);
            builder.EntitySet<FObjectKind>(nameof(FObjectKindController).Substring(0, 11));
            return builder.GetEdmModel();
        }

        public static void AddOdataConfig(this Microsoft.AspNetCore.Builder.WebApplication app)
        {
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapODataRoute("odata", "odata", GetRootModel());
            //});
        }

        public static void AddOdataConfig(this Microsoft.Extensions.DependencyInjection.IMvcBuilder builder)
        {
            builder.AddOData(options =>
            {
                options.AddRouteComponents("odata", GetRootModel());
                options.EnableQueryFeatures();
                options.OrderBy();
                options.Filter();
                options.Select();
            });

        }
    }
}
