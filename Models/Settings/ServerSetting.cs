namespace IanusGlobalServiceApi.Models
{
    /// <summary>
    /// ServerSetting
    /// </summary>
    public class ServerSetting
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// CommandTimeout: In seconds. The default is 30 seconds
        /// </summary>
        public int CommandTimeout { get; set; }
    }
}
