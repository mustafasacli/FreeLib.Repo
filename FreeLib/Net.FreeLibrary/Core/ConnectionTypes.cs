using System;

namespace Net.FreeLibrary.Core
{

    #region [ ConnectionTypes enum ]

    public enum ConnectionTypes : byte
    {
        SqlExpress = 0,
        SqlServer = 1,
        PostgreSQL = 2,
        DB2 = 3,
        OracleNet = 4,
        MySQL = 5,
        MariaDB = 6,
        VistaDB = 7,
        OleDb = 8,
        SQLite = 9,
        FireBird = 10,
        SqlServerCe = 11,
        Sybase = 12,
        Informix = 13,
        U2 = 14,
        Synergy = 15,
        Ingres = 16,

        /// <summary>
        /// Version must be upper than 11.7.
        /// </summary>
        SqlBase = 17,

        Odbc = 18,
        OracleManaged = 19,
        External = 20,
        Unknown = 21
    };

    #endregion [ ConnectionTypes enum ]


    #region [ ConnectionTypeBuilder class ]

    internal static class ConnectionTypeBuilder
    {
        public static ConnectionTypes GetConnectionType(string connTypeName)
        {
            ConnectionTypes conn_type = ConnectionTypes.Unknown;
            try
            {
                string drvr = connTypeName ?? string.Empty;
                drvr = drvr.TrimEnd().TrimStart().ToLower();
                drvr = drvr.Replace('ı', 'i');
                drvr = drvr.Replace("-", "");

                if (drvr.Equals("sqlserver") || drvr.Equals("mssql") || drvr.Equals("sql"))
                {
                    conn_type = ConnectionTypes.SqlServer;
                    return conn_type;
                }

                if (drvr.Equals("sqlexpress"))
                {
                    conn_type = ConnectionTypes.SqlExpress;
                    return conn_type;
                }

                if (drvr.Equals("postgresql") || drvr.Equals("pgsql"))
                {
                    conn_type = ConnectionTypes.PostgreSQL;
                    return conn_type;
                }

                if (drvr.Equals("db2"))
                {
                    conn_type = ConnectionTypes.DB2;
                    return conn_type;
                }

                if (drvr.Equals("oracle") || drvr.Equals("oraclenet"))
                {
                    conn_type = ConnectionTypes.OracleNet;
                    return conn_type;
                }

                if (drvr.Equals("oraclemanaged") || drvr.Equals("oraclemngd"))
                {
                    conn_type = ConnectionTypes.OracleManaged;
                    return conn_type;
                }

                if (drvr.Equals("mysql"))
                {
                    conn_type = ConnectionTypes.MySQL;
                    return conn_type;
                }

                if (drvr.Equals("mariadb"))
                {
                    conn_type = ConnectionTypes.MariaDB;
                    return conn_type;
                }

                if (drvr.Equals("vistadb"))
                {
                    conn_type = ConnectionTypes.VistaDB;
                    return conn_type;
                }

                if (drvr.Equals("oledb"))
                {
                    conn_type = ConnectionTypes.OleDb;
                    return conn_type;
                }

                if (drvr.Equals("sqlite"))
                {
                    conn_type = ConnectionTypes.SQLite;
                    return conn_type;
                }

                if (drvr.Equals("firebird") || drvr.Equals("fbird"))
                {
                    conn_type = ConnectionTypes.FireBird;
                    return conn_type;
                }

                if (drvr.Equals("sqlserverce") || drvr.Equals("sqlce"))
                {
                    conn_type = ConnectionTypes.SqlServerCe;
                    return conn_type;
                }

                if (drvr.Equals("sybase"))
                {
                    conn_type = ConnectionTypes.Sybase;
                    return conn_type;
                }

                if (drvr.Equals("informix"))
                {
                    conn_type = ConnectionTypes.Informix;
                    return conn_type;
                }

                if (drvr.Equals("u2"))
                {
                    conn_type = ConnectionTypes.U2;
                    return conn_type;
                }

                if (drvr.Equals("synergy"))
                {
                    conn_type = ConnectionTypes.Synergy;
                    return conn_type;
                }

                if (drvr.Equals("ingres"))
                {
                    conn_type = ConnectionTypes.Ingres;
                    return conn_type;
                }

                if (drvr.Equals("sqlbase"))
                {
                    conn_type = ConnectionTypes.SqlBase;
                    return conn_type;
                }

                if (drvr.Equals("odbc"))
                {
                    conn_type = ConnectionTypes.Odbc;
                    return conn_type;
                }

                if (drvr.Equals("external") || drvr.Equals("ext"))
                {
                    conn_type = ConnectionTypes.External;
                    return conn_type;
                }
            }
            catch (Exception)
            {
                return ConnectionTypes.Unknown;
            }

            return conn_type;
        }
    }

    #endregion [ ConnectionTypeBuilder class ]


}