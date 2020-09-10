namespace Net.FreeLibrary.QueryBuilding
{
    public enum QueryTypes : ushort
    {
        Select,
        Insert,
        Update,
        Delete,
        InsertAndGetId,
        SelectWhereId,
        SelectWhereChangeColumns,
        SelectChangeColumns,
        InsertAnyChange
    };
}