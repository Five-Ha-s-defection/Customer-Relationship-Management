﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace CustomerRelationshipManagement.Captchas
{
    public interface ICaptchaServer: IApplicationService
    {
        IActionResult Captcha(string id);
        /// <summary>
        /// 多次校验（https://gitee.com/pojianbing/lazy-captcha/issues/I4XHGM）
        /// 演示时使用HttpGet传参方便，这里仅做返回处理
        /// </summary>
        bool Validate2(string id, string code);
    }
}
