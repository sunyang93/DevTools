using DevTools.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.Generation;
using NJsonSchema.Yaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Core
{
    public class JsonSchemas
    {
        public string GenerateJsonSchema<T>(JsonSchemaOutputFormatter jsonSchemaOutputFormatter,
            JsonSerializerSettings? jsonSerializerSettings = null) where T : class
        {
            jsonSerializerSettings ??= new JsonSerializerSettings()
                {
                    ContractResolver = new DefaultContractResolver() // CamelCasePropertyNamesContractResolver()
                };
            string output = string.Empty;
            JsonSchema schema = JsonSchema.FromType<T>(new JsonSchemaGeneratorSettings()
            {
                SerializerSettings = jsonSerializerSettings
            });
            if (jsonSchemaOutputFormatter == JsonSchemaOutputFormatter.JSON)
            {
                output = schema.ToJson();
            }
            else if (jsonSchemaOutputFormatter == JsonSchemaOutputFormatter.YAML)
            {
                output = schema.ToYaml();
            }
            return output;
        }
    }

    public enum JsonSchemaOutputFormatter
    {
        JSON,
        YAML
    }
}
