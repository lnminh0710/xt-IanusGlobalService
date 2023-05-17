namespace IanusGlobalServiceApi.Models
{
    public class ServiceRequest
    {
        #region Ctors
        /// <summary>
        /// Default ctor for ServiceRequest
        /// </summary>
        public ServiceRequest()
        {

        }
        /// <summary>
        /// Default ctor for ServiceRequest
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <param name="serviceName">Service name</param>
        /// <param name="data">Data parameter</param>
        public ServiceRequest(string moduleName, string serviceName, object data)
        {
            this.ModuleName = moduleName;
            this.ServiceName = serviceName;
            this.Data = data;
        }
        #endregion

        #region Properties
        public string ModuleName { get; set; }
        //TODO: forse questo sarebbe da chiamare MethodName
        public string ServiceName { get; set; }
        public object Data { get; set; }
        #endregion
    }
}
