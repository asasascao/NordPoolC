using NordPoolC.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Config
{
    public class PConfig : ConfigBase
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; private set; } = "v1";

        /// <summary>
        /// 客户端id
        /// </summary>
        public string clientId { get; private set; } = $"{Guid.NewGuid()}-dotnet-client";

        /// <summary>
        /// 区域
        /// </summary>
        public int demoArea { get; private set; } = 2;//3 = Finland

        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public override ConfigBase LoadFromIniFile(string path)
        {
            version = INIHelper.ReadString("PConfig", "version", "v1", path);
            clientId = INIHelper.ReadString("PConfig", "clientId", $"{Guid.NewGuid()}-dotnet-client", path);
            demoArea = INIHelper.ReadInteger("PConfig", "demoArea", 2, path);
            return this;
        }
    }
}
