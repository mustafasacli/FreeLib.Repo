using Net.FreeLibrary.Core;
using System;
using System.Data;
using System.Data.Common;

namespace Net.FreeLibrary.Client
{
    internal sealed class Connection : IConnection
    {

        #region [ Private Fields ]

        private ConnectionTypes connection_type = ConnectionTypes.Unknown;
        private string connection_string = string.Empty;
        private DbProviderFactory factory = null;

        private DbConnection db_conn = null;
        private DbTransaction db_transaction = null;

        private Exception closing_error = null;

        #endregion [ Private Fields ]


        #region [ Connection Ctors ]

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="connectionType">Connection Type</param>
        /// <param name="connectionString">Connection String</param>
        public Connection(ConnectionTypes connectionType, string connectionString)
        {
            try
            {
                if (connectionType == ConnectionTypes.External || connectionType == ConnectionTypes.Unknown)
                {
                    throw new Exception("External or Unknown Connection can not be allowed.");
                }

                connection_string = connectionString;

                string factory_invariant_name = this.GetProviderName(connectionType);

                factory = DbProviderFactories.GetFactory(factory_invariant_name);
                db_conn = factory.CreateConnection();

                db_conn.ConnectionString = connection_string;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="connectionType">Connection Type</param>
        public Connection(ConnectionTypes connectionType)
            : this(connectionType, string.Empty)
        {
        }

        #endregion [ Connection Ctors ]


        #region [ GetProviderName method ]

        private string GetProviderName(ConnectionTypes connType)
        {
            string providerName = string.Empty;

            try
            {
                switch (connType)
                {
                    case ConnectionTypes.SqlExpress:
                    case ConnectionTypes.SqlServer:
                        providerName = "System.Data.SqlClient";
                        break;

                    case ConnectionTypes.PostgreSQL:
                        providerName = "Npgsql";
                        break;

                    case ConnectionTypes.DB2:
                        providerName = "IBM.Data.DB2";
                        break;

                    case ConnectionTypes.OracleNet:
                        providerName = "System.Data.OracleClient";
                        break;

                    case ConnectionTypes.MySQL:
                    case ConnectionTypes.MariaDB:
                        providerName = "MySql.Data.MySqlClient";
                        break;

                    case ConnectionTypes.VistaDB:
                        providerName = "System.Data.VistaDB5";//providerName = "VistaDB.Provider";
                        break;

                    case ConnectionTypes.OleDb:
                        providerName = "System.Data.OleDb";
                        break;

                    case ConnectionTypes.SQLite:
                        providerName = "System.Data.SQLite";
                        break;

                    case ConnectionTypes.FireBird:
                        providerName = "FirebirdSql.Data.FirebirdClient";
                        break;

                    case ConnectionTypes.SqlServerCe:
                        providerName = "System.Data.SqlServerCe";
                        break;

                    case ConnectionTypes.Sybase:
                        providerName = "Sybase.Data.AseClient";
                        break;

                    case ConnectionTypes.Informix:
                        providerName = "IBM.Data.Informix";
                        break;

                    case ConnectionTypes.U2:
                        providerName = "U2.Data.Client";
                        break;

                    case ConnectionTypes.Synergy:
                        providerName = "Synergex.Data.SynergyDBMSClient";
                        break;

                    case ConnectionTypes.Ingres:
                        providerName = "Ingres.Client";
                        break;

                    case ConnectionTypes.SqlBase:
                        providerName = "Gupta.SQLBase.Data";//"Unify.SQLBase.Data";
                        break;

                    case ConnectionTypes.Odbc:
                        providerName = "System.Data.Odbc";
                        break;

                    case ConnectionTypes.OracleManaged:
                        providerName = "Oracle.ManagedDataAccess.Client";
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return providerName;
        }

        #endregion [ GetProviderName method ]


        #region [ ConnectionType Property ]

        /// <summary>
        /// Gets Type Of Connection.
        /// </summary>
        public ConnectionTypes ConnectionType
        {
            get
            {
                return connection_type;
            }
        }

        #endregion [ ConnectionType Property ]


        #region [ ConnectionString Property ]

        /// <summary>
        /// Gets, Sets Connection String.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connection_string;
            }
            set
            {
                connection_string = value;
                db_conn.ConnectionString = connection_string;
            }
        }

        #endregion [ ConnectionString Property ]
        

        #region [ Dispose method ]

        /// <summary>
        /// Dispose the connection.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (db_transaction != null)
                    db_transaction.Dispose();
            }
            catch (Exception) { }

            try
            {
                this.Close();
            }
            catch (Exception) { }

            try
            {
                db_conn.Dispose();
            }
            catch (Exception) { }

            try
            {
                db_transaction = null;
                db_conn = null;
                factory = null;
            }
            catch (Exception) { }
        }

        #endregion [ Dispose method ]


        #region [ GetResultSet method ]

