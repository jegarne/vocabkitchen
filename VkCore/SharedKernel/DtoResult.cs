using System;
using System.Collections.Generic;
using System.Linq;
using VkCore.Interfaces;

namespace VkCore.SharedKernel
{
    public class DtoResult<T>
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public T Value { get; private set; }

        public void AddError(string key, string message)
        {
            if (!_errors.ContainsKey(key))
            {
                _errors.Add(key, new List<string>());
            }

            // don't add duplicate errors
            if (_errors[key].Contains(message))
                return;

            _errors[key].Add(message);
        }

        public void AddError(string message)
        {
            this.AddError(String.Empty, message);
        }

        public void AddErrors(IResponseErrorLogger errorLogger)
        {
            errorLogger.Errors.ToList().ForEach(e => this.AddError(String.Empty, e));
        }

        public void AddErrors(Dictionary<string, List<string>> errors)
        {
            errors.ToList().ForEach(kv =>
            {
                kv.Value.ForEach(v => this.AddError(kv.Key, v));
            });
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public Dictionary<string, List<string>> GetErrors()
        {
            return _errors;
        }

        public void SetValue(T result) { Value = result; }
    }
}
