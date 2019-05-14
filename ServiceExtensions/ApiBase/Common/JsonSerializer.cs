using ApiBase.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Extensions.ApiBase.Common
{
    public static class JsonSerializer
    {
        //set jsonserializer settings for the application
        public static JsonSerializerSettings Settings { get; }

        static JsonSerializer()
        {
            Settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(false, false)
                }
            };

            JsonConvert.DefaultSettings = () => Settings;
        }

        public static string GetResolvedPropertyName<TModel>(string propertyName)
        {
            var jsonProperty = typeof(TModel).GetProperty(propertyName).GetCustomAttribute<JsonPropertyAttribute>();

            return !string.IsNullOrEmpty(jsonProperty?.PropertyName)
                ? jsonProperty.PropertyName
                : GetResolvedPropertyName(propertyName);
        }

        public static string GetResolvedPropertyName(string propertyName)
        {
            return ((DefaultContractResolver)Settings.ContractResolver).GetResolvedPropertyName(propertyName);
        }

        public static void ValidatePropertyNames<T>(this JToken token,bool ignoreSnakeCase = true)
        {
            var invalidPropertyNames = new List<string>();

            foreach(var jsonPropertyName in token.Children().Select(childtokens => childtokens.Path).ToList())
            {
                var propertyName = typeof(T).GetProperty(ignoreSnakeCase ? jsonPropertyName.Replace("_", string.Empty) : jsonPropertyName,
                    BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);

                if (propertyName is null)
                    invalidPropertyNames.Add(jsonPropertyName); 
            }

            if(invalidPropertyNames.Any())
            {
                throw new ValidationException("Invalid property names");
            }
        }
    }
}
