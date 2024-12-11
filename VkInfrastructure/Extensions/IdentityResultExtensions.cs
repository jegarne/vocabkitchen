using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VkInfrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Dictionary<string, List<string>> GetErrors(this IdentityResult ir)
        {
            var result = new Dictionary<string, List<string>>();
            foreach (var error in ir.Errors)
            {
                if (result.ContainsKey(error.Code))
                    result[error.Code].Add(error.Description);
                else
                    result.Add(error.Code, new List<string>() { error.Description });
            }

            return result;
        }
    }
}
