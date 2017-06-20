using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace DemoApp1.Util
{
    public class SchemaValidator<T> : ISchemaValidator<T>
    {
        private readonly JSchema objectSchema;
        public SchemaValidator()
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            objectSchema = generator.Generate(typeof(T));            
        }
        public bool IsValid(string requestJson, out IList<string> errors)
        {
            JObject jObj = JObject.Parse(requestJson);
            return jObj.IsValid(objectSchema, out errors);
        }
    }
}