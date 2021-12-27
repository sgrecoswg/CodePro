using Mosaic.Core.Extensions.Validation;
using Mosaic.Core.Models;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Mosaic.Core.Extensions
{
    public class ExcelConverter : Notifier
    {
        #region prop/flds

        /// <summary>
        /// The full file path of the file we are going to convert.
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// The current book we are converting
        /// </summary>
        IWorkbook CurrentWorkbook { get; set; }

        #endregion

        #region  cnstr
        ExcelConverter(string path)
        {
            FilePath = Path.GetFullPath(path);
        }

        /// <summary>
        /// creates a converter for us that will convert a file at the specifed path to another type.
        /// </summary>
        /// <param name="path">the full path of the file we are going to convert.</param>
        /// <returns></returns>
        public static ExcelConverter Create(string path)
        {
            if (Validate.File.IsExcelFile(path))
            {
                return new ExcelConverter(path);
            }
            else
            {
                return null;
            }
        }

        public static async Task<ExcelConverter> CreateAsync(string path)
        {
            return await Task.Factory.StartNew(() => {
                return Create(path);
            });

        }

        #endregion

        /// <summary>
        /// Converts the excel file into another object.
        /// </summary>
        /// <typeparam name="T">The class we are going to convert to.</typeparam>
        /// <returns></returns>
        public virtual T Convert<T>() where T : class, new()
        {
            CurrentWorkbook = WorkbookFactory.Create(FilePath);
            var type = typeof(T);
            if (type.Equals(typeof(DataSet)))
            {
                DataSet ds = new DataSet();
                try
                {
                    for (int i = 0, len = CurrentWorkbook.NumberOfSheets; i < len; i++)
                    {
                        if (!CurrentWorkbook.IsSheetHidden(i) && !CurrentWorkbook.IsSheetVeryHidden(i))
                        {
                            XSSFSheet sheet = (XSSFSheet)CurrentWorkbook.GetSheetAt(i);

                            var sheetTables = sheet.GetTables();
                            if (sheetTables?.Count > 0)
                            {
                                ds.Tables.Add(sheetTables[0].To<DataTable>());
                            }
                            else
                            {
                                throw new ExcelSheetInvalidException($"Sheet {sheet.SheetName} does not have any tables to process.");
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    CurrentWorkbook.Close();
                }
                return ds as T;
            }
            else if (type.Equals(typeof(DataTable)))
            {
                DataTable dt = new DataTable();
                if (CurrentWorkbook.NumberOfSheets > 1)
                {
                    //????
                }
                try
                {
                    XSSFSheet sheet = (XSSFSheet)CurrentWorkbook.GetSheetAt(0);
                    dt = sheet.GetTables()[0].To<DataTable>();
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    CurrentWorkbook.Close();
                }
                return dt as T;
            }
            else if (type.Equals(typeof(XmlDocument)))
            {
                DataSet ds = CurrentWorkbook.To<DataSet>();
                XmlDocument xml = new XmlDocument();
                try
                {
                    xml.LoadXml(ds.GetXml());
                    ds.Dispose();
                }
                catch (Exception)
                {
                    throw;
                }
                finally { CurrentWorkbook.Close(); }
                return xml as T;
            }
            else if (false)
            {
                //CurrentWorkbook.Close();
                //??????
            }
            CurrentWorkbook.Close();
            return default(T);
        }

        public async Task<T> ConvertAsync<T>() where T : class, new()
        {
            return await Task.Factory.StartNew(() => {
                return Convert<T>();
            });
        }
    }

   
}
