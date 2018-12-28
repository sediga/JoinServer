using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace DataAccessLayer
{
    /// <summary>
    /// Implement DataLayerException
    /// </summary>
    [Serializable]
    public class DataLayerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DataLayerException class.
        /// </summary>
        public DataLayerException() : base() { }

        /// <summary>
        /// Initializes a new instance of the DataLayerException class.
        /// </summary>
        /// <param name="message">Error message text</param>
        public DataLayerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the DataLayerException class.
        /// </summary>
        /// <param name="info">Serialization information</param>
        /// <param name="context">Error contex</param>
        protected DataLayerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Initializes a new instance of the DataLayerException class.
        /// </summary>
        /// <param name="message">Error message text</param>
        /// <param name="innerException">Inner exception content</param>
        public DataLayerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
