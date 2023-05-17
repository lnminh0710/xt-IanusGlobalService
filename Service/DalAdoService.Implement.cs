using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using IanusGlobalServiceApi.Models;
using System.Data.SqlClient;
using System.Data;
using System.Dynamic;
using System.Text.Json;

namespace IanusGlobalServiceApi.Service
{
    /// <summary>
    /// Dal Ado Service
    /// </summary>
    public class DalAdoService : IDalAdoService
    {
        private readonly AppSettings _appSettings;
        private readonly ServerConfig _serverConfig;
        private string _connectionString = string.Empty;
        private int _commandTimeout = 0;//In seconds. The default is 30 seconds

        /// <summary>
        /// Dal Ado Service
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="appSettings"></param>
        /// <param name="appServerSetting"></param>
        public DalAdoService(IOptions<AppSettings> appSettings, IAppServerSetting appServerSetting)
        {
            _appSettings = appSettings.Value;
            _serverConfig = appServerSetting.ServerConfig;

            if (_serverConfig != null && _serverConfig.ServerSetting != null)
            {
                _connectionString = _serverConfig.ServerSetting.ConnectionString;
                _commandTimeout = _serverConfig.ServerSetting.CommandTimeout;
            }

            //Default
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = _appSettings.DefaultConnectionString;
            if (_commandTimeout <= 0)
                _commandTimeout = _appSettings.DefaultCommandTimeout;

            //The default is always 30 seconds
            if (_commandTimeout < 30)
                _commandTimeout = 30;
        }
        #region ExecuteStoredProcedure

        private List<List<dynamic>> ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters)
        {
            List<List<object>> dynamicResult = new List<List<object>>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = this.GetCommandTimeout(_commandTimeout);  //In Seconds

                    SqlParameter param = PrepareParameters(parameters);
                    cmd.Parameters.Add(param);
                    con.Open();
                    IDictionary<string, object> dynamicRow = null;
                    List<object> dynamicTable = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            do
                            {
                                dynamicTable = new List<object>();
                                int count = reader.FieldCount;
                                while (reader.Read())
                                {
                                    dynamicRow = new ExpandoObject();
                                    for (int i = 0; i < count; i++)
                                    {
                                        dynamicRow[reader.GetName(i)] = reader.GetValue(i);
                                    }
                                    dynamicTable.Add(dynamicRow);
                                }
                                dynamicResult.Add(dynamicTable);
                            }
                            while (reader.NextResult());
                        }
                    }//using
                }//using
            }//using

            return dynamicResult;
        }

        private static SqlParameter PrepareParameters(Dictionary<string, object> parameters)
        {
            DataTable tbl = new DataTable("_ObjectParameters");
            tbl.Columns.Add("keyword", typeof(string));
            tbl.Columns.Add("value", typeof(string));
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                tbl.Rows.Add(new object[] { parameter.Key, parameter.Value + string.Empty });
            }
            SqlParameter param = new SqlParameter("_ObjectParameters", tbl)
            {
                TypeName = "dbo.udtt_ObjectParameters",
                SqlDbType = SqlDbType.Structured
            };
            return param;
        }

        /// <summary>
        /// Get CommandTimeout: In Seconds
        /// </summary>
        /// <param name="commandTimeout">In Seconds</param>
        /// <returns></returns>
        private int GetCommandTimeout(int commandTimeout)
        {
            //In Seconds. The default of .Net is 30 seconds.
            if (commandTimeout > 30)
                return commandTimeout;

            return _commandTimeout;
        }
        #endregion

        #region GlobalService
        public List<List<dynamic>> GlobalService(object parameters)
        {
            try
            {
                Dictionary<string, object> dictParameters = JsonSerializer.Deserialize<Dictionary<string, object>>(parameters.ToString());
                CheckParameters(dictParameters);
                dictParameters.TryGetValue("MethodName", out object storedProcedureName);
                dictParameters.Remove("MethodName");
                return ExecuteStoredProcedure(storedProcedureName + string.Empty, dictParameters);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private void CheckParameters(Dictionary<string, object> parameters)
        {
            if ((parameters == null ? true : parameters.Count == 0))
            {
                throw new Exception("The service call hasn't any parameters. Please provide all the nesseary parameters");
            }
            if (!parameters.ContainsKey("MethodName"))
            {
                throw new Exception("Unable to identify the method to be called. Please provide a valid \"MethodName\" parameter.");
            }
        }
        #endregion
    }
}
