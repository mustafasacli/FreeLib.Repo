using Net.FreeLibrary.Client;
using Net.FreeLibrary.Conf;
using Net.FreeLibrary.Core;
using Net.FreeLibrary.Entity;
using Net.FreeLibrary.Extensions;
using Net.FreeLibrary.QueryBuilding;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Net.FreeLibrary.Logic
{

    public abstract class BaseDL : IBaseDL
    {

        #region [ private fields ]

        private IConnection Conn = null;
        private string conn_str;
        private DbProperty db_property;
        private ConnectionTypes conn_type;
        private String conn_name = "main";

        #endregion


        #region [ BaseDL ctors ]

        /// <summary>
        /// protected ctor of BaseDL.
        /// </summary>
        protected BaseDL()
            : this("main")
        {
        }

        /// <summary>
        /// protected ctor of BaseDL.
        /// </summary>
        /// <param name="connectionName">connection property name</param>
        protected BaseDL(string connectionName)
        {
            try
            {
                conn_name = connectionName;
                db_property = FreeConfiguration.BuildProperty(conn_name);

                this.Conn = ConnectionFactory.Build(db_property);

                conn_type = this.Conn.ConnectionType;
                //ConnectionTypeBuilder.GetConnectionType(db_property.ConnType);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// protected ctor of BaseDL.
        /// </summary>
        /// <param name="connectionType">Connection Type.</param>
        /// <param name="connectionString">Connection String.</param>
        protected BaseDL(ConnectionTypes connectionType, string connectionString)
        {
            try
            {
                conn_name = "from_driver";
                this.Conn = ConnectionFactory.Build(connectionType, connectionString);
                conn_type = this.Conn.ConnectionType;
                conn_str = this.Conn.ConnectionString;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ ConnectionName Hashmap ]

        /// <summary>
        /// Gets ConnectionName.
        /// </summary>
        public virtual String ConnectionName
        {
            get { return conn_name; }
        }

        #endregion


        #region [ Insert method ]

        /// <summary>
        /// Gets exceution result of Insert Operation.
        /// </summary>
        /// <param name="bo">BaseBO object.</param>
        /// <returns>returns int</returns>
        public virtual int Insert(IBaseBO bo)
        {
            int result = -100;

            try
            {
                IQuery q = QueryBuilder.BuildInsert(bo, conn_type);

                result = this.Conn.Execute(q.Text, CommandType.Text, q.Parameters);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ Insert And Get Id method ]

        /// <summary>
        /// Inserts and Gets Id of BaseBO object.
        /// </summary>
        /// <param name="bo">BaseBO object.</param>
        /// <returns>returns int</returns>
        public virtual int InsertAndGetId(IBaseBO bo)
        {
            int result = -100;

            try
            {
                IQuery q = QueryBuilder.BuildInsertAndGetId(bo, conn_type);

                result = this.Execute(q.Text, CommandType.Text, q.Parameters);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ Update method ]

        /// <summary>
        /// Gets exceution result of Update Operation.
        /// </summary>
        /// <param name="bo">BaseBO object.</param>
        /// <returns>returns int</returns>
        public virtual int Update(IBaseBO bo)
        {
            int result = -100;

            try
            {
                IQuery q = QueryBuilder.BuildUpdate(bo, conn_type);

                result = this.Execute(q.Text, CommandType.Text, q.Parameters);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ Delete method ]

        /// <summary>
        /// Gets exceution result of Delete Operation.
        /// </summary>
        /// <param name="bo">BaseBO object.</param>
        /// <returns>returns int</returns>
        public virtual int Delete(IBaseBO bo)
        {
            int result = -100;

            try
            {
                IQuery q = QueryBuilder.BuildDelete(bo, conn_type);

                result = this.Execute(q.Text, CommandType.Text, q.Parameters);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        /* ------------------------------------------- */

        #region [ Insert method ]

        /// <summary>
        /// Insert the record to table with table_name with given fields.
        /// </summary>
        /// <param name="table_name">table name</param>
        /// <param name="fields">column and values</param>
        /// <returns>Returns exec result of Insert.</returns>
        public virtual int Insert(string table_name, Hashmap fields)
        {
            int result = -100;
            try
            {
                if (string.IsNullOrWhiteSpace(table_name))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (table_name.Contains("drop") || table_name.Contains("--"))
                {
                    throw new Exception(
                            "Table Name can not be contain restricted characters and text.");
                }

                if (fields == null)
                {
                    throw new Exception(
                            "Column list can not be null.");
                }

                if (fields.IsEmpty())
                {
                    throw new Exception(
                            "Column list can not be empty.");
                }

                QueryFormat qf = new QueryFormat(QueryTypes.Insert);
                QueryAdds adds = new QueryAdds(this.conn_type);

                string query = "", cols = "", vals = "";

                Hashmap h = new Hashmap();

                foreach (var field in fields.Keys())
                {
                    cols = string.Format("{0}, {1}{2}{3}", cols, adds.Prefix, field, adds.Suffix);

                    vals = string.Format("{0}, {1}{2}", cols, adds.ParameterPrefix, field);

                    h.Set(string.Format("{0}{1}", adds.ParameterPrefix, field), fields.Get(field));
                }

                cols = cols.TrimStart(',').TrimStart();
                vals = vals.TrimStart(',').TrimStart();
                query = string.Format(qf.Format, table_name, cols, vals);

                result = this.Execute(query, CommandType.Text, h);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ Update method ]

        /// <summary>
        /// Update records with given parameters.
        /// </summary>
        /// <param name="table_name">table name</param>
        /// <param name="where_column">id column name, if null or empty value will be "id"</param>
        /// <param name="where_value">id column value</param>
        /// <param name="fields">column and values</param>
        /// <returns>Returns exec result of Update.</returns>
        public virtual int Update(string table_name, string where_column, object where_value, Hashmap fields)
        {
            int result = -1;

            try
            {
                if (string.IsNullOrWhiteSpace(table_name))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (table_name.Contains("drop") || table_name.Contains("--"))
                {
                    throw new Exception(
                            "Table Name can not be contain restricted characters and text.");
                }

                if (fields == null)
                {
                    throw new Exception(
                            "Column list can not be null.");
                }

                if (fields.IsEmpty())
                {
                    throw new Exception(
                            "Column list can not be empty.");
                }


                if (string.IsNullOrWhiteSpace(where_column))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (where_column.Contains("drop") || where_column.Contains("--"))
                {
                    throw new Exception(
                            "Table Name can not be contain restricted characters and text.");
                }


                QueryFormat qf = new QueryFormat(QueryTypes.Update);
                QueryAdds qo = new QueryAdds(this.conn_type);

                string query = "", cols = "", vals = "";
                //query = qf.Format;
                Hashmap h = new Hashmap();
                FreeParameter fp;
                foreach (var field in fields.Keys())
                {
                    cols = string.Format("{0}, {1}{2}{3}={4}{2}", cols, qo.Prefix, field, qo.Suffix, qo.ParameterPrefix);
                    fp = new FreeParameter
                    {
                        Name = string.Format("{0}{1}", qo.ParameterPrefix, field),
                        Value = fields.Get(field)
                    };
                    h.Set(string.Format("{0}{1}", qo.ParameterPrefix, field), fields.Get(field));
                }

                h.Set(string.Format("{0}{1}", qo.ParameterPrefix, where_column), where_value);

                cols = cols.TrimStart(',').TrimStart();
                vals = string.Format("{0}{1}{2}={3}{1}", qo.Prefix, where_column, qo.Suffix, qo.ParameterPrefix);
                query = string.Format(qf.Format, table_name, cols, vals);

                result = this.ExecuteQuery(query, h);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ Delete method ]

        public virtual int Delete(string table_name, string where_column, object where_value)
        {
            int result = -1;

            try
            {
                if (string.IsNullOrWhiteSpace(table_name))
                {
                    throw new Exception("Table Name can not be null or empty.");
                }

                if (table_name.Contains("drop") || table_name.Contains("--"))
                {
                    throw new Exception(
                            "Table Name can not be contain restricted characters and text.");
                }


                if (string.IsNullOrWhiteSpace(where_column))
                {
                    throw new Exception("Where Column Name can not be null or empty.");
                }

                if (where_column.Contains("drop") || where_column.Contains("--"))
                {
                    throw new Exception(
                            "Table Name can not be contain restricted characters and text.");
                }

                QueryFormat qf = new QueryFormat(QueryTypes.Delete);
                QueryAdds qo = new QueryAdds(this.conn_type);

                string query = "", vals = "";
                vals = string.Format("{0}{1}{2}={3}{1}", qo.Prefix, where_column, qo.Suffix, qo.ParameterPrefix);
                query = string.Format(qf.Format, table_name, vals);

                Hashmap p = new Hashmap();
                p.Set(string.Format("{0}{1}", qo.ParameterPrefix, where_column), where_value);

                result = this.ExecuteQuery(query, p);

            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ GetResultSet method ]

        protected DataSet GetResultSet(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            DataSet ds = null;

            try
            {
                ds = this.Conn.GetResultSet(queryOrProcedure, cmdType, parameters);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }

            return ds;
        }

        #endregion


        #region [ Execute method ]

        protected int Execute(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            int result = -100;

            try
            {
                this.Conn.Open();
                this.Conn.CreateTransaction();

                result = this.Conn.Execute(queryOrProcedure, cmdType, parameters);

                this.Conn.CommitTransaction();
            }
            catch (Exception)
            {
                this.Conn.RollbackTransaction();
                throw;
            }
            finally
            {
                this.Conn.Close();
            }

            return result;
        }

        #endregion


        #region [ ExecuteScalar method ]

        protected object ExecuteScalar(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            object result = null;

            try
            {
                this.Conn.Open();
                this.Conn.CreateTransaction();

                result = this.Conn.ExecuteScalar(queryOrProcedure, cmdType, parameters);

                this.Conn.CommitTransaction();
            }
            catch (Exception)
            {
                this.Conn.RollbackTransaction();
                throw;
            }
            finally
            {
                this.Conn.Close();
            }

            return result;
        }

        #endregion

        /* ------------------------------------------- */

        #region [ Get Table method ]

        /// <summary>
        /// Gets All Table Records of BaseBO object.
        /// </summary>
        /// <param name="bo">IBaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetTable(IBaseBO bo)
        {
            DataTable dt = null;

            try
            {
                IQuery q = QueryBuilder.Build(bo, conn_type, QueryTypes.Select);

                DataSet ds = this.GetResultSet(q.Text, CommandType.Text, q.Parameters);

                dt = ds.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        #endregion


        #region [ GetById method ]

        /// <summary>
        /// Gets One Record of BaseBO object if any object has BaseBO Id.
        /// </summary>
        /// <param name="bo">IBaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetById(IBaseBO bo)
        {
            DataTable dt = null;

            try
            {
                IQuery q = QueryBuilder.Build(bo, conn_type, QueryTypes.SelectWhereId);

                DataSet ds = this.GetResultSet(q.Text, CommandType.Text, q.Parameters);

                dt = ds.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        #endregion


        #region [ GetObjectListByIds method ]

        public List<T> GetObjectListByIds<T>(List<int> idList) where T : new()
        {
            try
            {
                List<T> t_List = null;

                if (idList == null)
                    return t_List;

                t_List = new List<T>();
                if (idList.Count == 0)
                    return t_List;

                T t = (T)Activator.CreateInstance(typeof(T));

                string IdCol = string.Format("{0}", t.GetType().GetMethod("GetIdColumn").Invoke(t, null));
                if (IdCol.Trim().Length == 0)
                    throw new Exception("Id Column can not be empty.");

                string tableName = string.Format("{0}", t.GetType().GetMethod("GetTableName").Invoke(t, null));

                if (tableName.Replace(" ", "").Length == 0)
                    throw new Exception("TableName can not be empty.");

                string strIn = string.Empty;

                foreach (var item in idList)
                {
                    strIn = string.Format("{0}, {1}", strIn, item);
                }
                strIn = strIn.TrimStart(',').TrimStart();

                DataTable dtBO = this.GetResultSetOfQuery(string.Format("Select * From {0} Where {1} IN ({2})", tableName, IdCol, strIn)).Tables[0];

                t_List = dtBO.ToList<T>(true);

                return t_List;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion


        #region [ GetObjectById method ]

        public T GetObjectById<T>(int Id) where T : new()
        {
            T result = default(T);
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));

                string id_col = string.Format("{0}", t.GetType().GetMethod("GetIdColumn").Invoke(t, null));

                if (string.IsNullOrWhiteSpace(id_col))
                    throw new Exception("Id Column can not be empty.");

                string tableName = string.Format("{0}", t.GetType().GetMethod("GetTableName").Invoke(t, null));
                if (string.IsNullOrWhiteSpace(tableName))
                    throw new Exception("TableName can not be empty.");

                IQueryFormat qformat = new QueryFormat(QueryTypes.SelectWhereId);
                IQueryAdds adds = new QueryAdds(conn_type);
                string q = qformat.Format;

                q = q.Replace("#TABLE_NAME#", tableName);
                q = q.Replace("#VALS#", string.Format("{0}={1}{0}", id_col, adds.ParameterPrefix));

                Hashmap h = new Hashmap();
                h.Set(string.Format("{0}{1}", adds.ParameterPrefix, id_col), Id);

                DataSet ds = this.GetResultSet(q, CommandType.Text, h);
                DataTable dt = ds.Tables[0];

                List<T> listT = dt.ToList<T>(true);

                result = listT[0];
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion


        #region [ GetSpecialColumnsWithBO method ]

        public DataTable GetSpecialColumnsWithBO(string[] columnList, BaseBO bo)
        {
            DataTable dt = null;

            try
            {
                if (columnList.IsNull())
                    throw new Exception("Column List can not be null.");

                if (columnList.Length == 0)
                    throw new Exception("Column List Length can not be Zero.");


                IQuery q = QueryBuilder.Build(bo, conn_type, QueryTypes.SelectWhereChangeColumns);
                DataSet ds = null;
                string query = "";


                int _index = q.Text.FirstIndexOf('*');
                string cols = string.Empty;
                foreach (var column in columnList)
                {
                    cols = string.Format("{0}, {1}", cols, column);
                }
                cols = cols.TrimStart(',').TrimStart();
                query = string.Format("{0} {1} {2}", q.Text.Substring(0, _index), cols, q.Text.Substring(_index + 1, q.Text.Length - _index - 1));

                ds = this.GetResultSet(query, CommandType.Text, q.Parameters);
                dt = ds.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        #endregion


        #region [ GetChangeColumnList method ]

        /// <summary>
        /// Gets Columns which baseBo object changelist contains.
        /// Example: Column Change List Of BaseBO : Col1, Col2, Col3.
        /// Query = Select Col1, Col2, Col3 From Table_Of_IBaseBO;
        /// </summary>
        /// <param name="bo">IBaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetChangeColumnList(IBaseBO bo)
        {
            DataTable dt = null;

            try
            {
                IQuery q = QueryBuilder.Build(bo, conn_type, QueryTypes.SelectChangeColumns);
                DataSet ds = null;

                ds = this.GetResultSet(q.Text, CommandType.Text, q.Parameters);
                dt = ds.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        #endregion


        #region [ Get Where Change Column List nethod ]

        /// <summary>
        /// Gets Columns which baseBo object changelist contains.
        /// Example: Column Change List Of BaseBO : Col1, Col2, Col3.
        /// Query = Select *  From TableOfBaseBO Where Col1=Col1Value And Col2=Col2Value And Col3=Col3Value;
        /// </summary>
        /// <param name="bo">IBaseBO object.</param>
        /// <returns>Returns a System.Data.DataTable</returns>
        public DataTable GetWhereChangeColumnList(IBaseBO bo)
        {
            DataTable dt = null;

            try
            {
                IQuery q = QueryBuilder.Build(bo, conn_type, QueryTypes.SelectWhereChangeColumns);
                DataSet ds = null;

                ds = this.GetResultSet(q.Text, CommandType.Text, q.Parameters);
                dt = ds.Tables[0];

            }
            catch (Exception)
            {
                throw;
            }

            return dt;
        }

        #endregion


        /* ------------------------------------------------- */

        #region [ Get ResultSet of Query ]

        /// <summary>
        /// Returns A DataSet with given Query without any parameter
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns A DataSet with given Query without any parameter</returns>
        public DataSet GetResultSetOfQuery(string query)
        {
            DataSet ds = null;

            try
            {
                ds = this.GetResultSet(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        #endregion


        #region [ Get ResultSet Of Procedure ]

        /// <summary>
        /// Gets ResultSet with given Procedure without any parameter.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns A DataSet with given Procedure without any parameter</returns>
        public DataSet GetResultSetOfProcedure(string procedure)
        {
            DataSet ds = null;

            try
            {
                ds = this.GetResultSet(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        #endregion

        /* ------------------------------------------------- */


        #region [ Get ResultSet of Query with Parameters ]

        /// <summary>
        /// Returns A DataSet with given Query with parameter(s).
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Hashmap which contains parameters</param>
        /// <returns>Returns A DataSet with given Query without any parameter</returns>
        public DataSet GetResultSetOfQuery(string query, Hashmap parameters)
        {
            DataSet ds = null;

            try
            {
                ds = this.GetResultSet(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        #endregion


        #region [ Get ResultSet Of Procedure ]

        /// <summary>
        /// Returns A DataSet with given Procedure without any parameter
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Hashmap parameters</param>
        /// <returns>Returns A DataSet with given Procedure without any parameter</returns>
        public DataSet GetResultSetOfProcedure(string procedure, Hashmap parameters)
        {
            DataSet ds = null;

            try
            {
                ds = this.GetResultSet(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Query ]

        /// <summary>
        /// Returns execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns execution value of query.</returns>
        public int ExecuteQuery(string query)
        {
            int result = -100;

            try
            {
                result = this.Execute(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        #region [ Execute Procedure ]

        /// <summary>
        /// Returns execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns execution value of Procedure.</returns>
        public int ExecuteProcedure(string procedure)
        {
            int result = -100;

            try
            {
                result = this.Execute(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Query with Parameters ]

        /// <summary>
        /// Returns execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Hashmap contains parameters.</param>
        /// <returns>Returns execution value of query with parameters.</returns>
        public int ExecuteQuery(string query, Hashmap parameters)
        {
            int result = -100;

            try
            {
                result = this.Execute(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        #region [ Execute Procedure ]

        /// <summary>
        /// Returns execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Hashmap contains parameters.</param>
        /// <returns>Returns execution value of Procedure with parameters.</returns>
        public int ExecuteProcedure(string procedure, Hashmap parameters)
        {
            int result = -100;

            try
            {
                result = this.Execute(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Scalar Query ]

        /// <summary>
        /// Returns scalar execution value of query.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <returns>Returns scalar execution value of query.</returns>
        public Object ExecuteScalarAsQuery(string query)
        {
            object result = null;

            try
            {
                result = this.ExecuteScalar(query, CommandType.Text, null);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        #region [ Execute Scalar As Procedure ]

        /// <summary>
        /// Returns scalar execution value of Procedure.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <returns>Returns scalar execution value of Procedure.</returns>
        public object ExecuteScalarAsProcedure(string procedure)
        {
            object result = null;

            try
            {
                result = this.ExecuteScalar(procedure, CommandType.StoredProcedure, null);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ Execute Scalar Query with Parameters ]

        /// <summary>
        /// Returns scalar execution value of query with parameters.
        /// </summary>
        /// <param name="query">Sql Query</param>
        /// <param name="parameters">Prperty contains parameters.</param>
        /// <returns>Returns scalar execution value of query with parameters.</returns>
        public object ExecuteScalarAsQuery(string query, Hashmap parameters)
        {
            object result = null;

            try
            {
                result = this.ExecuteScalar(query, CommandType.Text, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        #region [ Execute Scalar Query with Parameters ]

        /// <summary>
        /// Returns scalar execution value of Procedure with parameters.
        /// </summary>
        /// <param name="query">Sql Procedure</param>
        /// <param name="parameters">Hashmap contains parameters.</param>
        /// <returns>Returns scalar execution value of Procedure with parameters.</returns>
        public Object ExecuteScalarAsProcedure(string procedure, Hashmap parameters)
        {
            object result = null;

            try
            {
                result = this.ExecuteScalar(procedure, CommandType.StoredProcedure, parameters);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion

        /* ------------------------------------------------- */

        #region [ GetTableAsList method ]

        public List<T> GetTableAsList<T>() where T : new()
        {
            List<T> lst = null;
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T), null);
                DataTable dt = new DataTable();
                if (t is IBaseBO)
                {
                    IQuery q = QueryBuilder.Build((t as IBaseBO), conn_type, QueryTypes.Select);
                    DataSet ds = new DataSet();

                    ds = this.GetResultSet(q.Text, CommandType.Text, q.Parameters);

                    dt = ds.Tables[0];
                }
                else
                {
                    MethodInfo mthdInf = t.GetType().GetMethod("GetTableName");

                    if (mthdInf == null)
                        throw new NotImplementedException("You should implement GetTableName method.");

                    Object objStr = mthdInf.Invoke(t, null);

                    string tbl_name = objStr == null ? String.Empty : objStr.ToString();

                    if (string.IsNullOrWhiteSpace(tbl_name))
                    {
                        throw new Exception("You should define Table Name.");
                    }

                    dt = this.GetResultSet(String.Format("Select * From {0};", objStr), CommandType.Text, null).Tables[0];
                }

                lst = dt.ToList<T>(true);
            }
            catch (Exception)
            {
                throw;
            }

            return lst;
        }


        #endregion

        #region [ Tables method ]

        public virtual List<T> TableRecords<T>() where T : new()
        {
            List<T> lst = null;
            try
            {
                lst = GetTableAsList<T>();
            }
            catch (Exception)
            {
                throw;
            }
            return lst;
        }

        #endregion

        #region [ Dispose method ]

        /// <summary>
        /// Disposes DbManager object with sub objects.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (Conn != null)
                {
                    Conn.Dispose();
                }
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region [ BaseDL Destructor ]

        ~BaseDL()
        {
            Dispose();
        }

        #endregion

        #region [ ConnectionType Property ]

        /// <summary>
        /// Gets ConnectionType of Base DataLayer.
        /// </summary>
        public ConnectionTypes ConnectionType
        {
            get { return this.conn_type; }
        }

        #endregion

    }
}