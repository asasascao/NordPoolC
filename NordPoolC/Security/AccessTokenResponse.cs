using Newtonsoft.Json.Linq;

namespace NordPoolC.Security
{
    public class AccessTokenResponse
    {
        /// <summary>
        /// 口令
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 口令类别
        /// </summary>
        public string TokenType { get; set; }

        public AccessTokenResponse Deserialize(string tokenResponse)
        {
            var jo = JObject.Parse(tokenResponse);
            Token = jo != null && jo.ContainsKey("access_token") ? jo["access_token"].ToString() : "";
            ExpiresIn = jo != null && jo.ContainsKey("expires_in") ? int.Parse(jo["expires_in"].ToString()) :0;
            TokenType = jo != null && jo.ContainsKey("token_type") ? jo["token_type"].ToString() : "";
            return this;
        }
    }
}
