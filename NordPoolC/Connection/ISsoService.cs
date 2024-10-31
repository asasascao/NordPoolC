using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Connection
{
    public interface ISsoService
    {
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns>token</returns>
        Task<string> GetAuthTokenAsync();
    }
}
