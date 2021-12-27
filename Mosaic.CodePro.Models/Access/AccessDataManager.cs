using Mosaic.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models.Access
{
    public partial class AccessDataManager : Notifier
    {
        public string FilePath { get; set; }

        private AccessDataManager() { }
        public AccessDataManager(string filePath)
        {
            FilePath = filePath;
        }

        //public static DbConnection GetConnection()
        //{
        //    //return new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=..\\Test.mdb");
        //}

        public AccessInstance GetInstance()
        {
            try
            {
                DataTable userTables = GetTables();
                if (userTables == null)
                {
                    throw new Exception("Tables returned null;");
                }
                var instance = new AccessInstance(FilePath);
                instance.Tables = new List<IDataBaseTable>();
                instance.Name = new FileInfo(FilePath).Name.Split('.')[0];
                
                for (int i = 0; i < userTables.Rows.Count; i++)
                    instance.Tables.Add(new AccessInstance.AccessTable()
                    {
                        Name = userTables.Rows[i][2].ToString(),
                        Columns = new List<IDatabaseColumn>(),
                        IsSelected = false,
                        DatabaseName = instance.Name,
                        ServerName = instance.Name
                    });

                foreach (var tbl in instance.Tables)
                {
                    tbl.Columns = GetColumnsForTableByName(tbl.Name);
                }

                return instance;
            }
            catch (Exception e)
            {
                OnError(e);
                return null;
            }
            
        }

        public DataTable GetTables()
        {
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

                DataTable userTables = null;

                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={FilePath}";//$"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {FilePath}";

                    // We only want user tables, not system tables
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";

                    connection.Open();

                    // Get list of user tables
                    userTables = connection.GetSchema("Tables", restrictions);
                }
                return userTables;
            }
            catch (Exception e)
            {
                OnError(e);
                return null;
            }
           
        }

        public List<string> GetTableNames()
        {
            // Add list of table names to list
            var userTables = GetTables();
            List<string> results = new List<string>();
            for (int i = 0; i < userTables.Rows.Count; i++)
                results.Add(userTables.Rows[i][2].ToString());

            return results;
        }

        public DataTable GetColumnsByTableName(string tableName)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            using (DbConnection conn = factory.CreateConnection())
            {
                conn.Open();

                DbCommand command = conn.CreateCommand();
                // (1) we're not interested in any data
                command.CommandText = $"select * from {tableName} where 1 = 0";
                command.CommandType = CommandType.Text;

                DbDataReader reader = command.ExecuteReader();
                // (2) get the schema of the result set
                DataTable schemaTable = reader.GetSchemaTable();

                conn.Close();
                return schemaTable;
            }
        }

        public List<IDatabaseColumn> GetColumnsForTableByName(string tableName)
        {
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
            using (DbConnection conn = factory.CreateConnection())
            {
               

                DbCommand command = conn.CreateCommand();
                conn.ConnectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={FilePath}";//$"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = {FilePath}";
                // (1) we're not interested in any data
                command.CommandText = $"select * from {tableName} where 1 = 0";
                command.CommandType = CommandType.Text;
                command.Connection = conn;
                conn.Open();

                DbDataReader reader = command.ExecuteReader();
                // (2) get the schema of the result set
                DataTable schemaTable = reader.GetSchemaTable();

                conn.Close();

                List<IDatabaseColumn> results = new List<IDatabaseColumn>();
                foreach (DataRow row in schemaTable.Rows)
                {
                    results.Add(new DatabaseColumn()
                    {
                        DataType = row.Field<Type>("DataType").Name,
                        Name = row.Field<string>("ColumnName"),
                       // row.Field<int>("ColumnSize"));
                    });
                }
                
                return results;
            }

            
        }
    }
}
