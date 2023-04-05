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
using System.Xml.Linq;

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
                var displayName = "";
                if (display != null)
                {
                    if (display.ResourceType == null)
                        displayName = display.Name;
                    else
                    {
                        display.ResourceType.GetProperty("Culture").SetValue(null, Thread.CurrentThread.CurrentCulture);
                        displayName = display.ResourceType.GetProperty(display.Name, BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                    }
                    formItem.FormDescriptors.Add(new InputDisplay(displayName));
                }
                if (required != null)
                {

                    if (required.ErrorMessageResourceType == null)
                        formItem.FormDescriptors.Add(new InputRequired(true, string.Format(required.ErrorMessage, displayName)));
                    else
                    {
                        required.ErrorMessageResourceType.GetProperty("Culture").SetValue(null, Thread.CurrentThread.CurrentCulture);
                        var errorMessage = required.ErrorMessageResourceType.GetProperty(required.ErrorMessage, BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                        formItem.FormDescriptors.Add(new InputRequired(true, string.Format(errorMessage, displayName)));
                    }
                }
                if (dataType != null)
                    formItem.FormDescriptors.Add(new InputDataType(dataType.DataType));
                if (stringLength != null)
                {
                    var slError = "";
                    if (stringLength.ErrorMessageResourceType == null)
                        slError = stringLength.ErrorMessage;
                    else
                    {
                        stringLength.ErrorMessageResourceType.GetProperty("Culture").SetValue(null, Thread.CurrentThread.CurrentCulture);
                        slError = stringLength.ErrorMessageResourceType.GetProperty(stringLength.ErrorMessage, BindingFlags.Static | BindingFlags.Public).GetValue(null) as string;
                    }
                    formItem.FormDescriptors.Add(new InputStringLength(string.Format(slError, displayName, stringLength.MinimumLength, stringLength.MaximumLength), stringLength.MinimumLength, stringLength.MaximumLength));
                }
                formModel.Add(formItem);
            }
            return formModel;
        }
    }
}
