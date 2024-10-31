using System.Collections;
using System.Collections.Generic;

namespace NordPoolC.Extensions
{
    public static class IDictionaryEx
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns></returns>
        public static bool TryGetValue(this IDictionary dictionary, string key, out object value)
        {
            if (dictionary.Contains(key))
            {
                value = dictionary[key];
                return true;
            }
            value = null;
            return false;
        }

        /// <summary>
        /// 转换为IEnumerable<KeyValuePair<object,object>>
        /// </summary>
        /// <param name="dictionary">字典</param>
        /// <returns>IEnumerable<KeyValuePair<object,object>></returns>
        public static IEnumerable<KeyValuePair<object,object>> AsEnumerable(this IDictionary dictionary)
        {
            List<KeyValuePair<object, object>> dic = new List<KeyValuePair<object, object>>();
            foreach (var item in dictionary.Keys)
            {
                dic.Add(new KeyValuePair<object, object>(item, dictionary[item]));
            }
            return dic;
        }
    }
}
