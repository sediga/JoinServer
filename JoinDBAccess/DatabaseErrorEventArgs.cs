using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    /// <summary>
    /// Structure Database error Event arguments
    /// </summary>
    public class DatabaseErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Containing error text
        /// </summary>
        private string _errorText;

        /// <summary>
        /// Containing error text
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }
        }

        /// <summary>
        /// Initializes a new instance of the DatabaseErrorEventArgs class.
        /// </summary>
        /// <param name="errorText"></param>
        public DatabaseErrorEventArgs(string errorText)
        {
            _errorText = errorText;
        }
    }
}
