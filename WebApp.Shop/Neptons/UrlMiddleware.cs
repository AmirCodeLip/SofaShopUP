using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reflection;

namespace WebApp.Shop.Neptons
{
    public class UrlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly IContentTypeProvider _contentTypeProvider;
        private readonly string[] supportedFiles;
        private readonly IOptions<StaticFileOptions> _options;
        public const string StaticDirectoriesKey = "staticdirectories";

        public UrlMiddleware(RequestDelegate next, IWebHostEnvironment hostingEnv, IOptions<StaticFileOptions> options)
        {
            supportedFiles = new[] { ".css" };
            _next = next;
            _hostingEnv = hostingEnv;
            _options = options;
            _contentTypeProvider = (new FileExtensionContentTypeProvider(GetSupportedTypes(supportedFiles)));
        }

        public Dictionary<string, string> GetSupportedTypes(string[] supportedFiles)
        {
            return new Dictionary<string, string>(new FileExtensionContentTypeProvider().Mappings.Where(x => supportedFiles.Contains(x.Key)));
        }

        public Task Invoke(HttpContext context)
        {
            if (IsReactFile(context, out var filePath))
            {
                return TryServeReactFile(context, filePath);
            }
            return _next(context);
        }

        private static bool ValidateNoEndpoint(HttpContext context) => context.GetEndpoint() == null;

        private bool IsReactFile(HttpContext context, out string filePath)
        {
            if (context.Request.Path.HasValue)
            {
                var path = context.Request.Path.Value.ToString();
                if (path != null && (path.Length >= StaticDirectoriesKey.Length) && (path.Substring(1, StaticDirectoriesKey.Length) == StaticDirectoriesKey))
                {
                    filePath = path.Substring(StaticDirectoriesKey.Length + 1, path.Length - StaticDirectoriesKey.Length - 1);
                    return true;
                }
            }
            filePath = "";
            return false;
        }

        private async Task<Task> TryServeReactFile(HttpContext context, string subPath)
        {
            if (_contentTypeProvider.TryGetContentType(subPath, out var contentType))
            {
                var contentRoot = $"{_hostingEnv.ContentRootPath}app{subPath.Replace("/", "\\")}";
                context.Response.ContentType = contentType;
                using (var fs = new FileStream(contentRoot, FileMode.Open))
                    await fs.CopyToAsync(context.Response.Body);
                return Task.CompletedTask;
            }
            return _next(context);
        }
    }



}
