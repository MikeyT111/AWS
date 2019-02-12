using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSCore
{
    using Interfaces;

    public class DatabaseHandler : IHandler
    {
        private bool m_shouldContinue = false;
        private IDatabase m_database;

        public DatabaseHandler(IDatabase database)
        {
            m_database = database;
        }

        public async Task Display()
        {
            m_shouldContinue = true;

            while (m_shouldContinue)
            {
                Console.WriteLine("Commands:");
                Console.WriteLine("1: Display Databases");
                Console.WriteLine("2: Create Database");
                Console.WriteLine("3: Delete Database");
                Console.WriteLine("4: Insert Into Database");
                Console.WriteLine("5: Get data from Database");

                Console.WriteLine("9: Exit");

                string option = Utilities.DisplayMessageAndGetStringResult("Select Option:");

                switch (option)
                {
                    case "1":
                        await Database_Display();
                        break;
                    case "2":
                        await Database_Create();
                        break;
                    case "3":
                        await Database_Delete();
                        break;
                    case "4":
                        await Database_Insert();
                        break;
                    case "5":
                        await Database_Get();
                        break;
                    case "9":
                        m_shouldContinue = false;
                        break;
                    default:
                        Console.WriteLine("Invalid commmand");
                        break;
                }
            }
        }

        private async Task Database_Display()
        {
            Console.WriteLine("Available Tables: ");

            var tablenames = await GetTableNames();

            foreach (var item in tablenames)
            {
                Console.WriteLine("Table name: " + item);
            }

            Console.WriteLine("");
        }

        private async Task<List<string>> GetTableNames()
        {
            var tables = await m_database.GetTables();

            return tables.TableNames.ToList();
        }

        private async Task Database_Create()
        {
            Console.WriteLine("Enter new table name:");

            string tableName = Console.ReadLine();

            var TableNames = await GetTableNames();
            if (!TableNames.Any(x => x == tableName))
            {
                List<KeySchemaElement> schema = new List<KeySchemaElement>();
                List<AttributeDefinition> definitions = new List<AttributeDefinition>();

                Console.WriteLine("Enter the primary key name: e.g. Author (or empty to finish)");
                string attributeName = Console.ReadLine();

                KeyType kt = GetKeyType();
                ScalarAttributeType scalarType = getScalarType();

                schema.Add(new KeySchemaElement(attributeName, kt));

                Console.WriteLine("Enter the sort key name: e.g. Author (or empty to finish)");
                string SortKey = Console.ReadLine();

                if (string.IsNullOrEmpty(SortKey))
                {
                    ScalarAttributeType scalarType1 = getScalarType();
                    KeyType kt1 = GetKeyType();
                    schema.Add(new KeySchemaElement(SortKey, kt1));
                }

                definitions.Add(new AttributeDefinition(attributeName, scalarType));

                var result = await m_database.CreateTable(tableName, schema, definitions);
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("Table name already exists.");
            }
        }

        private KeyType GetKeyType()
        {
            Console.WriteLine("Enter the Key type: e.g. Hash/Range)");
            string attributeKey = Console.ReadLine();

            KeyType key = KeyType.RANGE;

            if (!string.IsNullOrEmpty(attributeKey))
            {
                if (attributeKey.ToLower() == "hash")
                {
                    key = KeyType.HASH;
                }
            }

            return key;
        }

        private ScalarAttributeType getScalarType()
        {
            Console.WriteLine("Enter the type: e.g. s/n/b");
            string attributeType = Console.ReadLine();
            ScalarAttributeType scalarType = ScalarAttributeType.S;

            if (attributeType == "n")
                scalarType = ScalarAttributeType.N;
            else if (attributeType == "b")
                scalarType = ScalarAttributeType.B;

            return scalarType;
        }

        private Dictionary<string, AttributeValue> AddNewAttribute()
        {
            Dictionary<string, AttributeValue> data = new Dictionary<string, AttributeValue>();

            string attName = Utilities.DisplayMessageAndGetStringResult("Enter attribute name:");

            List<string> values = new List<string>();

            bool shouldContinue = true;

            while (shouldContinue)
            {
                string attValue = Utilities.DisplayMessageAndGetStringResult("Enter value");
                values.Add(attValue);

                shouldContinue = Utilities.DisplayMessageAndGetBoolResult("Do you want to add more values? YES/NO");
            }

            if (values.Count > 1)
            {
                data.Add(attName, new AttributeValue(values));
            }
            else
            {
                data.Add(attName, new AttributeValue(values[0]));
            }

            return data;
        }

        private async Task Database_Delete()
        {
            await Database_Display();

            var tableNames = await GetTableNames();

            Console.WriteLine("Select the table name you want to delete:");
            string tableName = Console.ReadLine();

            bool tableExists = tableNames.Any(x => x == tableName);

            if (tableExists)
                await m_database.DeleteTable(tableName);
            else
                Console.WriteLine("Table doesn't exist");
        }

        private async Task Database_Insert()
        {
            await Database_Display();

            string tableName = Utilities.DisplayMessageAndGetStringResult("Select the table name you want to push data to:");

            var primaryKey = await m_database.GetTableInfo(tableName);
            Console.WriteLine("The primary key type is: " + primaryKey.PKType);

            string primaryKeyValue = Utilities.DisplayMessageAndGetStringResult("Enter the value you want to insert:");

            if (!string.IsNullOrEmpty(primaryKey.PKName))
            {
                AttributeValue atv = new AttributeValue();

                if (primaryKey.PKType == "N")
                {
                    // Convert this to a int brah.
                    atv.N = primaryKeyValue;
                }
                else if (primaryKey.PKType == "B")
                {
                    // Convert this shiz to binary bro. 
                }
                else
                {
                    atv.S = primaryKeyValue;
                }

                // Step 1. 
                // Set the primary key value.
                Dictionary<string, AttributeValue> dict = new Dictionary<string, AttributeValue>();
                dict.Add(primaryKey.PKName, atv);

                // Step 2.
                // Create a temp list... we may not even use it. 
                List<string> values = new List<string>();

                bool shouldContinue = true;

                // Step 3. 
                // Loop round as many times the user wants to add extra data to 
                while (shouldContinue)
                {
                    // Ask whether the user wasnts to add extra data.
                    bool addAgain = Utilities.DisplayMessageAndGetBoolResult("Do you want to add another item: YES/NO");

                    if (addAgain)
                    {
                        // Yes they do.
                        var f = AddNewAttribute();
                        f.ToList().ForEach(x => dict.Add(x.Key, x.Value));
                    }
                    else
                    {
                        // No they don't.
                        shouldContinue = false;
                    }
                }
                await m_database.InsertData(tableName, dict);
            }
        }

        private async Task Database_Get()
        {
            await Database_Display();
            string tableName = Utilities.DisplayMessageAndGetStringResult("Enter database name you want to get the data from.");

            var tableData = await m_database.GetTableInfo(tableName);

            Console.WriteLine("Available columns:");
            tableData.Keys.ForEach(x => Console.WriteLine(x.AttributeName));

            Dictionary<string, AttributeValue> ats = new Dictionary<string, AttributeValue>();

            foreach (var item in tableData.Keys)
            {
                string value = Utilities.DisplayMessageAndGetStringResult("Enter the value you want to query against: " + item.AttributeName);

                bool isKeyNumeric = Utilities.DisplayMessageAndGetBoolResult("Is the key numeric? YES/NO");
                if (isKeyNumeric)
                    ats.Add(item.AttributeName, new AttributeValue() { N = value });
                else
                    ats.Add(item.AttributeName, new AttributeValue() { S = value });
            }

            var result = await m_database.GetData(tableName, ats);

            if (result.IsItemSet)
            {
                foreach (var item in result.Item)
                {
                    Console.WriteLine(item.Key);

                    if (item.Value.N != null)
                        Console.WriteLine(item.Value.N);

                    if (item.Value.S != null)
                        Console.WriteLine(item.Value.S);

                    if (item.Value.SS != null)
                        item.Value.SS.ForEach(x => Console.WriteLine(x));

                }
            }
            else
            {

            }
        }

    }
}