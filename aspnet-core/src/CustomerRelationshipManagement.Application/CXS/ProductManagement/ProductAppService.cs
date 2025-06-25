//using CustomerRelationshipManagement.DTOS.UploadFileDto;
using CustomerRelationshipManagement.CXS.ErrorCode;
using CustomerRelationshipManagement.CXS.ProductManagementDto;
using CustomerRelationshipManagement.ProductCategory.Products;
using CustomerRelationshipManagement.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Validation;

namespace CustomerRelationshipManagement.CXS.ProductManagement
{
    [ApiExplorerSettings(GroupName = "v1")]
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IRepository<Product> productRepository;
        public IHttpContextAccessor httpContextAccessor;

        public ProductAppService(IRepository<Product> productRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 新增产品
        /// </summary>
        /// <param name="createProduct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        [HttpPost]
        public async Task<ProductDtos> AddProduct(CreateUpdateProductDtos createProduct)
        {
            //将接收到的对象转换成实体
            var product = ObjectMapper.Map<CreateUpdateProductDtos, Product>(createProduct);
            //保存到数据库
            product = await productRepository.InsertAsync(product);
            //将数据库操作成功后的 Product 实体转换为 BookDto 对象
            return ObjectMapper.Map<Product, ProductDtos>(product);
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpDelete]
        public async Task<ApiResult<ProductDtos>> DeletedProduct(Guid id)
        {
            var productdel = await productRepository.GetAsync(x => x.Id == id);
            if (productdel == null)
            {
                return ApiResult<ProductDtos>.Fail("产品不存在", ResultCode.Fail);
            }
            await productRepository.DeleteAsync(productdel);
            return ApiResult<ProductDtos>.Success(ObjectMapper.Map<Product, ProductDtos>(productdel), ResultCode.Success);

        }
        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="apiPaging"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        [HttpGet]
        public async Task<ApiResult<ApiPaging<ProductDtos>>> GetProduct([FromQuery] ApiPaging apiPaging)
        {
            try
            {
                var productlist = await productRepository.GetQueryableAsync();
                if (!string.IsNullOrEmpty(apiPaging.Keyword))
                {
                    productlist = productlist.Where(x => x.ProductCode.Contains(apiPaging.Keyword) || x.ProductBrand.Contains(apiPaging.Keyword));

                }
                var totalCount = productlist.Count();
                var totalPage = (int)Math.Ceiling((double)productlist.Count() / apiPaging.PageSize);
                var pagedProduct = productlist.OrderBy(x => x.Id).Skip((apiPaging.PageIndex - 1) * apiPaging.PageSize).Take(apiPaging.PageSize).ToList();
                return ApiResult<ApiPaging<ProductDtos>>.Success(new ApiPaging<ProductDtos>
                {
                    Data = ObjectMapper.Map<List<Product>, List<ProductDtos>>(pagedProduct),
                    TotalCount = totalCount,
                    TotalPage = totalPage,
                }, ResultCode.Success);

            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public async Task<ApiResult<ProductDtos>> GetProductId(Guid id)
        {
            try
            {
                var product = await productRepository.GetAsync(x => x.Id == id);
                if (product == null)
                {
                    return ApiResult<ProductDtos>.Fail("未找到该数据", ResultCode.Fail);
                }
                return ApiResult<ProductDtos>.Success(ObjectMapper.Map<Product, ProductDtos>(product), ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 修改产品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateProduct"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ApiResult<CreateUpdateProductDtos>> UpdateProduct(Guid id, CreateUpdateProductDtos updateProduct)
        {
            try
            {
                var product = await productRepository.GetAsync(x => x.Id == id);
                if (product == null)
                {
                    return ApiResult<CreateUpdateProductDtos>.Fail("产品不存在", ResultCode.NotFound);
                }
                var productDto = ObjectMapper.Map(updateProduct, product);
                await productRepository.UpdateAsync(productDto);
                return ApiResult<CreateUpdateProductDtos>.Success(ObjectMapper.Map<Product, CreateUpdateProductDtos>(productDto), ResultCode.Success);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 导出所有产品信息为返回的后端json
        /// </summary>
        /// <returns>Excel文件流</returns>
        [HttpGet]
        [Route("api/product/export")]
        public async Task<FileDto> ExportProductsAsyncJson()
        {
            // 从数据库查询所有产品信息
            var products = await productRepository.GetListAsync();

            // 创建一个新的Excel工作簿
            IWorkbook workbook = new XSSFWorkbook();
            // 在工作簿中创建一个名为“产品信息”的工作表
            ISheet sheet = workbook.CreateSheet("产品信息");

            // 创建表头行，并设置每一列的标题
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("产品分类");
            headerRow.CreateCell(1).SetCellValue("父级分类ID");
            headerRow.CreateCell(2).SetCellValue("产品图片");
            headerRow.CreateCell(3).SetCellValue("门幅");
            headerRow.CreateCell(4).SetCellValue("供应商");
            headerRow.CreateCell(5).SetCellValue("产品编号");
            headerRow.CreateCell(6).SetCellValue("产品描述");
            headerRow.CreateCell(7).SetCellValue("建议售价");
            headerRow.CreateCell(8).SetCellValue("备注");
            headerRow.CreateCell(9).SetCellValue("上架状态");
            headerRow.CreateCell(10).SetCellValue("成交价");

            // 遍历产品列表，将每个产品的信息写入Excel表格
            for (int i = 0; i < products.Count; i++)
            {
                var row = sheet.CreateRow(i + 1); // 数据从第2行开始
                var p = products[i];
                row.CreateCell(0).SetCellValue(p.CategoryId.ToString());
                row.CreateCell(1).SetCellValue(p.ParentId.ToString());
                row.CreateCell(2).SetCellValue(p.ProductImageUrl);
                row.CreateCell(3).SetCellValue(p.ProductBrand);
                row.CreateCell(4).SetCellValue(p.ProductSupplier);
                row.CreateCell(5).SetCellValue(p.ProductCode);
                row.CreateCell(6).SetCellValue(p.ProductDescription);
                row.CreateCell(7).SetCellValue(p.SuggestedPrice?.ToString() ?? "");
                row.CreateCell(8).SetCellValue(p.ProductRemark);
                row.CreateCell(9).SetCellValue(p.ProductStatus ? "上架" : "未上架");
                row.CreateCell(10).SetCellValue(p.DealPrice?.ToString() ?? "");
            }

            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);      // 写入流
                fileBytes = ms.ToArray();// 直接读取字节数组
            }
            return new FileDto("产品信息.xlsx", fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }



    }
}
