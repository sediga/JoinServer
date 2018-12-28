using System;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Data.Linq;
using System.Collections.ObjectModel;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement base class for all SQL server.
    /// Including common properties and voids.
    /// </summary>
    public abstract class DataLayerBase : IDataLayer
    {
        #region private Fields

        /// <summary>
        /// Data base connection
        /// </summary>
        private IDbConnection _connection;
        
        /// <summary>
        /// Connection string class instance
        /// </summary>
        private IDBConnectionString _connectionString;
        
        /// <summary>
        /// Transaction object
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// Indicate if exist error
        /// </summary>
        private bool _error;

        /// <summary>
        /// Text content last error
        /// </summary>
        private string _lastError;

        /// <summary>
        /// Re raise last exception
        /// </summary>
        private bool _reRaiseException;

        /// <summary>
        /// Keep connection alive
        /// </summary>
        private bool _keepConnection;

        /// <summary>
        /// Store current data base type
        /// </summary>
        private DatabaseTypes _dataBaseType;

        /// <summary>
        /// Default command type
        /// </summary>
        private CommandType _defaultCommandType;

        /// <summary>
        /// Data base command instance
        /// </summary>
        private IDbCommand _command;

        /// <summary>
        /// Indicate if object disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Last operation is reconnected
        /// </summary>
        private bool _inNotReconnected;

        /// <summary>
        /// Event which is cast in error occurs when working with database
        /// </summary>
        private event EventHandler<DatabaseErrorEventArgs> _dataBaseErrorEventHandler;

        /// <summary>
        /// MySql connection is reopened
        /// </summary>
        private bool _mySQLReOpen;

        /// <summary>
        /// Parameters prefix
        /// </summary>
        private string _paramPrefix;

        /// <summary>
        /// Flags are used to complete the list of entities
        /// </summary>
        private BindingFlags _propertyBindingFlag;

        #endregion

        #region protected Properties

        /// <summary>
        /// Flag that indicates whether the object is already released.
        /// </summary>
        protected bool Disposed
        {
            get { return _disposed; }
            set { _disposed = value; }
        }

        #endregion

        #region public Properties

        /// <summary>
        /// Connection to database.
        /// </summary>
        public IDbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Connection string text
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString.Text; }
            set { _connectionString.Text = value; }
        }

        /// <summary>
        /// Supported Connection String object.
        /// </summary>
        public IDBConnectionString ConnectionStringInstance
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Collection parameters for the query.
        /// </summary>
        public IDataParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string Sql
        {
            get { return _command.CommandText; }
            set { SetSQL(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText property is to be interpreted.
        /// Allows dynamic adjustment of the type of command before any operation.
        /// </summary>
        public CommandType CommandType
        {
            get { return _command.CommandType; }
            set { _command.CommandType = value; }
        }

        /// <summary>
        /// Global setup of the type of command by default.
        /// Automatic set DefaultCommandType after always set Sql text.
        /// </summary>
        public CommandType DefaultCommandType
        {
            get { return _defaultCommandType; }
            set { _defaultCommandType = value; }
        }

        /// <summary>
        /// Specifies the current type of database.
        /// </summary>
        public DatabaseTypes DatabaseType
        {
            get { return _dataBaseType; }
        }

        /// <summary>
        /// Indicates whether the server connection is open.
        /// </summary>
        public bool Connected
        {
            get { return GetConnected(); }
        }

        /// <summary>
        /// Indicate if last action over with error.
        /// </summary>
        public bool IsError
        {
            get { return _error; }
        }

        /// <summary>
        /// Content last thrown error.
        /// </summary>
        public string LastError
        {
            get { return _lastError; }
        }

        /// <summary>
        /// If true, throw exception. If false the contents of the error is recorded in LastError.
        /// </summary>
        public bool RERaiseException
        {
            get { return _reRaiseException; }
            set { _reRaiseException = value; }
        }

        /// <summary>
        /// Keep Connection alive.
        /// </summary>
        public bool KeepConnection
        {
            get { return _keepConnection; }
            set { _keepConnection = value; }
        }

        /// <summary>
        /// Indicates whether the transaction is started.
        /// </summary>
        public bool InTransaction
        {
            get { return _transaction != null; }
        }

        /// <summary>
        /// Parameter prefix.
        /// </summary>
        public string ParameterPrefix
        {
            get { return _paramPrefix; }
        }

        #endregion

        #region public Events
        
        /// <summary>
        /// Event that is thrown when an error occurs when working with database.
        /// </summary>
        public event EventHandler<DatabaseErrorEventArgs> DatabaseError
        {
            add { _dataBaseErrorEventHandler += value; }
            remove { _dataBaseErrorEventHandler -= value; }
        }

        #endregion

        #region public Constructor, Destructor

        /// <summary>
        /// Initializes a new instance of the DataLayerBase class.
        /// </summary>
        /// <param name="databaseType">Data base type and create this class</param>
        /// <param name="dbConnection">Connection specific SQL server type</param>
        /// <param name="parameterPrefix">Paramether prefix specific SQL server</param>
        /// <param name="parameterNames">Parameter names</param>
        protected DataLayerBase(DatabaseTypes databaseType, IDbConnection dbConnection, string parameterPrefix, ConnectionParameterNames parameterNames)
        {
            if (databaseType == DatabaseTypes.None)
                throw new ArgumentException("constructor DataLayerBase(). DatabaseType can not be 'None'!");

            _dataBaseType = databaseType;

            if (dbConnection == null)
                throw new ArgumentNullException("dbConnection", "Can not be Null!");
            _connection = dbConnection;

            _propertyBindingFlag = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

            // Create ConnectionString 
            _connectionString = new DBConnectionString(databaseType, parameterNames);
            _connectionString.Changed += new EventHandler(ConnectionString_Changed);

            _lastError = string.Empty;
            // This a default initialization
            //_reRaiseException = false;

            _keepConnection = true;

            _inNotReconnected = true;

            // Set specific Prameter prefix
            _paramPrefix = parameterPrefix;

            // Create command
            CreateCommand();
            // Set command type
            _command.CommandType = CommandType.Text;
            _defaultCommandType = CommandType.Text;
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the object
        /// </summary>
        /// <param name="disposing">true - free all menaged resources</param>
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    if (_connectionString != null)
                        _connectionString.Dispose();

                    if (_command != null)
                        _command.Dispose();

                    if (_connection != null)
                    {
                        _connection.Close();
                        _connection.Dispose();
                    }
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.

                // Note disposing has been done.
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor. Close and free connection and other linked objects
        /// </summary>
        ~DataLayerBase()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region public Connection voids
        
        /// <summary>
        /// Opens a connection to the database.
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="password">User password</param>
        /// <returns>Returns true if the connection is opened successfully.</returns>
        public bool Open(string userName, string password)
        {
            _connectionString.User = userName;
            _connectionString.Password = password;

            return Open();
        }

        /// <summary>
        /// Opens a connection to the database.
        /// </summary>
        /// <returns>Returns true if the connection is opened successfully.</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public bool Open()
        {
            // If not connnected - connect
            if (!GetConnected())
            {
                try
                {
                    // This fix bug Firebird command.
                    IDbCommand command = null;
                    if (_dataBaseType == DatabaseTypes.Firebird && _command != null)
                        command = _command;

                    _connection.Open();
                    _mySQLReOpen = false;

                    // This fix bug Firebird command. If connection broken - broken and Command
                    if (_dataBaseType == DatabaseTypes.Firebird)
                    {
                        CreateCommand();
                        // Copy command parameters from old connection.
                        if (command != null)
                        {
                            _command.CommandText = command.CommandText;
                            _command.CommandType = command.CommandType;
                            for (int i = 0; i < command.Parameters.Count; i++)
                                _command.Parameters.Add(command.Parameters[i]);

                            command.Dispose();
                        }
                    }

                    // Free old transaction
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                        //throw new DataLayerException("The latest transaction is unsuccessful.");
                    }

                    _error = false;
                }
                catch (Exception ex)
                {
                    // This fix bug MySQL re open connection
                    if (_dataBaseType == DatabaseTypes.MySql && !_mySQLReOpen)
                    {
                        _mySQLReOpen = true;
                        _connection.Close();
                        return Open();
                    }
                    else
                        ExceptionExecuteManipulate(_connection, ex.Message, false);

                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                }
            }
            else
                return true;

            return GetConnected();
        }

        /// <summary>
        /// Closes connection to database.
        /// </summary>
        public void Close()
        {
            _connection.Close();
        }

        #endregion

        #region public Transaction voids
        
        /// <summary>
        /// Begin (start) transaction
        /// </summary>
        public void BeginTransaction()
        {
            Close();

            if (Open())
                _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Begin (start) transaction
        /// </summary>
        /// <param name="il">Transaction Isolation Level</param>
        public void BeginTransaction(IsolationLevel il)
        {
            Close();

            if (Open())
                _transaction = _connection.BeginTransaction(il);
        }
        
        /// <summary>
        /// Rollback started transaction
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }
        
        /// <summary>
        /// Commit started transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }
        
        #endregion

        #region public AddParameter methods

        /// <summary>
        /// Add parameter to command. Allows adjustment of all properies.
        /// </summary>
        /// <returns>Returns the current command parameter</returns>
        public IDbDataParameter AddParameter()
        {
            IDbDataParameter param = _command.CreateParameter();
            _command.Parameters.Add(param);

            return param;
        }

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <returns>Returns the current command parameter</returns>
        public IDbDataParameter AddParameter(string name, object value)
        {
            IDbDataParameter param = AddParameter();

            param.ParameterName = name;
            param.Value = value;

            return param;
        }

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="dbType">Type Parameter</param>
        /// <returns>Returns the current command parameter</returns>
        public IDbDataParameter AddParameter(string name, object value, DbType dbType)
        {
            IDbDataParameter param = AddParameter();

            param.ParameterName = name;
            param.Value = value;
            param.DbType = dbType;

            return param;
        }

        /// <summary>
        /// Add parameter to command. Typically used for parameters of type Output and ReturnValue.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="dbType">Type Parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <returns>Returns the current command parameter</returns>
        public IDbDataParameter AddParameter(string name, DbType dbType, ParameterDirection direction)
        {
            IDbDataParameter param = AddParameter();

            param.ParameterName = name;
            param.DbType = dbType;
            param.Direction = direction;

            return param;
        }

        /// <summary>
        /// Add parameter to command.
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="dbType">Type Parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <returns>Returns the current command parameter</returns>
        public IDbDataParameter AddParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            IDbDataParameter param = AddParameter();

            param.ParameterName = name;
            param.Value = value;
            param.DbType = dbType;
            param.Direction = direction;

            return param;
        }

        /// <summary>
        /// Add parameters in the order they were submitted. 
        /// For example if you have the following parameters :ID and :Name and submitted the following values [12, "Plamen"], first be assigned to :ID (12), the second of :Name ("Plamen").
        /// The parameters direction is Input.
        /// </summary>
        /// <param name="parameters">Paramter list</param>
        public void AddParameters(params object[] parameters)
        {
            string sql = _command.CommandText;

            if (parameters == null || parameters.Length == 0 || String.IsNullOrEmpty(sql))
                return;

            int startIndex = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                startIndex = sql.IndexOf(_paramPrefix, startIndex, StringComparison.OrdinalIgnoreCase);
                if (startIndex > -1)
                {
                    startIndex++;
                    int endIndex = sql.IndexOfAny(Constants.SqlDivider(), startIndex);
                    string paramName = sql.Substring(startIndex, endIndex - startIndex);

                    AddParameter(paramName, parameters[i]);
                }
                else
                    throw new ArgumentOutOfRangeException("Parmeter in position " + (i + 1).ToString(CultureInfo.CurrentCulture) + " and value (" + parameters[i].ToString()
                        + ") does not exist in sql:\n" + sql);
            }
        }

        #endregion

        #region public Execute voids
        
        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public int ExecuteNonQuery()
        {
            int result = -1;

            if (Open())
            {
                IDbCommand dbComm = Command();

                try
                {
                    result = dbComm.ExecuteNonQuery();
                    _error = false;
                }
                catch (Exception ex)
                {
                    if (Reconnect(dbComm, ex))
                        return ExecuteNonQuery();
                }
            }

            _inNotReconnected = true;

            return result;
        }
        
        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public object ExecuteScalar()
        {
            object result = null;

            if (Open())
            {
                IDbCommand dbComm = Command();

                try
                {
                    result = dbComm.ExecuteScalar();
                    _error = false;
                }
                catch (Exception ex)
                {
                    if (Reconnect(dbComm, ex))
                        return ExecuteScalar();
                }
            }

            _inNotReconnected = true;

            return result;
        }
        
        /// <summary>
        /// Executes the query, and returns the IDataReader.
        /// </summary>
        /// <returns>Reads a forward-only stream of rows from a data source.</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IDataReader ExecuteDataReader()
        {
            IDataReader result = null;

            if (Open())
            {
                IDbCommand dbComm = Command();

                try
                {
                    result = dbComm.ExecuteReader();
                    _error = false;
                }
                catch (Exception ex)
                {
                    if (Reconnect(dbComm, ex))
                        return ExecuteDataReader();
                }
            }

            _inNotReconnected = true;

            return result;
        }
        
        /// <summary>
        /// Executes the query, and returns the completed table with the result.
        /// </summary>
        /// <returns>Completed table. Null - Error</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataTable ExecuteDataTable()
        {
            DataTable result = null;

            if (Open())
            {
                IDbCommand dbComm = Command();

                DbDataAdapter da = DBDataAdapter(dbComm);

                result = DataTable();

                try
                {
                    da.Fill(result);
                    _error = false;
                }
                catch (Exception ex)
                {
                    result.Dispose();
                    result = null;

                    if (Reconnect(dbComm, ex))
                        return ExecuteDataTable();
                }
                finally
                {
                    da.Dispose();
                }
            }

            _inNotReconnected = true;

            return result;
        }
        
        /// <summary>
        /// Executes the query, and returns the completed dataSet with the result.
        /// </summary>
        /// <returns>Completed dataSet. Null - Error</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataSet ExecuteDataSet()
        {
            DataSet result = null;

            if (Open())
            {
                IDbCommand dbComm = Command();

                DbDataAdapter da = DBDataAdapter(dbComm);

                result = DataSet();

                try
                {
                    da.Fill(result);
                    _error = false;
                }
                catch (Exception ex)
                {
                    result.Dispose();
                    result = null;

                    if (Reconnect(dbComm, ex))
                        return ExecuteDataSet();
                }
                finally
                {
                    da.Dispose();
                }
            }

            _inNotReconnected = true;

            return result;
        }

        /// <summary>
        /// Executes the query, and returns the completed Entity list with the result.
        /// Property names and the names of the result fields must be the same.
        /// </summary>
        /// <typeparam name="T">Type entity</typeparam>
        /// <returns>Completed list of entity</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessageAttribute("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public List<T> ExecuteAndFillList<T>() where T : new()
        {
            List<T> result = null;
            IDataReader dr = ExecuteDataReader();

            if (dr == null)
                return result;

            result = new List<T>();

            if (dr.FieldCount > 0)
            {
                // Prepare maping data
                EntityStructureData[] fld = PrepareFieldData(dr, typeof(T));

                try
                {
                    // Enumerate DataReader
                    object value;
                    EntityStructureData fieldInfo;
                    while (dr.Read())
                    {
                        T ent = new T();
                        for (int i = 0; i < fld.Length; i++)
                        {
                            fieldInfo = fld[i];
                            value = dr[fieldInfo.FieldPosition];
                            if (DBNull.Value != value)
                                fieldInfo.PropertyInfo.SetValue(ent, value, null);
                        }

                        result.Add(ent);
                    }

                    _error = false;
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionExecuteManipulate(result, ex.Message, true);
                }
                catch (Exception ex)
                {
                    ExceptionExecuteManipulate(result, ex.Message, true);
                }
            }

            dr.Dispose();

            return result;
        }

        /// <summary>
        /// Executes the query, and returns the completed Entity list with the result.
        /// Property names and the names of the result fields must be the same.
        /// </summary>
        /// <typeparam name="T">Type entity</typeparam>
        /// <returns>Completed list of entity</returns>
        [SuppressMessageAttribute("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessageAttribute("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T ExecuteAndFill<T>() where T : new()
        {
            T result = default(T);
            IDataReader dr = ExecuteDataReader();

            if (dr == null)
                return result;

            if (dr.FieldCount > 0)
            {
                result = new T();

                // Prepare maping data
                EntityStructureData[] fld = PrepareFieldData(dr, typeof(T));

                try
                {
                    // Enumerate DataReader
                    object value;
                    EntityStructureData fieldInfo;
                    if (dr.Read())
                    {
                        for (int i = 0; i < fld.Length; i++)
                        {
                            fieldInfo = fld[i];
                            value = dr[fieldInfo.FieldPosition];
                            if (DBNull.Value != value)
                                fieldInfo.PropertyInfo.SetValue(result, value, null);
                        }
                    }

                    _error = false;
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionExecuteManipulate(result, ex.Message, true);
                }
                catch (Exception ex)
                {
                    ExceptionExecuteManipulate(result, ex.Message, true);
                }
            }

            dr.Dispose();

            return result;
        }

        #endregion

        #region protected voids
        
        /// <summary>
        /// Fire event if throw database error
        /// </summary>
        /// <param name="sender">Object send</param>
        /// <param name="message">Error message</param>
        protected void OnDatabaseError(object sender, string message)
        {
            if (_dataBaseErrorEventHandler != null)
            {
                DatabaseErrorEventArgs ea = new DatabaseErrorEventArgs(message);
                _dataBaseErrorEventHandler(sender, ea);
            }
        }

        /// <summary>
        /// Reconnect if connection broken.
        /// </summary>
        /// <returns>true - seccusses reconnect</returns>
        protected bool Reconnect(object sender, Exception ex)
        {
            bool result = false;

            // New start
            if (_inNotReconnected && CheckConnectionBroken(ex))
            {
                _inNotReconnected = false;
                _connection.Close();

                result = Open();
            }

            if (!result)
                ExceptionExecuteManipulate(sender, ex.Message, true);

            return result;
        }

        /// <summary>
        /// Provide functionality throw and or show message Exceptions.
        /// </summary>
        /// <param name="sender">Object to send</param>
        /// <param name="message">Nativ message</param>
        /// <param name="executeError">true - execute error. false - connection error</param>
        protected void ExceptionExecuteManipulate(object sender, string message, bool executeError)
        {
            _error = true;

            string s;
            if (executeError)
                s = BuildErrorMessage(message);
            else
                s = "Native error: '" + message + "'"
                  + Constants.NewLine + "Error open connection. Parameters:"
                  + Constants.NewLine + "Server: '" + _connectionString.Server + "'"
                  + Constants.NewLine + "DataBase: '" + _connectionString.Database + "'"
                  + Constants.NewLine + "UserName: '" + _connectionString.User + "'";

            _lastError = s;

            // Throwing event
            OnDatabaseError(sender, s);

            if (_reRaiseException)
                throw new DataLayerException(s);
        }

        /// <summary>
        /// Get specific server DBCommand
        /// </summary>
        /// <returns>IDbCommand</returns>
        protected IDbCommand Command()
        {
            if (_transaction != null)
                _command.Transaction = _transaction;

            return _command;
        }

        /// <summary>
        /// Create DataTable and sel location
        /// </summary>
        /// <returns>DataTable</returns>
        protected static DataTable DataTable()
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;

            return dt;
        }

        /// <summary>
        /// Create DataSet and sel location
        /// </summary>
        /// <returns>DataSet</returns>
        protected static DataSet DataSet()
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;

            return ds;
        }

        #endregion

        #region protected abstract voids DBServerSpecific
        
        /// <summary>
        /// Check specific connection provider if connection broken
        /// </summary>
        /// <param name="ex">Exception from Exec void</param>
        /// <returns>true - connection broken</returns>
        protected abstract bool CheckConnectionBroken(Exception ex);
        
        /// <summary>
        /// Get specific server DbDataAdapter. Abstract must implement inherited class.
        /// </summary>
        /// <returns>DbCommand</returns>
        protected abstract DbDataAdapter DBDataAdapter(IDbCommand dbCommand);
        
        #endregion

        #region private voids
        
        /// <summary>
        /// Create a new command object to the current connection
        /// </summary>
        private void CreateCommand()
        {
            _command = _connection.CreateCommand();
        }

        /// <summary>
        /// Set sql text
        /// </summary>
        /// <param name="sql">Sql statement</param>
        private void SetSQL(string sql)
        {
            _command.Parameters.Clear();
            _command.CommandType = _defaultCommandType;
            _command.CommandText = sql;
        }

        /// <summary>
        /// ConnectionString is changed event handler.
        /// This causes a close connection to the database
        /// </summary>
        /// <param name="sender">Object sender - ConnectionString</param>
        /// <param name="e">Event arguments</param>
        private void ConnectionString_Changed(object sender, EventArgs e)
        {
            _connection.Close();
            _connection.ConnectionString = _connectionString.Text;
        }

        /// <summary>
        /// Get if connection opened.
        /// </summary>
        /// <returns></returns>
        private bool GetConnected()
        {
            return _connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// Build full message include: native message, SQL and Parameters.
        /// </summary>
        /// <param name="message">Native message</param>
        /// <returns>Full message informatio</returns>
        private string BuildErrorMessage(string message)
        {
            StringBuilder sb = new StringBuilder("Native error: " + message + Constants.NewLine);
            
            // Add sql text
            sb.AppendLine("SQL: " + _command.CommandText);

            // Add parameters
            sb.AppendLine("Parameters:");
            for (int i = 0; i < _command.Parameters.Count; i++)
            {
                DbParameter param = (DbParameter)_command.Parameters[i];
                sb.AppendFormat("Name ({0}), Type ({1}), Value ({2})" + Constants.NewLine
                    , ConvertToText(param.ParameterName), ConvertToText(param.DbType), ConvertToText(param.Value));
            }

            string s = sb.ToString();
            sb = null;

            return s;
        }

        /// <summary>
        /// Convert object to text
        /// </summary>
        /// <param name="obj">Object to convert</param>
        /// <returns>Text from object</returns>
        private static string ConvertToText(object obj)
        {
            return obj == null ? "null" : obj.ToString();
        }

        /// <summary>
        /// Prepare the data for mapping
        /// </summary>
        /// <param name="dr">Data reader from where data is read</param>
        /// <param name="entityType">Entity type</param>
        /// <returns>Mapped data fron source field and entity properties</returns>
        public EntityStructureData[] PrepareFieldData(IDataRecord dr, Type entityType)
        {
            List<EntityStructureData> result = new List<EntityStructureData>(dr.FieldCount);

            for (int i = 0; i < dr.FieldCount; i++)
            {
                string fieldName = dr.GetName(i);
                PropertyInfo pi = entityType.GetProperty(fieldName, _propertyBindingFlag);
                if (pi != null)
                    result. Add(new EntityStructureData(i, pi));
            }
            return result.ToArray();
        }

        #endregion
    }
}