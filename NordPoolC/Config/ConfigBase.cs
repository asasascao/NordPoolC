namespace NordPoolC.Config
{
    public abstract class ConfigBase
    {
        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public abstract ConfigBase LoadFromIniFile(string path);
    }
}
