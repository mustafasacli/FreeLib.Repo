using Net.FreeLibrary.Conf;
using Net.FreeLibrary.Core;
using System;

namespace Net.FreeLibrary.Client
{
    public static class ConnectionFactory
    {
        public static IConnection Build(DbProperty db_configuration)
        {
            IConnection conn = null;

            try
            {
                ConnectionTypes conn_type = ConnectionTypeBuilder.GetConnectionType(db_configuration.ConnType);

                if (conn_type == ConnectionTypes.Unknown)
                {
                    throw new Exception("Unknown Connection type can not be allowed.");
                }

                string conn_str = "";
                conn_str = db_configuration.ConnString;
                conn_str = conn_str ?? string.Empty;

                if (string.IsNullOrWhiteSpace(conn_str))
                {
                    Hashmap h = db_configuration.Keys;

                    if (h != null)
                    {
                        string[] keys_ = h.Keys();
                        foreach (var key in keys_)
                        {
                            conn_str = string.Format("{0}{1}={2};", conn_str, key, h.Get(key));
                        }

                        conn_str = conn_str.TrimEnd(';');
                    }
                }

                // IF CLAUSE 1
                if (conn_type == ConnectionTypes.External)
                {
                    conn = new ExternalConnection(db_configuration.InvariantName, conn_str);
                }
                else
                {
                    conn = new Connection(conn_type, conn_str);
                }
            }
            catch (Exception)
            {
                throw;
            }

            // RETURN
            return conn;
        }

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="connectionType">Connection Type</param>
        public static IConnection Build(ConnectionTypes connectionType)
        {
            IConnection conn = null;

            try
            {
                conn = new Connection(connectionType);
            }
            catch (Exception)
            {
                throw;
            }

            return conn;
        }

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="connectionType">Connection Type</param>
        /// <param name="connectionString">Connection String</param>
        public static IConnection Build(ConnectionTypes connectionType, string connectionString)
        {
            IConnection conn = null;

            try
            {
                conn = new Connection(connectionType, connectionString);
            }
            catch (Exception)
            {
                throw;
            }

            return conn;
        }

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="factoryInvariantName">DbFactory Invariant Name</param>
        public static IConnection BuildExternal(string invariantName)
        {
            IConnection conn = null;

            try
            {
                conn = new ExternalConnection(invariantName);
            }
            catch (Exception)
            {
                throw;
            }

            return conn;
        }

        /// <summary>
        /// Creates the IConnection with given parameters.
        /// </summary>
        /// <param name="factoryInvariantName">DbFactory Invariant Name</param>
        /// <param name="connectionString">Connection String</param>
        public static IConnection BuildExternal(string invariantName, string connectionString)
        {
            IConnection conn = null;

            try
            {
                conn = new ExternalConnection(invariantName, connectionString);
            }
            catch (Exception)
            {
                throw;
            }

            return conn;
        }

    }
}