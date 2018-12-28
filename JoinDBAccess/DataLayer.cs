using System;
using System.Configuration;
using System.Collections;

[assembly: CLSCompliant(true)]
namespace DataAccessLayer
{
    /// <summary>
    /// The main purpose of this class is to create instances for data providers.
    /// Implement pattern "Abstract factory"
    /// </summary>
    public abstract class DataLayer
    {
        #region Private fields

        /// <summary>
        /// Indicate data layer get for firs time
        /// </summary>
        private static bool _isNewInstance;

        /// <summary>
        /// Store key - value application settings
        /// </summary>
        private static Hashtable _appSettings;

        #endregion

        #region Public properties
        
        /// <summary>
        /// Indicate data layer get for firs time
        /// </summary>
        public static bool IsNewInstance
        {
            get { return _isNewInstance; }
        }

        #endregion

        #region public GetInstance voids.

        /// <summary>
        /// Take for instance DataLayer configured in App.config file.
        /// </summary>
        /// <example>This sample shows how to configure the appropriate sections in the app.config file.
        /// <code>
        ///   <configSections>
        ///     <section name="DataAccessLayer" type="System.Configuration.DictionarySectionHandler, System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
        ///   </configSections>
        ///     <DataAccessLayer>
        ///       <add key="ConnectionString" value="Server=localhost;User=SYSDBA;Password=masterkey;Database=C:\Program Files\Firebird\Firebird_2_1\examples\empbuild\EMPLOYEE.FDB"/>
        ///       <add key="DatabaseType" value="Firebird"/>
        ///       <!-- This key optional  -->
        ///       <!-- This is a default value -->
        ///       <add key="Singleton" value="true"/>
        ///     </DataAccessLayer>
        /// </code>
        /// </example>
        /// <returns>Instance of DataLayer</returns>
        public static IDataLayer GetInstance()
        {
            DatabaseTypes dbType = ConvertDBType(GetAppConfigSettings(Constants.AppConfigDatabaseType, true));

            return GetInstance(
                // Data base type
                dbType, 
                // Connection string
                GetAppConfigSettings(Constants.AppConfigConnectionString, true), 
                // Create singleton instance
                GetAppConfigSettings(Constants.AppConfigSingleton, false) != false.ToString()); 
        }

        /// <summary>
        /// Take for instance DataLayer.
        /// </summary>
        /// <param name="dbType">Data base type</param>
        /// <returns>Instance of DataLayer</returns>
        public static IDataLayer GetInstance(DatabaseTypes dbType)
        {
            IDataLayer dal = GetInstance(dbType, true);

            return dal;
        }

        /// <summary>
        /// Take for instance DataLayer.
        /// </summary>
        /// <param name="dbType">Data base type</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Instance of DataLayer</returns>
        public static IDataLayer GetInstance(DatabaseTypes dbType, string connectionString)
        {
            IDataLayer dal = GetInstance(dbType, true);
            dal.ConnectionString = connectionString;

            return dal;
        }

        /// <summary>
        /// Take specific Data layer instance and set connection string and create or not singleton instance
        /// </summary>
        /// <param name="dbType">Data base type</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="singleton">If parameter equal true - get singleton instance. If false - every call to the method generates a new instance.</param>
        /// <returns>Instance of DataLayer</returns>
        public static IDataLayer GetInstance(DatabaseTypes dbType, string connectionString, bool singleton)
        {
            IDataLayer dal = GetInstance(dbType, singleton);
            dal.ConnectionString = connectionString;

            return dal;
        }

        /// <summary>
        /// Take specific data layer and create or not singleton instance
        /// </summary>
        /// <param name="dbType">Data base type</param>
        /// <param name="singleton">If parameter equal true - get singleton instance. If false - every call to the method generates a new instance.</param>
        /// <returns>Instance of DataLayer</returns>
        public static IDataLayer GetInstance(DatabaseTypes dbType, bool singleton)
        {
            IDataLayer dal = null;

            switch (dbType)
            {
#if (ALL || MSSQL)
                case DatabaseTypes.MSSql:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerMSSql> gsMsSql = new GenericSingleton<DataLayerMSSql>();
                        dal = gsMsSql.Instance;
                        _isNewInstance = gsMsSql.IsNewInstance;
                    }
                    else
                        dal = new DataLayerMSSql();
                    break;
#endif
#if (ALL || POSTGREE)
                case DatabaseTypes.PostGre:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerPGSql> gsPG = new GenericSingleton<DataLayerPGSql>();
                        dal = gsPG.Instance;
                        _isNewInstance = gsPG.IsNewInstance;
                    }
                    else
                        dal = new DataLayerPGSql();
                    break;
#endif
                //case DataBaseType.dbtAccess:
                //    break;
#if (ALL || FIREBIRD)
                case DatabaseTypes.Firebird:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerFirebird> gsFB = new GenericSingleton<DataLayerFirebird>();
                        dal = gsFB.Instance;
                        _isNewInstance = gsFB.IsNewInstance;
                    }
                    else
                        dal = new DataLayerFirebird();
                    break;
#endif
#if (ALL || MYSQL)
                case DatabaseTypes.MySql:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerMySql> gsMySQL = new GenericSingleton<DataLayerMySql>();
                        dal = gsMySQL.Instance;
                        _isNewInstance = gsMySQL.IsNewInstance;
                    }
                    else
                        dal = new DataLayerMySql();
                    break;
