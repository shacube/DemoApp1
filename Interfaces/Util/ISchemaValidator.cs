using System;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface ISchemaValidator<T>
    {
        bool IsValid(string requestJson, out IList<string> errors);
    }
}