using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement structure parameters of the ConnectionString
    /// </summary>
    public sealed class ConnectionParameterNames
    {
        #region private fields

        private string _provider;
        private string _server;
        private string _database;
        private string _userName;
        private string _userPassword;

        #endregion

        #region public properties

        /// <summary>
        /// Data base provider name
        /// </summary>
        public string Provider
        {
            get { return _provider; }
        }

        /// <summary>
        /// Server name or IP address
        /// </summary>
        public string Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Database name
        /// </summary>
        public string Database
        {
            get { return _database; }
        }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// User password
        /// </summary>
        public string UserPassword
        {
            get { return _userPassword; }
        }

        /// <summary>
        /// Indicate if all parameters is empty
        /// </summary>
        public bool Empty
        {
            get
            {
                return
                    String.IsNullOrEmpty(_provider)
                    && String.IsNullOrEmpty(_server)
                    && String.IsNullOrEmpty(_database)
                    && String.IsNullOrEmpty(_userName)
                    && String.IsNullOrEmpty(_userPassword);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the DataLayerBase class.
        /// </summary>
        /// <param name="provider">Data base provider name</param>
        /// <param name="server">Server name or IP address</param>
        /// <param name="database">Database name</param>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">User password</param>
        public ConnectionParameterNames(string provider, string server, string database, string userName, string userPassword)
        {
            _provider = provider;
            _server = server;
            _database = database;
            _userName = userName;
            _userPassword = userPassword;
        }
    }
}
