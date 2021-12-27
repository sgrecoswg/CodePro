using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models
{
    public interface IDataBaseTable
    {
        /// <summary>
        /// The name of the server the dabase that holds the table in on.
        /// </summary>
        string ServerName { get; set; }

        string DatabaseName { get; set; }

        /// <summary>
        /// The name of the table
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Is the table selected?
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// List of available tables in the db
        /// </summary>
        List<IDatabaseColumn> Columns { get; set; }
    }

    public interface IDatabaseColumn
    {
        string Name { get; set; }
        string DataType { get; set; }
    }

    public abstract partial class BaseDataBaseTable : IDataBaseTable
    {
        public string ServerName { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is the table selected?
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// List of available tables in the db
        /// </summary>
        public List<IDatabaseColumn> Columns { get; set; }

        public BaseDataBaseTable()
        {
            Columns = new List<IDatabaseColumn>();
        }
    }

    public partial class DatabaseColumn : IDatabaseColumn
    {
        public string Name { get; set; }
        public string DataType { get; set; }
    }

    public abstract partial class BaseDataBaseContainer
    {
        /// <summary>
        /// The tables in the database
        /// </summary>
        public virtual List<IDataBaseTable> Tables { get; set; }

        /// <summary>
        /// The name of database
        /// </summary>
        public string Name { get; set; }
    }
}
