using SensibleProgramming.CodePro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.Data.SQL
{
    /// <summary>
    /// An instance of an SQL Server
    /// </summary>
    public class SQLInstance 
    {
        /// <summary>
        /// The name of the SQL server
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// The Database in the server
        /// </summary>
        public List<SQLServerDataBase> DataBases { get; set; } = new List<SQLServerDataBase>();

        public SQLServerDataBase SelectedDataBase { get; set; } = new SQLServerDataBase();

        /// <summary>
        /// A database in a sql server
        /// </summary>
        public class SQLServerDataBase : BaseDataBaseContainer
        {
            /// <summary>
            /// The stored procedures in the db
            /// </summary>
            public List<SQLDataBaseStoredProcedure> StoredProcedures { get; set; } = new List<SQLDataBaseStoredProcedure>();

            public SQLServerDataBase()
            {
                Tables = new List<IDataBaseTable>();
            }

            /// <summary>
            /// A SQL data table in a database
            /// </summary>
            public class SQLDataBaseTable : BaseDataBaseTable
            {
                public SQLDataBaseTable() { ServerName = ""; }
                
            }

            /// <summary>
            /// A databases stored procedure
            /// </summary>
            public class SQLDataBaseStoredProcedure
            {
                /// <summary>
                /// The name of the procedure
                /// </summary>
                public string Name { get; set; }

                public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
                /// <summary>
                /// Is it selected
                /// </summary>
                public bool IsSelected { get; set; }
            }

        }


    }

    
}
