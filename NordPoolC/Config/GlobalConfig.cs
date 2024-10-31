using System.Collections.Generic;

namespace NordPoolC.Config
{
    public class GlobalConfig
    {
        public static string Version => GetConfig<PConfig>().version;
        public static int DemoArea => GetConfig<PConfig>().demoArea;

        /// <summary>
        /// 配置对象
        /// </summary>
        private static Dictionary<string, ConfigBase> config = new Dictionary<string, ConfigBase>()
        { { "Sso", new Sso() }, { "Endpoints", new Endpoints() }, { "Credentials", new Credentials() }, { "PConfig", new PConfig() } };

        /// <summary>
        /// 从ini文件中加载
        /// </summary>
        /// <param name="path"></param>
        public static void LoadFromIniFile(string path)
        {
            if (config == null || config.Count <= 0) return;
            foreach (var item in config)
            {
                item.Value.LoadFromIniFile(path);
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        public static T GetConfig<T>() where T : ConfigBase
        {
            if (config == null || config.Count <= 0 || !config.ContainsKey(typeof(T).Name)) return default(T);
            return (T)(config[typeof(T).Name]);
        }
    }
}
