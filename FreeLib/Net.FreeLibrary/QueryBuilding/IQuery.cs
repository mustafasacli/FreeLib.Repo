using Net.FreeLibrary.Core;

namespace Net.FreeLibrary.QueryBuilding
{
    public interface IQuery
    {
        /// <summary>
        /// Gets Hashmap contains parameter(s).
        /// </summary>
        Hashmap Parameters { get; set; }

        /// <summary>
        /// Gets Sql Query.
        /// </summary>
        string Text { get; set; }
    }
}
