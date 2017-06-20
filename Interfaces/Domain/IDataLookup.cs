using System;
using System.Collections.Generic;

namespace DemoApp1
{
    public interface IDataLookup<T> : ICollection<T>
    {
        Dictionary<string, T> Map {get;}
        void Add(string key, T item);
        bool ContainsKey(string itemKey);
    }
}