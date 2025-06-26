//using CustomerRelationshipManagement.DTOS.Base64UploadDtos;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CustomerRelationshipManagement.CXS.ProductManagement
//{
//    [ApiController]
//    [Route("api/upload")]
//    public class UploadService
//    {
//        /// <summary>
//        /// 接收Base64图片并保存
//        /// </summary>
//        [HttpPost("base64")]
//        public async Task<IActionResult> UploadBase64Async([FromBody] Base64UploadDto input)
//        {
//            // 校验文件名和Base64字符串
//            if (string.IsNullOrEmpty(input.FileName) || string.IsNullOrEmpty(input.Base64))
//                return BadRequest("文件名或Base64内容不能为空");

//            // 校验文件扩展名
//            var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".bmp" };
//            var ext = Path.GetExtension(input.FileName).ToLower();
//            if (!allowedExts.Contains(ext))
//                return BadRequest("只允许上传jpg、jpeg、png、bmp格式的图片");

//            // Base64解码为字节数组
//            byte[] bytes;
//            try
//            {
//                bytes = Convert.FromBase64String(input.Base64);
//            }
//            catch
//            {
//                return BadRequest("Base64格式不正确");
//            }

//            // 生成保存路径（可自定义）
//            var saveDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
//            if (!Directory.Exists(saveDir))
//                Directory.CreateDirectory(saveDir);

//            var fileName = Guid.NewGuid().ToString("N") + ext;
//            var filePath = Path.Combine(saveDir, fileName);

//            // 保存文件
//            await System.IO.File.WriteAllBytesAsync(filePath, bytes);

//            // 返回图片访问地址
//            var url = $"/upload/{fileName}";
//            return Ok(new { url });
//        }
//    }
//}
