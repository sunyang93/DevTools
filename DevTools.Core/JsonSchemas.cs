using DevTools.Data;
using NJsonSchema;
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
        public string GenerateJsonSchema<T>(JsonSchemaOutputFormatter jsonSchemaOutputFormatter) where T : class
        {
            string output = string.Empty;
            JsonSchema schema = JsonSchema.FromType<T>();
            if(jsonSchemaOutputFormatter==JsonSchemaOutputFormatter.JSON)
            {
                output = schema.ToJson();
            }
            else if(jsonSchemaOutputFormatter==JsonSchemaOutputFormatter.YAML) 
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
