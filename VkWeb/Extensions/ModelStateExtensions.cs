using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace VkWeb.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddErrors(this ModelStateDictionary ms, Dictionary<string, List<string>> errors)
        {
            foreach (KeyValuePair<string, List<string>> error in errors)
            {
                error.Value.ForEach(v => ms.AddModelError(error.Key, v));
            }
        }
    }
}
