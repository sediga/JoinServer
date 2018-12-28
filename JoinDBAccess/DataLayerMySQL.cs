#if (ALL || MYSQL)
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement a wrapped of My SQL server
    /// </summary>
    internal class DataLayerMySql : DataLayerBase
    {
        /// <summary>
        /// Initializes a new instance of the DataLayerMySql class.
        /// </summary>
        public DataLayerMySql()
            : base(DatabaseTypes.MySql, new MySqlConnection(), "@"
            , new ConnectionParameterNames(string.Empty, "Server", "Database", "Username", "Password"))
        { }

        /// <summary>
        /// Releases all resources used by the DBConnectionString.
        /// </summary>
        /// <param name="disposing">
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// can be disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer and you should not reference
        /// other objects. Only unmanaged resources can be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            // Check to see if Dispose has already been called.
            if (!this.Disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.


                // Note disposing has been done.
                Disposed = true;
            }
        }

        /// <summary>
        /// Get server specific DataAdapter
        /// </summary>
        /// <returns>DbDataAdapter</returns>
        protected override DbDataAdapter DBDataAdapter(IDbCommand dbCommand)
        {
            return new MySqlDataAdapter((MySqlCommand)dbCommand);
        }

        /// <summary>
        /// Check specific connection provider if connection broken
        /// </summary>
        /// <param name="ex">Exception from Exec void</param>
        /// <returns>true - connection broken</returns>
        protected override bool CheckConnectionBroken(Exception ex)
        {
            bool b = true;
            //MySqlException e = ex as MySqlException;

            // TODO Must implement true code
            //if (e != null && e.Errors != null && e.Errors.Count > 0)
            //    b = true;

            return b;
        }
    }
}
#endif