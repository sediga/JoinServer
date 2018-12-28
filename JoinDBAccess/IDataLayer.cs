using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Data.Common;

namespace DataAccessLayer
{
    /// <summary>
    /// Provides an interface for connecting to different databases.
    /// </summary>
    public interface IDataLayer : IDisposable
    {
        #region Properties

        /// <summary>
        /// Connection to database.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Connection string text.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Supported Connection String object.
        /// </summary>
        IDBConnectionString ConnectionStringInstance { get; }

        /// <summary>
        /// Collection parameters for the query.
        /// </summary>
        IDataParameterCollection Parameters { get; }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        string Sql { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText property is to be interpreted.
        /// Allows dynamic adjustment of the type of command before any operation.
        /// </summary>
        CommandType CommandType { get; set; }

        /// <summary>
        /// Global setup of the type of command by default.
        /// </summary>
        CommandType DefaultCommandType { get; set; }

        /// <summary>
        /// Specifies the current type of database.
        /// </summary>
        DatabaseTypes DatabaseType { get; }

        /// <summary>
        /// Indicates whether the server connection is open.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Indicate if last action over with error.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Content last thrown error.
        /// </summary>
        string LastError { get; }

        /// <summary>
        /// If true, throw exception. If false the contents of the error is recorded in LastError.
        /// </summary>
        bool RERaiseException { get; set; }

        /// <summary>
        /// Keep Connection alive.
        /// </summary>
        bool KeepConnection { get; set; }

        /// <summary>
        /// Indicates whether the transaction is started.
        /// </summary>
        bool InTransaction { get; }

        /// <summary>
        /// Parameter prefix.
        /// </summary>
        string ParameterPrefix { get; }

        #endregion

        #region Events

        /// <summary>
        /// Event that is thrown when an error occurs when working with database.
        /// </summary>
        event EventHandler<DatabaseErrorEventArgs> DatabaseError;

        #endregion

        #region Connection voids

        /// <summary>
        /// Opens a connection to the database.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">User password</param>
        /// <returns>Returns true if the connection is opened successfully.</returns>
        bool Open(string userName, string password);

        /// <summary>
        /// Opens a connection to the database.
        /// </summary>
        /// <returns>Returns true if the connection is opened successfully.</returns>
        bool Open();

        /// <summary>
        /// Closes connection to database.
        /// </summary>
        void Close();

        #endregion

        #region Transaction manipulate

        /// <summary>
        /// Begin (start) transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Begin (start) transaction
        /// </summary>
        /// <param name="il">Transaction Isolation Level</param>
        void BeginTransaction(IsolationLevel il);

        /// <summary>
        /// Rollback started transaction
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Commit started transaction
        /// </summary>
        void CommitTransaction();

        #endregion

        #region Add parameter

        /// <summary>
        /// Add parameter to command. Allows adjustment of all properies.
        /// </summary>
        /// <returns>Returns the current command parameter</returns>
        IDbDataParameter AddParameter();

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Returns the current command parameter</returns>
        IDbDataParameter AddParameter(string name, object value);

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="dbType">Type Parameter</param>
        /// <returns>Returns the current command parameter</returns>
        IDbDataParameter AddParameter(string name, object value, DbType dbType);

        /// <summary>
        /// Add parameter to command. Typically used for parameters of type Output and ReturnValue.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="dbType">Type Parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <returns>Returns the current command parameter</returns>
        IDbDataParameter AddParameter(string name, DbType dbType, ParameterDirection direction);

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="dbType">Type Parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <returns>Returns the current command parameter</returns>
        IDbDataParameter AddParameter(string name, object value, DbType dbType, ParameterDirection direction);

        /// <summary>
        /// Add parameters in the order they were submitted. 
        /// For example if you have the following parameters :ID and :Name and submitted the following values [12, "Plamen"], first be assigned to :ID (12), the second of :Name ("Plamen").
        /// The parameters direction is Input.
        /// </summary>
        /// <param name="parameters">Paramter list</param>
        void AddParameters(params object[] parameters);

        #endregion

        #region Execute voids

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery();

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        object ExecuteScalar();

        /// <summary>
        /// Executes the query, and returns the IDataReader.
        /// </summary>
        /// <returns>Reads a forward-only stream of rows from a data source.</returns>
        IDataReader ExecuteDataReader();

        /// <summary>
        /// Executes the query, and returns the completed table with the result.
        /// </summary>
        /// <returns>Completed table. Null - Error</returns>
        DataTable ExecuteDataTable();

        /// <summary>
        /// Executes the query, and returns the completed dataSet with the result.
        /// </summary>
        /// <returns>Completed dataSet. Null - Error</returns>
        DataSet ExecuteDataSet();

        /// <summary>
        /// Executes the query, and returns the completed Entity list with the result.
        /// Property names and the names of the result fields must be the same.
        /// </summary>
        /// <typeparam name="T">Type entity</typeparam>
        /// <returns>Completed list of entity</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessageAttribute("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        List<T> ExecuteAndFillList<T>() where T : new();

        /// <summary>
        /// Executes the query, and returns the first row in the result set Entity. Additional rows are ignored.
        /// Property names and the names of the result fields must be the same.
        /// </summary>
        /// <typeparam name="T">Type entity</typeparam>
        /// <returns>Completed list of entity</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        T ExecuteAndFill<T>() where T : new();

        #endregion
    }
}
