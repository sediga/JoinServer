using System;
using System.Data;
using System.Reflection;

namespace DataAccessLayer
{
    #region Strictures
    /// <summary>
    /// Implement EntiryStructureData
    /// </summary>
    public class EntityStructureData
    {
        /// <summary>
        /// The position of the field in RowSet
        /// </summary>
        public int FieldPosition
        { get; private set; }
        
        /// <summary>
        /// Property info for entity field
        /// </summary>
        public PropertyInfo PropertyInfo
        { get; private set; }

        /// <summary>
        /// Initializes a new instance of the EntiryStructureData class.
        /// </summary>
        /// <param name="fieldPosition">The position of the field in RowSet</param>
        /// <param name="propertyInfo">Property info for entity field</param>
        public EntityStructureData(int fieldPosition, PropertyInfo propertyInfo)
        {
            FieldPosition = fieldPosition;
            PropertyInfo = propertyInfo;
        }
    }
    #endregion
}
