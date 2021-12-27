using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;

namespace Mosaic.Data.SQL
{
    public class SQLManager
    {
        #region SQL
        /// <summary>
        /// Gets the instances of SQL on the Network.
        /// </summary>
        /// <returns>System.Collections.Generic.List(String)</returns>
        public List<String> GetInstancesOfSQL()
        {
            List<String> instances = new List<String>();

            using (DataTable SQLSources = SqlDataSourceEnumerator.Instance.GetDataSources())
            {
                foreach (DataRow source in SQLSources.Rows)
                {
                    instances.Add(source["ServerName"].ToString() + @"\" + source["InstanceName"]);
                }
            }
            instances.Sort();
            return instances;
        }
               
        /// <summary>
        /// Gets the DataBases from SQL.
        /// </summary>
        /// <param name="dataBaseServer">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>         
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetDataBases(string dataBaseServer)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(@"Data Source=" + dataBaseServer + ";Integrated Security=True"))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                dt = con.GetSchema("Databases");

                return dt;
            }
        }

        /// <summary>
        /// Gets the DataBases from SQL.
        /// </summary>
        /// <param name="dataBaseServer">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>         
        /// <returns>System.Collections.Generic.List(String)</returns>
        public List<string> GetListOfDataBasesFromSQL(string databaseServer)
        {
            DataTable dt = new DataTable();
            List<string> returnList = new List<string>();

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseServer + ";Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    dt = con.GetSchema("Databases");

                    foreach (DataRow item in dt.Rows)
                    {
                        string tableName = item.ItemArray[0].ToString();
                        if (tableName != "master" &&
                            tableName != "tempdb" &&
                            tableName != "model" &&
                            tableName != "msdb" &&
                            !tableName.Contains("ReportServer"))
                        {
                            returnList.Add(tableName);
                        }

                    }
                    returnList.Sort();
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }

        /// <summary>
        /// Gets the Tables from SQL by the DataBase ,Server , and Table Name.
        /// </summary>
        /// <param name="dataBaseServer">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>       
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetDataTables(string databaseEngine, string database)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                   "Initial Catalog=" + database + ";" +
                                                                   "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    dt = con.GetSchema("Tables");
                    return dt;
                }
            }
            catch (Exception)
            {
                dt.Rows.Add("Error");
                return dt;
            }
        }

        /// <summary>
        /// Gets the Tables from SQL by the DataBase ,Server , and Table Name.
        /// </summary>
        /// <param name="dataBaseServer">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>       
        /// <returns>List of strings</returns>
        public List<String> GetListOfDataTablesFromSQL(string dataBaseServer, string database)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + dataBaseServer + ";" +
                                                                   "Initial Catalog=" + database + ";" +
                                                                   "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    dt = con.GetSchema("Tables");
                    foreach (DataRow item in dt.Rows)
                    {
                        returnList.Add(item.ItemArray[2].ToString());
                    }

                    returnList.Sort();
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }

        /// <summary>
        /// Gets the Table Schema from SQL by the DataBase ,Server , and Table Name.
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <param name="table">DataBase Table in the Data Base </param>
        /// <returns>System.Data.DataTable</returns>
        public DataTable GetSQLDataTableSchema(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                             "Initial Catalog=" + database + ";" +
                                                             "Integrated Security=True"))
            {
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }



                SqlCommand cmd = new SqlCommand("select * from " + table, con);
                dt = cmd.ExecuteReader().GetSchemaTable();
            }

            return dt;

        }

        /// <summary>
        /// Gets the Table Schema from SQL by the DataBase ,Server , and Table Name.
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <param name="table">DataBase Table in the Data Base </param>
        /// <returns>List of Strings</returns>
        public List<String> GetListOfDataTableSchemasFromSQL(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("select * from " + table, con);
                    dt = cmd.ExecuteReader().GetSchemaTable();

                    foreach (DataRow item in dt.Rows)
                    {
                        returnList.Add(item.ItemArray[0].ToString());
                    }
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }

        /// <summary>
        /// Gets a List of Properties with type from Table in SQL
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <param name="table">DataBase Table in the Data Base</param>
        /// <returns></returns>
        public List<String> GetTablePropertiesFromSQL(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();
            List<ClassProperty> lProps = new List<ClassProperty>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("select * from " + table, con);
                    dt = cmd.ExecuteReader().GetSchemaTable();

                    foreach (DataRow item in dt.Rows)
                    {
                        ClassProperty prop = new ClassProperty();
                        prop.Type = ClassProperty.GetCSharpType(item);
                        prop.Name = item.ItemArray[0].ToString();
                        lProps.Add(prop);

                    }

                    foreach (ClassProperty p in lProps)
                    {
                        returnList.Add(p.Type + " " + p.Name);
                    }
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }

        public string GetClassFromSql(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            string result = string.Empty;
            List<ClassProperty> lProps = new List<ClassProperty>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    

                    SqlCommand cmd = new SqlCommand(@"
                    declare @Result varchar(max) = 'public partial class ' + @TableName + ': I' + @TableName + '
    {'
    
    select @Result = @Result + '
        public virtual ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }
    '
    from
    (
        select 
            replace(col.name, ' ', '_') ColumnName,
            column_id ColumnId,
            case typ.name 
                when 'bigint' then 'long'
                when 'binary' then 'byte[]'
                when 'bit' then 'bool'
                when 'char' then 'string'
                when 'date' then 'DateTime'
                when 'datetime' then 'DateTime'
                when 'datetime2' then 'DateTime'
                when 'datetimeoffset' then 'DateTimeOffset'
                when 'decimal' then 'decimal'
                when 'float' then 'double'
                when 'image' then 'byte[]'
                when 'int' then 'int'
                when 'money' then 'decimal'
                when 'nchar' then 'string'
                when 'ntext' then 'string'
                when 'numeric' then 'decimal'
                when 'nvarchar' then 'string'
                when 'real' then 'float'
                when 'smalldatetime' then 'DateTime'
                when 'smallint' then 'short'
                when 'smallmoney' then 'decimal'
                when 'text' then 'string'
                when 'time' then 'TimeSpan'
                when 'timestamp' then 'long'
                when 'tinyint' then 'byte'
                when 'uniqueidentifier' then 'Guid'
                when 'varbinary' then 'byte[]'
                when 'varchar' then 'string'
                else 'UNKNOWN_' + typ.name
            end ColumnType,
            case 
                when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier') 
                then '?' 
                else '' 
            end NullableSign
        from sys.columns col
            join sys.types typ on
                col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
        where object_id = object_id(@TableName)
    ) t
    order by ColumnId
    
    set @Result = @Result  + '
    }'
                    
                    select @Result", con);
                    cmd.Parameters.Add(new SqlParameter("@TableName", table));
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {
                
                return exc.Message;
            }
        }

        public string GetDTOClassFromSql(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            string result = string.Empty;
            List<ClassProperty> lProps = new List<ClassProperty>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }



                    SqlCommand cmd = new SqlCommand(@"
                    	declare @Result varchar(max) = '[Table(""'+@TableName+'"")]
                    
      public partial class ' + @TableName + 'Entity : I' + @TableName + '
      {'
          select @Result = @Result + '
                  /// <summary>
                  /// '+@TableName+'.'+ColumnName+'
                  /// </summary>'+
          
                  (case when RequiredField = 0 then '
                  [Required]' else '' end)+
          
                  '
                  [Column(""'+ColumnName+'"")]
          
                  public virtual ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }
          '
          from
          (
              select
                  replace(col.name, ' ', '_') ColumnName,
                  column_id ColumnId,
                  case typ.name
                      when 'bigint' then 'long'
                      when 'binary' then 'byte[]'
                      when 'bit' then 'bool'
                      when 'char' then 'string'
                      when 'date' then 'DateTime'
                      when 'datetime' then 'DateTime'
                      when 'datetime2' then 'DateTime'
                      when 'datetimeoffset' then 'DateTimeOffset'
                      when 'decimal' then 'decimal'
                      when 'float' then 'double'
                      when 'image' then 'byte[]'
                      when 'int' then 'int'
                      when 'money' then 'decimal'
                      when 'nchar' then 'string'
                      when 'ntext' then 'string'
                      when 'numeric' then 'decimal'
                      when 'nvarchar' then 'string'
                      when 'real' then 'float'
                      when 'smalldatetime' then 'DateTime'
                      when 'smallint' then 'short'
                      when 'smallmoney' then 'decimal'
                      when 'text' then 'string'
                      when 'time' then 'TimeSpan'
                      when 'timestamp' then 'long'
                      when 'tinyint' then 'byte'
                      when 'uniqueidentifier' then 'Guid'
                      when 'varbinary' then 'byte[]'
                      when 'varchar' then 'string'
                      else 'UNKNOWN_' + typ.name
                  end ColumnType,
                  case 
                      when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier')
                      then '?' 
                      else '' 
                  end NullableSign,
                  col.is_nullable RequiredField
              from sys.columns col
                  join sys.types typ on
                      col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
              where object_id = object_id(@TableName)
          ) t
          order by ColumnId
          
          set @Result = @Result + '
          
     }'
                                        
                    select @Result", con);
                    cmd.Parameters.Add(new SqlParameter("@TableName", table));
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {

                return exc.Message;
            }
        }

        public string GetViewModelClassFromSql(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            string result = string.Empty;
            List<ClassProperty> lProps = new List<ClassProperty>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }



                    SqlCommand cmd = new SqlCommand(@"
                    	declare @Result varchar(max) = 'public partial class ' + @TableName + 'ViewModel : I' + @TableName + '
      {'
          select @Result = @Result + '
                  /// <summary>
                  /// '+@TableName+'.'+ColumnName+'
                  /// </summary>'+
          
                  (case when RequiredField = 0 then '
                  [Required]' else '' end)+
          
                  '
                  [Display(Name=""'+ColumnName+'"")]
                  '+TypeAttribute+'          
                  public virtual ' + ColumnType + NullableSign + ' ' + ColumnName + ' { get; set; }
          '
          from
          (
              select
                  replace(col.name, ' ', '_') ColumnName,
                  column_id ColumnId,
                  case typ.name
                      when 'bigint' then 'long'
                      when 'binary' then 'byte[]'
                      when 'bit' then 'bool'
                      when 'char' then 'string'
                      when 'date' then 'DateTime'
                      when 'datetime' then 'DateTime'
                      when 'datetime2' then 'DateTime'
                      when 'datetimeoffset' then 'DateTimeOffset'
                      when 'decimal' then 'decimal'
                      when 'float' then 'double'
                      when 'image' then 'byte[]'
                      when 'int' then 'int'
                      when 'money' then 'decimal'
                      when 'nchar' then 'string'
                      when 'ntext' then 'string'
                      when 'numeric' then 'decimal'
                      when 'nvarchar' then 'string'
                      when 'real' then 'float'
                      when 'smalldatetime' then 'DateTime'
                      when 'smallint' then 'short'
                      when 'smallmoney' then 'decimal'
                      when 'text' then 'string'
                      when 'time' then 'TimeSpan'
                      when 'timestamp' then 'long'
                      when 'tinyint' then 'byte'
                      when 'uniqueidentifier' then 'Guid'
                      when 'varbinary' then 'byte[]'
                      when 'varchar' then 'string'
                      else 'UNKNOWN_' + typ.name
                  end ColumnType,
                  case typ.name 
                                when 'bigint' then '[Range(0,1000)]'
                                when 'binary' then '[DataType(DataType.Upload)]'
                                when 'char' then'[StringLength(255, MinimumLength = 1)]'
                                when 'date' then '[DataType(DataType.Date)]'
                                when 'datetime' then '[DataType(DataType.Date)]'
                                when 'datetime2' then '[DataType(DataType.Date)]'
                                when 'datetimeoffset' then '[DataType(DataType.Date)]'
                                when 'image' then '[DataType(DataType.Upload)]'
                                when 'money' then '[DataType(DataType.Currency)]'
                                when 'nchar' then '[StringLength(255, MinimumLength = 1)]'
                                when 'ntext' then '[StringLength(255, MinimumLength = 1)]'                                
                                when 'nvarchar' then '[StringLength(255, MinimumLength = 1)]'                                
                                when 'smalldatetime' then '[DataType(DataType.Date)]'                                
                                when 'smallmoney' then '[DataType(DataType.Currency)]'
                                when 'text' then '[StringLength(255, MinimumLength = 1)]'
                                when 'time' then '[DataType(DataType.Date)]'
                                when 'timestamp' then '[DataType(DataType.Time)]'
                                when 'varbinary' then '[DataType(DataType.Upload)]'
                                when 'varchar' then '[StringLength(255, MinimumLength = 1)]'
                                else ''
                            end TypeAttribute,
                  case 
                      when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier')
                      then '?' 
                      else '' 
                  end NullableSign,
                  col.is_nullable RequiredField
              from sys.columns col
                  join sys.types typ on
                      col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
              where object_id = object_id(@TableName)
          ) t
          order by ColumnId
          
          set @Result = @Result + '
          
     }'
                                        
                    select @Result", con);
                    cmd.Parameters.Add(new SqlParameter("@TableName", table));
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {

                return exc.Message;
            }
        }

        public string GetJSONClassFromSql(string databaseEngine, string database, string table)
        {
            DataTable dt = new DataTable();
            string result = string.Empty;
            List<ClassProperty> lProps = new List<ClassProperty>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand(@"
                    declare @Result varchar(max) = 'var ' + @TableName + ' = {'
                    
                    select @Result = @Result + '
                '  + ColumnName + ' : ' + DefaultData + ',//' + ColumnType + NullableSign +'
                    '
                    from
                    (
                        select 
                            replace(col.name, ' ', '_') ColumnName,
                            column_id ColumnId,
                            case typ.name 
                                when 'bigint' then 'long'
                                when 'binary' then 'byte[]'
                                when 'bit' then 'bool'
                                when 'char' then 'string'
                                when 'date' then 'DateTime'
                                when 'datetime' then 'DateTime'
                                when 'datetime2' then 'DateTime'
                                when 'datetimeoffset' then 'DateTimeOffset'
                                when 'decimal' then 'decimal'
                                when 'float' then 'double'
                                when 'image' then 'byte[]'
                                when 'int' then 'int'
                                when 'money' then 'decimal'
                                when 'nchar' then 'string'
                                when 'ntext' then 'string'
                                when 'numeric' then 'decimal'
                                when 'nvarchar' then 'string'
                                when 'real' then 'float'
                                when 'smalldatetime' then 'DateTime'
                                when 'smallint' then 'short'
                                when 'smallmoney' then 'decimal'
                                when 'text' then 'string'
                                when 'time' then 'TimeSpan'
                                when 'timestamp' then 'long'
                                when 'tinyint' then 'byte'
                                when 'uniqueidentifier' then 'Guid'
                                when 'varbinary' then 'byte[]'
                                when 'varchar' then 'string'
                                else 'UNKNOWN_' + typ.name
                            end ColumnType,
                            case typ.name 
                                when 'bigint' then '0'
                                when 'binary' then '[]'
                                when 'bit' then 'false'
                                when 'char' then'""""'
                                when 'date' then 'new Date()'
                                when 'datetime' then 'new Date()'
                                when 'datetime2' then 'new Date()'
                                when 'datetimeoffset' then 'new Date()'
                                when 'decimal' then '0.0'
                                when 'float' then '0.000'
                                when 'image' then 'new Image()'
                                when 'int' then '0'
                                when 'money' then '0.00'
                                when 'nchar' then'""""'
                                when 'ntext' then'""""'
                                when 'numeric' then '0.0'
                                when 'nvarchar' then'""""'
                                when 'real' then '0.000'
                                when 'smalldatetime' then 'new Date()'
                                when 'smallint' then '0'
                                when 'smallmoney' then '0'
                                when 'text' then'""'
                                when 'time' then 'new Date()'
                                when 'timestamp' then '0'
                                when 'tinyint' then '0'
                                when 'uniqueidentifier' then 'Guid.New()'
                                when 'varbinary' then '[]'
                                when 'varchar' then'""""'
                                else 'UNKNOWN_' + typ.name
                            end DefaultData,
                            case 
                                when col.is_nullable = 1 and typ.name in ('bigint', 'bit', 'date', 'datetime', 'datetime2', 'datetimeoffset', 'decimal', 'float', 'int', 'money', 'numeric', 'real', 'smalldatetime', 'smallint', 'smallmoney', 'time', 'tinyint', 'uniqueidentifier') 
                                then '?' 
                                else '' 
                            end NullableSign
                        from sys.columns col
                            join sys.types typ on
                                col.system_type_id = typ.system_type_id AND col.user_type_id = typ.user_type_id
                        where object_id = object_id(@TableName)
                    ) t
                    order by ColumnId
                    
                    set @Result = @Result  + '
            }'
                    
                    select @Result", con);
                    cmd.Parameters.Add(new SqlParameter("@TableName", table));
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                    return result;
                }
            }
            catch (Exception exc)
            {

                return exc.Message;
            }
        }

        /// <summary>
        /// Gets the Stored Procedures from SQL by the DataBase and Server Name.
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <returns>List of Strings</returns>
        public List<String> GetListOfStoredProceduresFromSQL(string databaseEngine, string database)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                              "Initial Catalog=" + database + ";" +
                                                              "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    dt = con.GetSchema("Procedures");

                    foreach (DataRow item in dt.Rows)
                    {
                        string stp = item.ItemArray[2].ToString();
                        GetSPTypes(con, stp);
                        returnList.Add(stp);
                    }
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }

        }

        public Dictionary<string, string> GetStoredProcedureParameters(string databaseEngine, string database,string storedProcedure)
        {

            DataTable dt = new DataTable();
            Dictionary<string, string> results = new Dictionary<string, string>();
            //throw new NotImplementedException();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                                "Initial Catalog=" + database + ";" +
                                                                "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    string script = File.ReadAllText(@"C:\Dev\git\Mosaic.CodePro\Mosaic.CodePro.WPF\SQLScripts\parambuilder.sql");
                    script = script.Replace("~tablename~", storedProcedure);
                    SqlCommand cmd = new SqlCommand(script, con);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        results.Add(reader.GetString(2), reader.GetString(3));
                    }
                    return results;
                }
            }
            catch (Exception e)
            {
                results.Add("Error",e.Message);
                return results;
            }
        }

        /// <summary>
        /// Gets the Properties that are shared with other Tables in the DataBase
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <returns></returns>
        public List<String> GetSharedProperties(string databaseEngine, string dataBase)
        {
            List<String> shareProps = new List<String>();
            DataTable dtTables = GetDataTables(databaseEngine, dataBase);
            string tableNameInTable = dtTables.Rows[0].ItemArray[2].ToString();
            int nextCount;

            var mainResult = (from e in GetTablePropertiesFromSQL(databaseEngine, dataBase, tableNameInTable).AsEnumerable()
                              select e);

            var TempResult = mainResult;

            for (int i = 0; i < dtTables.Rows.Count; i++)
            {
                nextCount = i + 1;
                if (nextCount <= (dtTables.Rows.Count - 1))
                {
                    string nextTableName = dtTables.Rows[nextCount].ItemArray[2].ToString();

                    var result = (from t
                                  in GetTablePropertiesFromSQL(databaseEngine, dataBase, nextTableName).AsEnumerable()
                                  select t);
                    if (TempResult == null)
                    {
                        TempResult = mainResult.Except(result);
                    }
                    else
                    {
                        TempResult = mainResult.Intersect(result);
                        mainResult = TempResult;
                    }
                }
            }

            shareProps = mainResult.ToList();
            return shareProps;
        }

        public List<string> GetConstraints(string databaseEngine, string database)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=" + databaseEngine + ";" +
                                                              "Initial Catalog=" + database + ";" +
                                                              "Integrated Security=True"))
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    dt = con.GetSchema("TABLE_CONSTRAINTS");

                    foreach (DataRow item in dt.Rows)
                    {
                        string stp = item.ItemArray[2].ToString();
                        GetSPTypes(con, stp);
                        returnList.Add(stp);
                    }
                    return returnList;
                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }

        /// <summary>
        /// Gets the Properties that are not shared with other Tables in the DataBase
        /// </summary>
        /// <param name="databaseEngine">Server that the DataBase is on (i.e "ComputerName\Sql2008")</param>
        /// <param name="database">Name of the Data Base.</param>
        /// <returns></returns>
        public List<String> GetNonSharedProperties(string databaseEngine, string dataBase)
        {
            List<String> nonShareProps = new List<String>();
            DataTable dtTables = GetDataTables(databaseEngine, dataBase);
            string tableNameInTable = dtTables.Rows[0].ItemArray[2].ToString();
            int nextCount;

            var mainExResult = (from e in GetTablePropertiesFromSQL(databaseEngine, dataBase, tableNameInTable).AsEnumerable()
                                select e);
            var TempExResult = mainExResult;

            for (int i = 0; i < dtTables.Rows.Count; i++)
            {
                nextCount = i + 1;
                if (nextCount <= (dtTables.Rows.Count - 1))
                {
                    string nextTableName = dtTables.Rows[nextCount].ItemArray[2].ToString();

                    var result = (from t
                                  in GetTablePropertiesFromSQL(databaseEngine, dataBase, nextTableName).AsEnumerable()
                                  select t);
                    TempExResult = mainExResult.Except(result);
                    mainExResult = TempExResult;
                }
            }
            nonShareProps = mainExResult.ToList();
            return nonShareProps;
        }

        /// <summary>
        /// Creates a connectionstring based on the provider and table name
        /// </summary>
        /// <param name="dbProvider">DataProvider Enum  (i.e. </param>
        /// <param name="sDataBaseName"></param>
        /// <returns>string</returns>
        public static string CreateConnectionString(DataProvider dbProvider, string sDataBaseName = "", string sDataSourceOrPath = "")
        {
            string newConnectionString;
            switch (dbProvider)
            {
                case DataProvider.Odbc:
                    newConnectionString =
                        "Driver={SQL Native Client};" +
                        "Server=" + sDataSourceOrPath + ";" +
                        "Trusted_Connection=Yes;" +
                        "Database=" + sDataBaseName + ";";
                    break;
                case DataProvider.Access:
                    OleDbConnectionStringBuilder oledbBuilder = new OleDbConnectionStringBuilder();
                    oledbBuilder["Provider"] = "Microsoft.ACE.OLEDB.12.0";
                    oledbBuilder["Data Source"] = sDataSourceOrPath;
                    newConnectionString = oledbBuilder.ConnectionString;
                    break;
                case DataProvider.OleDb:
                    newConnectionString =
                        "Provider=Microsoft.Jet.OLEDB.4.0;" +
                        "Data Source=" + sDataBaseName +
                        ";Extended Properties=dBase IV";
                    break;
                case DataProvider.SqlClient:
                    newConnectionString =
                        "Data Source=" + sDataSourceOrPath + ";" +
                        "Initial Catalog=" + sDataBaseName + ";" +
                        "Integrated Security=True";
                    break;
                default:
                    newConnectionString = "";
                    break;
            }
            return newConnectionString;
        }
        #endregion

        public enum DataProvider
        {
            Odbc, OleDb, SqlClient, Access
        }
        /// <summary>
        /// Gets a Provider for the selected datasource
        /// </summary>
        /// <param name="dProvider">DataProvider Enum</param>
        /// <returns>string (i.e "System.Data.SqlClient")</returns>
        public static string GetProvider(DataProvider dProvider)
        {
            string provider;

            switch (dProvider)
            {
                case DataProvider.Odbc:
                    return provider = "System.Data.Odbc";
                    break;
                case DataProvider.OleDb:
                    return provider = "System.Data.OleDb";
                    break;
                case DataProvider.SqlClient:
                    return provider = "System.Data.SqlClient";
                    break;
                case DataProvider.Access:
                    return provider = "System.Data.OleDb";
                    break;
                default:
                    return provider = "System.Data.SqlClient";
                    break;
            }
        }

        public List<String> GetSPTypes(SqlConnection con, string sp)
        {
            DataTable dt = new DataTable();
            List<String> returnList = new List<String>();

            try
            {
                using (con)
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    int returnValue;


                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteReader();



                    returnValue = (int)(cmd.Parameters["retvalue"].Value);
                    returnList.Add("Return Value = " + returnValue.ToString());

                    return returnList;

                    //      using (SqlConnection connection = new SqlConnection(
                    //connectionString))
                    //      {
                    //          SqlCommand command = new SqlCommand(queryString, connection);
                    //          connection.Open();
                    //          SqlDataReader reader =
                    //              command.ExecuteReader(CommandBehavior.CloseConnection);
                    //          while (reader.Read())
                    //          {
                    //              Console.WriteLine(String.Format("{0}", reader[0]));
                    //          }
                    //      }

                }
            }
            catch (Exception)
            {
                returnList.Add("Error");
                return returnList;
            }
        }
    }

    public class ClassProperty
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public static string GetCSharpType(DataRow dr)
        {
            bool isNullable = Convert.ToBoolean(dr["AllowDBNull"]);
            string type = dr["DataType"].ToString();

            switch (dr["DataType"].ToString())
            {
                case "System.Int16":
                    if (isNullable)
                    {
                        return "Nullable<short>";
                    }
                    else
                    {
                        return "short";
                    }
                case "System.Int32":
                    if (isNullable)
                    {
                        return "Nullable<int>";
                    }
                    else
                    {
                        return "int";
                    }
                case "System.String":
                    return "string";
                case "System.DateTime":
                    return "DateTime";
                case "System.Byte":
                    if (isNullable)
                    {
                        return "Nullable<byte>";
                    }
                    else
                    {
                        return "byte";
                    }
                case "System.Decimal":
                    if (isNullable)
                    {
                        return "Nullable<Decimal>";
                    }
                    else
                    {
                        return "Decimal";
                    }
                case "System.Byte[]":
                    return "Binary";
                case "System.Boolean":
                    if (isNullable)
                    {
                        return "Nullable<bool>";
                    }
                    else
                    {
                        return "bool";
                    }

                case "System.GUID":
                    if (isNullable)
                    {
                        return "Nullable<GUID>";
                    }
                    else
                    {
                        return "GUID";
                    }
                case "System.Guid":
                    if (isNullable)
                    {
                        return "Nullable<Guid>";
                    }
                    else
                    {
                        return "Guid";
                    }
                default:
                    return "dynamic";
                //throw new Exception("Type not known");
            }
        }
    }
}
