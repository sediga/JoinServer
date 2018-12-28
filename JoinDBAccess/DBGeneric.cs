using System.Diagnostics.CodeAnalysis;
namespace DataAccessLayer
{
    /// <summary>
    /// This is class implement factory for single instances.
    /// Implement parrern Singleton
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    internal class GenericSingleton<T> where T : class, new()
    {
        private static T _instance;
        private static bool _isNewInstance;

        /// <summary>
        /// If first time create instance - true otherwise false
        /// </summary>
        [SuppressMessageAttribute("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public bool IsNewInstance
        {
            get { return _isNewInstance; }
        }

        /// <summary>
        /// Property return instance
        /// </summary>
        [SuppressMessageAttribute("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public T Instance
        {
            get
            {
                _isNewInstance = _instance == null;

                if (_instance == null)
                    _instance = new T();

                // Thred safe instance
                lock (_instance) { return _instance; }
            }
        }
    }
}
