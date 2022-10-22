﻿using DataLayer.Infrastructure.ViewModel.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    formItem.FormDescriptors.Add(new InputDisplay(display.Name));
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