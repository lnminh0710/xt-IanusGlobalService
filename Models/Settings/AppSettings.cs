using System.Collections.Generic;

namespace IanusGlobalServiceApi.Models
{
    /// <summary>
    /// AppSettings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Default ConnectionString
        /// </summary>
        public string DefaultConnectionString { get; set; }

        /// <summary>
        /// CommandTimeout: In seconds. The default is 30 seconds
        /// </summary>
        public int DefaultCommandTimeout { get; set; }

        /// <summary>
        /// ServerConfig
        /// </summary>
        public IList<ServerConfig> ServerConfig { get; set; }

        /// <summary>
        /// AppSettings
        /// </summary>
        public AppSettings()
        {
        }
    }
}
