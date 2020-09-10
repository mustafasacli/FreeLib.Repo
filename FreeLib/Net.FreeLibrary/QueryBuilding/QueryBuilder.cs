using Net.FreeLibrary.Core;
using Net.FreeLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Net.FreeLibrary.QueryBuilding
{
    public sealed class QueryBuilder
    {

        internal static IQuery Build(IBaseBO bo, IQueryFormat qformat, IQueryAdds adds, QueryTypes query_type, ConnectionTypes conn_type)
        {
            IQuery q = null;

            try
            {
                q = new Query();
                string query = string.Empty;
                query = qformat.Format;
                query = query ?? string.Empty;

                string table_name = "";
                string columns = "";
                string vals = "";
                Hashmap h = null;

                table_name = GetTableName(bo, adds);
                columns = GetColumns(bo, adds, query_type, conn_type);
                vals = GetVals(bo, adds, query_type, conn_type);

                h = GetParameters(bo, adds, query_type);

                query = query.Replace("#TABLE_NAME#", table_name);
                query = query.Replace("#COLUMNS#", columns);
                query = query.Replace("#VALS#", vals);

                q.Text = query;
                q.Parameters = h;
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        internal static string GetTableName(IBaseBO bo, IQueryAdds adds)
        {
            string tbl_name = "";
            try
            {
                tbl_name = bo.GetTableName();
                if (string.IsNullOrWhiteSpace(tbl_name))
                {
                    throw new Exception("Table Name can not be empty or white space");
                }

                tbl_name = string.Format("{0}{1}{2}", adds.Prefix, tbl_name, adds.Suffix);

            }
            catch (Exception)
            {
                throw;
            }

            return tbl_name;
        }

        internal static string GetColumns(IBaseBO bo, IQueryAdds adds, QueryTypes query_type, ConnectionTypes conn_type)
        {
            string cols = "";
            try
            {
                List<string> colList = bo.GetColumnChangeList();


                if (query_type == QueryTypes.Insert || query_type == QueryTypes.InsertAndGetId 
                    || query_type == QueryTypes.Update)
                {
                    string id_col = bo.GetIdColumn();
                    colList.Remove(id_col);
                }

                if (query_type == QueryTypes.Insert || query_type == QueryTypes.InsertAndGetId ||
                    query_type == QueryTypes.InsertAnyChange || query_type == QueryTypes.SelectChangeColumns)
                {

                    foreach (var col in colList)
                    {
                        cols = string.Concat(cols, ", ", col);
                    }
                    cols = cols.TrimStart(',').TrimStart();
                    // RETURN
                    return cols;
                }

                if (query_type == QueryTypes.Update)
                {
                    if (conn_type == ConnectionTypes.Odbc || conn_type == ConnectionTypes.External)
                    {
                        foreach (var col in colList)
                        {
                            cols = string.Concat(cols, ", ", col, "=", adds.ParameterPrefix);
                        }
                    }
                    else
                    {
                        foreach (var col in colList)
                        {
                            cols = string.Concat(cols, ", ", col, "=", adds.ParameterPrefix, col);
                        }
                    }

                    cols = cols.TrimStart(',').TrimStart();

                    // RETURN
                    return cols;
                }

            }
            catch (Exception)
            {
                throw;
            }

            return cols;
        }

        internal static string GetVals(IBaseBO bo, IQueryAdds adds, QueryTypes query_type, ConnectionTypes conn_type)
        {
            string vals = "";

            try
            {

                List<string> colList = bo.GetColumnChangeList();
                string id_col = "";

                //IF BLOCK 1
                if (query_type == QueryTypes.Insert || query_type == QueryTypes.InsertAndGetId
                    || query_type == QueryTypes.InsertAnyChange)
                {

                    if (query_type != QueryTypes.InsertAnyChange)
                    {
                        id_col = bo.GetIdColumn();
                        colList.Remove(id_col);
                    }

                    if (conn_type == ConnectionTypes.Odbc || conn_type == ConnectionTypes.External)
                    {
                        foreach (var col in colList)
                        {
                            vals = string.Concat(vals, ", ", adds.ParameterPrefix);
                        }
                    }
                    else
                    {
                        foreach (var col in colList)
                        {
                            vals = string.Concat(vals, ", ", adds.ParameterPrefix, col);
                        }
                    }

                    vals = vals.TrimStart(',').TrimStart();

                    //RETURN
                    return vals;
                }


                //IF BLOCK 2
                if (query_type == QueryTypes.Delete || query_type == QueryTypes.SelectWhereId
                    || query_type == QueryTypes.Update)
                {
                    id_col = bo.GetIdColumn();

                    if (conn_type == ConnectionTypes.Odbc || conn_type == ConnectionTypes.External)
                    {
                        vals = string.Format("{0}={1}", id_col, adds.ParameterPrefix);
                    }
                    else
                    {
                        vals = string.Format("{0}={1}{0}", id_col, adds.ParameterPrefix);
                    }
                    //RETURN
                    return vals;
                }


                //IF BLOCK 3
                if (query_type == QueryTypes.SelectWhereChangeColumns)
                {
                    if (conn_type == ConnectionTypes.Odbc || conn_type == ConnectionTypes.External)
                    {
                        foreach (var col in colList)
                        {
                            vals = string.Concat(vals, "AND ", col, "=", adds.ParameterPrefix);
                        }
                    }
                    else
                    {
                        foreach (var col in colList)
                        {
                            vals = string.Concat(vals, "AND ", col, "=", adds.ParameterPrefix, col);
                        }
                    }

                    if (vals.Length > 4)
                    {
                        vals = vals.Substring(4, vals.Length - 4);
                    }

                    //RETURN
                    return vals;
                }

            }
            catch (Exception)
            {
                throw;
            }

            //RETURN
            return vals;
        }

        internal static Hashmap GetParameters(IBaseBO bo, IQueryAdds adds, QueryTypes query_type)
        {
            Hashmap h = null;

            try
            {
                if (query_type != QueryTypes.Select || query_type != QueryTypes.SelectChangeColumns)
                {
                    h = new Hashmap();

                    List<string> colList = bo.GetColumnChangeList();
                    string id_col;
                    PropertyInfo propInfo;
                    Type t = bo.GetType();
                    id_col = bo.GetIdColumn();
                    object val;

                    // IF BLOCK 1
                    if (query_type == QueryTypes.Delete || query_type == QueryTypes.SelectWhereId)
                    {

                        if (string.IsNullOrWhiteSpace(id_col))
                        {
                            throw new Exception("Id Column can not be empty or whit space.");
                        }

                        propInfo = t.GetProperty(id_col);
                        if (propInfo == null)
                        {
                            throw new Exception("Id Column Property has been specified as wrong. Please define right.");
                        }

                        val = propInfo.GetValue(bo, null);

                        if (val == null)
                        {
                            throw new Exception(string.Format("Id Column({0}) Value must be specified.", id_col));
                        }

                        h.Set(string.Format("{0}{1}", adds.ParameterPrefix, id_col), val);

                        //RETURN
                        return h;
                    }


                    // IF BLOCK 2
                    if (query_type == QueryTypes.Insert || query_type == QueryTypes.InsertAndGetId)
                    {
                        colList.Remove(id_col);
                    }


                    foreach (var col in colList)
                    {
                        propInfo = null;
                        propInfo = t.GetProperty(col);
                        if (propInfo == null)
                        {
                            throw new Exception(string.Format("{0} Column Property has been specified as wrong. Please define right.", col));
                        }

                        val = propInfo.GetValue(bo, null);

                        h.Set(string.Format("{0}{1}", adds.ParameterPrefix, col), val);
                    }

                    //RETURN
                    return h;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //RETURN
            return h;
        }

        public static IQuery Build(IBaseBO bo, ConnectionTypes conn_type, QueryTypes query_type)
        {
            IQuery q = null;

            try
            {
                IQueryFormat qformat = new QueryFormat(query_type);
                IQueryAdds adds = new QueryAdds(conn_type);

                q = Build(bo, qformat, adds, query_type, conn_type);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        public static IQuery BuildInsert(IBaseBO bo, ConnectionTypes conn_type)
        {
            IQuery q = null;
            try
            {
                q = Build(bo, conn_type, QueryTypes.Insert);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        public static IQuery BuildInsertAnyChange(IBaseBO bo, ConnectionTypes conn_type)
        {
            IQuery q = null;
            try
            {
                q = Build(bo, conn_type, QueryTypes.InsertAnyChange);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        public static IQuery BuildInsertAndGetId(IBaseBO bo, ConnectionTypes conn_type)
        {
            IQuery q = null;
            try
            {
                q = Build(bo, conn_type, QueryTypes.InsertAndGetId);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        public static IQuery BuildUpdate(IBaseBO bo, ConnectionTypes conn_type)
        {
            IQuery q = null;
            try
            {
                q = Build(bo, conn_type, QueryTypes.Update);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

        public static IQuery BuildDelete(IBaseBO bo, ConnectionTypes conn_type)
        {
            IQuery q = null;
            try
            {
                q = Build(bo, conn_type, QueryTypes.Delete);
            }
            catch (Exception)
            {
                throw;
            }

            return q;
        }

    }
}
