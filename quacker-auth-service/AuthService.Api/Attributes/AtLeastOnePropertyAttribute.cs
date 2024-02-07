using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AuthService.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AtLeastOnePropertyAttribute : ValidationAttribute
    {
        private string[] PropertyList { get; set; }

        public AtLeastOnePropertyAttribute(params string[] propertyList)
        {
            this.PropertyList = propertyList;
        }

        public override object TypeId
        {
            get
            {
                return this;
            }
        }

        public override bool IsValid(object value)
        {
            PropertyInfo propertyInfo;
            foreach (string propertyName in PropertyList)
            {
                propertyInfo = value.GetType().GetProperty(propertyName);

                if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
