using NordPoolC.Helper;

namespace NordPoolC.Config
{
    public class SslPoint
    {
        /// <summary>
        /// 服务机
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public int SslPort { get; private set; }
        
        /// <summary>
        /// url
        /// </summary>
        public string Uri { get; private set; }

        /// <summary>
        /// 心跳频率
        /// </summary>
        public int HeartbeatOutgoingInterval { get; private set; }

        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public void LoadFromIniFile(string path,string sectionName, string subPath)
        {
            string tradingStr = INIHelper.ReadString("Endpoints", "Trading", "", path);
            string marketdataStr = INIHelper.ReadString("Endpoints", "MarketData", "", path);

            Host = INIHelper.ReadString(sectionName, subPath + "_Host", "", path);
            SslPort = INIHelper.ReadInteger(sectionName, subPath + "_SslPort", 80, path);
            Uri = INIHelper.ReadString(sectionName, subPath + "_Uri", "", path);
            HeartbeatOutgoingInterval = INIHelper.ReadInteger(sectionName, subPath + "_HeartbeatOutgoingInterval", 1000, path);
        }
    }
}
