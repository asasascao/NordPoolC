using NordPoolC.Helper;
using System;

namespace NordPoolC.Config
{
    public class Sso : ConfigBase
    {
        /// <summary>
        /// url
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// 范围
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public override ConfigBase LoadFromIniFile(string path)
        {
            string uriStr = INIHelper.ReadString("Sso", "Uri", "", path);
            Uri = uriStr == null|| string.IsNullOrWhiteSpace(uriStr)?null: new Uri(uriStr);
            Scope = INIHelper.ReadString("Sso", "Scope", "", path);
            ClientId = INIHelper.ReadString("Sso", "ClientId", "", path);
            ClientSecret = INIHelper.ReadString("Sso", "ClientSecret", "", path);
            return this;
        }
    }
}
