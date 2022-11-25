using DataLayer.Infrastructure.ViewModels.Form;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Numerics;
using DataLayer.Domin;

namespace DataLayer.Infrastructure.Services
{
    public class TSModelGeneratore
    {
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
                        Header = "import IFormModel from './../../mylibraries/asp-communication/interfaces/IFormModel'",
                        ModelType = typeof(FormModel)
                    });
                return defaultModels;
            }
        }

        public static void AddWebModels(IWebHostEnvironment hostingEnv)
        {
            var defaultModels = DefaultModels;
            var contentRoot = $"{hostingEnv.ContentRootPath}app\\src\\webModels";
            var types = typeof(InfrastructureResolveDependencies).Assembly.GetTypes().Where(x => (x.Namespace ?? "").StartsWith(WebModelsPath));
            if (!Directory.Exists(contentRoot))
                Directory.CreateDirectory(contentRoot);
            foreach (var model in types)
            {
                var usageType = model.GetCustomAttributes<TSModelUsageAttribute>().FirstOrDefault();
                if (usageType != null)
                {
                    if (usageType.CompileOption == CompileOption.ignore)
                        continue;
                }
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
                            string propertyTypeString = "";
                            var propertyType = property.PropertyType;
                            bool isNullable = false;
                            if (propertyType.Namespace == "System" && propertyType.Name == "Nullable`1")
                            {
                                isNullable = true;
                                propertyType = propertyType.GetGenericArguments()[0]; // use this...
                            }
                            if (propertyType == typeof(String))
                            {
                                propertyTypeString = propertyType.Name.ToLower();
                            }
                            else if (propertyType == typeof(Guid))
                            {
                                propertyTypeString = "string";
                            }
                            else if (propertyType == typeof(int))
                            {
                                propertyTypeString = "Int32Array";
                            }
                            else if (propertyType == typeof(bool))
                            {
                                propertyTypeString = "boolean";
                            }
                            else if ((propertyType.Namespace ?? "").StartsWith(WebModelsPath))
                            {
                                propertyTypeString = property.Name;
                                var folderInfoProp = GetFolder(contentRoot, propertyType);
                                var folderInfoPropWPN = folderInfoProp.webPathLocation.StartsWith(folderInfo.webPathLocation) ?
                                    folderInfoProp.webPathLocation.Substring(folderInfo.webPathLocation.Length,
                                    folderInfoProp.webPathLocation.Length - folderInfo.webPathLocation.Length)
                                    : folderInfoProp.webPathLocation;
                                if (propertyType.BaseType == typeof(Enum))
                                {
                                    fileHeaderData.AppendLine(@$"import {{ {property.Name} }} from './{folderInfoPropWPN}{property.Name}'");
                                }
                                else
                                {
                                    fileHeaderData.AppendLine(@$"import {property.Name} from './{folderInfoPropWPN}{property.Name}'");
                                }
                            }
                            else if (defaultModels.Any(x => x.ModelType == propertyType))
                            {
                                var defaultModel = defaultModels.FirstOrDefault(x => x.ModelType == propertyType);
                                fileHeaderData.AppendLine(defaultModel?.Header);
                                propertyTypeString = defaultModel.HasDefaultName ? defaultModel.DefaultName : defaultModel.ModelType.Name;
                            }
                            if (string.IsNullOrEmpty(propertyTypeString))
                                continue;


                            var resultLine = $"    {property.Name}: {propertyTypeString}" + (isNullable ? " | null" : "");
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

    }

    public class DefaultTSComponent
    {
        public bool HasDefaultName { get; private set; } = false;
        public string defaultName;
        public string DefaultName
        {
            get => defaultName; set
            {
                HasDefaultName = !string.IsNullOrWhiteSpace(value);
                defaultName = value;
            }
        }
        public string Header { get; set; }
        public Type ModelType { get; set; }
    }

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class TSModelUsageAttribute : Attribute
    {
        public TSModelUsageAttribute()
        {
        }

        private CompileOption compileOption = CompileOption.compile;
        public CompileOption CompileOption
        {
            set { compileOption = value; }
            get { return compileOption; }
        }

    }

    public enum CompileOption
    {
        compile, ignore
    }
}