        /// <summary>
        /// Gets Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as DataSet.</returns>
        public DataSet GetResultSet(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            DataSet ds = null;

            try
            {
                using (DbCommand cmd = db_conn.CreateCommand())
                {
                    cmd.CommandText = queryOrProcedure;
                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        string[] keys = parameters.Keys();

                        DbParameter prm;
                        foreach (var key in keys)
                        {
                            prm = cmd.CreateParameter();
                            prm.ParameterName = key;
                            prm.Value = parameters.Get(key);
                            cmd.Parameters.Add(prm);
                        }
                    }

                    using (DbDataAdapter adapter = factory.CreateDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        ds = new DataSet();
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ds;
        }

        #endregion [ GetResultSet method ]


        #region [ Execute method ]

        /// <summary>
        /// Gets Execution Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as int.</returns>
        public int Execute(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            int result = -100;

            try
            {
                using (DbCommand cmd = db_conn.CreateCommand())
                {
                    cmd.CommandText = queryOrProcedure;
                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        string[] keys = parameters.Keys();

                        DbParameter prm;
                        foreach (var key in keys)
                        {
                            prm = cmd.CreateParameter();
                            prm.ParameterName = key;
                            prm.Value = parameters.Get(key);
                            cmd.Parameters.Add(prm);
                        }
                    }

                    if (db_transaction != null)
                    {
                        cmd.Transaction = db_transaction;
                    }

                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion [ Execute method ]


        #region [ ExecuteExecuteScalar method ]

        /// <summary>
        /// Gets Scalar Execution Result for given parameters.
        /// </summary>
        /// <param name="queryOrProcedure">Query Text or Stored Procedure name</param>
        /// <param name="cmdType">Command Type; StoredProcedure, Table, Text</param>
        /// <param name="parameters">Hashmap parameter that includes parameters</param>
        /// <returns>Result returns as object.</returns>
        public object ExecuteScalar(string queryOrProcedure, CommandType cmdType, Hashmap parameters)
        {
            object result = null;

            try
            {
                using (DbCommand cmd = db_conn.CreateCommand())
                {
                    cmd.CommandText = queryOrProcedure;
                    cmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        string[] keys = parameters.Keys();

                        DbParameter prm;
                        foreach (var key in keys)
                        {
                            prm = cmd.CreateParameter();
                            prm.ParameterName = key;
                            prm.Value = parameters.Get(key);
                            cmd.Parameters.Add(prm);
                        }
                    }

                    if (db_transaction != null)
                    {
                        cmd.Transaction = db_transaction;
                    }

                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #endregion [ ExecuteExecuteScalar method ]


        #region [ Open method ]

        /// <summary>
        /// Opens the IConnection.
        /// </summary>
        public void Open()
        {
            try
            {
                if (db_conn != null)
                {
                    if (db_conn.State != ConnectionState.Open)
                    {
                        db_conn.Open();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                closing_error = null;
            }
        }

        #endregion [ Open method ]


        #region [ Close method ]

        /// <summary>
        /// Closes the IConnection.
        /// Return clauses;
        /// - NullConnection returns -1,
        /// - Connection is already closed returns 0,
        /// - After closing Connection returns 1,
        /// - Any Error has occured returns -2
        /// </summary>
        /// <returns>Result returns as int.</returns>
        public int Close()
        {
            int result = -1;
            try
            {
                if (db_conn != null)
                {
                    result = 0;
                    if (db_conn.State != ConnectionState.Closed)
                    {
                        db_conn.Close();
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                closing_error = ex;
                result = -2;
            }
            return result;
        }

        #endregion [ Close method ]


        #region [ GetCloseError method ]

        /// <summary>
        /// Gets Closing Error.
        /// </summary>
        /// <returns>Result return as Exception object.</returns>
        public Exception GetCloseError()
        {
            return closing_error;
        }

        #endregion

        
        #region [ GetState method ]

        /// <summary>
        /// Gets State of the IConnection.
        /// </summary>
        /// <returns>Result returns as ConnectionState.</returns>
        public ConnectionState GetState()
        {
            ConnectionState state_ = ConnectionState.Broken;

            try
            {
                if (db_conn != null)
                {
                    state_ = db_conn.State;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return state_;
        }

        #endregion [ GetState method ]


        #region [ CommitTransaction method ]

        /// <summary>
        /// Commits the Transaction.
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (db_transaction != null)
                {
                    db_transaction.Commit();
                    db_transaction.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db_transaction = null;
            }
        }

        #endregion [ CommitTransaction method ]


        #region [ RollbackTransaction method ]

        /// <summary>
        /// Rollbacks the Transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (db_transaction != null)
                {
                    db_transaction.Rollback();
                    db_transaction.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db_transaction = null;
            }
        }

        #endregion [ RollbackTransaction method ]


        #region [ CreateTransaction method ]

        /// <summary>
        /// Creates the Transaction.
        /// </summary>
        public void CreateTransaction()
        {
            try
            {
                if (db_conn != null)
                {
                    if (db_conn.State != ConnectionState.Open)
                    {
                        this.Open();
                    }

                    if (db_transaction == null)
                    {
                        db_transaction = db_conn.BeginTransaction();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion [ CreateTransaction method ]

    }
}