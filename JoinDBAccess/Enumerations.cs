using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace DataAccessLayer
{
    /// <summary>
    /// Enumeration include all supported SQL servers
    /// </summary>
    [Flags]
    public enum DatabaseTypes
    {
        /// <summary>
        /// Unexpected data base
        /// </summary>
        None = 0, 
        /// <summary>
        /// Microsoft SQL Server
        /// </summary>
        MSSql = 1, 
        /// <summary>
        /// PostgreSQL server
        /// </summary>
        PostGre = 2, 
        /// <summary>
        /// Firebird SQL server
        /// </summary>
        Firebird = 4,
        /// <summary>
        /// MySQL Server
        /// </summary>
        MySql = 8, 
        /// <summary>
        /// SQLite database engine
        /// </summary>
        [SuppressMessageAttribute("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        SQLite = 16, 
        /// <summary>
        /// Oracle Database
        /// </summary>
        Oracle = 32, 
        /// <summary>
        /// SQL Server Compact
        /// </summary>
        SqlServerCE = 64,
        /// <summary>
        /// Ole db provider
        /// </summary>
        OleDB = 128
    };
}
