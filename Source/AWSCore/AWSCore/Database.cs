using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace AWSCore
{
    using Enums;
    using Interfaces;
    using System.Threading.Tasks;

    public class Database : BaseService, IDatabase
    {
        private AmazonDynamoDBClient m_client;

        public Database(Credentials credentials) : base("AmazonDatabase", ServiceTypes.Database)
        {
            m_client = new AmazonDynamoDBClient(credentials.PublicKey, credentials.SecretKey, RegionEndpoint.USEast2);
        }

        public async Task<ListTablesResponse> GetTables()
        {
            ListTablesRequest request = new ListTablesRequest();
            ListTablesResponse response = await m_client.ListTablesAsync(request);
            return response;
        }

        public async Task<CreateTableResponse> CreateTable(string tableName, List<KeySchemaElement> schema, List<AttributeDefinition> attributeDefinitions)
        {
            ProvisionedThroughput throughput = new ProvisionedThroughput(5, 5);
            CreateTableRequest request = new CreateTableRequest(tableName, schema, attributeDefinitions, throughput);
            CreateTableResponse response = await m_client.CreateTableAsync(request);
            return response;
        }

        public async Task<DeleteTableResponse> DeleteTable(string tableName)
        {
            DeleteTableRequest request = new DeleteTableRequest(tableName);
            DeleteTableResponse response = await m_client.DeleteTableAsync(request);
            return response;
        }

        public async Task<PutItemResponse> InsertData(string tableName, Dictionary<string, AttributeValue> values)
        {
            PutItemRequest request = new PutItemRequest(tableName, values);
            PutItemResponse response = await m_client.PutItemAsync(request);
            return response;
        }

        public async Task<TableInfo> GetTableInfo(string tableName)
        {
            DescribeTableRequest request = new DescribeTableRequest(tableName);
            DescribeTableResponse response = await m_client.DescribeTableAsync(request);
            var item = response.Table.KeySchema.FirstOrDefault(x => x.KeyType == KeyType.HASH);

            string pk = string.Empty;

            if (item != null)
            {
                pk = item.AttributeName;
            }

            var valueType = response.Table.AttributeDefinitions.FirstOrDefault(x => x.AttributeName == pk);

            ScalarAttributeType scalar = ScalarAttributeType.S;
            if (valueType != null)
            {
                scalar = valueType.AttributeType;
            }

            TableInfo ti = new TableInfo(pk, scalar, response.Table.KeySchema.ToList());
            return ti;
        }

        public async Task<GetItemResponse> GetData(string tableName, Dictionary<string, AttributeValue> keys)
        {
            GetItemRequest request = new GetItemRequest(tableName, keys);
            GetItemResponse response = await m_client.GetItemAsync(request);
            return response;
        }
    }
}
