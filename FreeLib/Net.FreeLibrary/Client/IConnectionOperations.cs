using Net.FreeLibrary.Core;
using System;
using System.Data;

namespace Net.FreeLibrary.Client
{
    internal interface IConnectionOperations : IBaseConnectionOperations
    {
        DataSet GetResultSetOfQuery(String query);

        DataSet GetResultSetOfQuery(String query, Hashmap parameters);

        DataSet GetResultSetOfProcedure(String procedure);

        DataSet GetResultSetOfProcedure(String procedure, Hashmap parameters);

        /* -------------------------------------------------------------- */

        Int32 ExecuteQuery(String query);

        Int32 ExecuteQuery(String query, Hashmap parameters);

        Int32 ExecuteProcedure(String procedure);

        Int32 ExecuteProcedure(String procedure, Hashmap parameters);

        /* -------------------------------------------------------------- */

        Object ExecuteScalarAsQuery(String query);

        Object ExecuteScalarAsQuery(String query, Hashmap parameters);

        Object ExecuteScalarAsProcedure(String procedure);

        Object ExecuteScalarAsProcedure(String procedure, Hashmap parameters);
    }
}