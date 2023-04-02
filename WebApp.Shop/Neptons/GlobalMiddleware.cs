using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System.Threading;
using DataLayer.UnitOfWork;

namespace WebApp.Shop.Neptons
{
    public class GlobalMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalMiddleware(RequestDelegate next)
        {
            _next = next;
            //_hostingEnv = hostingEnv;
        }

        public Dictionary<string, string> GetSupportedTypes(string[] supportedFiles)
        {
            return new Dictionary<string, string>(new FileExtensionContentTypeProvider().Mappings.Where(x => supportedFiles.Contains(x.Key)));
        }

        public Task Invoke(HttpContext context)
        {
            //var h = context.Request.Headers;
            var culture = context.Request.Headers["Culture"].FirstOrDefault();
            if (culture != null)
            {
                Thread.CurrentThread.CurrentCulture = ConstTypes.SupportedLanguages.List[culture].CultureInfo;
            }
            //if (IsReactFile(context, out var filePath))
            //{
            //    return TryServeReactFile(context, filePath);
            //}
            return _next(context);
        }



        private async Task<Task> TryServeReactFile(HttpContext context, string subPath)
        {
            //var contentRoot = $"{_hostingEnv.ContentRootPath}app{subPath.Replace("/", "\\")}";
            //context.Response.ContentType = contentType;
            //using (var fs = new FileStream(contentRoot, FileMode.Open))
            //    await fs.CopyToAsync(context.Response.Body);
            //return Task.CompletedTask;
            return _next(context);
        }
    }
}
