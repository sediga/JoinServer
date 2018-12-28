using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement application costants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Contains characters that separate the keywords and paramerite in a SQL query.
        /// </summary>
        public static char[] SqlDivider()
        { 
            return ",*()-\r\n\t ".ToCharArray(); 
        } 

        /// <summary>
        /// Contains newline characters.
        /// </summary>
        public readonly static string NewLine = Environment.NewLine;

        /// <summary>
        /// Contains the delimiter between parameters in ConnectionString.
        /// </summary>
        public const string ParameterDivider = ";";

        /// <summary>
        /// Contains characters between the name of the parameter and its value in ConnectionString.
        /// </summary>
        public const string ParameterValueDivider = "=";

        /// <summary>
        /// Name of the section settings DataLayer in app.config file.
        /// </summary>
        public const string AppConfigDatabaseSection = "DataAccessLayer";

        /// <summary>
        /// Name of key 'Datatabase type' in the settings section.
        /// </summary>
        public const string AppConfigDatabaseType = "DatabaseType";

        /// <summary>
        /// Name of key 'Connection string' in the settings section.
        /// </summary>
        public const string AppConfigConnectionString = "ConnectionString";

        /// <summary>
        /// Name of key 'Singleton' in the settings section.
        /// </summary>
        public const string AppConfigSingleton = "Singleton";
    }
}
