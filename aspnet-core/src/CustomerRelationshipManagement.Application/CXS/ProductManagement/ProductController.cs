//using CustomerRelationshipManagement.ProductManagementDto;
using CustomerRelationshipManagement.Products;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace CustomerRelationshipManagement.CXS.ProductManagement
{
    public class ProductController: ControllerBase
    {
        private readonly IRepository<Product> productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        ///// <summary>
        ///// 导出所有产品信息为Excel文件
        ///// </summary>
        ///// <returns>Excel文件流</returns>
        //[HttpGet]
        //[Route("api/product")]
        //public async Task<IActionResult> ExportProductsAsyncExcel()
        //{
        //    // 从数据库查询所有产品信息
        //    var products = await productRepository.GetListAsync();

        //    // 创建一个新的Excel工作簿
        //    IWorkbook workbook = new XSSFWorkbook();
        //    // 在工作簿中创建一个名为“产品信息”的工作表
        //    ISheet sheet = workbook.CreateSheet("产品信息");

        //    // 创建表头行，并设置每一列的标题
        //    var headerRow = sheet.CreateRow(0);
        //    headerRow.CreateCell(0).SetCellValue("产品分类");
        //    headerRow.CreateCell(1).SetCellValue("父级分类ID");
        //    headerRow.CreateCell(2).SetCellValue("产品图片");
        //    headerRow.CreateCell(3).SetCellValue("门幅");
        //    headerRow.CreateCell(4).SetCellValue("供应商");
        //    headerRow.CreateCell(5).SetCellValue("产品编号");
        //    headerRow.CreateCell(6).SetCellValue("产品描述");
        //    headerRow.CreateCell(7).SetCellValue("建议售价");
        //    headerRow.CreateCell(8).SetCellValue("备注");
        //    headerRow.CreateCell(9).SetCellValue("上架状态");
        //    headerRow.CreateCell(10).SetCellValue("成交价");

        //    // 遍历产品列表，将每个产品的信息写入Excel表格
        //    for (int i = 0; i < products.Count; i++)
        //    {
        //        var row = sheet.CreateRow(i + 1); // 数据从第2行开始
        //        var p = products[i];
        //        row.CreateCell(0).SetCellValue(p.CategoryId.ToString());
        //        row.CreateCell(1).SetCellValue(p.ParentId.ToString());
        //        row.CreateCell(2).SetCellValue(p.ProductImageUrl);
        //        row.CreateCell(3).SetCellValue(p.ProductBrand);
        //        row.CreateCell(4).SetCellValue(p.ProductSupplier);
        //        row.CreateCell(5).SetCellValue(p.ProductCode);
        //        row.CreateCell(6).SetCellValue(p.ProductDescription);
        //        row.CreateCell(7).SetCellValue(p.SuggestedPrice?.ToString() ?? "");
        //        row.CreateCell(8).SetCellValue(p.ProductRemark);
        //        row.CreateCell(9).SetCellValue(p.ProductStatus ? "上架" : "未上架");
        //        row.CreateCell(10).SetCellValue(p.DealPrice?.ToString() ?? "");
        //    }

        //    byte[] fileBytes;
        //    using (var ms = new MemoryStream())
        //    {
        //        workbook.Write(ms);      // 写入流
        //        fileBytes = ms.ToArray();// 直接读取字节数组
        //    }
        //    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "产品信息.xlsx");

        //}


        ///// <summary>
        ///// 导出所有产品信息为Excel文件
        ///// </summary>
        ///// <returns>Excel文件流</returns>
        //[HttpGet("export")]
        //public async Task<IActionResult> ExportProductsAsyncExcel()
        //{
        //    // 查询所有产品
        //    var products = await productRepository.GetListAsync();

        //    // 定义表头
        //    var headers = new List<string>
        //{
        //    "产品分类", "父级分类ID", "产品图片", "门幅", "供应商", "产品编号",
        //    "产品描述", "建议售价", "备注", "上架状态", "成交价"
        //};

        //    // 定义每列对应的属性选择器
        //    var selectors = new List<Func<Product, object>>
        //{
        //    p => p.CategoryId,
        //    p => p.ParentId,
        //    p => p.ProductImageUrl,
        //    p => p.ProductBrand,
        //    p => p.ProductSupplier,
        //    p => p.ProductCode,
        //    p => p.ProductDescription,
        //    p => p.SuggestedPrice,
        //    p => p.ProductRemark,
        //    p => p.ProductStatus ? "上架" : "未上架",
        //    p => p.DealPrice
        //};

        //    // 调用通用导出方法
        //    var fileBytes = ExcelExportHelper.ExportToExcel(products, headers, selectors, "产品信息");

        //    // 返回文件流
        //    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "产品信息.xlsx");
        //}
        /// <summary>
        /// 导出所有产品信息为Excel文件
        /// </summary>
        /// <returns>Excel文件流</returns>
        [HttpGet("export")]
        public async Task<IActionResult> ExportProductsAsyncExcel()
        {
            // 从数据库查询所有产品
            var products = await productRepository.GetListAsync();

            // 定义表头（与Excel列一一对应）2
            var headers = new List<string>
        {
            "产品分类", "父级分类ID", "产品图片", "门幅", "供应商", "产品编号",
            "产品描述", "建议售价", "备注", "上架状态", "成交价"
        };

            // 定义每列对应的属性选择器（可自定义格式）
            var selectors = new List<Func<Product, object>>
        {
            p => p.CategoryId,                                 // 产品分类
            p => p.ParentId,                                   // 父级分类ID
            p => p.ProductImageUrl,                            // 产品图片
            p => p.ProductBrand,                               // 门幅
            p => p.ProductSupplier,                            // 供应商
            p => p.ProductCode,                                // 产品编号
            p => p.ProductDescription,                         // 产品描述
            p => p.SuggestedPrice,                             // 建议售价
            p => p.ProductRemark,                              // 备注
            p => p.ProductStatus ? "上架" : "未上架",           // 上架状态（自定义格式）
            p => p.DealPrice                                   // 成交价
        };

            // 调用通用导出方法，生成Excel字节流
            var fileBytes = ExcelExportHelper.ExportToExcel(products, headers, selectors, "产品信息");

            // 返回文件流，浏览器会自动下载
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "产品信息.xlsx");
        }
    }
}
