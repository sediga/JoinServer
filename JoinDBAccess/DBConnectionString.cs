using System;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement ConnectionString.
    /// </summary>
    internal class DBConnectionString : IDBConnectionString
    {
        #region private fields
        private DatabaseTypes _databaseType;
        private bool _additonUserAndPassword;

        private bool _addMissingParameter;

        private string _connectionString;

        private string _provider;
        private string _server;
        private string _database;
        private string _user;
        private string _password;

        private string _otherSettings;
        // Track whether Dispose has been called.
        private bool disposed;
        private ConnectionParameterNames _parameterNames;

        private EventHandler _changed;
        #endregion

        #region public Properties

        /// <summary>
        /// Gets or sets the connection string text
        /// </summary>
        public string Text
        {
            get { return GetConnectionString(); }
            set { SetConnectionString(value); }
        }

        /// <summary>
        /// Get the <c>DatabaseTypes</c>
        /// </summary>
        public DatabaseTypes DatabaseType
        {
            get { return _databaseType; }
        }

        /// <summary>
        /// Get and set value that indicates add aser name and password to connection string
        /// If necessary User and password. Situation "Integrated security"
        /// </summary>
        public bool AdditionUserNameAndPassword
        {
            get { return _additonUserAndPassword; }
            set { SetAdditonUserAndPassword(value); }
        }

        /// <summary>
        /// Get and set value indicate add missing parameters to connection string.
        /// </summary>
        public bool AddMissingParameter
        {
            get { return _addMissingParameter; }
            set { SetAddMissingParameter(value); }
        }

        /// <summary>
        /// Get the provider parameter name
        /// </summary>
        public string ProviderParameter
        {
            get { return _parameterNames.Provider; }
        }
        /// <summary>
        /// Gets or sets the provider value
        /// </summary>
        public string Provider
        {
            get { return _provider; }
            set { SetProvider(value); }
        }

        /// <summary>
        /// Get the server parameter name
        /// </summary>
        public string ServerParameter
        {
            get { return _parameterNames.Server; }
        }
        
        /// <summary>
        /// Gets or sets the Server name or IP
        /// </summary>
        public string Server
        {
            get { return _server; }
            set { SetServer(value); }
        }

        /// <summary>
        /// Gets the database paramter name
        /// </summary>
        public string DatabaseParameter
        {
            get { return _parameterNames.Database; }
        }

        /// <summary>
        /// Gets or sets the data base name
        /// </summary>
        public string Database
        {
            get { return _database; }
            set { SetDatabase(value); }
        }

        /// <summary>
        /// Gets the user parameter name
        /// </summary>
        public string UserParameter
        {
            get { return _parameterNames.UserName; }
        }

        /// <summary>
        /// Gets or sets the user value
        /// </summary>
        public string User
        {
            get { return _user; }
            set { SetUser(value); }
        }

        /// <summary>
        /// Gets the password parameter name
        /// </summary>
        public string PasswordParameter
        {
            get { return _parameterNames.UserPassword; }
        }

        /// <summary>
        /// Gets or sets password value
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { SetPassword(value); }
        }

        /// <summary>
        /// Gets the other settings. 
        /// <example>"Persist Security Info=True;"</example>
        /// </summary>
        public string OtherSettings
        {
            get { return _otherSettings; }
        }

        #endregion

        #region public event

        /// <summary>
        /// Occurs when the any value is changed.
        /// </summary>
        public event EventHandler Changed
        {
            add { _changed += value; }
            remove { _changed -= value; }
        }

        #endregion

        #region public Constructor & Dispose
        
        /// <summary>
        /// Initailize connection string prarams.
        /// </summary>
        public DBConnectionString(DatabaseTypes databaseType, ConnectionParameterNames parameterNames)
        {
            if (databaseType == DatabaseTypes.None)
                throw new ArgumentException("constructor DBConnectionString. DataBase type can not be 'None'!");

            // Plamen Kovandjiev 20100119. Change logics set parameterNames
            if (parameterNames == null || parameterNames.Empty)
                throw new ArgumentException("constructor DBConnectionString. ParameterNames type can not be Null or Empty!");

            _databaseType = databaseType;

            _additonUserAndPassword = true;
            _addMissingParameter = true;

            OnChanged();

            _connectionString = string.Empty;

            _provider = string.Empty;
            _server = string.Empty;
            _database = string.Empty;
            _user = string.Empty;
            _password = string.Empty;
            _otherSettings = string.Empty;

            // Get pameters name
            _parameterNames = parameterNames;
            //_parameterNames = DatabaseUtility.GetParameterName(databaseType);
        }

        /// <summary>
        /// Releases all resources used by the DBConnectionString.
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
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
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
                disposed = true;
            }
        }

        /// <summary>
        /// Dispose unmanaged resources
        /// </summary>
        ~DBConnectionString()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        #endregion

        #region protected
        
        /// <summary>
        /// Raises the <c>Changed</c> event.
        /// </summary>
        protected void OnChanged()
        {
            if (_changed != null)
            {
                _changed(this, EventArgs.Empty);
            }
        }

        #endregion

        #region private Properies voids

        /// <summary>
        /// Get current connection string
        /// </summary>
        /// <returns>Connection string value</returns>
        private string GetConnectionString()
        {
            if (_databaseType == DatabaseTypes.None)
                throw new ArgumentException("void GetConnectionString. DataBase type can not be 'None'!");

            return _connectionString;
        }

        /// <summary>
        /// Set connection string
        /// </summary>
        /// <param name="value">Connection string</param>
        private void SetConnectionString(string value)
        {
            // Check DataBaseType can not be none.
            if (_databaseType == DatabaseTypes.None)
                throw new ArgumentException("void SetConnectionString. DataBase type can not be 'None'!");

            if (!_connectionString.Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                if (SplitConnectionString(value))
                    _connectionString = value;

                OnChanged();
            }
        }

        /// <summary>
        /// Set provider name
        /// </summary>
        /// <param name="value">New provider name</param>
        private void SetProvider(string value)
        {
            if (String.IsNullOrEmpty(_parameterNames.Provider))
                return;

            string paramName = _parameterNames.Provider;
            string oldValue = _provider;

            if ((oldValue != value) && (SetParamValue(paramName, oldValue, value)))
                _provider = value;
        }

        /// <summary>
        /// Set server name
        /// </summary>
        /// <param name="value">New value</param>
        private void SetServer(string value)
        {
            if (String.IsNullOrEmpty(_parameterNames.Server))
                return;

            string paramName = _parameterNames.Server;
            string oldValue = _server;

            if ((oldValue != value) && (SetParamValue(paramName, oldValue, value)))
                _server = value;
        }

        /// <summary>
        /// Set database name
        /// </summary>
        /// <param name="value">New value</param>
        private void SetDatabase(string value)
        {
            if (String.IsNullOrEmpty(_parameterNames.Database))
                return;

            string paramName = _parameterNames.Database;
            string oldValue = _database;

            if ((oldValue != value) && (SetParamValue(paramName, oldValue, value)))
                _database = value;
        }

        /// <summary>
        /// Set user name
        /// </summary>
        /// <param name="value">New value</param>
        private void SetUser(string value)
        {
            if ((String.IsNullOrEmpty(_parameterNames.UserName))
                || (!_additonUserAndPassword))
                return;

            if ((_user != value) && (SetParamValue(_parameterNames.UserName, _user, value)))
                _user = value;
        }

        /// <summary>
        /// Set password
        /// </summary>
        /// <param name="value">New value</param>
        private void SetPassword(string value)
        {
            if ((String.IsNullOrEmpty(_parameterNames.UserPassword))
                || (!_additonUserAndPassword))
                return;

            if ((_password != value) && SetParamValue(_parameterNames.UserPassword, _password, value))
                _password = value;
        }

        /// <summary>
        /// Add user name and password if exist
        /// </summary>
        /// <param name="value">New value</param>
        private void SetAdditonUserAndPassword(bool value)
        {
            if (_additonUserAndPassword == value)
                return;

            _additonUserAndPassword = value;
            OnChanged();
        }

        /// <summary>
        /// Set missing parameter
        /// </summary>
        /// <param name="value">New value</param>
        private void SetAddMissingParameter(bool value)
        {
            if (_addMissingParameter == value)
                return;

            _addMissingParameter = value;
            OnChanged();
        }

        #endregion

        #region private support voids
        /// <summary>
        /// Set connection string param value
        /// </summary>
        /// <param name="param">Param name</param>
        /// <param name="oldValue">Old value</param>
        /// <param name="newValue">New value</param>
        /// <returns>true - seccussed</returns>
        private bool SetParamValue(string param, string oldValue, string newValue)
        {
            if (String.IsNullOrEmpty(param))
                throw new ArgumentNullException("param");

            string s = param + Constants.ParameterValueDivider;
            int i = _connectionString.IndexOf(s, StringComparison.OrdinalIgnoreCase);

            // If parameter not exist - test add for it
            if ((i == -1)
                && (PutParameter(param, newValue)))
            {
                OnChanged();
                return true;
            }

            if (i == -1)
                throw new ArgumentException("Param name '"
                    + param + "' can not exist in connection string:"
                    + Constants.NewLine + _connectionString + "'!");

            // Change Old paramter + value
            _connectionString = _connectionString.Replace(s + oldValue, s + newValue);

            OnChanged();
            return true;
        }
        /// <summary>
        /// Put parameter if not exist and property AddMissingParameter is true
        /// </summary>
        /// <param name="param">Param name</param>
        /// <param name="value">Param value</param>
        /// <returns></returns>
        private bool PutParameter(string param, string value)
        {
            if (!_addMissingParameter)
                return false;

            if (!param.Equals(_parameterNames.UserName, StringComparison.OrdinalIgnoreCase)
                && !param.Equals(_parameterNames.UserPassword, StringComparison.OrdinalIgnoreCase)
                && !param.Equals(_parameterNames.Provider, StringComparison.OrdinalIgnoreCase)
                && !param.Equals(_parameterNames.Server, StringComparison.OrdinalIgnoreCase)
                && !param.Equals(_parameterNames.Database, StringComparison.OrdinalIgnoreCase)
               )
                throw new ArgumentException("Param name '"
                    + param + "' can not exist in connection type '"
                    + _databaseType.ToString() + "'!");

            if (!_connectionString.EndsWith(Constants.ParameterDivider, StringComparison.Ordinal))
                _connectionString += Constants.ParameterDivider;

            string prm = param + Constants.ParameterValueDivider + value;
            _connectionString += prm;

            return true;
        }
        /// <summary>
        /// Get param value and delete this piece
        /// </summary>
        /// <param name="text">Connection string</param>
        /// <param name="paramName">Param name</param>
        /// <param name="paramValue">Out param value</param>
        private static void GetParamsValue(ref string text, string paramName, out string paramValue)
        {
            GetParamsValue(ref text, paramName, out paramValue, true);
        }
        /// <summary>
        /// Get param value.
        /// </summary>
        /// <param name="text">Connection string</param>
        /// <param name="paramName">Param name</param>
        /// <param name="paramValue">Out param value</param>
        /// <param name="deletePramText">true - delete this piece</param>
        private static void GetParamsValue(ref string text, string paramName, out string paramValue, bool deletePramText)
        {
            paramValue = string.Empty;

            if (String.IsNullOrEmpty(paramName))
                return;

            string param = paramName + Constants.ParameterValueDivider;

            // Get param position
            int startPos = text.IndexOf(param, StringComparison.OrdinalIgnoreCase);

            if (startPos == -1)
                return;

            int lenParam = param.Length;
            int lenDevider = Constants.ParameterDivider.Length;
            int startParamPos = startPos + lenDevider + (lenParam - 1);

            // Find end Param + value position (ParameterDivider) position
            int endPos = text.IndexOf(Constants.ParameterDivider, startPos, StringComparison.OrdinalIgnoreCase);

            if (endPos == -1)
            {
                endPos = text.Length;
                lenDevider = 0;
            }

            // Get param value
            paramValue = text.Substring(startParamPos, endPos - startParamPos);

            if (deletePramText)
                text = text.Remove(startPos, endPos - startPos + lenDevider);
        }
        /// <summary>
        /// Split connection string of params
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>true - seccussed</returns>
        private bool SplitConnectionString(string connectionString)
        {
            string s = connectionString;

            GetParamsValue(ref s, _parameterNames.Provider, out _provider);
            GetParamsValue(ref s, _parameterNames.Server, out _server);
            GetParamsValue(ref s, _parameterNames.Database, out _database);
            GetParamsValue(ref s, _parameterNames.UserName, out _user);
            GetParamsValue(ref s, _parameterNames.UserPassword, out _password);

            if ((s.Length > 0) && (s.Length - 1 == s.LastIndexOf(Constants.ParameterDivider, StringComparison.Ordinal)))
                s = s.Remove(s.Length - 1, Constants.ParameterDivider.Length);

            return true;
        }
        #endregion
    }
}
