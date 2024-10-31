using NordPoolC.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NordPoolC.Builders
{
    public static class DestinationBuilder
    {
        /// <summary>
        /// 拼接目标路径
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="mode">发送模式</param>
        /// <param name="topic">话题</param>
        /// <returns>目标路径</returns>
        public static string ComposeDestination(string user, string version, PublishingMode mode, string topic)
        {
            return ComposeDestination(user, version, $"{mode.ToString().ToLower()}/{topic}");
        }

        /// <summary>
        /// 拼接目标路径
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="version">版本</param>
        /// <param name="topic">话题</param>
        /// <returns>目标路径</returns>
        public static string ComposeDestination(string user, string version, string topic)
        {
            return $"/user/{user}/{version}/{topic}";
        }

        /// <summary>
        /// 拼接目标路径
        /// </summary>
        /// <param name="version">版本</param>
        /// <param name="topic">话题</param>
        /// <returns>目标路径</returns>
        public static string ComposeDestination(string version, string topic)
        {
            return $"/{version}/{topic}";
        }
    }
}
