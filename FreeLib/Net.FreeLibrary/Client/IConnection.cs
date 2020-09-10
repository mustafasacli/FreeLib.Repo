using Net.FreeLibrary.Core;
using System;
using System.Data;

namespace Net.FreeLibrary.Client
{
    public interface IConnection : IDisposable, IBaseConnectionOperations
    {
        /// <summary>
        /// Opens the IConnection.
        /// </summary>
        void Open();

        /// <summary>
        /// Closes the IConnection.
        /// Return clauses;
        /// - NullConnection returns -1,
        /// - Connection is already closed returns 0,
        /// - After closing Connection returns 1,
        /// - Any Error has occured returns -2
        /// </summary>
        /// <returns>Result returns as int.</returns>
        int Close();

        /// <summary>
        /// Gets Closing Error.
        /// </summary>
        /// <returns>Result return as Exception object.</returns>
        Exception GetCloseError();

        /// <summary>
        /// Gets State of the IConnection.
        /// </summary>
        /// <returns>Result returns as ConnectionState.</returns>
        ConnectionState GetState();

        /// <summary>
        /// Commits the Transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollbacks the Transaction.
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Creates the Transaction.
        /// </summary>
        void CreateTransaction();

        /// <summary>
        /// Gets Type Of IConnection.
        /// </summary>
        ConnectionTypes ConnectionType { get; }

        /// <summary>
        /// Gets, Sets Connection String Of IConnection.
        /// </summary>
        String ConnectionString { get; set; }
    }
}