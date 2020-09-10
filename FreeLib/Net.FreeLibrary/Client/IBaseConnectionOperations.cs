using Net.FreeLibrary.Core;
using System.Data;

namespace Net.FreeLibrary.Client
{
    public interface IBaseConnectionOperations
    {

        /// <summary>
        /// Gets Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as DataSet.</returns>
        DataSet GetResultSet(string queryOrProcedure, CommandType cmdType, Hashmap parameters);


        /// <summary>
        /// Gets Execution Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as int.</returns>
        int Execute(string queryOrProcedure, CommandType cmdType, Hashmap parameters);

        /// <summary>
        /// Gets Scalar Execution Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as object.</returns>
        object ExecuteScalar(string queryOrProcedure, CommandType cmdType, Hashmap parameters);
    }
}