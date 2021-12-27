using SensibleProgramming.Core.Extensions;
using SensibleProgramming.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models.Excel
{
    public partial class ExcelDataManager : Notifier
    {
        public string FilePath { get; set; }

        private ExcelDataManager() { }
        public ExcelDataManager(string filePath)
        {
            FilePath = filePath;
        }

        public async Task<ExcelInstance> GetInstance()
        {
            try
            {
                var converter = await ExcelConverter.CreateAsync(FilePath);
                var ds = await converter.ConvertAsync<DataSet>();
                
                if (ds == null)
                {
                    throw new Exception("Converter returned a null DataSet;");
                }

                var instance = new ExcelInstance(FilePath);
                instance.Tables = new List<IDataBaseTable>();
                instance.Name = new FileInfo(FilePath).Name.Split('.')[0];

                foreach (DataTable tbl in ds.Tables)
                {
                    var newTbl = new ExcelInstance.ExcelTable()
                    {
                        Name = tbl.TableName,

                        IsSelected = false,
                        DatabaseName = instance.Name,
                        ServerName = instance.Name
                    };

                    for (int i = 0; i < tbl.Columns.Count; i++)
                        newTbl.Columns = (from DataColumn col in tbl.Columns
                                          select new DatabaseColumn()
                                          {
                                              Name = col.ColumnName,
                                              DataType = col.DataType?.Name ?? "string"
                                          } as IDatabaseColumn).ToList();

                    instance.Tables.Add(newTbl);
                }

                return instance;
            }
            catch (Exception e)
            {
                OnError(e);
                return null;
            }

        }
    }
}
