#if (ALL || OLEDB)
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement a wrapped of OleDB
    /// </summary>
    internal class DataLayerOleDB : DataLayerBase
    {
        /// <summary>
        /// Initializes a new instance of the DataLayerOleDB class.
        /// </summary>
        public DataLayerOleDB()
            : base(DatabaseTypes.OleDB, new OleDbConnection(), ":"
            , new ConnectionParameterNames(string.Empty, string.Empty, "Data Source", "User ID", "Password"))
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
            return new OleDbDataAdapter((OleDbCommand)dbCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected override bool CheckConnectionBroken(Exception ex)
        {
            bool b = true;
            //OleDbException e = ex as OleDbException;

            // TODO Must implement true code
            //if (e != null && e.Errors != null && e.Errors.Count > 0)
            //    b = true;

            return b;
        }
    }
}
#endif