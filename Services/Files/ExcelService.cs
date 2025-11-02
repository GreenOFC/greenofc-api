using _24hplusdotnetcore.Common.Attributes;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _24hplusdotnetcore.Services.Files
{
    public interface IExcelService
    {
        byte[] Generate<T>(string templatePath, IList<T> models, string workSheetName = null) where T : new();
    }

    public class ExcelService: IExcelService, IScopedLifetime
    {
        public byte[] Generate<T>(string templatePath, IList<T> models, string workSheetName = null) where T : new()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (FileStream templateStream = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream newStream = new MemoryStream())
                {
                    using (var excel = new ExcelPackage(newStream, templateStream))
                    {
                        var ws = string.IsNullOrEmpty(workSheetName) ? excel.Workbook.Worksheets[0] : excel.Workbook.Worksheets.FirstOrDefault(x => x.Name == workSheetName);

                        foreach (var prop in typeof(T).GetProperties())
                        {
                            foreach (var attr in prop.GetCustomAttributes(true))
                            {
                                if (attr is ExportAttribute exportAttribute)
                                {
                                    var query = from cell in ws.Cells["A:XFD"]
                                                where cell.Value?.ToString().Contains(exportAttribute.ExportName) == true
                                                select cell;

                                    foreach (var cell in query)
                                    {
                                        for (int i = 0; i < models.Count; i++)
                                        {
                                            ws.Cells[cell.End.Row + i, cell.End.Column].Value = GetAttributeValue(models[i], prop, exportAttribute);
                                        }
                                    }
                                }
                            }
                        }
                        excel.Save();
                    }
                    return newStream.ToArray();
                }
            }
        }

        private object GetAttributeValue<TAttribute, T>(T model, PropertyInfo propertyInfo, TAttribute attribute) where TAttribute : ExportAttribute
        {
            object value = propertyInfo.GetValue(model);

            if (value == null || attribute == null)
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format) && value is IFormattable)
            {
                return (value as IFormattable).ToString(attribute.Format, CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(attribute.Format))
            {
                return string.Format(attribute.Format, value);
            }

            return propertyInfo.GetValue(model);
        }
    }
}
