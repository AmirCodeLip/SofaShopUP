using DataLayer.Infrastructure.ViewModels;
using DataLayer.Infrastructure.ViewModels.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataLayer.UnitOfWork;

namespace DataLayer.Infrastructure.Infrastructure
{
    public static class FormManager
    {
        private static Type requiredAttributeType = typeof(RequiredAttribute);
        private static Type displayAttributeType = typeof(DisplayAttribute);
        private static Type dataTypeAttributeType = typeof(DataTypeAttribute);
        private static Type stringLengthType = typeof(StringLengthAttribute);
        public static FormModel GetFromFrom(Type inputModel)
        {
            //var culture = centralizeData.httpContext.Request.Headers["Culture"].FirstOrDefault();
            //if (culture != null)
            //{
            //    Thread.CurrentThread.CurrentCulture = ConstTypes.SupportedLanguages.List[ConstTypes.SupportedLanguages.faIR].CultureInfo;
            //}
            FormModel formModel = new FormModel();
            foreach (var property in TypeDescriptor.GetProperties(inputModel))
            {
                var propertyDescriptor = ((PropertyDescriptor)property);
                var attributes = propertyDescriptor.Attributes.Cast<Attribute>();
                var required = attributes.FirstOrDefault(x => x.GetType() == requiredAttributeType) as RequiredAttribute;
                var display = attributes.FirstOrDefault(x => x.GetType() == displayAttributeType) as DisplayAttribute;
                var dataType = attributes.FirstOrDefault(x => x.GetType() == dataTypeAttributeType) as DataTypeAttribute;
                var stringLength = attributes.FirstOrDefault(x => x.GetType() == stringLengthType) as StringLengthAttribute;
                var formItem = new FormItem();
                formItem.Name = propertyDescriptor.Name;
                if (required != null)
                    formItem.FormDescriptors.Add(new InputRequired(true, required.ErrorMessage));
                if (display != null)
                {
                    if (display.ResourceType == null)
                        formItem.FormDescriptors.Add(new InputDisplay(display.Name));
                    else
                    {
                        display.ResourceType.GetProperty("Culture").SetValue(null, Thread.CurrentThread.CurrentCulture);
                        var nameProperty = display.ResourceType.GetProperty(display.Name, BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                        formItem.FormDescriptors.Add(new InputDisplay(nameProperty));
                    }
                }
                if (dataType != null)
                    formItem.FormDescriptors.Add(new InputDataType(dataType.DataType));
                if (stringLength != null)
                    formItem.FormDescriptors.Add(new InputStringLength(stringLength.ErrorMessage, stringLength.MinimumLength, stringLength.MaximumLength));
                formModel.Add(formItem);
            }
            return formModel;
        }
    }
}
