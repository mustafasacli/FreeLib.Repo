using System.Collections.Generic;
namespace Net.FreeLibrary.Entity
{
    public interface IBaseBO
    {
        /// <summary>
        /// Returns Table Name Of IBaseBO object.
        /// </summary>
        /// <returns>Returns Table Name Of IBaseBO object.</returns>
        string GetTableName();

        /// <summary>
        ///  Returns Identity Name Of IBaseBO object.
        /// </summary>
        /// <returns>Returns Identity Column Name Of IBaseBO object.</returns>
        string GetIdColumn();

        /// <summary>
        /// Clears all change columns list.
        /// </summary>
        void Clear();


        /// <summary>
        /// Gets Count of Changed Property.
        /// </summary>
        int ChangeSetCount { get; }

        /// <summary>
        /// Returns true if Value of Property with given name changes, return true; else returns false.
        /// </summary>
        /// <param name="propName">Property Name</param>
        /// <returns>Returns bool object.</returns>
        bool IsPropertyChanged(string propName);

        List<string> GetColumnChangeList();

        /*
        int Insert();


        int InsertAndGetId();


        int Update();
        

        int Delete();
        */
    }
}
