using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    /// <summary>
    /// Provide interface Connection string class
    /// </summary>
    public interface IDBConnectionString : IDisposable
    {
        /// <summary>
        /// Gets or sets the connection string text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Get the <c>DatabaseTypes</c>
        /// </summary>
        DatabaseTypes DatabaseType { get; }

        /// <summary>
        /// Get and set value that indicates add aser name and password to connection string
        /// If necessary User and password. Situation "Integrated security"
        /// </summary>
        bool AdditionUserNameAndPassword { get; set; }

        /// <summary>
        /// Get and set value indicate add missing parameters to connection string.
        /// </summary>
        bool AddMissingParameter { get; set; }

        /// <summary>
        /// Get the provider parameter name
        /// </summary>
        string Provider { get; set; }

        /// <summary>
        /// Gets or sets the provider value
        /// </summary>
        string ProviderParameter { get; }

        /// <summary>
        /// Get the server parameter name
        /// </summary>
        string ServerParameter { get; }

        /// <summary>
        /// Gets or sets the Server name or IP
        /// </summary>
        string Server { get; set; }

        /// <summary>
        /// Gets the database paramter name
        /// </summary>
        string DatabaseParameter { get; }

        /// <summary>
        /// Gets or sets the data base name
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// Gets the user parameter name
        /// </summary>
        string UserParameter { get; }

        /// <summary>
        /// Gets or sets the user value
        /// </summary>
        string User { get; set; }

        /// <summary>
        /// Gets the password parameter name
        /// </summary>
        string PasswordParameter { get; }

        /// <summary>
        /// Gets or sets password value
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets the other settings. 
        /// <example>"Persist Security Info=True;"</example>
        /// </summary>
        string OtherSettings { get; }

        /// <summary>
        /// Occurs when the any value is changed.
        /// </summary>
        event EventHandler Changed;
    }
}
