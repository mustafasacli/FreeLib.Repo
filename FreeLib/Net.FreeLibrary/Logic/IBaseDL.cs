using Net.FreeLibrary.Core;
using Net.FreeLibrary.Entity;
using System;

namespace Net.FreeLibrary.Logic
{
    internal interface IBaseDL : IConnectionOperations, IDisposable
    {
        /// <summary>
        /// Inserts BaseBO object and returns execution value.
        /// </summary>
        /// <param name="baseBO">BaseBO object.</param>
        /// <returns>Returns Execution value of Insert query.</returns>
        int Insert(IBaseBO baseBO);

        /// <summary>
        /// Inserts BaseBO object and returns Identity value.
        /// </summary>
        /// <param name="baseBO">IBaseBO object.</param>
        /// <returns>Returns Identity value</returns>
        int InsertAndGetId(IBaseBO baseBO);

        /// <summary>
        /// Updates BaseBO object and returns execution value of delete query.
        /// </summary>
        /// <param name="baseBO">IBaseBO object.</param>
        /// <returns>returns execution value of delete query.</returns>
        int Delete(IBaseBO baseBO);

        /// <summary>
        /// Updates BaseBO object and returns execution value of update query.
        /// </summary>
        /// <param name="baseBO">BaseBOIBaseBO object.</param>
        /// <returns>returns execution value of update query.</returns>
        int Update(IBaseBO baseBO);

        /// <summary>
        /// Returns QueryBuilder object with with given parameters.
        /// </summary>
        /// <param name="queryType"> Query type for QueryBuilder </param>
        /// <param name="tableObject"> BaseBO object </param>
        /// <returns> QueryBuilder object</returns>
        //QueryBuilder CreateQueryBuilder(QueryTypes queryType, BaseBO tableObject);

        /// <summary>
        /// Returns Driver Type of Data Layer.
        /// </summary>
        ConnectionTypes ConnectionType { get; }

        /// <summary>
        /// Insert the record to table with table_name with given fields.
        /// </summary>
        /// <param name="table_name">table name</param>
        /// <param name="fields">column and values</param>
        /// <returns>Returns exec result of Insert.</returns>
        int Insert(string table_name, Hashmap fields);

        /// <summary>
        /// Update records with given parameters.
        /// </summary>
        /// <param name="table_name">table name</param>
        /// <param name="where_column">id column name, if null or empty value will be "id"</param>
        /// <param name="where_value">id column value</param>
        /// <param name="fields">column and values</param>
        /// <returns>Returns exec result of Update.</returns>
        int Update(string table_name, string where_column, object where_value, Hashmap fields);

        /// <summary>
        /// Deletes records with given parameters.
        /// </summary>
        /// <param name="table_name">table name whose record will be deleted.</param>
        /// <param name="where_column">where column name</param>
        /// <param name="where_value">where col value</param>
        /// <returns>Returns exec result of Delete.</returns>
        int Delete(string table_name, string where_column, object where_value);
    }
}