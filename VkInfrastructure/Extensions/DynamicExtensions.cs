using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace VkInfrastructure.Extensions
{
    public static class DynamicUtilities
    {
        public static bool HasProperty(dynamic checkMe, string property)
        {
            if (checkMe is ExpandoObject)
                return ((IDictionary<string, object>)checkMe).ContainsKey(property);

            return checkMe.GetType().GetProperty(property) != null;
        }
    }
}
