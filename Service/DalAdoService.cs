using System.Collections.Generic;

namespace IanusGlobalServiceApi.Service
{
    /// <summary>
    /// IDal Ado Service
    /// </summary>
    public interface IDalAdoService
    {
        ///// <summary>
        ///// Execute Stored Procedure
        ///// </summary>
        ///// <returns></returns>
        //List<List<dynamic>> ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object> parameters, int commandTimeout = 30);

        /// <summary>
        /// GlobalService
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<List<dynamic>> GlobalService(object parameters);
    }
}
