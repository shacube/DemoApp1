using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DemoApp1.Domain
{
    public class DataLookupBase<T> : IDataLookup<T>
    {
        public static readonly string MapItemDocPathFormat = @"map.{0}";
        public static readonly string ModifiedWhenDocPath = "modifiedwhen";

        [JsonProperty("map") ]
        public Dictionary<string, T> Map {get; set;}        

        [JsonProperty("count")]
        public int Count { get {return Map.Count;} }
        
        [JsonIgnore]
        public bool IsReadOnly { get {return false;} }

        public DataLookupBase()
        {
            Map = new Dictionary<string, T>();
        }        

        public DataLookupBase(IEnumerable<T> initList)
        {
            Map = new Dictionary<string, T>();
            if(initList != null)
            {
                foreach(var item in initList)
                {
                    this.Add(item);
                }
            }
        }
        
        public virtual void Add(T item)
        {
            Map.Add(item.ToString(), item);
        }

        public virtual void Add(string key, T item)
        {
            Map.Add(key, item);
        }

        public void Clear()
        {
            Map.Clear();
        }
        
        public virtual bool Contains(T item)
        {
            return Map.ContainsKey(item.ToString());
        }

        public virtual bool ContainsKey(string itemKey)
        {
            return Map.ContainsKey(itemKey);
        }
        
        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }
        
        public virtual bool Remove(T item)
        {
            return Map.Remove(item.ToString());
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return Map.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Map.Values.GetEnumerator();
        }                        

    }
}