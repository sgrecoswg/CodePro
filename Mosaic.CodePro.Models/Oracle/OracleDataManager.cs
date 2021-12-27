using Mosaic.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models.Oracle
{
    public partial class OracleInstance : BaseDataBaseContainer
    {

        public OracleInstance() { Tables = new List<IDataBaseTable>(); }


    }

    public class OracleTable : BaseDataBaseTable
    {

    }

    public partial class OracleDataManager : Notifier
    {
        public OracleDataManager() { }

        public OracleInstance GetInstance(string servername,string dbName)
        {
            
                DataTable SchemaTable;
                OracleInstance result = new OracleInstance();
                // used to hold a list of views and tables
                var arrViews = new List<string>();
                var arrTables = new List<string>();

            try
            {
                using (OleDbConnection conn = new OleDbConnection($"Provider=MSDAORA;DataSource={servername};UserId=ois;Password=ois_owner;"))
                {
                   
                        // open the connection to the database
                        conn.Open();

                        // Get the Tables
                        SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new Object[] { null, null, null, "TABLE" });

                        // Store the table names in the class scoped array list of table names
                        for (int i = 0; i < SchemaTable.Rows.Count; i++)
                        {
                            result.Tables.Add(new OracleTable()
                            {
                                Name = SchemaTable.Rows[i]["Table_Name"].ToString(),
                                DatabaseName = "",
                                IsSelected = false,
                                ServerName = "",
                                Columns = GetColumnsByTableName(SchemaTable.Rows[i]["Table_Name"].ToString(), conn)
                            });

                            arrTables.Add(SchemaTable.Rows[i].ItemArray[2].ToString());
                        }

                        // Get the Views
                        //SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new  Object[] { null, null, null, "VIEW" });
                        //
                        //// Store the view names in the class scoped array
                        //// list of view names
                        //for (int i = 0; i < SchemaTable.Rows.Count; i++)
                        //{
                        //    arrViews.Add(SchemaTable.Rows[i].
                        //    ItemArray[2].ToString());
                        //}
                        return result;
                   
                }
            }
            catch (Exception ex)
            {

                OnError(ex);
                return null;
            }

        }

        /*
         column names

        OracleDataAdapter adap = new OracleDataAdapter("select column_name from user_tab_columns where table_name = '" + selTbl + "' order by column_id", connection);
        DataTable dt = new DataTable();
        adap.Fill(dt);
        foreach (DataRow row In dt.Rows)
        {lstColumns.Items.Add(row("column_name"));}
        }

        How can I programmatically get the name of the Oracle database I am connecting to? I tried:
        using (OracleConnection connection = new OracleConnection(oraConnectStr))
            {
                connection.Open();
                return connection.Database;
            }

         SELECT NAME FROM v$database;
         */


       
        public OracleInstance StoreTableAndViewNames(string servername)
        {
            // temporary holder for the schema information for the current database connection
            DataTable SchemaTable;
            OracleInstance result = new OracleInstance();
            // used to hold a list of views and tables
            var arrViews = new List<string>();
            var arrTables = new List<string>();

            
            using (OleDbConnection conn = new  OleDbConnection($"Data Source={servername};User Id=myUsername;Password=myPassword;"))
                {
                    try
                    {
                        // open the connection to the database
                        conn.Open();

                        // Get the Tables
                        SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new  Object[] { null, null, null, "TABLE" });

                        // Store the table names in the class scoped array list of table names
                        for (int i = 0; i < SchemaTable.Rows.Count; i++)
                        {
                            result.Tables.Add(new OracleTable()
                            {
                                Name = SchemaTable.Rows[i]["Table_Name"].ToString(),
                                DatabaseName = "",
                                IsSelected = false,
                                ServerName = "",
                                Columns = GetColumnsByTableName(SchemaTable.Rows[i]["Table_Name"].ToString(), conn)
                            });

                            arrTables.Add(SchemaTable.Rows[i].ItemArray[2].ToString());
                        }

                    // Get the Views
                    //SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new  Object[] { null, null, null, "VIEW" });
                    //
                    //// Store the view names in the class scoped array
                    //// list of view names
                    //for (int i = 0; i < SchemaTable.Rows.Count; i++)
                    //{
                    //    arrViews.Add(SchemaTable.Rows[i].
                    //    ItemArray[2].ToString());
                    //}
                    return result;
                    }
                    catch (Exception ex)
                    {
                    
                        OnError(ex);
                    return null;
                    }
                }
            }

        private List<IDatabaseColumn> GetColumnsByTableName(string name, OleDbConnection conn)
        {
            var results = new List<IDatabaseColumn>();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                DataTable dtField =  conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new
                object[] { null, null, name });

                foreach (DataRow dr in dtField.Rows)
                {
                    results.Add(new DatabaseColumn() { Name = dr["COLUMN_NAME"].ToString(), DataType = "" });
                }
            }
            catch (Exception ex)
            {

                OnError(ex);
            }

            return results;
        }
    }
}
