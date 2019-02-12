using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWSCore
{
    /// <summary>
    /// Class used to store information about the table.
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        /// <param name="type">The type of table.</param>
        /// <param name="keys">The keys for the table.</param>
        public TableInfo(string name, ScalarAttributeType type, List<KeySchemaElement> keys)
        {
            PKName = name;
            PKType = type;
            Keys = keys;
        }

        /// <summary>
        /// The primary key name.
        /// </summary>
        public string PKName { get; }

        /// <summary>
        /// The primary key type.
        /// </summary>
        public ScalarAttributeType PKType { get; }

        /// <summary>
        /// The Keys of the table.
        /// </summary>
        public List<KeySchemaElement> Keys { get; }
    }
}
