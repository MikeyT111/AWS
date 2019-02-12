using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AWSCore.Interfaces
{
    public interface IDatabase
    {
        /// <summary>
        /// Function used to get all tables.
        /// </summary>
        /// <returns></returns>
        Task<ListTablesResponse> GetTables();

        /// <summary>
        /// Functioned used to create a table.
        /// </summary>
        /// <param name="tableName">The name of the table to create.</param>
        /// <param name="schema">The schema that we want to use.</param>
        /// <param name="attributeDefinitions">The atteributes for the table.</param>
        /// <returns></returns>
        Task<CreateTableResponse> CreateTable(string tableName, List<KeySchemaElement> schema, List<AttributeDefinition> attributeDefinitions);

        /// <summary>
        /// Function used to delete a table.
        /// </summary>
        /// <param name="tableName">The name of the table to delete.</param>
        /// <returns>The result of deleting a table.</returns>
        Task<DeleteTableResponse> DeleteTable(string tableName);

        /// <summary>
        /// Function used to insert data into a table.
        /// </summary>
        /// <param name="tableName">The name of the table to insert the data into.</param>
        /// <param name="values">The values to insert the data to.</param>
        /// <returns></returns>
        Task<PutItemResponse> InsertData(string tableName, Dictionary<string, AttributeValue> values);

        /// <summary>
        /// Function used to get the information about the table.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns></returns>
        Task<TableInfo> GetTableInfo(string tableName);

        /// <summary>
        /// Function used to get data from the table.
        /// </summary>
        /// <param name="tableName">The name of the table to get the data from.</param>
        /// <param name="keys">The key information for the query.</param>
        /// <returns></returns>
        Task<GetItemResponse> GetData(string tableName, Dictionary<string, AttributeValue> keys);

    }
}
