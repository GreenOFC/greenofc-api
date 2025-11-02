using _24hplusdotnetcore.Common.Attributes;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace _24hplusdotnetcore.Services
{
    public interface IExcelImportService
    {
        IEnumerable<T> DrawSheetToObjects<T>(ExcelWorksheet worksheet, int startRow = 2, bool isConvertDatetime = true) where T : new();
    }

    public static class EPPLusExtensions
    {
        public static IEnumerable<T> DrawSheetToObjects<T>(this ExcelImportService excelImport, ExcelWorksheet worksheet, int startRow = 2) where T : new()
        {
            return excelImport.DrawSheetToObjects<T>(worksheet, startRow);
        }

        public static bool IsLastRowEmpty(this ExcelWorksheet worksheet)
        {
            var empties = new List<bool>();

            for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
            {
                var rowEmpty = worksheet.Cells[worksheet.Dimension.End.Row, i].Value == null ? true : false;
                empties.Add(rowEmpty);
            }

            return empties.All(e => e);
        }

        public static ExcelWorksheet TrimLastEmptyRows(this ExcelWorksheet worksheet)
        {
            while (worksheet.IsLastRowEmpty())
                worksheet.DeleteRow(worksheet.Dimension.End.Row);

            return worksheet;
        }
    }

    public class ExcelImportService : IExcelImportService, IScopedLifetime
    {
        private readonly ILogger<ExcelImportService> _logger;

        public ExcelImportService(ILogger<ExcelImportService> logger)
        {
            _logger = logger;
        }
        public IEnumerable<T> DrawSheetToObjects<T>(ExcelWorksheet worksheet, int startRow = 2, bool isConvertDatetime = true) where T : new()
        {

            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(Column);

            var columns = typeof(T).GetProperties()
                .Where(x => x.CustomAttributes.Any(columnOnly))
                .Select(p => new
                {
                    Property = p,
                    Column = p.GetCustomAttributes<Column>().First().ColumnIndex,
                    DateFormat = p.GetCustomAttributes<DateFormat>().FirstOrDefault()?.Format
                }).ToList();

            var rows = worksheet.TrimLastEmptyRows().Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            var collection = rows.Skip(startRow).Select(row =>
            {
                var tnew = new T();
                columns.ForEach(col =>
                {
                    try
                    {

                        var val = worksheet.Cells[row, col.Column];
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }
                        if (col.Property.PropertyType == typeof(short))
                        {
                            col.Property.SetValue(tnew, val.GetValue<short>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(short?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<short?>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(int))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(int?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int?>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(long))
                        {
                            col.Property.SetValue(tnew, val.GetValue<long>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(long?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<long?>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(float))
                        {
                            col.Property.SetValue(tnew, val.GetValue<float>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(float?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<float?>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(double))
                        {
                            col.Property.SetValue(tnew, val.GetValue<double>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(double?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<double?>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(decimal))
                        {
                            col.Property.SetValue(tnew, val.GetValue<decimal>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(decimal?))
                        {
                            col.Property.SetValue(tnew, val.GetValue<decimal?>());
                            return;
                        }


                        if (col.Property.PropertyType == typeof(DateTime) || col.Property.PropertyType == typeof(DateTime?))
                        {
                            if (isConvertDatetime)
                            {
                                var value = val.Text;

                                char[] splilit = new char[3] { '/', '-', '.' };
                                var formatedDate = value.Split(splilit);

                                if (formatedDate.Length == 2)
                                {
                                    value = formatedDate[0].PadLeft(2, '0') + "/" + formatedDate[1];
                                }

                                if (formatedDate.Length == 3)
                                {
                                    value = formatedDate[0] + "/" + formatedDate[1].PadLeft(2, '0') + "/" + formatedDate[2];
                                }

                                var format = col.DateFormat != null ? col.DateFormat.Split(",")
                                        : new string[] { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy" };

                                var validDate = DateTime
                                                .TryParseExact(value, format,
                                                    CultureInfo.InvariantCulture,
                                                    DateTimeStyles.None,
                                                    out DateTime dt);

                                if (!validDate)
                                {
                                    col.Property.SetValue(tnew, null);
                                }

                                col.Property.SetValue(tnew, dt);
                                return;
                            }
                        }
                        col.Property.SetValue(tnew, val.Text);
                    }
                    catch (System.Exception ex)
                    {
                        _logger.LogError(ex, ex.Message);
                        throw;
                    }
                });

                return tnew;
            });

            return collection;
        }
    }
}
