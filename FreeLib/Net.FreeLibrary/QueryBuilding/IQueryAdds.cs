namespace Net.FreeLibrary.QueryBuilding
{

    /// <summary>
    /// Represents Parameter Prefix, Column-Table Prefix And Suffix and last IdentityInsert Part.
    /// </summary>
    public interface IQueryAdds
    {

        /// <summary>
        /// Gets Parameter Name Prefix.
        /// </summary>
        string ParameterPrefix { get; }

        /// <summary>
        /// Gets Table and Column Name Prefix.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Gets Table and Column Name Suffix.
        /// </summary>
        string Suffix { get; }

        /// <summary>
        /// Gets Identity Insert Part of Query.
        /// </summary>
        string IdentityInsertAdd { get; }

    }
}
