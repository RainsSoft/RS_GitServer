using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSGit
{
    public static class TaskExtensions
    {
#if NET40
      


#endif
    }
    public static class DictionaryExtensions
    {
        public static TValue TryGetValue<TKey, TValue>( this IDictionary<TKey, TValue> thisObject,  TKey key)
        {
            TValue result;
            thisObject.TryGetValue(key, out result);
            return result;
        }
        public static bool TryAdd<TKey, TValue> (this IDictionary<TKey, TValue> thisObject, TKey key, TValue value) {
            if (thisObject.ContainsKey(key)) return false;
            else { 
                thisObject.Add(key, value);
                return true;
            }
        }
        public static void AddRange<TKey, TValue>( this IDictionary<TKey, TValue> thisObject,  IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                thisObject.Add(keyValuePair);
            }
        }

        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> thisObject,  IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                thisObject[keyValuePair.Key] = keyValuePair.Value;
            }
        }
    }

}