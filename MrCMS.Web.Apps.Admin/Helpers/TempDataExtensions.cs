using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MrCMS.Helpers;
using Newtonsoft.Json;

namespace MrCMS.Web.Apps.Admin.Helpers
{
    public static class TempDataExtensions
    {
        public static void Set<T>(this ITempDataDictionary tempData, T model, params object[] context) where T : class
        {
            if (tempData == null)
                return;

            var key = typeof(T).FullName;
            var serializableObjects = context.Where(x => x != null);
            if (serializableObjects.Any())
                key += "." + string.Join(".", serializableObjects);
            tempData[key] = JsonConvert.SerializeObject(model);
        }
        public static void Set(this ITempDataDictionary tempData, Type type, object model, params object[] context)
        {
            if (tempData == null)
                return;
            var key = type.FullName;
            var serializableObjects = context.Where(x => x != null);
            if (serializableObjects.Any())
                key += "." + string.Join(".", serializableObjects);
            tempData[key] = JsonConvert.SerializeObject(model);
        }

        public static object Get(this ITempDataDictionary tempData, Type type, params object[] context)
        {
            if (tempData == null)
                return type.GetDefaultValue();

            var key = type.FullName;
            var serializableObjects = context.Where(x => x != null);
            if (serializableObjects.Any())
                key += "." + string.Join(".", serializableObjects);
            if (!tempData.ContainsKey(key))
                return type.GetDefaultValue();
            return JsonConvert.DeserializeObject(tempData[key].ToString(), type);
        }

        public static T Get<T>(this ITempDataDictionary tempData, params object[] context) where T : class
        {
            return Get(tempData, typeof(T), context) as T;
        }
    }
}