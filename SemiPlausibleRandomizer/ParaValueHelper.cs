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

        public static string GetString(this ParaValue.Record record, string key)
        {
            return (record.Get(key) as ParaValue.String).Item;
        }

        public static string GetString(this ParaValue.Record record, string key, string defaultValue)
        {
            if (record.TryGet(key, out var value) && value.IsString)
            {
                return (value as ParaValue.String).Item;
            }
            return defaultValue;
        }
        public static int GetInt(this ParaValue.Record record, string key, int defaultValue)
        {
            if (record.TryGet(key, out var value) && value.IsNumber)
            {
                return (int)((value as ParaValue.Number).Item);
            }
            return defaultValue;
        }

        public static bool GetBool(this ParaValue.Record record, string key, bool defaultValue = false)
        {
            if (record.TryGet(key, out var value) && value.IsString)
            {
                return (value as ParaValue.String).Item == "yes";
            }
            return defaultValue;
        }

        public static IEnumerable<ParaValue.Record> GetAllRecords(this ParaValue.Record record, string key)
        {
            return record.properties.Where(item => item.Item1 == key && item.Item2.IsRecord).Select(item => item.Item2 as ParaValue.Record);
        }

        public static IEnumerable<string> GetAllStrings(this ParaValue.Record record, string key)
        {
            return record.properties.Where(item => item.Item1 == key && item.Item2.IsString).Select(item => (item.Item2 as ParaValue.String).Item);
        }

        public static int GetInt(this ParaValue.Record record, string key)
        {
            return (int)((record.Get(key) as ParaValue.Number).Item);
        }

        public static IEnumerable<string> ToStringCollection(this ParaValue value)
        {
            if (value.IsArray)
            {
                return (value as ParaValue.Array).elements.Where(x => x.IsString).Select(x => (x as ParaValue.String).Item);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public static IEnumerable<int> ToIntCollection(this ParaValue value)
        {
            if (value.IsArray)
            {
                return (value as ParaValue.Array).elements.Where(x => x.IsNumber).Select(x => (int)((x as ParaValue.Number).Item));
            }
            else
            {
                return Enumerable.Empty<int>();
            }
        }
    }
}
