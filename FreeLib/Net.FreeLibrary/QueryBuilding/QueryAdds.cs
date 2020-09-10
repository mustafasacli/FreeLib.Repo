using Net.FreeLibrary.Core;
using System;

namespace Net.FreeLibrary.QueryBuilding
{

    /// <summary>
    /// Represents Parameter Prefix, Column-Table Prefix And Suffix and last IdentityInsert Part.
    /// </summary>
    class QueryAdds : IQueryAdds
    {
        private string _parameterPrefix = "";
        private string _prefix = "";
        private string _suffix = "";
        private string _identityInsertAdd = "";

        /// <summary>
        /// Creates Query Object for parameter prefix, column prefix-suffix and identity insert parts.
        /// </summary>
        /// <param name="conn_type"></param>
        public QueryAdds(ConnectionTypes conn_type)
        {
            _parameterPrefix = GetParameterPrefix(conn_type);
            _prefix = GetPrefix(conn_type);
            _suffix = GetSuffix(conn_type);
            _identityInsertAdd = GetIdentityInsert(conn_type);
        }

        /// <summary>
        /// Gets Parameter Name Prefix.
        /// </summary>
        public string ParameterPrefix
        {
            get { return _parameterPrefix; }
        }

        /// <summary>
        /// Gets Table and Column Name Prefix.
        /// </summary>
        public string Prefix
        {
            get { return _prefix; }
        }

        /// <summary>
        /// Gets Table and Column Name Suffix.
        /// </summary>
        public string Suffix
        {
            get { return _suffix; }
        }

        /// <summary>
        /// Gets Identity Insert Part of Query.
        /// </summary>
        public string IdentityInsertAdd
        {
            get { return _identityInsertAdd; }
        }

        #region [ Get Parameter Prefix ]

        /// <summary>
        /// Returns Prefix for parameters according to Connection Type.
        /// </summary>
        /// <returns> Returns Prefix for parameters according to Connection Type.</returns>
        private string GetParameterPrefix(ConnectionTypes conn_type)
        {
            string _s = "";

            switch (conn_type)
            {
                case ConnectionTypes.DB2:
                case ConnectionTypes.OleDb:
                case ConnectionTypes.SqlExpress:
                case ConnectionTypes.SqlServer:
                case ConnectionTypes.VistaDB:
                case ConnectionTypes.SqlServerCe:
                case ConnectionTypes.MySQL:
                case ConnectionTypes.MariaDB:
                case ConnectionTypes.SQLite:
                case ConnectionTypes.PostgreSQL:
                    _s = "@";
                    break;

                case ConnectionTypes.OracleNet:
                case ConnectionTypes.OracleManaged:
                case ConnectionTypes.FireBird:
                case ConnectionTypes.SqlBase:
                    _s = ":";
                    break;

                case ConnectionTypes.Odbc:
                case ConnectionTypes.External:
                    _s = "?";
                    break;

                default:
                case ConnectionTypes.Informix:
                case ConnectionTypes.Ingres:
                case ConnectionTypes.Sybase:
                case ConnectionTypes.Synergy:
                case ConnectionTypes.U2:
                    break;
                //return String.Empty;
            }

            return _s;
        }
        #endregion


        #region [ Get Prefix ]

        /// <summary>
        /// Returns Prefix for columns and tables according to Connection Type.
        /// </summary>
        /// <returns> Returns Prefix for columns and tables according to Connection Type.</returns>
        private string GetPrefix(ConnectionTypes conn_type)
        {
            string _prfx = "";
            switch (conn_type)
            {
                case ConnectionTypes.OleDb:
                case ConnectionTypes.SqlExpress:
                case ConnectionTypes.SqlServer:
                case ConnectionTypes.VistaDB:
                case ConnectionTypes.SqlServerCe:
                    _prfx = "[";
                    break;

                case ConnectionTypes.PostgreSQL:
                    _prfx = "\"";
                    break;

                default:
                    break;
            }
            return _prfx;
        }

        #endregion


        #region [ Get Suffix ]

        /// <summary>
        /// Returns Suffix for columns and tables according to Connection Type.
        /// </summary>
        /// <returns>Returns Suffix for columns and tables according to Connection Type.</returns>
        private string GetSuffix(ConnectionTypes conn_type)
        {
            string _sfx = "";

            switch (conn_type)
            {
                case ConnectionTypes.OleDb:
                case ConnectionTypes.SqlExpress:
                case ConnectionTypes.SqlServer:
                case ConnectionTypes.VistaDB:
                case ConnectionTypes.SqlServerCe:
                    _sfx = "]";
                    break;

                case ConnectionTypes.PostgreSQL:
                    _sfx = "\"";
                    break;

                default:
                    break;
            }

            return _sfx;
        }

        #endregion


        #region [ Get Identity Insert ]

        /// <summary>
        /// Returns GetIdentity query part of InsertAndGetId.
        /// </summary>
        /// <returns> Returns GetIdentity query part of InsertAndGetId. </returns>
        private string GetIdentityInsert(ConnectionTypes conn_type)
        {
            string _s = "";
            switch (conn_type)
            {
                case ConnectionTypes.OleDb:
                case ConnectionTypes.SqlExpress:
                case ConnectionTypes.SqlServer:
                case ConnectionTypes.VistaDB:
                case ConnectionTypes.SqlServerCe:
                    _s = "SELECT SCOPE_IDENTITY();";//String.Format("SELECT IDENT_CURRENT('{0}');", _baseBO.GetTable());
                    break;

                case ConnectionTypes.OracleNet:
                case ConnectionTypes.OracleManaged:
                    _s = String.Format(" Returning {0}#ID_COL#{1} As IdCol", _prefix, _suffix);
                    break;

                case ConnectionTypes.SQLite:
                    _s = "select last_insert_rowid();";
                    break;

                case ConnectionTypes.MySQL:
                case ConnectionTypes.MariaDB:
                    _s = "SELECT LAST_INSERT_ID();";
                    break;

                case ConnectionTypes.PostgreSQL:
                    _s = "SELECT LASTVAL();";
                    break;

                default:
                    break;
            }
            return _s;
        }

        #endregion

    }
}