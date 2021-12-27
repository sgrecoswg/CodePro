using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.Xml;

namespace Mosaic.Core.Extensions
{
    public static class NPOIExcelExtensions
    {

        public static T To<T>(this IWorkbook workBook) where T : class, new()
        {
            var type = typeof(T);
            if (type.Equals(typeof(DataSet)))
            {
                DataSet ds = new DataSet();
                for (int i = 0, len = workBook.NumberOfSheets; i < len; i++)
                {
                    if (!workBook.IsSheetHidden(i) && !workBook.IsSheetVeryHidden(i))
                    {
                        //ds.Tables.Add(XlsxTableToDT(((XSSFSheet)workBook.GetSheetAt(i)).GetTables()[0]));
                        ds.Tables.Add(XlsxToDT(workBook.GetSheetAt(i)));
                    }
                }
                return ds as T;
            }
            else if (type.Equals(typeof(DataTable)))
            {
                DataTable dt = XlsxToDT(workBook.GetSheetAt(0));
                return dt as T;
            }
            else if (type.Equals(typeof(XmlDocument)))
            {
                DataSet ds = workBook.To<DataSet>();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(ds.GetXml());
                ds.Dispose();
                return xml as T;
            }

            return default(T);
        }

        public static T To<T>(this ISheet sheet) where T : class, new()
        {
            var type = typeof(T);

            if (type.Equals(typeof(DataSet)))
            {
                DataSet ds = new DataSet();
                DataTable newTable = XlsxToDT(sheet);
                if (newTable.Rows.Count > 0)
                {
                    ds.Tables.Add(newTable);
                }

                return ds as T;
            }
            else if (type.Equals(typeof(DataTable)))
            {
                DataTable dt = XlsxToDT(sheet);

                return dt as T;
            }
            else if (type.Equals(typeof(XmlDocument)))
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException("Sorry I don't know how to convert that to that type.");
        }

        public static T To<T>(this XSSFTable tbl) where T : class, new()
        {
            var type = typeof(T);

            if (type.Equals(typeof(DataTable)))
            {
                DataTable dt = XlsxTableToDT(tbl);

                return dt as T;
            }
            else if (type.Equals(typeof(XmlDocument)))
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException("Sorry I don't know how to convert that to that type.");
        }

