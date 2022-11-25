using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DataLayer.Infrastructure.ViewModels
{
    public class CentralizeData
    {
        public readonly ModelStateDictionary modelState;
        public readonly HttpContext httpContext;
        public CentralizeData(HttpContext httpContext, ModelStateDictionary modelState)
        {
            this.httpContext = httpContext;
            this.modelState = modelState;
        }
    }
}
