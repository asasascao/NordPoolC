using NordPoolC.Helper;

namespace NordPoolC.Config
{
    public class Credentials : ConfigBase
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// 从Ini文件加载
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>对象</returns>
        public override ConfigBase LoadFromIniFile(string path)
        {
            Username=INIHelper.ReadString("Credentials", "Username", "", path);
            Password = INIHelper.ReadString("Credentials", "Password", "", path);
            return this;
        }
    }
}
