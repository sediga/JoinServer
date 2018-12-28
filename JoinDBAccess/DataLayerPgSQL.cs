#if (ALL || POSTGREE)
using System.Data;
using System.Data.Common;
using Npgsql;
using System;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement a wrapped of Postgree SQL server
    /// </summary>
    internal class DataLayerPGSql : DataLayerBase
    {
        /// <summary>
        /// Initializes a new instance of the DataLayerPGSql class.
        /// </summary>
        public DataLayerPGSql()
            : base(DatabaseTypes.PostGre, new NpgsqlConnection(), ":"
            , new ConnectionParameterNames(string.Empty, "Server", "Database", "User ID", "Password"))
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
            return new NpgsqlDataAdapter((NpgsqlCommand)dbCommand);
        }

        /// <summary>
        /// Check specific connection provider if connection broken
        /// </summary>
        /// <param name="ex">Exception from Exec void</param>
        /// <returns>true - connection broken</returns>
        protected override bool CheckConnectionBroken(Exception ex)
        {
            bool b = false;
            NpgsqlException e = ex as NpgsqlException;

            //if (e != null && e.Errors != null && e.Errors.Count > 0)
            //{
            //    NpgqlError error = (NpgsqlError)e.Errors[0];
            //    b = String.IsNullOrEmpty(error.Code) && String.IsNullOrEmpty(error.ErrorSql); // Error no connect DB
            //}

            return b;
        }
    }
}
#endif