using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace IanusGlobalServiceApi.Models
{    
    /// <summary>
    /// AppServerSetting
    /// </summary>
    public class AppServerSetting : IAppServerSetting
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// AppServerSetting
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="appSettings"></param>
        public AppServerSetting(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// ServerConfig
        /// </summary>
        public ServerConfig ServerConfig
        {
            get
            {
                string host = _httpContextAccessor.HttpContext.Request.Host.Host;
                return _appSettings.ServerConfig.Where(p => p.Domain.Equals(host, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
        }
    }
}
