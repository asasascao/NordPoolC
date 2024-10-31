using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NordPoolC.Enums;

namespace NordPoolC.Commands
{
    public class TokenRefreshCommand
    {
        /// <summary>
        /// 旧token
        /// </summary>
        public string OldToken { get; set; }

        /// <summary>
        /// 新token
        /// </summary>
        public string NewToken { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        public CommandType? Type { get; set; }

        public override string ToString()
        {
            JObject jo = new JObject();
            jo.Add("OldToken", OldToken);
            jo.Add("NewToken", NewToken);
            return jo.ToString(Formatting.None);
        }
    }
}