#endif
#if (ALL || SQLITE)
                case DatabaseTypes.SQLite:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerSQLite> gsSQLite = new GenericSingleton<DataLayerSQLite>();
                        dal = gsSQLite.Instance;
                        _isNewInstance = gsSQLite.IsNewInstance;
                    }
                    else
                        dal = new DataLayerSQLite();
                    break;
#endif
#if (ALL || ORACLE)
                case DatabaseTypes.Oracle:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerOracle> gsOracle = new GenericSingleton<DataLayerOracle>();
                        dal = gsOracle.Instance;
                        _isNewInstance = gsOracle.IsNewInstance;
                    }
                    else
                        dal = new DataLayerOracle();
                    break;
#endif
#if (ALL || SQLSERVERCE)
                case DatabaseTypes.SqlServerCE:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerSqlServerCe> gsSqlCe = new GenericSingleton<DataLayerSqlServerCe>();
                        dal = gsSqlCe.Instance;
                        _isNewInstance = gsSqlCe.IsNewInstance;
                    }
                    else
                        dal = new DataLayerSqlServerCe();
                    break;
#endif
#if (ALL || OLEDB)
                case DatabaseTypes.OleDB:
                    if (singleton)
                    {
                        GenericSingleton<DataLayerOleDB> gsOleDb = new GenericSingleton<DataLayerOleDB>();
                        dal = gsOleDb.Instance;
                        _isNewInstance = gsOleDb.IsNewInstance;
                    }
                    else
                        dal = new DataLayerOleDB();
                    break;
#endif
                //case DataBaseType.dbtNone:
                //    break;
                default:
                    throw new ArgumentException("void GetInstance. Connection type '"
                        + dbType.ToString() + "' not exists!");
            }

            return dal;
        }

        #endregion

        #region Private voids

        /// <summary>
        /// Convert string to DatabaseTypes
        /// </summary>
        /// <param name="type">String type</param>
        /// <returns>DatabaseTypes</returns>
        private static DatabaseTypes ConvertDBType(string type)
        {
            type = type.Trim();

            DatabaseTypes dbType = DatabaseTypes.None;

            Object o = Enum.Parse(typeof(DatabaseTypes), type, true);
            if (o != null)
                dbType = (DatabaseTypes)o;
            else
                throw new ArgumentException("Undefined type of database " + type + ".");

            return dbType;
        }
        
        /// <summary>
        /// Get key from section
        /// </summary>
        /// <param name="keyName">Key name</param>
        /// <param name="mandatory">If the value is equal to the true and the key does not exist or is not completed throws an exception.</param>
        /// <returns>Key value</returns>
        private static string GetAppConfigSettings(string keyName, bool mandatory)
        {
            string error = String.Empty;
            string value = String.Empty;

            // Get keys
            // If keys already selected skip this.
            if (_appSettings == null)
            {
                try
                {
                    _appSettings = (Hashtable)ConfigurationManager.GetSection(Constants.AppConfigDatabaseSection);
                }
                catch (ConfigurationErrorsException ex)
                {
                    error = ex.Message + Constants.NewLine
                        + "In config file sectin name \"" + Constants.AppConfigDatabaseSection + "\" not exist.";
                    _appSettings = null;
                }
            }

            // Get current key
            if (_appSettings != null)
            {
                Object o = null;
                try
                {
                    o = _appSettings[keyName];
                }
                catch (NotSupportedException ex)
                {
                    error = ex.Message + Constants.NewLine
                        + "In config file key name \"" + keyName + "\" not exist.";
                }

                if (o != null)
                    value = o.ToString();

                if (String.IsNullOrEmpty(value) && mandatory && !String.IsNullOrEmpty(error))
                    error = "In config file key name \"" + keyName + "\" not exist.";
            }

            if (!String.IsNullOrEmpty(error))
                throw new ArgumentException(error);

            return value;
        }

        #endregion
    }
}
