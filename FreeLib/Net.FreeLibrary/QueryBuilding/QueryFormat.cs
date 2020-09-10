namespace Net.FreeLibrary.QueryBuilding
{

    /// <summary>
    /// Represents format of Query.
    /// </summary>
    internal class QueryFormat : IQueryFormat
    {
        private string _format = "";

        /// <summary>
        /// Gets Format Of Query Type.
        /// </summary>
        public string Format
        {
            get { return _format; }
        }

        /// <summary>
        /// Creates Format Of Given Query Type.
        /// </summary>
        /// <param name="query_type"></param>
        public QueryFormat(QueryTypes query_type)
        {
            _format = get_query_format(query_type);
        }


        #region [ Query Format of QueryType ]

        /// <summary>
        /// Returns Format of Query according to QueryType.
        /// </summary>
        /// <param name="queryType"></param>
        /// <returns></returns>
        private string get_query_format(QueryTypes queryType)
        {
            string f = "";

            switch (queryType)
            {
                case QueryTypes.Select:
                    f = "SELECT * FROM #TABLE_NAME#;";
                    break;

                case QueryTypes.Insert:
                case QueryTypes.InsertAndGetId:
                    f = "INSERT INTO #TABLE_NAME#(#COLUMNS#) VALUES(#VALS#);";
                    f = "INSERT INTO #TABLE_NAME#(#COLUMNS#) VALUES(#VALS#)";
                    break;

                case QueryTypes.Update:
                    f = "UPDATE #TABLE_NAME# SET #COLUMNS# WHERE #VALS#;";
                    break;

                case QueryTypes.Delete:
                    f = "DELETE FROM #TABLE_NAME# WHERE #VALS#;";
                    break;

                case QueryTypes.SelectWhereChangeColumns:
                case QueryTypes.SelectWhereId:
                    f = "SELECT * FROM #TABLE_NAME# WHERE #VALS#;";
                    break;

                case QueryTypes.SelectChangeColumns:
                    f = "SELECT #COLUMNS# FROM #TABLE_NAME#;";
                    break;

                default:
                    break;
            }

            return f;
        }

        #endregion [ Query Format of QueryType ]


    }
}