using CustomerRelationshipManagement.ApiResults;
using CustomerRelationshipManagement.DTOS.Captchas;
using Lazy.Captcha.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Captchas
{
    /// <summary>
    /// 验证码服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class CaptchaServer : ApplicationService,ICaptchaServer
    {
        private readonly ICaptcha captcha;

        public CaptchaServer(ICaptcha captcha)
        {
            this.captcha = captcha;
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<CaptchaDto>> GetCaptchaAsync(string id)
        {
            //生成验证码
            var info = captcha.Generate(id);
            //转为base64
            var base64 = Convert.ToBase64String(info.Bytes);
            // 返回结果

            return ApiResult<CaptchaDto>.Success(ResultCode.Success, new CaptchaDto
            {
                Img = "data:image/png;base64," + base64,
            });
        }
        /// <summary>
        /// 多次校验（https://gitee.com/pojianbing/lazy-captcha/issues/I4XHGM）
        /// 演示时使用HttpGet传参方便，这里仅做返回处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        public bool Validate2(string id, string code)
        {
           
            return captcha.Validate(id, code,false);
        }
    }
}
