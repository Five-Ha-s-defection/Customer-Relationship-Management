using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

public static class ExportHelper
{
    // 对象集合转DataTable（只导出指定字段）
    public static DataTable ToDataTable<T>(IEnumerable<T> data, List<string> fields)
    {
        var dt = new DataTable();
        foreach (var field in fields)
        {
            dt.Columns.Add(field);
        }
        foreach (var item in data)
        {
            var row = dt.NewRow();
            foreach (var field in fields)
            {
                var value = item?.GetType().GetProperty(field)?.GetValue(item, null);
                row[field] = value ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    // DataTable转Excel流
    public static MemoryStream DataTableToExcel(DataTable dt)
    {
        var workbook = new XSSFWorkbook();
        var sheet = workbook.CreateSheet("Sheet1");
        // 表头
        var header = sheet.CreateRow(0);
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            header.CreateCell(i).SetCellValue(dt.Columns[i].ColumnName);
        }
        // 数据
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var row = sheet.CreateRow(i + 1);
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                row.CreateCell(j).SetCellValue(dt.Rows[i][j]?.ToString());
            }
        }
        var ms = new MemoryStream();
        workbook.Write(ms);
        ms.Position = 0;
        return ms;
    }
}