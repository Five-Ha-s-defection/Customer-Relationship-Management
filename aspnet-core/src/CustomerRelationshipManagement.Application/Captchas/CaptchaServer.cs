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
        public IActionResult Captcha(string id)
        {
            try
            {
                var info = captcha.Generate(id);
                // 有多处验证码且过期时间不一样，可传第二个参数覆盖默认配置。
                //var info = _captcha.Generate(id,120);
                var stream = new MemoryStream(info.Bytes);
                return new FileStreamResult(stream, "image/jpeg");
               
            }
            catch (Exception ex)
            { 
                return new ContentResult()
                {
                    Content = ex.Message,
                    StatusCode = 500
                };
            }
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
            return captcha.Validate(id, code, false);
        }
    }
}
