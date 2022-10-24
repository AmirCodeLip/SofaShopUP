using DataLayer.Infrastructure;
using DataLayer.Infrastructure.ViewModel;
using DataLayer.Infrastructure.ViewModel.Form;
using DataLayer.Infrastructure.WebModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Primitives;
using System.Text;
using WebApp.Shop.Neptons.Models;
using static NuGet.Packaging.PackagingConstants;

namespace WebApp.Shop.Neptons
{
    public static class StaticExtensions
    {
        public static IApplicationBuilder UseReactCommunication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UrlMiddleware>();
        }
        const string WebModelsPath = "DataLayer.Infrastructure.WebModels";

        public static (string destination, string webPathLocation) GetFolder(string contentRoot, Type model)
        {
            var nameSpace = model.Namespace ?? WebModelsPath;
            var webPathLocation = nameSpace.Substring(WebModelsPath.Length, (nameSpace.Length - WebModelsPath.Length)).Replace(".", "/");
            var folder = model.Namespace?.Substring(WebModelsPath.Length, nameSpace.Length - WebModelsPath.Length);
            var destination = contentRoot;
            if (!string.IsNullOrWhiteSpace(webPathLocation))
            {
                webPathLocation = webPathLocation.Substring(1, webPathLocation.Length - 1) + "/";
            }
            if (!string.IsNullOrEmpty(folder))
            {
                foreach (var item in folder.Split("."))
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    destination += $"\\{item}";
                    if (!Directory.Exists(destination))
                        Directory.CreateDirectory(destination);
                }
            }
            destination += $"\\{model.Name}.ts";
            return (destination, webPathLocation);
        }
       
        static List<DefaultTSComponent> DefaultModels
        {
            get
            {
                var defaultModels = new List<DefaultTSComponent>();

                defaultModels.Add(
                    new DefaultTSComponent
                    {
                        DefaultName = "IFormModel",
                        Header = "import IFormModel from './../../src/mylibraries/asp-communication/interfaces/IFormModel'",
                        ModelType = typeof(FormModel)
                    });
                return defaultModels;
            }
        }
    
        public static void AddWebModels(IWebHostEnvironment hostingEnv)
        {
            var defaultModels = DefaultModels;
            var contentRoot = $"{hostingEnv.ContentRootPath}app\\webModels";
            var types = typeof(InfrastructureResolveDependencies).Assembly.GetTypes().Where(x => (x.Namespace ?? "").StartsWith(WebModelsPath));
            if (!Directory.Exists(contentRoot))
                Directory.CreateDirectory(contentRoot);
            foreach (var model in types)
            {
                var folderInfo = GetFolder(contentRoot, model);
                if (File.Exists(folderInfo.destination))
                    File.Delete(folderInfo.destination);
                using (var stram = new FileStream(folderInfo.destination, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(stram))
                    {
                        var properties = model.GetProperties();
                        StringBuilder fileHeaderData = new StringBuilder();
                        StringBuilder fileData = new StringBuilder();
                        if (model.BaseType == typeof(Enum))
                        {
                            var fields = model.GetFields();
                            for (var i = 1; i < fields.Length; i++)
                            {
                                var field = fields[i];
                                fileData.Append($"    {field.Name}");
                                if (i != fields.Length - 1)
                                    fileData.AppendLine(",");

                            }
                            sw.WriteLine($"export enum {model.Name} {{");
                            sw.WriteLine(fileData.ToString());
                            sw.WriteLine("}");
                            continue;
                        }
                        for (var i = 0; i < properties.Length; i++)
                        {
                            var property = properties[i];
                            string propertyType = "";
                            if (property.PropertyType == typeof(String))
                            {
                                propertyType = property.PropertyType.Name.ToLower();
                            } 
                            if (property.PropertyType == typeof(Guid))
                            {
                                propertyType = "string";
                            }
                            else if (property.PropertyType == typeof(int))
                            {
                                propertyType = "Int32Array";
                            }
                            else if ((property.PropertyType.Namespace ?? "").StartsWith(WebModelsPath))
                            {
                                propertyType = property.Name;
                                var folderInfoProp = GetFolder(contentRoot, property.PropertyType);
                                var folderInfoPropWPN = folderInfoProp.webPathLocation.StartsWith(folderInfo.webPathLocation) ?
                                    folderInfoProp.webPathLocation.Substring(folderInfo.webPathLocation.Length,
                                    folderInfoProp.webPathLocation.Length - folderInfo.webPathLocation.Length)
                                    : folderInfoProp.webPathLocation;
                                if (property.PropertyType.BaseType == typeof(Enum))
                                {
                                    fileHeaderData.AppendLine(@$"import {{ {property.Name} }} from './{folderInfoPropWPN}{property.Name}'");
                                }
                                else
                                {
                                    fileHeaderData.AppendLine(@$"import {property.Name} from './{folderInfoPropWPN}{property.Name}'");
                                }
                            }
                            else if (defaultModels.Any(x => x.ModelType == property.PropertyType))
                            {
                                var defaultModel = defaultModels.FirstOrDefault(x => x.ModelType == property.PropertyType);
                                fileHeaderData.AppendLine(defaultModel?.Header);
                                propertyType = defaultModel.HasDefaultName ? defaultModel.DefaultName : defaultModel.ModelType.Name;
                            }
                            if (string.IsNullOrEmpty(propertyType))
                                continue;
                            var resultLine = $"    {property.Name}: {propertyType}";
                            if (i != properties.Length - 1)
                                resultLine += ",";
                            fileData.AppendLine(resultLine);
                        }
                        sw.WriteLine(fileHeaderData.ToString());
                        sw.WriteLine($"export default interface {model.Name} {{");
                        sw.Write(fileData.ToString());
                        sw.WriteLine("}");
                    }

                }
            }
        }

        public static CentralizeData CentralizeDataFiller(this Controller controller)
        {
            return new CentralizeData(controller.HttpContext, controller.ModelState);
        }
    }
}
