using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRelationshipManagement.CXS.ProductManagement
{
    /// <summary>
    /// Excel导出工具类，支持通过Func选择器自定义每列内容
    /// </summary>
    public static class ExcelExportHelper
    {
        /// <summary>
        /// 通用导出Excel方法（Func选择器版）
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="data">要导出的数据列表</param>
        /// <param name="headers">表头（列名）</param>
        /// <param name="propertySelectors">每列对应的属性选择器（Func）</param>
        /// <param name="sheetName">工作表名称，默认Sheet1</param>
        /// <returns>Excel文件字节数组</returns>
        public static byte[] ExportToExcel<T>(
            List<T> data,
            List<string> headers,
            List<Func<T, object>> propertySelectors,
            string sheetName = "Sheet1")
        {
            // 创建Excel工作簿
            IWorkbook workbook = new XSSFWorkbook();
            // 创建工作表
            ISheet sheet = workbook.CreateSheet(sheetName);

            // 创建表头行
            var headerRow = sheet.CreateRow(0);
            for (int i = 0; i < headers.Count; i++)
            {
                // 设置每一列的标题
                headerRow.CreateCell(i).SetCellValue(headers[i]);
            }

            // 填充数据行
            for (int i = 0; i < data.Count; i++)
            {
                // 创建新行
                var row = sheet.CreateRow(i + 1);
                for (int j = 0; j < propertySelectors.Count; j++)
                {
                    // 通过Func选择器获取每一列的值
                    var value = propertySelectors[j](data[i]);
                    // 写入单元格，若为null则写空字符串
                    row.CreateCell(j).SetCellValue(value?.ToString() ?? "");
                }
            }

            // 写入内存流并返回字节数组
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);      // 将Excel内容写入内存流
                return ms.ToArray();     // 返回字节数组
            }
        }
    }
}
