using Net.FreeLibrary.Core;
using System.Data;

namespace Net.FreeLibrary.Logic
{
    internal interface IConnectionOperations
    {
        DataSet GetResultSetOfQuery(string query);

        DataSet GetResultSetOfQuery(string query, Hashmap parameters);

        DataSet GetResultSetOfProcedure(string procedure);

        DataSet GetResultSetOfProcedure(string procedure, Hashmap parameters);

        /* -------------------------------------------------------------- */

        int ExecuteQuery(string query);

        int ExecuteQuery(string query, Hashmap parameters);

        int ExecuteProcedure(string procedure);

        int ExecuteProcedure(string procedure, Hashmap parameters);

        /* -------------------------------------------------------------- */

        object ExecuteScalarAsQuery(string query);

        object ExecuteScalarAsQuery(string query, Hashmap parameters);

        object ExecuteScalarAsProcedure(string procedure);

        object ExecuteScalarAsProcedure(string procedure, Hashmap parameters);
    }
}