        static DataTable XlsxToDT(ISheet sheet)
        {
            try
            {
                DataTable dt = new DataTable
                {
                    TableName = sheet.SheetName
                };

                IRow headerRow = sheet.GetRow(0);
                var rows = sheet.GetRowEnumerator();

                int colCount = headerRow.LastCellNum;
                int rowCount = sheet.LastRowNum;

                for (int c = 0; c < colCount; c++)
                {
                    var cell = headerRow.GetCell(c);
                    dt.Columns.Add(cell.ToString());
                }

                while (rows.MoveNext())
                {
                    IRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < colCount; i++)
                    {
                        ICell cell = row.GetCell(i);
                        if (cell != null)
                        {
                            GetCellValue(sheet, dr, i, cell);
                        }
                        else
                        {
                            dr[i] = string.Empty;
                        }
                    }

                    dt.Rows.Add(dr);
                }
                return dt;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        static DataTable XlsxTableToDT(XSSFTable tbl)
        {
            try
            {


                var startCell = tbl.GetStartCellReference();
                int startingRow = startCell.Row;
                int startingCol = startCell.Col;

                var endcell = tbl.GetEndCellReference();
                int rowCount = endcell.Row + 1;
                int colCount = endcell.Col + 1;

                ISheet sheet = tbl.GetXSSFSheet();
                IRow headerRow = sheet.GetRow(startingRow);
                IRow udfRow = sheet.GetRow(startingRow - 1);

                DataTable dt = new DataTable
                {
                    TableName = sheet.SheetName
                };

                for (int c = 0; c < colCount; c++)
                {
                    var cell = headerRow.GetCell(c);
                    switch (cell.CellType)
                    {
                        case CellType.Unknown:
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownColumn_{c}", typeof(string));
                            break;
                        case CellType.Numeric:                           
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownNumericColumn_{c}", typeof(double));
                            break;
                        case CellType.String:                           
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownNumericColumn_{c}", typeof(string));
                            break;
                        case CellType.Formula:
                            //dr[i] = cell?.StringCellValue ?? string.Empty;
                            //IFormulaEvaluator _eval = sheet.Workbook.GetCreationHelper().CreateFormulaEvaluator();
                            //var value = _eval.Evaluate(cell);
                            //GetFormulaValue(value, dr, i);
                            break;
                        case CellType.Blank:
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownNumericColumn_{c}", typeof(string));
                            // dr.Table.Columns[i].DataType = typeof(string);
                            //dr[i] = string.Empty;
                            break;
                        case CellType.Boolean:
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownNumericColumn_{c}", typeof(bool));
                            //  dr.Table.Columns[i].DataType = typeof(bool);
                            //dr[i] = cell?.BooleanCellValue ?? false;
                            break;
                        case CellType.Error:
                            //dr[i] = $"Error! CellValue => {cell.ErrorCellValue.ToString()}";
                            break;
                        default:
                            dt.Columns.Add(cell?.StringCellValue ?? $"UnknownNumericColumn_{c}", typeof(string));
                            // dr.Table.Columns[i].DataType = typeof(string);
                            //dr[i] = string.Empty;
                            break;
                    }                    
                }

                //var rows = sheet.GetRowEnumerator();

                //for (int i = startingRow + 1; i < rowCount; i++)
                //{
                //    var row = sheet.GetRow(i);
                //    var dr = dt.NewRow();
                //    for (int c = 0; c < colCount; c++)
                //    {
                //        ICell cell = row.GetCell(c);
                //        if (cell != null)
                //        {
                //            GetCellValue(sheet, dr, c, cell);
                //        }
                //        else
                //        {
                //            dr[c] = string.Empty;
                //        }
                //    }
                //    dt.Rows.Add(dr);
                //}

                return dt;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public static DataTable SkipColumns(this DataTable dt, Func<DataColumn, bool> f)
        {
            foreach (DataColumn c in dt.Columns)
            {
                if (f(c))
                {
                    dt.Columns.Remove(c);
                }
            }

            return dt;
        }

        private static void GetCellValue(ISheet sheet, DataRow dr, int i, ICell cell)
        {
            switch (cell.CellType)
            {
                case CellType.Unknown:
                   // dr.Table.Columns[i].DataType = typeof(string);
                    dr[i] = "Unknown Cell Type";
                    break;
                case CellType.Numeric:
                   // dr.Table.Columns[i].DataType = typeof(double);

                    //if (dr.Table.Columns[i].ColumnName.ToLower() == "start date" ||
                    //    dr.Table.Columns[i].ColumnName.ToLower() == "end date" ||
                    //    dr.Table.Columns[i].ColumnName.ToLower() == "end time")
                    //{
                    //    dr[i] = Convert.ToDateTime(cell?.DateCellValue ?? default(DateTime));
                    //}
                    //else
                    //{
                    dr[i] = Convert.ToDouble(cell?.NumericCellValue ?? default(double));
                    //}
                    break;
                case CellType.String:
                    //dr.Table.Columns[i].DataType = typeof(string);
                    dr[i] = cell?.StringCellValue ?? string.Empty;
                    break;
                case CellType.Formula:
                    dr[i] = cell?.StringCellValue ?? string.Empty;
                    //IFormulaEvaluator _eval = sheet.Workbook.GetCreationHelper().CreateFormulaEvaluator();
                    //var value = _eval.Evaluate(cell);
                    //GetFormulaValue(value, dr, i);
                    break;
                case CellType.Blank:
                   // dr.Table.Columns[i].DataType = typeof(string);
                    dr[i] = string.Empty;
                    break;
                case CellType.Boolean:
                  //  dr.Table.Columns[i].DataType = typeof(bool);
                    dr[i] = cell?.BooleanCellValue ?? false;
                    break;
                case CellType.Error:
                    dr[i] = $"Error! CellValue => {cell.ErrorCellValue.ToString()}";
                    break;
                default:
                   // dr.Table.Columns[i].DataType = typeof(string);
                    dr[i] = string.Empty;
                    break;
            }
        }

        static void GetFormulaValue(CellValue value, DataRow dr, int i)
        {
            switch (value.CellType)
            {
                case CellType.Unknown:
                    dr[i] = string.Empty;
                    break;
                case CellType.Numeric:
                    dr[i] = Convert.ToInt32(value?.NumberValue ?? default(int));
                    break;
                case CellType.String:
                    dr[i] = value?.StringValue ?? string.Empty;
                    break;
                case CellType.Formula:
                    break;
                case CellType.Blank:
                    dr[i] = string.Empty;
                    break;
                case CellType.Boolean:
                    dr[i] = value?.BooleanValue ?? false;
                    break;
                case CellType.Error:
                    dr[i] = $"Error! CellValue => {value.ErrorValue.ToString()}";
                    break;
                default:
                    dr[i] = string.Empty;
                    break;
            }
        }
    }
}
