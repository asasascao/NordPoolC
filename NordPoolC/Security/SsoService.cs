using Newtonsoft.Json;
using NordPoolC.Config;
using NordPoolC.Connection;
using NordPoolC.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NordPoolC.Security
{
    public class SsoService: ISsoService
    {
        private static Lazy<SsoService> sso = new Lazy<SsoService>();
        public static ISsoService Instance => sso.Value;


        private readonly string _grantType;
        private readonly string _scope;

        private readonly HttpClient _httpClient;

        /// <summary>
        /// token
        /// </summary>
        private string ConstructPayloadForToken
            => $"grant_type={_grantType}&scope={_scope}&username={GlobalConfig.GetConfig<Credentials>().Username}&" +
            $"password={System.Net.WebUtility.UrlEncode(GlobalConfig.GetConfig<Credentials>().Password)}";

        public SsoService()
        {
            var ssoOptions=GlobalConfig.GetConfig<Sso>();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ssoOptions.ClientId}:{ssoOptions.ClientSecret}")));
            _httpClient.BaseAddress = ssoOptions.Uri;
            _grantType = "password";
            _scope = "global";
        }

        /// <summary>
        /// 异步获取token
        /// </summary>
        /// <returns></returns>
        /// <exception cref="TokenRequestFailedException"></exception>
        public async Task<string> GetAuthTokenAsync()
        {
            try
            {
                const string mediaType = "application/x-www-form-urlencoded";
                var response = await _httpClient.PostAsync("", new StringContent(ConstructPayloadForToken, Encoding.UTF8, mediaType));
                response.EnsureSuccessStatusCode();
                var tokenResponse = await response.Content.ReadAsStringAsync();
                return (new AccessTokenResponse().Deserialize(tokenResponse)).Token;
            }
            catch (Exception e)
            {
                throw new TokenRequestFailedException("Failed to retrieve auth token! Check username and password!", e);
            }
        }
    }
}
