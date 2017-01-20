using Pfarah;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SemiPlausibleRandomizer
{
    static class ParaValueHelper
    {
        public static ParaValue Get(this ParaValue.Record record, string key)
        {
            if (record.TryGet(key, out var value))
            {
                return value;
            }
            throw new Exception($"Key {key} not found in ParaValue record");
        }

        public static bool TryGet(this ParaValue.Record record, string key, out ParaValue value)
        {
            foreach (var item in record.properties)
            {
                if (item.Item1 == key)
                {
                    value = item.Item2;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public static IEnumerable<string> ToStringArray(this ParaValue value)
        {
            if (value.IsArray)
            {
                return (value as ParaValue.Array).elements.Select(x => (x as ParaValue.String).Item);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
