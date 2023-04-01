using DataLayer.Infrastructure.ViewModels.Form;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Text;

namespace DataLayer.Infrastructure.Services
{
    public class TSModelGeneratore
    {
        const string WebModelsPath = "DataLayer.Infrastructure.WebModels";

        public static (string destination, string webPathLocation) GetFolder(string contentRoot, Type model, string webModelsPath)
        {
            var nameSpace = model.Namespace ?? webModelsPath;
            var webPathLocation = nameSpace.Substring(webModelsPath.Length, (nameSpace.Length - webModelsPath.Length)).Replace(".", "/");
            var folder = model.Namespace?.Substring(webModelsPath.Length, nameSpace.Length - webModelsPath.Length);
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

        public static void AddFileManagerWebModel(IWebHostEnvironment hostingEnv)
        {
            var types = typeof(InfrastructureResolveDependencies).Assembly.GetTypes().Where(x => (x.Namespace ?? "").StartsWith(WebModelsPath));
            AddWebModels(types, $"{hostingEnv.ContentRootPath}app\\src\\webModels", DefaultModels, WebModelsPath);
        }

        private static inPublicTypeResponse inPublicType(Type propertyType, argumentPulbic ap)
        {
            var response = new inPublicTypeResponse();
            if (propertyType.Namespace == "System" && propertyType.Name == "Nullable`1")
            {
                response.IsNullable = true;
                propertyType = propertyType.GetGenericArguments()[0]; // use this...
            }
            if (propertyType == typeof(String))
            {
                response.PropertyTypeString = propertyType.Name.ToLower();
            }
            else if (propertyType == typeof(Guid))
            {
                response.PropertyTypeString = "string";
            }
            else if (propertyType == typeof(int))
            {
                response.PropertyTypeString = "number";
            }
            else if (propertyType == typeof(bool))
            {
                response.PropertyTypeString = "boolean";
            }
            else if (propertyType.IsGenericType)
            {
                var generic = getGenericType(propertyType, ap);
                response.FileHeaderData.Append(generic.FileHeaderData);
                response.PropertyTypeString = generic.PropertyTypeString;
            }
            else if ((propertyType.Namespace ?? "").StartsWith(WebModelsPath))
            {
                response.PropertyTypeString = propertyType.Name;
                var folderInfoProp = GetFolder(ap.destinationRoot, propertyType, ap.webModelsPath);
                var folderInfoPropWPN = folderInfoProp.webPathLocation.StartsWith(folderInfoProp.webPathLocation) ?
                    folderInfoProp.webPathLocation.Substring(folderInfoProp.webPathLocation.Length,
                    folderInfoProp.webPathLocation.Length - folderInfoProp.webPathLocation.Length)
                    : folderInfoProp.webPathLocation;
                if (propertyType.BaseType == typeof(Enum))
                {
                    response.FileHeaderData.AppendLine(@$"import {{{propertyType.Name}}} from './{folderInfoPropWPN}{propertyType.Name}'");
                }
                else
                {
                    response.FileHeaderData.AppendLine(@$"import {propertyType.Name} from './{folderInfoPropWPN}{propertyType.Name}'");
                }
            }
            else if (ap.defaultModels != null && ap.defaultModels.Any(x => x.ModelType == propertyType))
            {
                var defaultModel = ap.defaultModels.FirstOrDefault(x => x.ModelType == propertyType);
                response.FileHeaderData.AppendLine(defaultModel?.Header);
                response.PropertyTypeString = defaultModel.HasDefaultName ? defaultModel.DefaultName : defaultModel.ModelType.Name;
            }
            else
            {
                response.State = false;
            }
            return response;
        }

        private static inPublicTypeResponse getGenericType(Type propertyType, argumentPulbic ap)
        {
            inPublicTypeResponse response = new();
            if (propertyType.Name == "List`1" && propertyType.Namespace == "System.Collections.Generic")
            {
                var generic = propertyType.GetGenericArguments()[0];

                if (generic.IsGenericType)
                {
                    var g2 = getGenericType(generic, ap);
                    response.FileHeaderData.Append(g2.FileHeaderData);
                    response.PropertyTypeInfo = $"Array<{g2.PropertyTypeString}>";
                }
                var exist = inPublicType(generic, ap);
                if (exist.State)
                {
                    response.FileHeaderData.Append(exist.FileHeaderData);
                    response.PropertyTypeString = $"Array<{exist.PropertyTypeString}>";
                }

            }
            return response;
        }

        public static void AddWebModels(IEnumerable<Type> types, string destinationRoot, List<DefaultTSComponent> defaultModels, string webModelsPath)
        {
            if (!Directory.Exists(destinationRoot))
                Directory.CreateDirectory(destinationRoot);
            argumentPulbic argumentPulbic = new(destinationRoot, webModelsPath, defaultModels);
            foreach (var model in types)
            {
                var usageType = model.GetCustomAttributes<TSModelUsageAttribute>().FirstOrDefault();
                if (usageType != null)
                {
                    if (usageType.CompileOption == CompileOption.ignore)
                        continue;
                }
                var folderInfo = GetFolder(destinationRoot, model, webModelsPath);
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
                            var modelDescription = property.GetCustomAttribute<TSModelDescriptionAttribute>();
                            string propertyTypeString = "";
                            var propertyType = property.PropertyType;
                            var exist = inPublicType(propertyType, argumentPulbic);
                            fileHeaderData.Append(exist.FileHeaderData);
                            if (exist.State)
                                propertyTypeString = exist.PropertyTypeString;
                            if (string.IsNullOrEmpty(propertyTypeString))
                                continue;
                            var resultLine = $"    {property.Name + ((modelDescription != null && modelDescription.Optional) ? "?" : "")}: {propertyTypeString}" + (exist.IsNullable ? " | null" : "");
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
        private class inPublicTypeResponse
        {
            public inPublicTypeResponse()
            {
                FileHeaderData = new StringBuilder();
                State = true;
            }
            public bool IsNullable { get; set; }
            public string PropertyTypeInfo { get; set; }
            public string PropertyTypeString { get; set; }
            public StringBuilder FileHeaderData { get; set; }
            public bool State { get; set; }
        }
        public class argumentPulbic
        {
            public argumentPulbic(string destinationRoot, string webModelsPath, List<DefaultTSComponent> defaultModels)
            {
                this.destinationRoot = destinationRoot;
                this.webModelsPath = webModelsPath;
                this.defaultModels = defaultModels;
            }
            public readonly string webModelsPath;
            public readonly string destinationRoot;
            public readonly List<DefaultTSComponent> defaultModels;
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

    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
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

    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class TSModelDescriptionAttribute : Attribute
    {
        public TSModelDescriptionAttribute()
        {
        }
        public bool Optional { set; get; } = false;
    }

    public enum CompileOption
    {
        compile, ignore
    }
}
