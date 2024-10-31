namespace NordPoolC.Config
{
    public class Endpoints : ConfigBase
    {
        /// <summary>
        /// 交易
        /// </summary>
        public SslPoint Trading { get; private set; }

        /// <summary>
        /// 市场信息
        /// </summary>
        public SslPoint MarketData { get; private set; }

        public Endpoints()
        {
            Trading = new SslPoint();
            MarketData = new SslPoint();
        }

        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public override ConfigBase LoadFromIniFile(string path)
        {
            Trading.LoadFromIniFile(path,nameof(Endpoints), nameof(Trading));
            MarketData.LoadFromIniFile(path,nameof(Endpoints), nameof(MarketData)); 
            return this;
        }
    }
}
