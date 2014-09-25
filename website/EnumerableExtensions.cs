using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website
{
    public static class EnumerableExtensions
    {
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> kvpList)
        {
            return kvpList.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
